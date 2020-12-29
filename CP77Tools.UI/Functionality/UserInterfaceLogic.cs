using CP77Tools.Tasks;
using CP77Tools.UI.Functionality.Customs;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Data;

namespace CP77Tools.UI.Functionality
{
    public class UserInterfaceLogic
    {
        private SUI sui;
        public UserInterfaceLogic(SUI _sui) { this.sui = _sui; }



        // Creates Tasks based on Taskindex.
        private void TaskManager(General.TaskType taskindex)
        {
            switch (taskindex)
            {
                case General.TaskType.Archive:
                    if (sui.archivedata.Archive_Path.Length > 0) { StartArchiveTask(); }
                    break;

                case General.TaskType.CR2W:
                    if (sui.cr2wdata.CR2W_Path.Length > 0) { StartCR2WTask(); }
                    break;

                case General.TaskType.Repack:
                    if (sui.repackdata.Repack_Path.Length > 0) { StartRepackTask(); }
                    break;

                case General.TaskType.Dump:
                    if (sui.dumpdata.Dump_Path.Length > 0) { StartDumpTask(); }
                    break;

                case General.TaskType.Hash:
                    if (sui.hashdata.Hash_Input != "") { StartHashTask(); }
                    break;

                case General.TaskType.Oodle:
                    if (sui.oodledata.Oodle_Path.Length > 0) { StartOodleTask(); }
                    break;
            }
        }

        private void StartOodleTask()
        {
            Task OTask = new Task(() => ConsoleFunctions.OodleTask(sui.oodledata.Oodle_Path[0], sui.oodledata.Oodle_OutPath, sui.oodledata.Oodle_Decompress)); // FIX THIS WHEN MULTISELECT IS POSSIBLE!
            OTask.Start(); 
            
            OTask.Wait(); sui.log.TaskFinished(General.TaskType.Oodle);
        }

        private void StartHashTask()
        {
            //Task HTask = new Task(() => ConsoleFunctions.HashTask(sui.generaldata.Hash_Input, sui.generaldata.Hash_Missing));
            //sui.generaldata.InterceptLog = true;
            //HTask.Start();
            //HTask.Wait(); sui.log.TaskFinished(General.TaskType.Hash);
        }

        private void StartDumpTask()
        {
            Task DTask = new Task(() => ConsoleFunctions.DumpTask(sui.dumpdata.Dump_Path, sui.dumpdata.Dump_Imports, sui.dumpdata.Dump_MissingHashes, sui.dumpdata.Dump_Info, sui.dumpdata.Dump_ClassInfo));// FIX THIS WHEN MULTISELECT IS POSSIBLE!
            DTask.Start(); 
            
            DTask.Wait(); sui.log.TaskFinished(General.TaskType.Dump);
        }

        private void StartRepackTask()
        {

            Task RTask = new Task(() => ConsoleFunctions.PackTask(sui.repackdata.Repack_Path, sui.repackdata.Repack_OutPath));  // FIX THIS TOO 
            RTask.Start(); 
            
            RTask.Wait(); sui.log.TaskFinished(General.TaskType.Repack);
        }

        private void StartCR2WTask()
        {
            Task CTask = new Task(() => ConsoleFunctions.Cr2wTask(sui.cr2wdata.CR2W_Path, sui.cr2wdata.CR2W_OutPath, sui.cr2wdata.CR2W_All, sui.cr2wdata.CR2W_Chunks)); // FIX THIS WHEN MULTISELECT IS POSSIBLE!
            CTask.Start(); 
            
            CTask.Wait(); sui.log.TaskFinished(General.TaskType.CR2W);
        }

        private void StartArchiveTask()
        {
            Task ATask = new Task(() => ConsoleFunctions.ArchiveTask(sui.archivedata.Archive_Path, sui.archivedata.Archive_OutPath, sui.archivedata.Archive_Extract, sui.archivedata.Archive_Dump, sui.archivedata.Archive_List, sui.archivedata.Archive_Uncook, sui.archivedata.Archive_UncookFileType, sui.archivedata.Archive_Hash, sui.archivedata.Archive_Pattern, sui.archivedata.Archive_Regex));
            ATask.Start();
            sui.archivedata.resetarchivedata();
            ATask.Wait();                               // sui.log.TaskFinished(General.TaskType.Archive);

        }





        // Creates Thread and sends TaskIndicator to taskmanager to run task on thread.
        public void ThreadedTaskSender(General.TaskType item) { Thread worker = new Thread(() => TaskManager(item)); worker.IsBackground = true; worker.Start(); }

   

       



        // TooltipsSetter
        public void SetToolTips()
        {
            //Archive
        //    sui.Archive_SelectArchive_UIElement_Button.ToolTip = sui.generaldata.ToolTipArchive_Path;
       //     sui.Archive_SelectOutputPath_UIElement_Button.ToolTip = sui.generaldata.ToolTipArchive_OutputPath;
      //      sui.Archive_Dump_UIElement_Checkbox.ToolTip = sui.generaldata.ToolTipArchive_Dump;
      //      sui.Archive_Extract_UIElement_Checkbox.ToolTip = sui.generaldata.ToolTipArchive_Extract;
      //      sui.Archive_List_UIElement_Checkbox.ToolTip = sui.generaldata.ToolTipArchive_List;
      //      sui.Archive_Uncook_UIElement_Checkbox.ToolTip = sui.generaldata.ToolTipArchive_Uncook;
     //       sui.Archive_Hash_UIElement_TextBox.ToolTip = sui.generaldata.ToolTipArchive_Hash;
      //      sui.Archive_Start_UIElement_Button.ToolTip = sui.generaldata.ToolTipArchive;
            //Dump
       //     sui.Dump_PathIndicatorSelected_UIElement_TextBlock.ToolTip = sui.generaldata.ToolTipDump_Path;
       //     sui.Dump_Imports_UIElement_CheckBox.ToolTip = sui.generaldata.ToolTipDump_Imports;
       //     sui.Dump_MissingHashes_UIElement_CheckBox.ToolTip = sui.generaldata.ToolTipDump_MissingHashes;
       //     sui.Dump_Info_UIElement_CheckBox.ToolTip = sui.generaldata.ToolTipDump_Info;
            //CR2W
            //Hash
            //Oodle
        }
    }
}
