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
using System.Linq;

namespace CP77.Common.Audio
{
    /* Modern 2 or 6 byte header */
    public class RIFFPacket
    {
        private readonly long offset;
        private readonly ushort size;
        private readonly uint absolute_granule;
        private readonly bool no_granule;

        public RIFFPacket(EndianAwareBinaryReader reader, long o, bool no_granule = false)
        {
            offset = o;
            size = ushort.MaxValue;
            absolute_granule = 0;
            this.no_granule = no_granule;

            reader.BaseStream.Position = offset;

            size = reader.ReadUInt16();
            if (!this.no_granule)
            {
                absolute_granule = reader.ReadUInt32();
            }
        }

        public long HeaderSize() { return no_granule ? 2 : 6; }
        public long Offset() { return offset + HeaderSize(); }
        public ushort Size() { return size; }
        public uint Granule() { return absolute_granule; }
        public long NextOffset() { return offset + HeaderSize() + size; }
    };

    public class WwiseRIFFVorbisFile
    {
        public readonly string codebooks_name;
        public long file_size;

        public bool little_endian;

        public long riff_size;
        public long fmt_offset, cue_offset, LIST_offset, smpl_offset, vorb_offset, data_offset;
        public long fmt_size, cue_size, LIST_size, smpl_size, vorb_size, data_size;

        // RIFF fmt
        public ushort channels;
        public uint sample_rate;
        public uint avg_bytes_per_second;

        // RIFF extended fmt
        public ushort ext_unk;
        public uint subtype;

        // cue info
        public uint cue_count;

        // smpl info
        public uint loop_count, loop_start, loop_end;

        // vorbis info
        public uint sample_count;
        public uint setup_packet_offset;
        public uint first_audio_packet_offset;
        public uint uid;
        public byte blocksize_0_pow;
        public byte blocksize_1_pow;

        public bool header_triad_present, old_packet_headers;
        public bool no_granule, mod_packets;

        public WwiseRIFFVorbisFile(string codebook_name)
        {
            riff_size = -1;
            fmt_offset = -1;
            cue_offset = -1;
            LIST_offset = -1;
            smpl_offset = -1;
            vorb_offset = -1;
            data_offset = -1;
            fmt_size = -1;
            cue_size = -1;
            LIST_size = -1;
            smpl_size = -1;
            vorb_size = -1;
            data_size = -1;
            channels = 0;
            sample_rate = 0;
            avg_bytes_per_second = 0;
            ext_unk = 0;
            subtype = 0;
            cue_count = 0;
            loop_count = 0;
            loop_start = 0;
            loop_end = 0;
            sample_count = 0;
            setup_packet_offset = 0;
            first_audio_packet_offset = 0;
            uid = 0;
            blocksize_0_pow = 0;
            blocksize_1_pow = 0;
            header_triad_present = false;
            old_packet_headers = false;
            no_granule = false;
            mod_packets = false;
            codebooks_name = codebook_name;
        }
    }

    public class WwiseRIFFVorbis
    {
        public static void GenerateWem(string codebook_name, Stream in_stream, Stream out_stream)
        {
            // Surprisingly I cannot find a resource on how to ENCODE to Wwise
            // vgmstream only decodes, and ww2ogg only decodes
            // TODO: reverse the GenerateOgg method to generate a .wem file

            throw new NotImplementedException("No way right now to convert to .wem");
        }

        public static void GenerateOgg(string codebooks_name, Stream in_stream, Stream out_stream)
        {
            if (!in_stream.CanSeek)
            {
                throw new Exception("Bad stream. Must be able to seek. Use file or memorymapped stream");
            }

            WwiseRIFFVorbisFile file = new WwiseRIFFVorbisFile(codebooks_name);

            EndianAwareBinaryReader reader = new EndianAwareBinaryReader(in_stream);

            Setup(file, reader);

            if (file.header_triad_present)
            {
                throw new Exception("Assumption failed! CP77 does use triad");
            }
            if (file.old_packet_headers)
            {
                throw new Exception("Assumption failed! CP77 does use old packet headers");
            }

            using OggStream os = new OggStream(out_stream);

            GenerateOggHeader(file, reader, os, out bool[] mode_blockflag, out int mode_bits);
            GenerateOggBody(file, reader, os, mode_blockflag, mode_bits);
        }

