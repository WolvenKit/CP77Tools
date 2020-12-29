using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Functionality.Customs;
using CP77Tools.UI.Views.Tasks;
using MahApps.Metro.Controls;
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
    /// Interaction logic for CustomTab.xaml
    /// </summary>
    public partial class CustomTab : UserControl
    {
        public CustomTab()
        {
            InitializeComponent();
        }

        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {
            OMD OutPathSelector = new OMD(TaskType.CR2W, OMD_Type.Single, true);
            OutPathSelector.Title = "Select Outpath";
            OutPathSelector.Cr2wCustomTab = this;
            OutPathSelector.cr2wsubtasktype = CR2WData.CR2WTaskType.Custom;
            OutPathSelector.Show();
        }




        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD InputSelector = new OMD(TaskType.CR2W, OMD_Type.Multi, false);
            InputSelector.Cr2wCustomTab = this;
            InputSelector.cr2wsubtasktype = CR2WData.CR2WTaskType.Custom;
            InputSelector.Show();
        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDumpTaskk();



        }

        private void CreateDumpTaskk()
        {
            TabItem NewTask = new TabItem();
            NewTask.Header = "[" + CR2WData.CR2WTaskType.Custom + "]";
            var sometask = new TaskTemplate(General.TaskType.CR2W);
            sometask.ArchiveTaskConceptGrid.ItemsSource = null;
            //      sometask.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;   // LETS ADD THIS LATER JUST INFO THO 
            sometask.TaskFinalGroup.Header = "Custom Task Settings";
            sometask.ArchiveSelectedInputConceptDropDown1.ItemsSource = ArchiveSelectedInputConceptDropDown1.ItemsSource;
            NewTask.Content = sometask;
            SUI.sui.generaldata.ToolsInstance.ArchiveSubTab.Items.Add(NewTask);


        }

        private void ArchiveExtractSwitch_Toggled(object sender, RoutedEventArgs e) // all info
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    SUI.sui.cr2wdata.CR2W_All = true;
                }
                else
                {
                    SUI.sui.cr2wdata.CR2W_All = false;

                }
            }
        }

        private void ArchiveDumpSwitch_Toggled(object sender, RoutedEventArgs e) // class info (These are terribly named lol)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    SUI.sui.cr2wdata.CR2W_Chunks = true;
                }
                else
                {
                    SUI.sui.cr2wdata.CR2W_Chunks = false;

                }
            }
        }
    }
}
