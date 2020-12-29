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
    /// Interaction logic for ExtractSingleTab.xaml
    /// </summary>
    public partial class ExtractSingleTab : UserControl
    {
        public ExtractSingleTab()
        {
            InitializeComponent();
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

        }

        private void HashTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SUI.sui.hashdata.Hash_Input = HashTextBox.Text;
        }

        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {
            OMD OutPathSelector = new OMD(TaskType.Archive, OMD_Type.Single, true);
            OutPathSelector.Title = "Select Outpath";
            OutPathSelector.ArchiveSingleTab = this;
            OutPathSelector.archivesubtasktype = ArchiveData.TaskType.Single;
            OutPathSelector.Show();
        }




        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD InputSelector = new OMD(TaskType.Archive, OMD_Type.Multi, false);
            InputSelector.ArchiveSingleTab = this;
            InputSelector.archivesubtasktype = ArchiveData.TaskType.Single;
            InputSelector.Show();
        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateSingleExtractTask();



        }

        private void CreateSingleExtractTask()
        {
            TabItem NewTask = new TabItem();
            NewTask.Header = "[" + ArchiveData.TaskType.Single + "]";
            var sometask = new TaskTemplate(General.TaskType.Archive);
            sometask.ArchiveTaskConceptGrid.ItemsSource = null;
            //      sometask.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;
            sometask.TaskFinalGroup.Header = "Custom Archive Task Settings";
            sometask.ArchiveSelectedInputConceptDropDown1.ItemsSource = SelectedInputConceptDropDown1.ItemsSource;
            NewTask.Content = sometask;
            SUI.sui.generaldata.ToolsInstance.ArchiveSubTab.Items.Add(NewTask);


        }
    }
}