        public static void Setup(WwiseRIFFVorbisFile file, EndianAwareBinaryReader reader)
        {
            file.file_size = reader.BaseStream.Length;


            #region check RIFF header
            reader.BaseStream.Position = 0;
            string riff_head = Encoding.ASCII.GetString(reader.ReadBytes(4));

            file.little_endian = riff_head switch
            {
                "RIFX" => false,
                "RIFF" => true,
                _ => throw new Exception("Missing RIFF"),
            };

            reader.IsLittleEndian = file.little_endian;

            file.riff_size = reader.ReadUInt32() + 8;

            if (file.riff_size > file.file_size)
            {
                throw new Exception("RIFF truncated");
            }

            string wave_head = Encoding.ASCII.GetString(reader.ReadBytes(4));
            if (wave_head != "WAVE")
            {
                throw new Exception("Missing WAVE");
            }
            #endregion

            #region read chunks
            long chunk_offset = 12;
            while (chunk_offset < file.riff_size)
            {
                reader.BaseStream.Position = chunk_offset;

                if (chunk_offset + 8 > file.riff_size) throw new Exception("Chunk header truncated");

                string chunk_type = Encoding.ASCII.GetString(reader.ReadBytes(4));

                uint chunk_size = reader.ReadUInt32();

                if (chunk_type == "fmt ")
                {
                    file.fmt_offset = chunk_offset + 8;
                    file.fmt_size = chunk_size;
                }
                else if (chunk_type == "cue ")
                {
                    file.cue_offset = chunk_offset + 8;
                    file.cue_size = chunk_size;
                }
                else if (chunk_type == "LIST")
                {
                    file.LIST_offset = chunk_offset + 8;
                    file.LIST_size = chunk_size;
                }
                else if (chunk_type == "smpl")
                {
                    file.smpl_offset = chunk_offset + 8;
                    file.smpl_size = chunk_size;
                }
                else if (chunk_type == "vorb")
                {
                    file.vorb_offset = chunk_offset + 8;
                    file.vorb_size = chunk_size;
                }
                else if (chunk_type == "data")
                {
                    file.data_offset = chunk_offset + 8;
                    file.data_size = chunk_size;
                }

                chunk_offset = chunk_offset + 8 + chunk_size;
            }

            if (chunk_offset > file.riff_size) throw new Exception("Chunk truncated");

            // check that we have the chunks we're expecting
            if (-1 == file.fmt_offset && -1 == file.data_offset) throw new Exception("Expected fmt, data chunks");

            // read fmt
            if (-1 == file.vorb_offset && 0x42 != file.fmt_size) throw new Exception("Expected 0x42 fmt if vorb missing");

            if (-1 != file.vorb_offset && 0x28 != file.fmt_size && 0x18 != file.fmt_size && 0x12 != file.fmt_size) throw new Exception("Bad fmt size");

            if (-1 == file.vorb_offset && 0x42 == file.fmt_size)
            {
                // fake it out
                file.vorb_offset = file.fmt_offset + 0x18;
            }
            #endregion

            #region Misc
            reader.BaseStream.Position = file.fmt_offset;
            if (0xFFFFU != reader.ReadUInt16()) throw new Exception("Bad codec id");
            file.channels = reader.ReadUInt16();
            file.sample_rate = reader.ReadUInt32();
            file.avg_bytes_per_second = reader.ReadUInt32();
            if (0U != reader.ReadUInt16()) throw new Exception("Bad block align");
            if (0U != reader.ReadUInt16()) throw new Exception("Expected 0 bps");
            if (file.fmt_size - 0x12 != reader.ReadUInt16()) throw new Exception("Bad extra fmt length");

            if (file.fmt_size - 0x12 >= 2)
            {
                // read extra fmt
                file.ext_unk = reader.ReadUInt16();
                if (file.fmt_size - 0x12 >= 6)
                {
                    file.subtype = reader.ReadUInt32();
                }
            }

            if (file.fmt_size == 0x28)
            {
                byte[] whoknowsbuf = reader.ReadBytes(16);
                byte[] whoknowsbuf_check = { 1, 0, 0, 0, 0, 0, 0x10, 0, 0x80, 0, 0, 0xAA, 0, 0x38, 0x9b, 0x71 };
                if (!whoknowsbuf_check.SequenceEqual(whoknowsbuf)) throw new Exception("Expected signature in extra fmt?");
            }

            // read cue
            if (-1 != file.cue_offset)
            {
                reader.BaseStream.Position = file.cue_offset;

                file.cue_count = reader.ReadUInt32();
            }

            // read smpl
            if (-1 != file.smpl_offset)
            {
                reader.BaseStream.Position = file.smpl_offset + 0x1C;
                file.loop_count = reader.ReadUInt32();

                if (1 != file.loop_count) throw new Exception("Expected one loop");

                reader.BaseStream.Position = file.smpl_offset + 0x2c;
                file.loop_start = reader.ReadUInt32();
                file.loop_end = reader.ReadUInt32();
            }

            // read vorb
            switch (file.vorb_size)
            {
                case -1:
                case 0x28:
                case 0x2A:
                case 0x2C:
                case 0x32:
                case 0x34:
                    reader.BaseStream.Position = file.vorb_offset + 0x00;
                    break;

                default:
                    throw new Exception("Bad vorb size");
            }

            file.sample_count = reader.ReadUInt32();

            switch (file.vorb_size)
            {
                case -1:
                case 0x2A:
                    {
                        file.no_granule = true;

                        reader.BaseStream.Position = file.vorb_offset + 0x4;
                        uint mod_signal = reader.ReadUInt32();

                        // set
                        // D9     11011001
                        // CB     11001011
                        // BC     10111100
                        // B2     10110010
                        // unset
                        // 4A     01001010
                        // 4B     01001011
                        // 69     01101001
                        // 70     01110000
                        // A7     10100111 !!!

                        // seems to be 0xD9 when _mod_packets should be set
                        // also seen 0xCB, 0xBC, 0xB2
                        if (0x4A != mod_signal && 0x4B != mod_signal && 0x69 != mod_signal && 0x70 != mod_signal)
                        {
                            file.mod_packets = true;
                        }
                        reader.BaseStream.Position = file.vorb_offset + 0x10;
                        break;
                    }

                default:
                    reader.BaseStream.Position = file.vorb_offset + 0x18;
                    break;
            }


            file.setup_packet_offset = reader.ReadUInt32();
            file.first_audio_packet_offset = reader.ReadUInt32();

            switch (file.vorb_size)
            {
                case -1:
                case 0x2A:
                    reader.BaseStream.Position = file.vorb_offset + 0x24;
                    break;

                case 0x32:
                case 0x34:
                    reader.BaseStream.Position = file.vorb_offset + 0x2C;
                    break;
            }

            switch (file.vorb_size)
            {
                case 0x28:
                case 0x2C:
                    // ok to leave _uid, _blocksize_0_pow and _blocksize_1_pow unset
                    file.header_triad_present = true;
                    file.old_packet_headers = true;
                    break;

                case -1:
                case 0x2A:
                case 0x32:
                case 0x34:
                    file.uid = reader.ReadUInt32();
                    file.blocksize_0_pow = reader.ReadByte();
                    file.blocksize_1_pow = reader.ReadByte();
                    break;
            }

            if (file.header_triad_present)
            {
                throw new Exception("CP77 shouldn't have triad. Assumption failed.");
            }

            // check/set loops now that we know total sample count
            if (0 != file.loop_count)
            {
                if (file.loop_end == 0)
                {
                    file.loop_end = file.sample_count;
                }
                else
                {
                    file.loop_end = file.loop_end + 1;
                }

                if (file.loop_start >= file.sample_count || file.loop_end > file.sample_count || file.loop_start > file.loop_end)
                {
                    throw new Exception("Loops out of range");
                }
            }

            // check subtype now that we know the vorb info
            // this is clearly just the channel layout
            switch (file.subtype)
            {
                case 4:     /* 1 channel, no seek table */
                case 3:     /* 2 channels */
                case 0x33:  /* 4 channels */
                case 0x37:  /* 5 channels, seek or not */
                case 0x3b:  /* 5 channels, no seek table */
                case 0x3f:  /* 6 channels, no seek table */
                    break;
                default:
                    //throw new Exception("unknown subtype");
                    break;
            }
            #endregion
        }

