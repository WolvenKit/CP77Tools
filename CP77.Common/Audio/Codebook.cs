/**
    Copyright (c) 2002, Xiph.org Foundation
    Copyright (c) 2009-2016, Adam Gashlin
    Copyright (c) 2020, Patrick Demian

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions
    are met:

    - Redistributions of source code must retain the above copyright
    notice, this list of conditions and the following disclaimer.

    - Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in the
    documentation and/or other materials provided with the distribution.

    - Neither the name of the Xiph.org Foundation nor the names of its
    contributors may be used to endorse or promote products derived from
    this software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
    ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
    LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
    A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE FOUNDATION
    OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
    SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
    LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
    DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
    THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
    OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace CP77.Common.Audio
{
    class Codebook
    {
        public static uint ilog(uint v)
        {
            uint ret = 0;
            while (v != 0)
            {
                ret++;
                v >>= 1;
            }
            return ret;
        }

        private uint BookMaptype1Quantvals(uint entries, uint dimensions)
        {
            /* get us a starting hint, we'll polish it below */
            uint bits = ilog(entries);
            uint vals = entries >> (int)((bits - 1) * (dimensions - 1) / dimensions);

            for (; ; )
            {
                ulong acc = 1;
                ulong acc1 = 1;
                uint i;
                for (i = 0; i < dimensions; i++)
                {
                    acc *= vals;
                    acc1 *= vals + 1;
                }
                if (acc <= entries && acc1 > entries)
                {
                    return (uint)vals;
                }
                else
                {
                    if (acc > entries)
                    {
                        vals--;
                    }
                    else
                    {
                        vals++;
                    }
                }
            }
        }

        private byte[] codebook_data;
        private long[] codebook_offsets;
        private long codebook_count;

        public Codebook()
        {
            codebook_data = null;
            codebook_offsets = null;
            codebook_count = 0;
        }

        public Codebook(string filename)
        {
            string codebook_path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "avconv/", filename);

            using (FileStream fs = new FileStream(codebook_path, FileMode.Open, FileAccess.Read))
            {
                EndianAwareBinaryReader reader = new EndianAwareBinaryReader(fs);

                long file_size = fs.Length;

                // skip to end of file to get offset
                fs.Position = file_size - 4;
                long offset_offset = reader.ReadUInt32LE();
                codebook_count = (file_size - offset_offset) / 4;

                codebook_data = new byte[offset_offset];
                codebook_offsets = new long[codebook_count];

                // go back to start of file
                fs.Position = 0;

                for (long i = 0; i < offset_offset; i++)
                {
                    codebook_data[i] = reader.ReadByte();
                }

                for (long i = 0; i < codebook_count; i++)
                {
                    codebook_offsets[i] = reader.ReadUInt32LE();
                }
            }
        }

        public byte GetCodebook(int i)
        {
            if (codebook_data == null || codebook_offsets == null)
            {
                throw new Exception("codebook library not loaded");
            }

            if (i >= codebook_count - 1 || i < 0) throw new IndexOutOfRangeException($"i is outside of range (0, {codebook_count - 1})");

            return codebook_data[codebook_offsets[i]];
        }

        public long GetCodeBookSize(int i)
        {
            if (codebook_data == null || codebook_offsets == null)
            {
                throw new Exception("codebook library not loaded");
            }

            if (i >= codebook_count - 1 || i < 0) throw new IndexOutOfRangeException($"i is outside of range (0, {codebook_count - 1})");
            return codebook_offsets[i + 1] - codebook_offsets[i];
        }

        public void Rebuild(int i, OggStream bos)
        {
            uint cb_size;

            {
                long signed_cb_size = GetCodeBookSize(i);

                if (-1 == signed_cb_size) throw new Exception($"Invalid ID {i}");

                cb_size = (uint)signed_cb_size;
            }

            long offset = codebook_offsets[i];

            byte[] arr = new byte[cb_size];
            for (int j = 0; j < cb_size; j++)
            {
                arr[j] = codebook_data[offset + j];
            }

            BitStream bis = new BitStream(new EndianAwareBinaryReader(new MemoryStream(arr)));

            Rebuild(bis, cb_size, bos);
        }

        public void Rebuild(BitStream bis, uint cb_size, OggStream bos)
        {
            /* IN: 4 bit dimensions, 14 bit entry count */

            uint dimensions = bis.GetBits(4);
            uint entries = bis.GetBits(14);

            //cout << "Codebook " << i << ", " << dimensions << " dimensions, " << entries << " entries" << endl;
            //cout << "Codebook with " << dimensions << " dimensions, " << entries << " entries" << endl;

            /* OUT: 24 bit identifier, 16 bit dimensions, 24 bit entry count */
            bos.Write(0x564342, 24);
            bos.Write(dimensions, 16);
            bos.Write(entries, 24);

            // gather codeword lengths

            /* IN/OUT: 1 bit ordered flag */
            uint ordered = bis.GetBits(1);
            bos.Write(ordered, 1);
            if (ordered != 0)
            {
                //cout << "Ordered " << endl;

                /* IN/OUT: 5 bit initial length */
                uint initial_length = bis.GetBits(5);
                bos.Write(initial_length, 5);

                uint current_entry = 0;
                while (current_entry < entries)
                {
                    /* IN/OUT: ilog(entries-current_entry) bit count w/ given length */
                    int number_size = (int)ilog((uint)entries - current_entry);
                    uint number = bis.GetBits(number_size);
                    bos.Write(number, number_size);
                    current_entry += number;
                }
                if (current_entry > entries) throw new Exception("current_entry out of range");
            }
            else
            {
                /* IN: 3 bit codeword length length, 1 bit sparse flag */
                uint codeword_length_length = bis.GetBits(3);
                uint sparse = bis.GetBits(1);

                //cout << "Unordered, " << codeword_length_length << " bit lengths, ";

                if (0 == codeword_length_length || 5 < codeword_length_length)
                {
                    throw new Exception("nonsense codeword length");
                }

                /* OUT: 1 bit sparse flag */
                bos.Write(sparse, 1);
                //if (sparse)
                //{
                //    cout << "Sparse" << endl;
                //}
                //else
                //{
                //    cout << "Nonsparse" << endl;
                //}

                for (uint i = 0; i < entries; i++)
                {
                    bool present_bool = true;

                    if (sparse != 0)
                    {
                        /* IN/OUT 1 bit sparse presence flag */
                        uint present = bis.GetBits(1);
                        bos.Write(present, 1);

                        present_bool = (0 != present);
                    }

                    if (present_bool)
                    {
                        /* IN: n bit codeword length-1 */
                        uint codeword_length = bis.GetBits((int)codeword_length_length);

                        /* OUT: 5 bit codeword length-1 */
                        bos.Write(codeword_length, 5);
                    }
                }
            } // done with lengths


            // lookup table

            /* IN: 1 bit lookup type */
            uint lookup_type = bis.GetBits(1);
            /* OUT: 4 bit lookup type */
            bos.Write(lookup_type, 4);

            if (0 == lookup_type)
            {
                //cout << "no lookup table" << endl;
            }
            else if (1 == lookup_type)
            {
                //cout << "lookup type 1" << endl;

                /* IN/OUT: 32 bit minimum length, 32 bit maximum length, 4 bit value length-1, 1 bit sequence flag */
                uint min = bis.GetBits(32);
                uint max = bis.GetBits(32);
                uint value_length = bis.GetBits(4);
                uint sequence_flag = bis.GetBits(1);
                bos.Write(min, 32);
                bos.Write(max, 32);
                bos.Write(value_length, 4);
                bos.Write(sequence_flag, 1);

                uint quantvals = BookMaptype1Quantvals(entries, dimensions);
                for (uint i = 0; i < quantvals; i++)
                {
                    /* IN/OUT: n bit value */
                    uint val = bis.GetBits((int)value_length + 1);
                    bos.Write(val, (int)value_length + 1);
                }
            }
            else if (2 == lookup_type)
            {
                throw new Exception("didn't expect lookup type 2");
            }
            else
            {
                throw new Exception("invalid lookup type");
            }

            //cout << "total bits read = " << bis.get_total_bits_read() << endl;

            /* check that we used exactly all bytes */
            /* note: if all bits are used in the last byte there will be one extra 0 byte */
            if (0 != cb_size && bis.GetTotalBitsRead() / 8 + 1 != cb_size)
            {
                throw new Exception("Size Mismatch");
            }
        }
    }
}
