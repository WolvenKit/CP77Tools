using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP77Tools.UI.Data.Tasks
{
    public class DumpData
    {

        public enum DumpTaskType { Imports, MissingHashes, Info, ClassInfo, Custom }

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
    }
}