        private static void GenerateOggHeader(WwiseRIFFVorbisFile file, EndianAwareBinaryReader reader, OggStream os, out bool[] mode_blockflag, out int mode_bits)
        {
            #region generate identification packet
            os.Write(1, 8);
            os.Write(Encoding.ASCII.GetBytes("vorbis"));

            os.Write(0, 32); // version
            os.Write(file.channels, 8); // channels
            os.Write(file.sample_rate, 32); // sample rate
            os.Write(0, 32); // bitrate max
            os.Write(file.avg_bytes_per_second * 8, 32); // bitrate nominal
            os.Write(0, 32); // bitrate min
            os.Write(file.blocksize_0_pow, 4); //blocksize_0
            os.Write(file.blocksize_1_pow, 4); //blocksize_1
            os.Write(1, 1); //framing

            // identification packet on its own page
            os.FlushPage();
            #endregion

            #region generate comment packet
            os.Write(3, 8);
            os.Write(Encoding.ASCII.GetBytes("vorbis"));

            string vendor = "converted from Audiokinetic Wwise by CP77 with use from ww2ogg";
            os.Write(Encoding.ASCII.GetByteCount(vendor), 32);
            os.Write(Encoding.ASCII.GetBytes(vendor));

            if (0 == file.loop_count)
            {
                // no user comments
                os.Write(0, 32);
            }
            else
            {
                // two comments, loop start and end

                os.Write(2, 32); // user comment count

                string loop_start_str = $"LoopStart={file.loop_start}";
                string loop_end_str = $"LoopEnd={file.loop_end}";

                os.Write(Encoding.ASCII.GetByteCount(loop_start_str), 32);
                os.Write(Encoding.ASCII.GetBytes(loop_start_str));

                os.Write(Encoding.ASCII.GetByteCount(loop_end_str), 32);
                os.Write(Encoding.ASCII.GetBytes(loop_end_str));
            }

            os.Write(1, 1); //framing

            //os.flush_bits();
            os.FlushPage();
            #endregion

            #region generate setup packet
            os.Write(5, 8);
            os.Write(Encoding.ASCII.GetBytes("vorbis"));

            RIFFPacket setup_packet = new RIFFPacket(reader, file.data_offset + file.setup_packet_offset, file.no_granule);

            reader.BaseStream.Position = setup_packet.Offset();
            if (setup_packet.Granule() != 0) throw new Exception("setup packet granule != 0");

            BitStream ss = new BitStream(reader);

            // codebook count
            uint codebook_count = ss.GetBits(8) + 1;
            os.Write(codebook_count - 1, 8);

            //cout << codebook_count << " codebooks" << endl;

            // rebuild codebooks
            /* external codebooks */

            Codebook cbl = new Codebook(file.codebooks_name);

            for (uint i = 0; i < codebook_count; i++)
            {
                uint codebook_id = ss.GetBits(10);
                cbl.Rebuild((int)codebook_id, os);
            }

            // Time Domain transforms (placeholder)
            os.Write(0, 6); // time count less 1
            os.Write(0, 16); //dummy_time_value

            // floor count
            uint floor_count = ss.GetBits(6) + 1;
            os.Write(floor_count - 1, 6);

            #endregion

            #region rebuild floors
            for (uint i = 0; i < floor_count; i++)
            {
                // Always floor type 1
                os.Write(1, 16);

                uint floor1_partitions = ss.GetBits(5);
                os.Write(floor1_partitions, 5);

                uint[] floor1_partition_class_list = new uint[floor1_partitions];

                uint maximum_class = 0;
                for (uint j = 0; j < floor1_partitions; j++)
                {
                    uint floor1_partition_class = ss.GetBits(4);
                    os.Write(floor1_partition_class, 4);

                    floor1_partition_class_list[j] = floor1_partition_class;

                    if (floor1_partition_class > maximum_class)
                        maximum_class = floor1_partition_class;
                }

                uint[] floor1_class_dimensions_list = new uint[maximum_class + 1];

                for (uint j = 0; j <= maximum_class; j++)
                {
                    uint class_dimensions_less1 = ss.GetBits(3);
                    os.Write(class_dimensions_less1, 3);

                    floor1_class_dimensions_list[j] = class_dimensions_less1 + 1;

                    uint class_subclasses = ss.GetBits(2);
                    os.Write(class_subclasses, 2);

                    if (0 != class_subclasses)
                    {
                        uint masterbook = ss.GetBits(8);
                        os.Write(masterbook, 8);

                        if (masterbook >= codebook_count)
                        {
                            throw new Exception("invalid floor1 masterbook");
                        }
                    }

                    for (uint k = 0; k < (1U << (int)class_subclasses); k++)
                    {
                        uint subclass_book_plus1 = ss.GetBits(8);
                        os.Write(subclass_book_plus1, 8);

                        int subclass_book = (int)subclass_book_plus1 - 1;
                        if (subclass_book >= 0 && (uint)(subclass_book) >= codebook_count)
                        {
                            throw new Exception("invalid floor1 subclass book");
                        }
                    }
                }

                uint floor1_multiplier_less1 = ss.GetBits(2);
                os.Write(floor1_multiplier_less1, 2);

                uint rangebits = ss.GetBits(4);
                os.Write(rangebits, 4);

                for (uint j = 0; j < floor1_partitions; j++)
                {
                    uint current_class_number = floor1_partition_class_list[j];
                    for (uint k = 0; k < floor1_class_dimensions_list[current_class_number]; k++)
                    {
                        os.Write(ss.GetBits((int)rangebits), (int)rangebits);
                    }
                }
            }
            #endregion

            #region residue
            uint residue_count = ss.GetBits(6) + 1;
            os.Write(residue_count - 1, 6);

            // rebuild residues
            for (uint i = 0; i < residue_count; i++)
            {
                uint residue_type = ss.GetBits(2);
                os.Write(residue_type, 16); //intentional

                if (residue_type > 2)
                {
                    throw new Exception("invalid residue type");
                }

                uint residue_begin = ss.GetBits(24), residue_end = ss.GetBits(24), residue_partition_size_less1 = ss.GetBits(24);
                uint residue_classifications_less1 = ss.GetBits(6);
                uint residue_classbook = ss.GetBits(8);

                uint residue_classifications = residue_classifications_less1 + 1;
                os.Write(residue_begin, 24);
                os.Write(residue_end, 24);
                os.Write(residue_partition_size_less1, 24);
                os.Write(residue_classifications_less1, 6);
                os.Write(residue_classbook, 8);

                if (residue_classbook >= codebook_count)
                {
                    throw new Exception("invalid residue classbook");
                }

                uint[] residue_cascade = new uint[residue_classifications];

                for (uint j = 0; j < residue_classifications; j++)
                {
                    uint high_bits = 0;
                    uint low_bits = ss.GetBits(3);
                    os.Write(low_bits, 3);

                    uint bitflag = ss.GetBits(1);
                    os.Write(bitflag, 1);
                    if (bitflag != 0)
                    {
                        high_bits = ss.GetBits(5);
                        os.Write(high_bits, 5);
                    }

                    residue_cascade[j] = high_bits * 8 + low_bits;
                }

                for (uint j = 0; j < residue_classifications; j++)
                {
                    for (uint k = 0; k < 8; k++)
                    {
                        if ((residue_cascade[j] & (1 << (int)k)) != 0)
                        {
                            uint residue_book = ss.GetBits(8);
                            os.Write(residue_book, 8);

                            if (residue_book >= codebook_count) throw new Exception("invalid residue book");
                        }
                    }
                }
            }
            #endregion

            #region mapping
            uint mapping_count_less1 = ss.GetBits(6);
            uint mapping_count = mapping_count_less1 + 1;
            os.Write(mapping_count_less1, 6);

            for (uint i = 0; i < mapping_count; i++)
            {
                // always mapping type 0, the only one
                uint mapping_type = 0;
                os.Write(mapping_type, 16);

                uint submaps_flag = ss.GetBits(1);
                os.Write(submaps_flag, 1);

                uint submaps = 1;
                if (submaps_flag != 0)
                {
                    uint submaps_less1 = ss.GetBits(4);
                    submaps = submaps_less1 + 1;
                    os.Write(submaps_less1, 4);
                }

                uint square_polar_flag = ss.GetBits(1);
                os.Write(square_polar_flag, 1);

                if (square_polar_flag != 0)
                {
                    uint coupling_steps_less1 = ss.GetBits(8);
                    uint coupling_steps = coupling_steps_less1 + 1;
                    os.Write(coupling_steps_less1, 8);

                    for (uint j = 0; j < coupling_steps; j++)
                    {
                        int mag_angle_size = (int)Codebook.ilog((uint)(file.channels - 1));
                        uint magnitude = ss.GetBits(mag_angle_size);
                        uint angle = ss.GetBits(mag_angle_size);

                        os.Write(magnitude, mag_angle_size);
                        os.Write(angle, mag_angle_size);

                        if (angle == magnitude || magnitude >= file.channels || angle >= file.channels)
                        {
                            throw new Exception("invalid coupling");
                        }
                    }
                }

                // a rare reserved field not removed by Ak!
                uint mapping_reserved = ss.GetBits(2);
                os.Write(mapping_reserved, 2);
                if (0 != mapping_reserved)
                {
                    throw new Exception("mapping reserved field nonzero");
                }

                if (submaps > 1)
                {
                    for (uint j = 0; j < file.channels; j++)
                    {
                        uint mapping_mux = ss.GetBits(4);
                        os.Write(mapping_mux, 4);

                        if (mapping_mux >= submaps)
                        {
                            throw new Exception("mapping_mux >= submaps");
                        }
                    }
                }

                for (uint j = 0; j < submaps; j++)
                {
                    // Another! Unused time domain transform configuration placeholder!
                    uint time_config = ss.GetBits(8);
                    os.Write(time_config, 8);

                    uint floor_number = ss.GetBits(8);
                    os.Write(floor_number, 8);
                    if (floor_number >= floor_count)
                    {
                        throw new Exception("invalid floor mapping");
                    }

                    uint residue_number = ss.GetBits(8);
                    os.Write(residue_number, 8);
                    if (residue_number >= residue_count)
                    {
                        throw new Exception("invalid residue mapping");
                    }
                }
            }
            #endregion

            #region mode
            uint mode_count_less1 = ss.GetBits(6);
            uint mode_count = mode_count_less1 + 1;
            os.Write(mode_count_less1, 6);

            mode_blockflag = new bool[mode_count];
            mode_bits = (int)Codebook.ilog(mode_count - 1);

            for (uint i = 0; i < mode_count; i++)
            {
                uint block_flag = ss.GetBits(1);
                os.Write(block_flag, 1);

                mode_blockflag[i] = (block_flag != 0);

                // only 0 valid for windowtype and transformtype
                uint windowtype = 0, transformtype = 0;
                os.Write(windowtype, 16);
                os.Write(transformtype, 16);

                uint mapping = ss.GetBits(8);
                os.Write(mapping, 8);
                if (mapping >= mapping_count)
                {
                    throw new Exception("invalid mode mapping");
                }
            }

            uint framing = 1;
            os.Write(framing, 1);
            #endregion mode

            os.FlushPage();

            if ((ss.GetTotalBitsRead() + 7) / 8 != setup_packet.Size())
            {
                throw new Exception("didn't read exactly setup packet");
            }

            if (setup_packet.NextOffset() != file.data_offset + file.first_audio_packet_offset)
            {
                throw new Exception("first audio packet doesn't follow setup packet");
            }
        }

