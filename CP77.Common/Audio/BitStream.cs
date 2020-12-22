using System;
using System.Collections.Generic;
using System.Text;

namespace CP77.Common.Audio
{
    /// <summary>
    /// Bitstream allows a reader to read individual bits
    /// Currently only used for audio reader
    /// </summary>
    public class BitStream
    {
        private EndianAwareBinaryReader reader;
        private byte bit_buffer;
        private int bits_left;
        private ulong total_bits_read;

        public BitStream(EndianAwareBinaryReader reader)
        {
            this.reader = reader;
            bit_buffer = 0;
            bits_left = 0;
            total_bits_read = 0;
        }

        public bool GetBit()
        {
            if (bits_left == 0)
            {
                bit_buffer = reader.ReadByte();
                bits_left = 8;
            }
            total_bits_read++;
            bits_left--;
            return (bit_buffer & (0x80 >> bits_left)) != 0;
        }

        public ulong GetTotalBitsRead()
        {
            return total_bits_read;
        }

        public uint GetBits(int count)
        {
            uint total = 0;
            for (int i = 0; i < count; i++)
            {
                bool val = GetBit();

                if (val)
                {
                    total |= (1U << i);
                }
            }
            return total;
        }
    }
}
