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

namespace CP77Tools.UI.Views.Tabs.CR2W
{
    /// <summary>
    /// Interaction logic for ClassInfoTab.xaml
    /// </summary>
    public partial class ClassInfoTab : UserControl
    {
        public ClassInfoTab()
        {
            InitializeComponent(); 
            SUI.sui.cr2wdata.CR2W_Chunks = true;

        }

        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {
            OMD OutPathSelector = new OMD(TaskType.CR2W, OMD_Type.Single, true);
            OutPathSelector.Title = "Select Outpath";
            OutPathSelector.Cr2WClassInfoTab = this;
            OutPathSelector.cr2wsubtasktype = CR2WData.CR2WTaskType.Chunks;
            OutPathSelector.Show();
        }




        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD InputSelector = new OMD(TaskType.CR2W, OMD_Type.Multi, false);
            InputSelector.Cr2WClassInfoTab = this;
            InputSelector.cr2wsubtasktype = CR2WData.CR2WTaskType.Chunks;
            InputSelector.Show();
        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDumpTaskk();



        }

        private void CreateDumpTaskk()
        {
            TabItem NewTask = new TabItem();
            NewTask.Header = "[" + CR2WData.CR2WTaskType.Chunks + "]";
            var sometask = new TaskTemplate(General.TaskType.CR2W);
            sometask.ArchiveTaskConceptGrid.ItemsSource = null;
            //      sometask.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;   // LETS ADD THIS LATER JUST INFO THO 
            sometask.TaskFinalGroup.Header = "Class Info Task Settings";
            sometask.ArchiveSelectedInputConceptDropDown1.ItemsSource = ArchiveSelectedInputConceptDropDown1.ItemsSource;
            NewTask.Content = sometask;
            SUI.sui.generaldata.ToolsInstance.ArchiveSubTab.Items.Add(NewTask);


        }
    }
}
