using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CP77Tools.Model
{
    public class Archive
    {
        public ArHeader Header { get; set; }
        public uint FilesCount { get; set; }
        public ArTable Table { get; set; }

        private string filepath;


        private BinaryReader binaryReader { get; set; }

        public Archive(string path)
        {
            filepath = path;
            binaryReader = new BinaryReader(new FileStream(path, FileMode.Open));
            Header = new ArHeader(binaryReader);
            binaryReader.BaseStream.Seek((long)Header.Tableoffset, SeekOrigin.Begin);
            Table = new ArTable(binaryReader);
            FilesCount = Table.Table1count;
        }

        public byte[] GetFileData(int idx)
        {
            if (idx < Table.FileInfo.Count)
            {
                var entry = Table.FileInfo[idx];
                var startindex = (int)entry.firstDataSector;
                var nextindex = (int)entry.lastDataSector;

                using var ms = new MemoryStream();
                using var bw = new BinaryWriter(ms);
                for (int i = startindex; i < nextindex; i++)
                {
                    var offsetentry = this.Table.Offsets[i];
                    binaryReader.BaseStream.Seek((long)offsetentry.Offset, SeekOrigin.Begin);
                    var buffer = binaryReader.ReadBytes((int)offsetentry.PhysicalSize);
                    bw.Write(buffer);
                }

                return ms.ToArray();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DumpInfo()
        {
            

            // dump chache info
            using (var writer = File.CreateText($"{this.filepath}.info"))
            {
                writer.WriteLine($"Magic: {Header.Magic}\r\n");
                writer.WriteLine($"Version: {Header.Version}\r\n");
                writer.WriteLine($"Tableoffset: {Header.Tableoffset}\r\n");
                writer.WriteLine($"Tablesize: {Header.Tablesize}\r\n");
                writer.WriteLine($"Unk3: {Header.Unk3}\r\n");
                writer.WriteLine($"Filesize: {Header.Filesize}\r\n");
                writer.WriteLine($"Size: {Table.Size}\r\n");
                writer.WriteLine($"Checksum: {Table.Checksum}\r\n");
                writer.WriteLine($"Num: {Table.Num}\r\n");
                writer.WriteLine($"Table1count: {Table.Table1count}\r\n");
                writer.WriteLine($"Table2count: {Table.Table2count}\r\n");
                writer.WriteLine($"Table3count: {Table.Table3count}\r\n");

            }

            const string head = //"Hash\t" +
                                "Offset," +
                                "RamSize," +
                                "VirtualSize," +
                                "Hash," +
                                "Unknown1," +
                                "Unknown2," +
                                "somebool," +
                                "startindex," +
                                "nextindex," +
                                "startTable3Index," +
                                "nextTable3Index," +
                                "Footer,"
                                ;

            // dump and extract files
            using (var writer = File.CreateText($"{this.filepath}.csv"))
            {
                // write header
                writer.WriteLine(head);

                // write info elements
                foreach (var entry in Table.FileInfo)
                {
                    var x = entry.Value;
                    var idx = entry.Key;

                    var offsetEntry = Table.Offsets[idx];
                    //var hashEntry = Table.HashTable[idx];

                    //string ext = x.Name.Split('.').Last();

                    string info =
                        //$"{hashEntry.Hash:X2}\t +" +
                        $"{offsetEntry.Offset}," +
                        $"{offsetEntry.VirtualSize}," +
                        $"{offsetEntry.PhysicalSize}," +
                        $"{x.NameHash:X2}," +
                        $"{x.Unknown1:X2}," +
                        $"{x.Unknown2:X2}," +
                        $"{x.somebool}," +
                        $"{x.firstDataSector}," +
                        $"{x.lastDataSector}," +
                        $"{x.firstUnkIndex}," +
                        $"{x.lastUnkIndex}," +
                        $"{x.Footer:X2}";
                    
                    writer.WriteLine(info);
                }
            }
        }
    }




    public partial class ArHeader
    {
        public byte[] Magic { get; set; }
        public uint Version { get; set; }
        public ulong Tableoffset { get; set; }
        public ulong Tablesize { get; set; }
        public ulong Unk3 { get; set; }
        public ulong Filesize { get; set; }

        public ArHeader(BinaryReader br)
        {
            Read(br);
        }

        private void Read(BinaryReader br)
        {
            Magic = br.ReadBytes(4);
            if (!(Magic.SequenceEqual(new byte[] { 82, 68, 65, 82 })))
            {
                throw new NotImplementedException();
            }
            Version = br.ReadUInt32();
            Tableoffset = br.ReadUInt64();
            Tablesize = br.ReadUInt64();
            Unk3 = br.ReadUInt64();
            Filesize = br.ReadUInt64();
        }
    }
    public partial class ArTable
    {
        

        public uint Num { get; private set; }
        public uint Size { get; private set; }
        public ulong Checksum { get; private set; }
        public uint Table1count { get; private set; }
        public uint Table2count { get; private set; }
        public uint Table3count { get; private set; }
        public Dictionary<int, FileInfoEntry> FileInfo { get; private set; }
        public Dictionary<int, OffsetEntry> Offsets { get; private set; }
        public Dictionary<int, HashEntry> HashTable { get; private set; }

        public ArTable(BinaryReader br)
        {
            Read(br);

            FileInfo = new Dictionary<int, FileInfoEntry>();
            Offsets = new Dictionary<int, OffsetEntry>();
            HashTable = new Dictionary<int, HashEntry>();

            // read tables
            for (int i = 0; i < Table1count; i++)
            {
                FileInfo.Add(i, new FileInfoEntry(br));
            }

            for (int i = 0; i < Table2count; i++)
            {
                Offsets.Add(i, new OffsetEntry(br));
            }

            for (int i = 0; i < Table3count; i++)
            {
                HashTable.Add(i, new HashEntry(br));
            }
        }

        private void Read(BinaryReader br)
        {
            Num = br.ReadUInt32();
            Size = br.ReadUInt32();
            Checksum = br.ReadUInt64();
            Table1count = br.ReadUInt32();
            Table2count = br.ReadUInt32();
            Table3count = br.ReadUInt32();
        }
    }
    public partial class HashEntry
    {
        public ulong Hash { get; set; }

        public HashEntry(BinaryReader br)
        {
            Read(br);
        }

        private void Read(BinaryReader br)
        {
            Hash = br.ReadUInt64();
        }
    }
    public partial class OffsetEntry
    {

        public ulong Offset { get; set; }
        public uint PhysicalSize { get; set; }
        public uint VirtualSize { get; set; }

        public OffsetEntry(BinaryReader br)
        {
            Read(br);
        }

        private void Read(BinaryReader br)
        {
            Offset = br.ReadUInt64();
            PhysicalSize = br.ReadUInt32();
            VirtualSize = br.ReadUInt32();
        }
    }
    public partial class FileInfoEntry
    {
        public ulong NameHash { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint somebool { get; private set; }
        public uint firstDataSector { get; private set; }
        public uint lastDataSector { get; private set; }
        public uint firstUnkIndex { get; private set; }
        public uint lastUnkIndex { get; private set; }

        public byte[] Footer { get; set; }

        public FileInfoEntry(BinaryReader br)
        {
            Read(br);
        }

        private void Read(BinaryReader br)
        {
            NameHash = br.ReadUInt64();
            Unknown1 = br.ReadUInt32();
            Unknown2 = br.ReadUInt32();

            somebool = br.ReadUInt32();
            firstDataSector = br.ReadUInt32();
            lastDataSector = br.ReadUInt32();
            firstUnkIndex = br.ReadUInt32();
            lastUnkIndex = br.ReadUInt32();

            Footer = br.ReadBytes(20);
        }
    }
}



