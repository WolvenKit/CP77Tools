using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CP77.Common.Audio
{
    /// <summary>
    /// Endian Aware Binary Reader takes Endian data into account
    /// It can read both Little and Big Endian and swap on the fly
    /// </summary>
    public class EndianAwareBinaryReader : BinaryReader
    {
        public EndianAwareBinaryReader(Stream stream) : base(stream)
        {
            IsLittleEndian = BitConverter.IsLittleEndian;
        }

        private Func<ushort> read_uint16;
        private Func<uint> read_uint32;
        private bool little_endian;

        private byte[] ReadExactBytes(int count)
        {
            var data = base.ReadBytes(count);

            if (data.Length != count)
            {
                throw new EndOfStreamException("Unexpected end of stream");
            }

            return data;
        }

        public bool IsLittleEndian
        {
            get
            {
                return little_endian;
            }
            set
            {
                little_endian = value;
                if (little_endian)
                {
                    read_uint16 = ReadUInt16LE;
                    read_uint32 = ReadUInt32LE;
                }
                else
                {
                    read_uint16 = ReadUInt16BE;
                    read_uint32 = ReadUInt32BE;
                }
            }
        }

        public override ushort ReadUInt16()
        {
            return read_uint16();
        }

        public override uint ReadUInt32()
        {
            return read_uint32();
        }

        public uint ReadUInt32LE()
        {
            byte[] b = ReadExactBytes(4);

            uint v = 0;
            for (int i = 3; i >= 0; i--)
            {
                v <<= 8;
                v |= b[i];
            }

            return v;
        }

        public ushort ReadUInt16LE()
        {
            byte[] b = ReadExactBytes(2);

            ushort v = 0;
            for (int i = 1; i >= 0; i--)
            {
                v <<= 8;
                v |= b[i];
            }

            return v;
        }

        public uint ReadUInt32BE()
        {
            byte[] b = ReadExactBytes(4);

            uint v = 0;
            for (int i = 0; i < 4; i++)
            {
                v <<= 8;
                v |= b[i];
            }

            return v;
        }

        public ushort ReadUInt16BE()
        {
            byte[] b = ReadExactBytes(2);

            ushort v = 0;
            for (int i = 0; i < 2; i++)
            {
                v <<= 8;
                v |= b[i];
            }

            return v;
        }
    }
}
