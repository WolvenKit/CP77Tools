using CP77Tools.Tasks;
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
        private MainWindow app;
        public UI(MainWindow mainWindow) { this.app = mainWindow; }



        // Creates Tasks based on Taskindex.
        private void TaskManager(int taskindex)
        {
            switch (taskindex)
            {
                case 0:
                    if (app.data.Archive_Path != "" && app.data.Archive_OutPath != "")
                    {
                        Task task = new Task(() => ConsoleFunctions.ArchiveTask(app.data.Archive_Path, app.data.Archive_OutPath, app.data.Archive_Extract, app.data.Archive_Dump, app.data.Archive_List, app.data.Archive_Uncook, app.data.Archive_UncookFileType, app.data.Archive_Hash, app.data.Archive_Pattern, app.data.Archive_Regex));
                        task.Start(); task.Wait(); app.log.TaskFinished(MainWindow.TaskType.Archive);
                    }
                    break;

                case 1: if (app.data.Dump_Path != "" && app.data.Dump_OutPath != "") { } break;
                //  Thread worker = new Thread(() => TaskManager(1));
                // worker.IsBackground = true;
                // worker.Start();
                // ConsoleFunctions.DumpTask(Dump_Path, Dump_OutPath, Dump_Imports, Dump_MissingHashes, Dump_Info);
                case 2: if (app.data.CR2W_Path != "" && app.data.CR2W_OutPath != "") { } break;
                // Thread worker = new Thread(() => TaskManager(2));
                // worker.IsBackground = true;
                // worker.Start();
                // ConsoleFunctions.Cr2wTask(CR2W_Path, CR2W_OutPath, CR2W_All, CR2W_Chunks);

                case 3: ConsoleFunctions.HashTask(app.data.Hash_Input, app.data.Hash_Missing); break;

                case 4: break;
                    // if (CR2W_Path != "" && CR2W_OutPath != "") { ConsoleFunctions.OodleTask(Oodle_Path, Oodle_OutPath, Oodle_Decompress); } 
            }
        }



        // Creates Thread and sends TaskIndicator to taskmanager to run task on thread.
        public void ThreadedTaskSender(int item) { Thread worker = new Thread(() => TaskManager(item)); worker.IsBackground = true; worker.Start(); }

        // Open file dialog with filter based on typeindicator.
        public void OpenFile(int TypeIndicator)
        {
            string FileFilter = app.InputFileTypes[TypeIndicator];
            OpenFileDialog openFileDialog = new OpenFileDialog(); openFileDialog.Multiselect = false; openFileDialog.Filter = FileFilter;
            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                switch (TypeIndicator)
                {
                    case 0: app.UIElement_Archive_PathIndicator_Selected.Text = openFileDialog.SafeFileName; app.data.Archive_Path = openFileDialog.FileName; break;
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
                if (CurrentPageIndex >= app.Main_ItemList_UIElement_ListBox.Items.Count - 1) { CurrentPageIndex = app.Main_ItemList_UIElement_ListBox.Items.Count - 1; }
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
                        app.UIElement_Archive_PathIndicator_Output.Text = dialog.FileName.ReverseTruncate(34); app.data.Archive_OutPath = dialog.FileName;
                        break;


                }
            }
        }

        // TooltipsSetter
        public void SetToolTips()
        {
            //Archive
            app.UIElement_Button_ArchiveSelectArchive.ToolTip = app.data.ToolTipArchive_Path;
            app.UIElement_Button_ArchiveSelectOutputPath.ToolTip = app.data.ToolTipArchive_OutputPath;
            app.UIElement_Checkbox_ArchiveDump.ToolTip = app.data.ToolTipArchive_Dump;
            app.UIElement_Checkbox_ArchiveExtract.ToolTip = app.data.ToolTipArchive_Extract;
            app.UIElement_Checkbox_ArchiveList.ToolTip = app.data.ToolTipArchive_List;
            app.UIElement_Checkbox_ArchiveUncook.ToolTip = app.data.ToolTipArchive_Uncook;
            app.UIElement_TextBox_ArchiveHash.ToolTip = app.data.ToolTipArchive_Hash;
            app.UIElement_Button_ArchiveStart.ToolTip = app.data.ToolTipArchive;
            //Dump
            app.UIElement_Dump_PathIndicator_Selected.ToolTip = app.data.ToolTipDump_Path;
            app.UIElement_Checkbox_DumpImports.ToolTip = app.data.ToolTipDump_Imports;
            app.UIElement_Checkbox_DumpMissingHashes.ToolTip = app.data.ToolTipDump_MissingHashes;
            app.UIElement_Checkbox_DumpInfo.ToolTip = app.data.ToolTipDump_Info;
            //CR2W
            //Hash
            //Oodle
        }










    }
}
