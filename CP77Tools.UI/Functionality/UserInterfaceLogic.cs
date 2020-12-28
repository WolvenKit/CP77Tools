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
        private void TaskManager(int taskindex)
        {
            switch (taskindex)
            {
                case 0:
                //    if (sui.generaldata.Archive_Path.Length > 0)
                 //   {
                   //     Task ATask = new Task(() => ConsoleFunctions.ArchiveTask(sui.generaldata.Archive_Path, sui.generaldata.Archive_OutPath, sui.generaldata.Archive_Extract, sui.generaldata.Archive_Dump, sui.generaldata.Archive_List, sui.generaldata.Archive_Uncook, sui.generaldata.Archive_UncookFileType, sui.generaldata.Archive_Hash, sui.generaldata.Archive_Pattern, sui.generaldata.Archive_Regex));
                 //       ATask.Start(); ATask.Wait(); sui.log.TaskFinished(MainWindow.TaskType.Archive);
                  //  }
               //     break;

                case 1:
                    if (sui.generaldata.CR2W_Path.Length > 0)
                    {
                        Task CTask = new Task(() => ConsoleFunctions.Cr2wTask(sui.generaldata.CR2W_Path, sui.generaldata.CR2W_OutPath, sui.generaldata.CR2W_All, sui.generaldata.CR2W_Chunks)); // FIX THIS WHEN MULTISELECT IS POSSIBLE!
                        CTask.Start(); CTask.Wait(); sui.log.TaskFinished(General.TaskType.CR2W);
                    }
                    break;

                case 2:
                    if (sui.generaldata.Repack_Path.Length > 0)
                    {
                        Task RTask = new Task(() => ConsoleFunctions.PackTask(sui.generaldata.Repack_Path, sui.generaldata.Repack_OutPath));  // FIX THIS TOO 
                        RTask.Start(); RTask.Wait(); sui.log.TaskFinished(General.TaskType.Repack);
                    }
                    break;

                case 3:
                    if (sui.generaldata.Dump_Path.Length > 0)
                    {
                        Task DTask = new Task(() => ConsoleFunctions.DumpTask(sui.generaldata.Dump_Path, sui.generaldata.Dump_Imports, sui.generaldata.Dump_MissingHashes, sui.generaldata.Dump_Info, sui.generaldata.Dump_ClassInfo));// FIX THIS WHEN MULTISELECT IS POSSIBLE!
                        DTask.Start(); DTask.Wait(); sui.log.TaskFinished(General.TaskType.Dump);
                    }
                    break;

                case 4:
                    if (sui.generaldata.Hash_Input != "")
                    {
                        //Task HTask = new Task(() => ConsoleFunctions.HashTask(sui.generaldata.Hash_Input, sui.generaldata.Hash_Missing));
                        //sui.generaldata.InterceptLog = true;
                        //HTask.Start();
                        //HTask.Wait(); sui.log.TaskFinished(General.TaskType.Hash);
                    }
                    break;

                case 5:
                    if (sui.generaldata.Oodle_Path.Length > 0)
                    {
                        Task OTask = new Task(() => ConsoleFunctions.OodleTask(sui.generaldata.Oodle_Path[0], sui.generaldata.Oodle_OutPath, sui.generaldata.Oodle_Decompress)); // FIX THIS WHEN MULTISELECT IS POSSIBLE!
                        OTask.Start(); OTask.Wait(); sui.log.TaskFinished(General.TaskType.Oodle);
                    }
                    break;
            }
        }





        // Creates Thread and sends TaskIndicator to taskmanager to run task on thread.
        public void ThreadedTaskSender(int item) { Thread worker = new Thread(() => TaskManager(item)); worker.IsBackground = true; worker.Start(); }

   

       

        // Open Folder Dialog, change UI based on typeindicator.
        public void OpenFolder(int TypeIndicator)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog(); dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                switch (TypeIndicator)
                {
                    case 0:
                  //      sui.Archive_PathIndicator_Output_UIElement_TextBlock.Text = dialog.FileName.ReverseTruncate(34);
                     //   sui.generaldata.Archive_OutPath = dialog.FileName;
                        break;
                    case 1: // CR2W
                   //     sui.CR2W_PathIndicator_UIElement_TextBlock.Text = dialog.FileName.ReverseTruncate(34);
                        sui.generaldata.CR2W_OutPath = dialog.FileName;
                        break;
                    case 2: // REPACK
                //        sui.Repack_PathIndicatorOutput_UIElement_TextBlock.Text = dialog.FileName.ReverseTruncate(34);
                        sui.generaldata.Repack_OutPath = dialog.FileName;
                        break;
                    case 3: // Dump
                //        sui.Dump_PathIndicatorOutput_UIElement_TextBlock.Text = dialog.FileName.ReverseTruncate(34);
                        sui.generaldata.Dump_OutPath = dialog.FileName;
                        break;
                    case 4: // Hash
                        break;
                    case 5: // Oodle
                   //     sui.Oodle_PathOut_UIElement_TextBlock.Text = dialog.FileName.ReverseTruncate(34);
                        sui.generaldata.Oodle_OutPath = dialog.FileName;
                        break;



                }
            }
        }



        public void OpenF(Data.General.OMD_Type oMD_Type, General.TaskType taskType)
        {
            switch (taskType)
            {
                case General.TaskType.Archive:
            //        sui.Archive_SelectedFilesDropDown_UIElement_ComboBox.ItemsSource = null;

                    break;
                case General.TaskType.CR2W:
              //      sui.CR2W_SelectedDropdown_UIElement_ComboBox.ItemsSource = null;

                    break;
                case General.TaskType.Dump:
              //      sui.Dump_SelectedDropdown_UIElement_ComboBox.ItemsSource = null;

                    break;
                case General.TaskType.Hash:
                    break;
                case General.TaskType.Oodle:
             //       sui.Oodle_SelectedDropdown_UIElement_ComboBox.ItemsSource = null;

                    break;
                case General.TaskType.Repack:
                 //   sui.Repack_SelectedDropdown_UIElement_ComboBox.ItemsSource = null;

                    break;
            }
        //    OpenMultiDialog OMD_Selector = new Functionality.Customs.OpenMultiDialog(app, taskType, oMD_Type);
           // OMD_Selector.Show();
        }

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
