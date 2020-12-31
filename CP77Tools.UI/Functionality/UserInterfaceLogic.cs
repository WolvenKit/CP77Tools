using CP77Tools.Tasks;
using CP77Tools.UI.Functionality.Customs;
using Microsoft.Win32;
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
using System.Collections.Concurrent;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace CP77Tools.UI.Functionality
{
    public class UserInterfaceLogic
    {
        private SUI sui;
        public UserInterfaceLogic(SUI _sui) { this.sui = _sui; }

        public static ArchiveData.TaskType selectedArchiveTaskType;
        public static CR2WData.CR2WTaskType selectedCR2WTaskType;
        public static DumpData.DumpTaskType selectedDumpTaskType;
        public static OodleData.OodleTaskType selectedOodleTaskType;
        public static RepackData.RepackTaskType selectedRepackTaskType;

        private General.TaskType CurrentTaskType;

        // Creates Tasks based on Taskindex.
        private void TaskManager(General.TaskType taskindex, Views.Tasks.TaskTemplate taskTemplate)
        {
            CurrentTaskType = taskindex;
            switch (taskindex)
            {
                case General.TaskType.Archive:
                    if (sui.archivedata.Archive_Path.Length > 0) { StartArchiveTask(taskTemplate); }
                    break;

                case General.TaskType.CR2W:
                    if (sui.cr2wdata.CR2W_Path.Length > 0) { StartCR2WTask(taskTemplate); }
                    break;

                case General.TaskType.Repack:
                    if (sui.repackdata.Repack_Path.Length > 0) { StartRepackTask(taskTemplate); }
                    break;

                case General.TaskType.Dump:
                    if (sui.dumpdata.Dump_Path.Length > 0) { StartDumpTask(taskTemplate); }
                    break;

                case General.TaskType.Hash:
                    if (sui.hashdata.Hash_Input != "") { StartHashTask(); }
                    break;

                case General.TaskType.Oodle:
                    if (sui.oodledata.Oodle_Path.Length > 0) { StartOodleTask(taskTemplate); }
                    break;
            }
        }

        private void StartOodleTask(Views.Tasks.TaskTemplate taskTemplate)
        {
            var p1 = sui.oodledata.Oodle_Path[0];
            var p2 = sui.oodledata.Oodle_OutPath;
            var p3 = sui.oodledata.Oodle_Decompress;
            sui.oodledata.resetoodledata();

            Task OTask = new Task(() => ConsoleFunctions.OodleTask(p1,p2,p3)); // FIX THIS WHEN MULTISELECT IS POSSIBLE!

            backgroundworker("[ Oodle | " + selectedOodleTaskType.ToString() + " ]", "An Oodle task is busy... \n Please wait..."); // ShowDialog

            OTask.Start(); 
            
           OTask.Wait(); sui.log.TaskFinished(General.TaskType.Oodle, General.TaskIDList.Count);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                TabItem NewTask = new TabItem();
                NewTask.Header = "[" + selectedOodleTaskType.ToString() + " - " + SUI.sui.generaldata.TaskIDGen() + "]";


                taskTemplate.TaskTitleLabel.Content = "Oodle Task : " + selectedOodleTaskType.ToString();


                taskTemplate.ArchiveTaskConceptGrid.ItemsSource = null;
                //  taskTemplate.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;
                taskTemplate.TaskFinalGroup.Header = selectedOodleTaskType.ToString() + " Oodle Task Settings";
                //  taskTemplate.ArchiveSelectedInputConceptDropDown1.ItemsSource = ArchiveSelectedInputConceptDropDown1.ItemsSource;
                NewTask.Content = taskTemplate;
                SUI.sui.generaldata.ToolsInstance.OodleSubTab.Items.Add(NewTask);

            }));
        }

        private void StartHashTask()
        {
            //Task HTask = new Task(() => ConsoleFunctions.HashTask(sui.generaldata.Hash_Input, sui.generaldata.Hash_Missing));
            //sui.generaldata.InterceptLog = true;
            //HTask.Start();
            //HTask.Wait(); sui.log.TaskFinished(General.TaskType.Hash);
        }

        private void StartDumpTask(Views.Tasks.TaskTemplate taskTemplate)
        {
            var p1 = sui.dumpdata.Dump_Path;
            var p2 = sui.dumpdata.Dump_Imports;
            var p3 = sui.dumpdata.Dump_MissingHashes;
            var p4 = sui.dumpdata.Dump_Info;
            var p5 = sui.dumpdata.Dump_ClassInfo;
            sui.dumpdata.resetdumpdata();

            Task DTask = new Task(() => ConsoleFunctions.DumpTask(p1,p2,p3,p4,p5));// FIX THIS WHEN MULTISELECT IS POSSIBLE!

            backgroundworker("[ Dump | " + selectedDumpTaskType.ToString() + " ]", "A Dump task is busy... \n Please wait..."); // ShowDialog


            DTask.Start(); 
            
            DTask.Wait();

            sui.log.TaskFinished(General.TaskType.Dump, General.TaskIDList.Count);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                TabItem NewTask = new TabItem();
                NewTask.Header = "[" + selectedDumpTaskType.ToString() + " - " + SUI.sui.generaldata.TaskIDGen() + "]";


                taskTemplate.TaskTitleLabel.Content = "Dump Task : " + selectedDumpTaskType.ToString();


                taskTemplate.ArchiveTaskConceptGrid.ItemsSource = null;
                //  taskTemplate.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;
                taskTemplate.TaskFinalGroup.Header = selectedDumpTaskType.ToString() + " Dump Task Settings";
                //  taskTemplate.ArchiveSelectedInputConceptDropDown1.ItemsSource = ArchiveSelectedInputConceptDropDown1.ItemsSource;
                NewTask.Content = taskTemplate;
                SUI.sui.generaldata.ToolsInstance.DumpSubTab.Items.Add(NewTask);

            }));
        }

        private void StartRepackTask(Views.Tasks.TaskTemplate taskTemplate)
        {
            var p1 = sui.repackdata.Repack_Path;
            var p2 = sui.repackdata.Repack_OutPath;
            sui.repackdata.resetrepackdata();
            Task RTask = new Task(() => ConsoleFunctions.PackTask(p1,p2));  // FIX THIS TOO 

            backgroundworker("[ Repack | " + selectedRepackTaskType.ToString() + " ]", "A Repack task is busy... \n Please wait..."); // ShowDialog

            RTask.Start();             
           RTask.Wait(); 



            sui.log.TaskFinished(General.TaskType.Repack, General.TaskIDList.Count);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                TabItem NewTask = new TabItem();
                NewTask.Header = "[" + selectedRepackTaskType.ToString() + " - " + SUI.sui.generaldata.TaskIDGen() + "]";


                taskTemplate.TaskTitleLabel.Content = "Repack Task : " + selectedRepackTaskType.ToString();


                taskTemplate.ArchiveTaskConceptGrid.ItemsSource = null;
                //  taskTemplate.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;
                taskTemplate.TaskFinalGroup.Header = selectedRepackTaskType.ToString() + " Repack Task Settings";
                //  taskTemplate.ArchiveSelectedInputConceptDropDown1.ItemsSource = ArchiveSelectedInputConceptDropDown1.ItemsSource;
                NewTask.Content = taskTemplate;
                SUI.sui.generaldata.ToolsInstance.RepackSubTab.Items.Add(NewTask);

            }));
        }

        private void StartCR2WTask(Views.Tasks.TaskTemplate taskTemplate)
        {
            var p1 = sui.cr2wdata.CR2W_Path;
            var p2 = sui.cr2wdata.CR2W_OutPath;
            var p3 = sui.cr2wdata.CR2W_All;
            var p4 = sui.cr2wdata.CR2W_Chunks;
            sui.cr2wdata.resetcr2wdata();

            Task CTask = new Task(() => ConsoleFunctions.Cr2wTask(p1,p2,p3,p4)); // FIX THIS WHEN MULTISELECT IS POSSIBLE!




            backgroundworker("[ CR2W | " + selectedCR2WTaskType.ToString() + " ]", "A CR2W task is busy... \n Please wait..."); // ShowDialog


            CTask.Start(); 
            
            CTask.Wait(); 





            sui.log.TaskFinished(General.TaskType.CR2W, General.TaskIDList.Count);



            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                TabItem NewTask = new TabItem();
                NewTask.Header = "[" + selectedCR2WTaskType.ToString() + " - " + SUI.sui.generaldata.TaskIDGen() + "]";


                taskTemplate.TaskTitleLabel.Content = "CR2W Task : " + selectedCR2WTaskType.ToString();


                taskTemplate.ArchiveTaskConceptGrid.ItemsSource = null;
                //  taskTemplate.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;
                taskTemplate.TaskFinalGroup.Header = selectedCR2WTaskType.ToString() + " CR2W Task Settings";
                //  taskTemplate.ArchiveSelectedInputConceptDropDown1.ItemsSource = ArchiveSelectedInputConceptDropDown1.ItemsSource;
                NewTask.Content = taskTemplate;
                SUI.sui.generaldata.ToolsInstance.CR2WSubTab.Items.Add(NewTask);

            }));
        }

        private void StartArchiveTask(Views.Tasks.TaskTemplate taskTemplate)
        {
            var p1 = sui.archivedata.Archive_Path;
            var p2 = sui.archivedata.Archive_OutPath;
            var p3 = sui.archivedata.Archive_Extract;
            var p4 = sui.archivedata.Archive_Dump;
            var p5 = sui.archivedata.Archive_List;
            var p6 = sui.archivedata.Archive_Uncook;
            var p7 = sui.archivedata.Archive_UncookFileType;
            var p8 = sui.archivedata.Archive_Hash;
            var p9 = sui.archivedata.Archive_Pattern;
            var p10 = sui.archivedata.Archive_Regex;  // Set locals 

            sui.archivedata.resetarchivedata(); // Reset Globals

            Task ATask = new Task(() => ConsoleFunctions.ArchiveTask(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10));  // Create Task


            backgroundworker("[ Archive | " + selectedArchiveTaskType.ToString() + " ]", "An Archive task is busy... \n Please wait..."); // ShowDialog

            ATask.Start();
            ATask.Wait();


            SUI.sui.log.TaskFinished(General.TaskType.Archive, General.TaskIDList.Count);


            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
              {
                  TabItem NewTask = new TabItem();
                  NewTask.Header = "[" + selectedArchiveTaskType.ToString() + " - " + SUI.sui.generaldata.TaskIDGen() + "]";


                  taskTemplate.TaskTitleLabel.Content = "Archive Task : " + selectedArchiveTaskType.ToString();


                  taskTemplate.ArchiveTaskConceptGrid.ItemsSource = null;
                //  taskTemplate.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;
                taskTemplate.TaskFinalGroup.Header = selectedArchiveTaskType.ToString() + " Archive Task Settings";
                //  taskTemplate.ArchiveSelectedInputConceptDropDown1.ItemsSource = ArchiveSelectedInputConceptDropDown1.ItemsSource;
                NewTask.Content = taskTemplate;
                  SUI.sui.generaldata.ToolsInstance.ArchiveSubTab.Items.Add(NewTask);

            }));
        }


        public static ProgressDialogController controller;

        public void backgroundworker(string title,string message)
        {
           SUI.sui.ProgressDialogHelper(title,message);
        }

        private void Controller_Canceled(object sender, EventArgs e)
        {
            controller.CloseAsync();
        }


        // Creates Thread and sends TaskIndicator to taskmanager to run task on thread.
        public void ThreadedTaskSender(General.TaskType item, Views.Tasks.TaskTemplate taskTemplate) { Thread worker = new Thread(() => TaskManager(item,taskTemplate)); worker.IsBackground = true; worker.Start(); }
        
       



        public void SetToolTips()
        {
    
        }
    }
}
