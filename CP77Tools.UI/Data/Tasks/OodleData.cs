using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP77Tools.UI.Data.Tasks
{
    public class OodleData
    {
        public enum OodleTaskType { Decompress, Custom }

        //Oodle
        public string ToolTipOodle = "Some helper functions related to oodle compression.";
        public string ToolTipOodle_Path = "";
        public string ToolTipOodle_Decompress = "";

        public string[] Oodle_Path;
        public string Oodle_OutPath;
        public bool Oodle_Decompress = false;
    }
}
