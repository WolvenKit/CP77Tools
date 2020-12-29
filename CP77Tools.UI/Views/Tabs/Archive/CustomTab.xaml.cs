using ControlzEx.Theming;
using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Functionality.Customs;
using CP77Tools.UI.Functionality.ToolsWindow;
using CP77Tools.UI.Views.Tasks;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WolvenKit.Common.Tools.DDS;
using static CP77Tools.UI.Data.General;

namespace CP77Tools.UI.Views.Tabs.Archive
{
    /// <summary>
    /// Interaction logic for CustomTab.xaml
    /// </summary>
    public partial class CustomTab : UserControl
    {
        private SUI sui;
        public CustomTab()
        {
            InitializeComponent();
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());

            ArchiveFunc.LoadCollectionData();
            ArchiveTaskConceptGrid.ItemsSource = ArchiveData.ArchiveConceptTaskDict;
            sui = SUI.sui;

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //   TabItem NewTask = new TabItem();
            //  NewTask.Header = "Concept Task";
            //   ArchiveSubTab.Items.Add(NewTask);

            //   TabItem Current = (TabItem)ArchiveSubTab.Items[0];
            //  Current.Header = "Next";


            //  ArchiveTaskConceptGrid.CanUserAddRows = true;
        }



        private void ArchiveExtractSwitch_Toggled(object sender, RoutedEventArgs e)
        {

            SwitchHelper(sender, "Extract");

        }

        private void SwitchHelper(object sender, string a)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    sui.archivedata.Archive_Extract = true;
                    ChangeCollectionData(a, "Enabled");
                }
                else
                {
                    sui.archivedata.Archive_Extract = false;
                    ChangeCollectionData(a, "Disabled");

                }
            }
        }

        public void ChangeCollectionData(string Key, string NewValue)
        {
            ArchiveData.ArchiveConceptTaskDict.Remove(Key);
            ArchiveData.ArchiveConceptTaskDict.Add(Key, NewValue);
            ArchiveTaskConceptGrid.ItemsSource = null;
            ArchiveTaskConceptGrid.ItemsSource = ArchiveData.ArchiveConceptTaskDict;
        }

        private void ArchiveDumpSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            SwitchHelper(sender, "Dump");

        }

        private void ArchiveListSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            SwitchHelper(sender, "List");

        }

        private void ArchiveUncookSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            SwitchHelper(sender, "Uncook");

        }

        private void ArchiveTGARadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                sui.archivedata.Archive_UncookFileType = EUncookExtension.tga;
                ChangeCollectionData("Uncook File Extension", "Default (TGA)");

            }

        }

        private void ArchiveDDSRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            sui.archivedata.Archive_UncookFileType = EUncookExtension.dds;
            ChangeCollectionData("Uncook File Extension", "DDS");


        }

        private void ArchiveJPEGRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            sui.archivedata.Archive_UncookFileType = EUncookExtension.jpeg;
            ChangeCollectionData("Uncook File Extension", "JPEG");

        }

        private void ArchiveJPGRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            sui.archivedata.Archive_UncookFileType = EUncookExtension.jpg;
            ChangeCollectionData("Uncook File Extension", "JPG");

        }

 

        private void ArchivePNGRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            sui.archivedata.Archive_UncookFileType = EUncookExtension.png;
            ChangeCollectionData("Uncook File Extension", "PNG");

        }

        private void ArchiveBMPRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            sui.archivedata.Archive_UncookFileType = EUncookExtension.bmp;
            ChangeCollectionData("Uncook File Extension", "BMP");

        }

        private void ArchivePatternTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeCollectionData("Pattern", ArchivePatternTextBox.Text);

        }

        private void ArchiveRegexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeCollectionData("Regex", ArchiveRegexTextBox.Text);

        }

        private string PresetNameToSave;



        private void ArchivePresetNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PresetNameToSave = ArchivePresetNameTextBox.Text;
        }

        private void ArchiveSavePresetButton_Click(object sender, RoutedEventArgs e)
        {
            if (PresetNameToSave != null && PresetNameToSave.Length > 0)
            {
                if (!ArchivePresetComboBox.Items.Contains(PresetNameToSave))
                {
                    Preset NewPreset = new Preset(TaskType.Archive, PresetNameToSave, ArchiveData.ArchiveConceptTaskDict);
                    ArchivePresetComboBox.Items.Add(PresetNameToSave);
                    ArchiveData.presets.Add(NewPreset);
                    ArchivePresetComboBox.SelectedIndex = 0;

                }

            }
        }

        private void ArchiveDeleteSelectedPresetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateArchiveTask(selectedTaskType);



        }

        private void CreateArchiveTask(ArchiveData.TaskType taskType)
        {
            TabItem NewTask = new TabItem();
            NewTask.Header = "[" + ArchiveData.TaskType.Custom + "]";
            NewTask.Content = new TaskTemplate();


           // ArchiveSubTab.Items.Add(NewTask);

        }

        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {
            OMD OutPathSelector = new OMD(OMD_Type.Single, true);
            OutPathSelector.Title = "Select Outpath";
            OutPathSelector.ArchiveCustomTab = this;

            OutPathSelector.Show();



        }


     

        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD InputSelector = new OMD(OMD_Type.Multi, false);
            InputSelector.ArchiveCustomTab = this;

            InputSelector.Show();
        }

        //private void ArchiveLoadPresets(string SelectedPreset)
        //{
        //    if (this.IsLoaded)
        //    {
        //        foreach (Preset preset in ArchiveData.presets)
        //        {
        //            if (preset.Name == SelectedPreset)
        //            {
        //                Trace.WriteLine("ShouldWork");
        //                ArchiveData.ArchiveConceptTaskDict = preset.Pdata;

        //                foreach (var dat in ArchiveData.ArchiveConceptTaskDict)
        //                {
        //                    switch (dat.Key)
        //                    {
        //                        case "Extract":
        //                            ArchiveExtractSwitch.IsOn = true;
        //                            break;
        //                    }
        //                }

        //            }

        //        }
        //    }

        //}

        private void ArchivePresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  ArchiveLoadPresets(ArchivePresetComboBox.SelectedItem.ToString()) ;
        //   Trace.WriteLine(ArchivePresetComboBox.SelectedItem.ToString());
        }
        private ArchiveData.TaskType selectedTaskType;

 
    }
}