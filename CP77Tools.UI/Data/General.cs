using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolvenKit.Common.Tools.DDS;

namespace CP77Tools.UI.Data
{
    public class General
    {

        public enum OMD_Type        {            Single,            Multi        }

        public enum ArchiveTaskType { Extract,Dump,Uncook,List,Single,Custom}
        public enum DumpTaskType {Imports,MissingHashes,Info,ClassInfo,Custom }
        public enum CR2WTaskType { All,Chunks,Custom}
        public enum HashTaskType { Convert,Missing,Custom}
        public enum OodleTaskType { Decompress, Custom}
        public enum RepackTaskType {Repack, Custom}
        public enum TaskType { Archive, Dump, CR2W, Hash, Oodle, Repack, }


        public bool InterceptLog = false;

        //Archive 
        public string ToolTipArchive = "Target an archive to extract files or dump information.";
        public string ToolTipArchive_Path = "Input path to .archive.";
        public string ToolTipArchive_OutputPath = "Output directory to extract files to.";
        public string ToolTipArchive_Extract = "Extract files from archive.";
        public string ToolTipArchive_Dump = "Dump archive information.";
        public string ToolTipArchive_List = "List contents of archive.";
        public string ToolTipArchive_Uncook = "Uncooks textures from archive.";
        public string ToolTipArchive_Uext = "Uncook extension (tga, bmp, jpg, png, dds). Default is tga.";
        public string ToolTipArchive_Hash = "Extract single file with given hash.";
        public string ToolTipArchive_Pattern = "Use optional search pattern, e.g. *.ink.\nIf both regex and pattern is defined, pattern will be used first.";
        public string ToolTipArchive_Regex = "Use optional regex pattern.\nIf both regex and pattern is defined, pattern will be used first.";

        public string[] Archive_Path = new string[0];
        public string Archive_OutPath = "";
        public bool Archive_Extract = false;
        public bool Archive_Dump = false;
        public bool Archive_List = false;
        public bool Archive_Uncook = false;
        public EUncookExtension Archive_UncookFileType = EUncookExtension.tga;
        public ulong Archive_Hash = 0;
        public string Archive_Pattern = "";
        public string Archive_Regex = "";

        //Dump
        public string ToolTipDump = "Target an archive or a directory to dump archive information.";
        public string ToolTipDump_Path = "Input path to .archive or to a directory (runs over all archives in directory).";
        public string ToolTipDump_OutputPath = "Output directory";
        public string ToolTipDump_Imports = "Dump all imports (all filenames that are referenced by all files in the archive).";
        public string ToolTipDump_MissingHashes = "List all missing hashes of all input archives.";
        public string ToolTipDump_Info = "Dump all xbm info.";
        public string ToolTipDump_ClassInfo = "Dump all class info.";

        public string[] Dump_Path;
        public string Dump_OutPath = "";
        public bool Dump_Imports = false;
        public bool Dump_MissingHashes = false;
        public bool Dump_Info = false;
        public bool Dump_ClassInfo = false;

        //CR2W
        public string ToolTipCR2W = "Target a specific cr2w (extracted) file and dumps file information.";
        public string ToolTipCR2W_Path = "Input path to a cr2w file.";
        public string ToolTipCR2W_OutputPath = "Output directory";
        public string ToolTipCR2W_All = "Dump all information.";
        public string ToolTipCR2W_Chunks = "Dump all class information of file.";

        public string[] CR2W_Path;
        public string CR2W_OutPath = "";
        public bool CR2W_All = false;
        public bool CR2W_Chunks = false;

        //Hash
        public string ToolTipHash = "Some helper functions related to hashes.";
        public string ToolTipHash_Input = "Create FNV1A hash of given string";
        public string ToolTipHash_OutputPath = "Output directory";
        public string ToolTipHash_Missing = "";

        public string Hash_Input = "";
        public string Hash_Output = "";
        public bool Hash_Missing = false;

        //Oodle
        public string ToolTipOodle = "Some helper functions related to oodle compression.";
        public string ToolTipOodle_Path = "";
        public string ToolTipOodle_Decompress = "";

        public string[] Oodle_Path;
        public string Oodle_OutPath;
        public bool Oodle_Decompress = false;


        //Repack
        public string ToolTipRepack = "Pack a folder of files into an .archive file.";
        public string ToolTipRepack_Path = "Input path. Can be a path to one .archive, or the content directory.\nIf this is a directory, all archives in it will be processed.";
        public string ToolTipRepack_OutPath = "Output directory to extract files to.\nIf not specified, will output to a new child directory, in place.";

        public string[] Repack_Path;


        // OTHER
        public string[] OMD_Output;


        //repack
        public string Repack_OutPath;
    }
}
