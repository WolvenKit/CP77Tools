using CP77.Common.Services;
using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Views.Tabs.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CP77Tools.UI.Views.Tasks
{
    /// <summary>
    /// Interaction logic for TaskTemplate.xaml
    /// </summary>
    public partial class TaskTemplate : UserControl
    {
        public General.TaskType localtasktype;
        public TaskTemplate(General.TaskType maintype)
        {
            localtasktype = maintype;
            InitializeComponent();
            SUI.sui.UI_Logger.PropertyChanged += UI_Logger_PropertyChanged;
            SUI.sui.UI_Logger.OnStringLogged += UI_Logger_OnStringLogged;
            SUI.sui.UI_Logger.PropertyChanging += UI_Logger_PropertyChanging;

            switch (localtasktype)
            {
                case General.TaskType.Archive:
                    StartArchiveTask();
                    break;
                case General.TaskType.CR2W:
                    StartCR2WTask();
                    break;
                case General.TaskType.Dump:
                    StartDumpTask();
                    break;
                case General.TaskType.Repack:
                    StartRepackTask();
                    break;
                case General.TaskType.Oodle:
                    StartOodleTask();
                    break;
                case General.TaskType.Hash:
                    StartHashTask();
                    break;

            }
        }

        private void UI_Logger_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
        }

        private void UI_Logger_OnStringLogged(object sender, CP77.Common.Services.LogStringEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                AddToTaskLog(e.Logtype, e.Message);


            }));

        }

        private void AddToTaskLog(Logtype logtype, string message)
        {
            if (logtype.ToString() == "Error")
            {
                LogItem TaskLogItem = new LogItem(1);
                TaskLogItem.SenderLabel.Content = localtasktype.ToString();
                TaskLogItem.SenderText.Text = "[" + logtype + "] : " + message;
                TaskLogItem.HorizontalAlignment = HorizontalAlignment.Stretch;

                TaskLogWrap.Children.Add(TaskLogItem);

            }
            else if (logtype.ToString() == "Success")
            {
                LogItem TaskLogItem = new LogItem(2);
                TaskLogItem.SenderLabel.Content = localtasktype.ToString();
                TaskLogItem.SenderText.Text = "[" + logtype + "] : " + message;
                TaskLogItem.HorizontalAlignment = HorizontalAlignment.Stretch;

                TaskLogWrap.Children.Add(TaskLogItem);

            }
            else
            {
                LogItem TaskLogItem = new LogItem(7);
                TaskLogItem.SenderLabel.Content = localtasktype.ToString();
                TaskLogItem.SenderText.Text = "[" + logtype + "] : " + message;
                TaskLogItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                TaskLogWrap.Children.Add(TaskLogItem);

            }
        }

        private void UI_Logger_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void StartHashTask()
        {
            SUI.sui.ui.ThreadedTaskSender( General.TaskType.Hash);
        }

        private void StartOodleTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.Oodle);
        }

        private void StartRepackTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.Repack);
        }

        private void StartDumpTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.Dump);
        }

        private void StartCR2WTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.CR2W);
        }

        public void StartArchiveTask()
        {
            SUI.sui.ui.ThreadedTaskSender(General.TaskType.Archive);
        }

        private void TaskLogWrap_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TaskLogWrap.ItemWidth = TaskLogWrap.Width;
        }
    }
}
