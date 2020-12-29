using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP77Tools.UI.Data.Tasks
{
    public class CR2WData
    {
        public enum CR2WTaskType { All, Chunks, Custom }
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

        internal void resetcr2wdata()
        {
            CR2W_Path = null;
            CR2W_OutPath = "";
            CR2W_All = false;
            CR2W_Chunks = false;
        }
    }
}
