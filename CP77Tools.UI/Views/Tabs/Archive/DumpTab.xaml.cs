using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Functionality.Customs;
using CP77Tools.UI.Views.Tasks;
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
using static CP77Tools.UI.Data.General;

namespace CP77Tools.UI.Views.Tabs.Archive
{
    /// <summary>
    /// Interaction logic for DumpTab.xaml
    /// </summary>
    public partial class DumpTab : UserControl
    {
        public DumpTab()
        {
            InitializeComponent(); 
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            SUI.sui.archivedata.Archive_Dump = true;
        }

        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {
            OMD OutPathSelector = new OMD(TaskType.Archive, OMD_Type.Single, true);
            OutPathSelector.Title = "Select Outpath";
            OutPathSelector.ArchiveDumpTab = this;
            OutPathSelector.archivesubtasktype = ArchiveData.TaskType.Dump;
            OutPathSelector.Show();
        }




        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD InputSelector = new OMD(TaskType.Archive, OMD_Type.Multi, false);
            InputSelector.ArchiveDumpTab = this;
            InputSelector.archivesubtasktype = ArchiveData.TaskType.Dump;
            InputSelector.Show();
        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDumpTaskk();



        }

        private void CreateDumpTaskk()
        {
            TabItem NewTask = new TabItem();
            NewTask.Header = "[" + ArchiveData.TaskType.Dump + " - " + SUI.sui.generaldata.TaskIDGen() + "]";
            var sometask = new TaskTemplate(General.TaskType.Archive);
            sometask.ArchiveTaskConceptGrid.ItemsSource = null;
            sometask.TaskTitleLabel.Content = "Task : Dump Archive Task";
            //      sometask.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;   // LETS ADD THIS LATER JUST INFO THO 
            sometask.TaskFinalGroup.Header = "Dump Task Settings";
            sometask.ArchiveSelectedInputConceptDropDown1.ItemsSource = DumpSelectedInputConceptDropDown1.ItemsSource;
            NewTask.Content = sometask;
            SUI.sui.generaldata.ToolsInstance.ArchiveSubTab.Items.Add(NewTask);


        }

    }
}
