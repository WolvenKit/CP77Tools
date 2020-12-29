using System;
using System.Collections.Generic;
using WolvenKit.Common.Tools.DDS;
using static CP77Tools.UI.Data.General;

namespace CP77Tools.UI.Data.Tasks
{
    public class ArchiveData
    {
        public enum TaskType { Extract, Dump, Uncook, List, Single, Custom }
        public static Dictionary<string, string> ArchiveConceptTaskDict = new Dictionary<string, string>();
        public static List<Preset> presets = new List<Preset>();
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




        public void resetarchivedata()
        {
            Archive_Path = new string[0];
             Archive_OutPath = "";
             Archive_Extract = false;
             Archive_Dump = false;
             Archive_List = false;
             Archive_Uncook = false;
             Archive_UncookFileType = EUncookExtension.tga;
             Archive_Hash = 0;
             Archive_Pattern = "";
             Archive_Regex = "";
        }
    
    }
}
