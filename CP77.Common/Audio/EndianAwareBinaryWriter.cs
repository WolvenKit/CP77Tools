using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CP77.Common.Audio
{
    /// <summary>
    /// Endian Aware Binary Writer takes Endian data into account
    /// It can read both Little and Big Endian and swap on the fly
    /// </summary>
    class EndianAwareBinaryWriter : BinaryWriter
    {
        public EndianAwareBinaryWriter(Stream stream) : base(stream)
        {
            IsLittleEndian = BitConverter.IsLittleEndian;
        }

        private Action<ushort> write_uint16;
        private Action<uint> write_uint32;
        private bool little_endian;

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
                    write_uint16 = WriteUInt16LE;
                    write_uint32 = WriteUInt32LE;
                }
                else
                {
                    write_uint16 = WriteUInt16BE;
                    write_uint32 = WriteUInt32BE;
                }
            }
        }

        public override void Write(uint value)
        {
            write_uint32(value);
        }

        public override void Write(ushort value)
        {
            write_uint16(value);
        }

        public void WriteUInt32LE(uint value)
        {
            Span<byte> b = stackalloc byte[4];

            for (int i = 0; i < 4; i++)
            {
                b[i] = (byte)(value & 0xFF);
                value >>= 8;
            }

            base.Write(b);
        }

        public void WriteUInt16LE(ushort value)
        {
            Span<byte> b = stackalloc byte[2];

            for (int i = 0; i < 2; i++)
            {
                b[i] = (byte)(value & 0xFF);
                value >>= 8;
            }

            base.Write(b);
        }

        public void WriteUInt32BE(uint value)
        {
            Span<byte> b = stackalloc byte[4];

            for (int i = 3; i >= 0; i--)
            {
                b[i] = (byte)(value & 0xFF);
                value >>= 8;
            }

            base.Write(b);
        }

        public void WriteUInt16BE(ushort value)
        {
            Span<byte> b = stackalloc byte[2];

            for (int i = 1; i >= 0; i--)
            {
                b[i] = (byte)(value & 0xFF);
                value >>= 8;
            }

            base.Write(b);
        }
    }
}
