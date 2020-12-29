using System;
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

namespace CP77Tools.UI.Views.Tabs.Dump
{
    /// <summary>
    /// Interaction logic for CustomTab.xaml
    /// </summary>
    public partial class CustomTab : UserControl
    {
        public CustomTab()
        {
            InitializeComponent();
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

        }
        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {
            OMD OutPathSelector = new OMD(TaskType.Dump, OMD_Type.Single, true);
            OutPathSelector.Title = "Select Outpath";
            OutPathSelector.DumpCustomTab = this;
            OutPathSelector.dumpsubtasktype = DumpData.DumpTaskType.Custom;
            OutPathSelector.Show();
        }




        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD InputSelector = new OMD(TaskType.Dump, OMD_Type.Multi, false);
            InputSelector.DumpCustomTab = this;
            InputSelector.dumpsubtasktype = DumpData.DumpTaskType.Custom;
            InputSelector.Show();
        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDumpTaskk();



        }

        private void CreateDumpTaskk()
        {
            TabItem NewTask = new TabItem();
            NewTask.Header = "[" + DumpData.DumpTaskType.Custom + "]";
            var sometask = new TaskTemplate(General.TaskType.Dump);
            sometask.ArchiveTaskConceptGrid.ItemsSource = null;
            //      sometask.ArchiveTaskConceptGrid.ItemsSource = this.ArchiveTaskConceptGrid.ItemsSource;   // LETS ADD THIS LATER JUST INFO THO 
            sometask.TaskFinalGroup.Header = "Custom Task Settings";
            sometask.ArchiveSelectedInputConceptDropDown1.ItemsSource = ArchiveSelectedInputConceptDropDown1.ItemsSource;
            NewTask.Content = sometask;
            SUI.sui.generaldata.ToolsInstance.ArchiveSubTab.Items.Add(NewTask);


        }

        private void ArchiveExtractSwitch_Toggled(object sender, RoutedEventArgs e) // Classinfo
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    SUI.sui.dumpdata.Dump_ClassInfo = true;
                }
                else
                {
                    SUI.sui.dumpdata.Dump_ClassInfo = false;

                }
            }
        }

        private void ArchiveDumpSwitch_Toggled(object sender, RoutedEventArgs e) //  Imports
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    SUI.sui.dumpdata.Dump_Imports = true;
                }
                else
                {
                    SUI.sui.dumpdata.Dump_Imports = false;

                }
            }
        }

        private void ArchiveExtractSwitch_Toggled2(object sender, RoutedEventArgs e) // Missinghashes
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    SUI.sui.dumpdata.Dump_MissingHashes = true;
                }
                else
                {
                    SUI.sui.dumpdata.Dump_MissingHashes = false;

                }
            }
        }

        private void ArchiveDumpSwitch_Toggled2(object sender, RoutedEventArgs e) // XbmInfo
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    SUI.sui.dumpdata.Dump_Info = true;
                }
                else
                {
                    SUI.sui.dumpdata.Dump_Info = false;

                }
            }
        }
    }
}
