using CP77Tools.UI.Data.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP77Tools.UI.Functionality.ToolsWindow
{
    public class ArchiveFunc
    {

        public static void LoadCollectionData()
        {
            ArchiveData.ArchiveConceptTaskDict.Clear();
            ArchiveData.ArchiveConceptTaskDict.Add("Selected Outpath", "None");
            ArchiveData.ArchiveConceptTaskDict.Add("Extract", "Disabled");
            ArchiveData.ArchiveConceptTaskDict.Add("List", "Disabled");
            ArchiveData.ArchiveConceptTaskDict.Add("Dump", "Disabled");
            ArchiveData.ArchiveConceptTaskDict.Add("Uncook", "Disabled");
            ArchiveData.ArchiveConceptTaskDict.Add("Uncook File Extension", "Default (TGA)");
            ArchiveData.ArchiveConceptTaskDict.Add("Pattern", "None");
            ArchiveData.ArchiveConceptTaskDict.Add("Regex", "None");
        }
    }
}
