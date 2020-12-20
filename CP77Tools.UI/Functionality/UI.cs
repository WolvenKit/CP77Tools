using CP77Tools.Tasks;
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
                    if (app.data.Archive_Path.Length > 0)
                    {
                        Task task = new Task(() => ConsoleFunctions.ArchiveTask(app.data.Archive_Path, app.data.Archive_OutPath, app.data.Archive_Extract, app.data.Archive_Dump, app.data.Archive_List, app.data.Archive_Uncook, app.data.Archive_UncookFileType, app.data.Archive_Hash, app.data.Archive_Pattern, app.data.Archive_Regex));
                        task.Start(); task.Wait(); app.log.TaskFinished(MainWindow.TaskType.Archive);
                    }
                    break;

                case 1:
                    if (app.data.CR2W_Path != "")
                    {
                        Task task = new Task(() => ConsoleFunctions.Cr2wTask(app.data.CR2W_Path,app.data.CR2W_OutPath,app.data.CR2W_All,app.data.CR2W_Chunks));
                        task.Start(); task.Wait(); app.log.TaskFinished(MainWindow.TaskType.Archive);
                    }
                    break;
 
                case 2: if (app.data.CR2W_Path != "" && app.data.CR2W_OutPath != "") { } break;


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
                    case 0:
                        app.UIElement_Archive_PathIndicator_Selected.Text = openFileDialog.SafeFileName;
                        Array.Resize(ref app.data.Archive_Path, app.data.Archive_Path.Length + 1); app.data.Archive_Path[app.data.Archive_Path.Length - 1] = openFileDialog.FileName;
                        break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                }
            }
        }





        public void NextItemsInView()
        {
            app.Main_NextItems_UIElement_Button.Opacity = 0.6;
            app.Main_ItemList_UIElement_ListBox.ScrollIntoView(app.Main_ItemList_UIElement_ListBox.Items[app.ui.Pagehandler(true)]);
        }
        public void PreviousItemsInView()
        {
            app.Main_PreviousItems_UIElement_Button.Opacity = 0.6;
            if (app.ui.CurrentPageIndex > 0)
            {
                app.Main_ItemList_UIElement_ListBox.ScrollIntoView(app.Main_ItemList_UIElement_ListBox.Items[app.ui.Pagehandler(false)]);
            }
        }
        public void GoToFirstItemInView()
        {
            app.Main_ItemList_UIElement_ListBox.ScrollIntoView(app.Main_ItemList_UIElement_ListBox.Items[0]);
        }


        // Items Page Handler  // If you can do the repeating lines in a simpler way let me know

        public int CurrentPageIndex = 0;
        public int Pagehandler(bool plus)
        {
            var EnabledCount = 0;
            var lowest = 0;
            foreach (Control item in app.Main_ItemList_UIElement_ListBox.Items) { if (item.IsEnabled) { EnabledCount++; } }
            if (plus)
            {
                if (CurrentPageIndex <= EnabledCount)
                {
                    CurrentPageIndex += 2;
                    if (CurrentPageIndex == 0) { CurrentPageIndex = 1; }
                    if (CurrentPageIndex == 1) { CurrentPageIndex = 1; }
                    if (CurrentPageIndex == 2) { CurrentPageIndex = 3; }
                    if (CurrentPageIndex == 3) { CurrentPageIndex = 3; }
                    if (CurrentPageIndex == 4) { CurrentPageIndex = 5; }
                    if (CurrentPageIndex == 5) { CurrentPageIndex = 5; }
                    if (CurrentPageIndex == 6) { CurrentPageIndex = 7; }
                    if (CurrentPageIndex == 7) { CurrentPageIndex = 7; }
                    if (CurrentPageIndex == 8) { CurrentPageIndex = 9; }
                    if (CurrentPageIndex == 9) { CurrentPageIndex = 9; }
                }
            }
            if (!plus)
            {
                if (CurrentPageIndex > lowest)
                {
                    CurrentPageIndex -= 2;
                    if (CurrentPageIndex == 0) { CurrentPageIndex = 0; }
                    if (CurrentPageIndex == 1) { CurrentPageIndex = 0; }
                    if (CurrentPageIndex == 2) { CurrentPageIndex = 2; }
                    if (CurrentPageIndex == 3) { CurrentPageIndex = 2; }
                    if (CurrentPageIndex == 4) { CurrentPageIndex = 4; }
                    if (CurrentPageIndex == 5) { CurrentPageIndex = 4; }
                    if (CurrentPageIndex == 6) { CurrentPageIndex = 6; }
                    if (CurrentPageIndex == 7) { CurrentPageIndex = 6; }
                    if (CurrentPageIndex == 8) { CurrentPageIndex = 8; }
                    if (CurrentPageIndex == 9) { CurrentPageIndex = 8; }
                }
            }
            if (CurrentPageIndex > EnabledCount) { CurrentPageIndex = EnabledCount; }
            if (CurrentPageIndex < 0) { CurrentPageIndex = 0; }
            return CurrentPageIndex;
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
