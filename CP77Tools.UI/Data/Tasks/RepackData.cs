using CP77Tools.UI.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP77Tools.UI.Data.Tasks
{
    public class RepackData
    {
        public enum RepackTaskType { Repack, Custom }

        //Repack
        public string ToolTipRepack = "Pack a folder of files into an .archive file.";
        public string ToolTipRepack_Path = "Input path. Can be a path to one .archive, or the content directory.\nIf this is a directory, all archives in it will be processed.";
        public string ToolTipRepack_OutPath = "Output directory to extract files to.\nIf not specified, will output to a new child directory, in place.";

        public string[] Repack_Path;
        //repack
        public string Repack_OutPath;

        internal void resetrepackdata()
        {
            Repack_Path = null;
            //repack
            Repack_OutPath = null;
    }
    }
}
