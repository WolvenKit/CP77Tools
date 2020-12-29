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
        public TaskTemplate(General.TaskType maintype, ArchiveData.TaskType subtype)
        {
            InitializeComponent();

            switch (maintype)
            {
                case General.TaskType.Archive:
                    StartArchiveTask(subtype);
                    break;
            }
        }


        public void StartArchiveTask(ArchiveData.TaskType subtype)
        {


            switch (subtype)
            {
                case ArchiveData.TaskType.Custom:
                    SUI.sui.ui.ThreadedTaskSender(0);
                    break;
            }


        }


        public void StartCustomArchiveTask()
        {
        }

        public void AddToTaskLog(int type, string text)
        {

        }
    }
}