        private static void GenerateOggBody(WwiseRIFFVorbisFile file, EndianAwareBinaryReader reader, OggStream os, bool[] mode_blockflag, int mode_bits)
        {
            bool prev_blockflag = false;

            // Audio pages
            {
                long offset = file.data_offset + file.first_audio_packet_offset;

                while (offset < file.data_offset + file.data_size)
                {
                    uint size, granule;
                    long packet_header_size, packet_payload_offset, next_offset;

                    {
                        RIFFPacket audio_packet = new RIFFPacket(reader, offset, file.no_granule);
                        packet_header_size = audio_packet.HeaderSize();
                        size = audio_packet.Size();
                        packet_payload_offset = audio_packet.Offset();
                        granule = audio_packet.Granule();
                        next_offset = audio_packet.NextOffset();
                    }

                    if (offset + packet_header_size > file.data_offset + file.data_size)
                    {
                        throw new Exception("page header truncated");
                    }

                    offset = packet_payload_offset;

                    reader.BaseStream.Position = offset;
                    // HACK: don't know what to do here
                    if (granule == 0xFFFFFFFFU)
                    {
                        os.Granule = 1;
                    }
                    else
                    {
                        os.Granule = granule;
                    }

                    // first byte
                    if (file.mod_packets)
                    {
                        // need to rebuild packet type and window info

                        if (mode_blockflag == null)
                        {
                            throw new Exception("didn't load mode_blockflag");
                        }

                        // OUT: 1 bit packet type (0 == audio)
                        uint packet_type = 0;
                        os.Write(packet_type, 1);

                        uint mode_number_p;
                        uint remainder_p;

                        {
                            // collect mode number from first byte

                            BitStream ss = new BitStream(reader);

                            // IN/OUT: N bit mode number (max 6 bits)
                            mode_number_p = ss.GetBits(mode_bits);
                            os.Write(mode_number_p, mode_bits);

                            // IN: remaining bits of first (input) byte
                            remainder_p = ss.GetBits(8 - mode_bits);
                        }

                        if (mode_blockflag[mode_number_p])
                        {
                            // long window, peek at next frame

                            reader.BaseStream.Position = next_offset;
                            bool next_blockflag = false;
                            if (next_offset + packet_header_size <= file.data_offset + file.data_size)
                            {

                                // mod_packets always goes with 6-byte headers
                                RIFFPacket audio_packet = new RIFFPacket(reader, next_offset, file.no_granule);
                                uint next_packet_size = audio_packet.Size();
                                if (next_packet_size > 0)
                                {
                                    reader.BaseStream.Position = audio_packet.Offset();

                                    BitStream ss = new BitStream(reader);
                                    uint next_mode_number = ss.GetBits(mode_bits);

                                    next_blockflag = mode_blockflag[next_mode_number];
                                }
                            }

                            // OUT: previous window type bit
                            os.Write(prev_blockflag ? 1 : 0, 1);

                            // OUT: next window type bit
                            os.Write(next_blockflag ? 1 : 0, 1);

                            // fix seek for rest of stream
                            reader.BaseStream.Position = offset + 1;
                        }

                        prev_blockflag = mode_blockflag[mode_number_p];

                        // OUT: remaining bits of first (input) byte
                        os.Write(remainder_p, 8 - mode_bits);
                    }
                    else
                    {
                        // nothing unusual for first byte
                        int v = reader.ReadByte();
                        os.Write(v, 8);
                    }

                    // remainder of packet
                    for (uint i = 1; i < size; i++)
                    {
                        int v = reader.ReadByte();
                        os.Write(v, 8);
                    }

                    offset = next_offset;
                    os.FlushPage(false, (offset == file.data_offset + file.data_size));
                }
                if (offset > file.data_offset + file.data_size) throw new Exception("page truncated");
            }
        }
    }
}
