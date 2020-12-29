using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
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

namespace CP77Tools.UI.Views.Tasks
{
    /// <summary>
    /// Interaction logic for TaskTemplate.xaml
    /// </summary>
    public partial class TaskTemplate : UserControl
    {
        public TaskTemplate(General.TaskType maintype)
        {
            InitializeComponent();

            switch (maintype)
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



        public void AddToTaskLog(int type, string text)
        {

        }
    }
}
