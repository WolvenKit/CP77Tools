using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CP77Tools.UI.Functionality
{
    public class UI
    {
        private MainWindow App_UI;
        public UI(MainWindow mainWindow)        {            this.App_UI = mainWindow;        }



        private void TaskManager(int taskindex)
        {
            switch (taskindex)
            {
                case 0:
                    if (App_UI._General.Archive_Path != "" && App_UI._General.Archive_OutPath != "")
                    {
                        Task task = new Task(() => ConsoleFunctions.ArchiveTask(App_UI._General.Archive_Path, App_UI._General.Archive_OutPath, App_UI._General.Archive_Extract, App_UI._General.Archive_Dump, App_UI._General.Archive_List, App_UI._General.Archive_Uncook, App_UI._General.Archive_UncookFileType, App_UI._General.Archive_Hash, App_UI._General.Archive_Pattern, App_UI._General.Archive_Regex));
                        task.Start();
                        task.Wait();
                        App_UI._Logging.TaskFinished(MainWindow.TaskType.Archive);
                    }
                    break;

                case 1:
                    if (App_UI._General.Dump_Path != "" && App_UI._General.Dump_OutPath != "")
                    {
                        //  Thread worker = new Thread(() => TaskManager(1));
                        // worker.IsBackground = true;
                        // worker.Start();
                        // ConsoleFunctions.DumpTask(Dump_Path, Dump_OutPath, Dump_Imports, Dump_MissingHashes, Dump_Info);
                    }
                    break;
                case 2:
                    if (App_UI._General.CR2W_Path != "" && App_UI._General.CR2W_OutPath != "")
                    {
                        // Thread worker = new Thread(() => TaskManager(2));
                        // worker.IsBackground = true;
                        // worker.Start();
                        // ConsoleFunctions.Cr2wTask(CR2W_Path, CR2W_OutPath, CR2W_All, CR2W_Chunks);
                    }
                    break;

                case 3:
                    ConsoleFunctions.HashTask(App_UI._General.Hash_Input, App_UI._General.Hash_Missing);
                    break;

                case 4:
                    // if (CR2W_Path != "" && CR2W_OutPath != "") { ConsoleFunctions.OodleTask(Oodle_Path, Oodle_OutPath, Oodle_Decompress); }
                    break;
            }

        }


        public void ThreadedTaskSender(int item)
        {
            Thread worker = new Thread(() => TaskManager(item));
            worker.IsBackground = true;
            worker.Start();
        }

        // Open file dialog with filter based on typeindicator.
        public void OpenFile(int TypeIndicator)
        {
            string FileFilter = App_UI.InputFileTypes[TypeIndicator];
            OpenFileDialog openFileDialog = new OpenFileDialog(); openFileDialog.Multiselect = false; openFileDialog.Filter = FileFilter;
            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                switch (TypeIndicator)
                {
                    case 0: App_UI.UIElement_Archive_PathIndicator_Selected.Text = openFileDialog.SafeFileName; App_UI._General.Archive_Path = openFileDialog.FileName; break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                }
            }
        }

        // Items Page Handler
        public int CurrentPageIndex = 3;
        public int Pagehandler(bool plus)
        {
            if (CurrentPageIndex <= 0) { CurrentPageIndex = 3; }
            if (plus)
            {
                CurrentPageIndex += 1;
                if (CurrentPageIndex >= 0 && CurrentPageIndex <= 3) { CurrentPageIndex = 3; }
                if (CurrentPageIndex >= 4 && CurrentPageIndex <= 7) { CurrentPageIndex = 7; }
                if (CurrentPageIndex >= 8 && CurrentPageIndex <= 11) { CurrentPageIndex = 11; }
                if (CurrentPageIndex >= App_UI.UIElement_ItemList.Items.Count - 1) { CurrentPageIndex = App_UI.UIElement_ItemList.Items.Count - 1; }
                return CurrentPageIndex;
            }
            if (!plus)
            {
                CurrentPageIndex -= 1;
                if (CurrentPageIndex >= 0 && CurrentPageIndex <= 2) { CurrentPageIndex = 0; }
                if (CurrentPageIndex >= 3 && CurrentPageIndex <= 6) { CurrentPageIndex = 0; }
                if (CurrentPageIndex >= 7 && CurrentPageIndex <= 10 || CurrentPageIndex == 9) { CurrentPageIndex = 4; }
                return CurrentPageIndex;
            }
            else { return 3; }
        }

        // Open Folder Dialog, change UI based on typeindicator.
        public void OpenFolder(int TypeIndicator)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog(); dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                switch (TypeIndicator)
                {
                    case 0:
                        App_UI.UIElement_Archive_PathIndicator_Output.Text = dialog.FileName.ReverseTruncate(34); App_UI._General.Archive_OutPath = dialog.FileName;
                        break;


                }
            }
        }

        // TooltipsSetter
        public void SetToolTips()
        {
            //Archive
            App_UI.UIElement_Button_ArchiveSelectArchive.ToolTip = App_UI._General.ToolTipArchive_Path; App_UI.UIElement_Button_ArchiveSelectOutputPath.ToolTip = App_UI._General.ToolTipArchive_OutputPath;
            App_UI.UIElement_Checkbox_ArchiveDump.ToolTip = App_UI._General.ToolTipArchive_Dump; App_UI.UIElement_Checkbox_ArchiveExtract.ToolTip = App_UI._General.ToolTipArchive_Extract;
            App_UI.UIElement_Checkbox_ArchiveList.ToolTip = App_UI._General.ToolTipArchive_List; App_UI.UIElement_Checkbox_ArchiveUncook.ToolTip = App_UI._General.ToolTipArchive_Uncook;
            App_UI.UIElement_TextBox_ArchiveHash.ToolTip = App_UI._General.ToolTipArchive_Hash; App_UI.UIElement_Button_ArchiveStart.ToolTip = App_UI._General.ToolTipArchive;
            //Dump
            App_UI.UIElement_Dump_PathIndicator_Selected.ToolTip = App_UI._General.ToolTipDump_Path; App_UI.UIElement_Checkbox_DumpImports.ToolTip = App_UI._General.ToolTipDump_Imports;
            App_UI.UIElement_Checkbox_DumpMissingHashes.ToolTip = App_UI._General.ToolTipDump_MissingHashes; App_UI.UIElement_Checkbox_DumpInfo.ToolTip = App_UI._General.ToolTipDump_Info;
            //CR2W
            //Hash
            //Oodle
        }










    }
}
