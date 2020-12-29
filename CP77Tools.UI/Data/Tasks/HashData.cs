using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP77Tools.UI.Data.Tasks
{
    public class HashData
    {
        public enum HashTaskType { Convert, Missing, Custom }

        //Hash
        public string ToolTipHash = "Some helper functions related to hashes.";
        public string ToolTipHash_Input = "Create FNV1A hash of given string";
        public string ToolTipHash_OutputPath = "Output directory";
        public string ToolTipHash_Missing = "";

        public string Hash_Input = "";
        public string Hash_Output = "";
        public bool Hash_Missing = false;
    }
}
