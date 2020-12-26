using ControlzEx.Theming;
using CP77Tools.UI.Data;
using CP77Tools.UI.Functionality.Customs;
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

namespace CP77Tools.UI.Views
{
    /// <summary>
    /// Interaction logic for Tools.xaml
    /// </summary>
    public partial class Tools : Page
    {
        private SUI sui;



        public Tools()
        {
            InitializeComponent();
            
            ThemeManager.Current.ChangeTheme(this, "Dark.Steel");
            LoadCollectionData();
            ArchiveTaskConceptGrid.ItemsSource = ArchiveConceptTaskDict;
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

        public static Dictionary<string, string> ArchiveConceptTaskDict = new Dictionary<string, string>();

        private void LoadCollectionData()
        {
            ArchiveConceptTaskDict.Clear();
            ArchiveConceptTaskDict.Add("Selected Outpath", "None");
            ArchiveConceptTaskDict.Add("Extract", "Disabled");
            ArchiveConceptTaskDict.Add("List", "Disabled");
            ArchiveConceptTaskDict.Add("Dump", "Disabled");
            ArchiveConceptTaskDict.Add("Uncook", "Disabled");
            ArchiveConceptTaskDict.Add("Uncook File Extension", "Default (TGA)");
            ArchiveConceptTaskDict.Add("Pattern", "None");
            ArchiveConceptTaskDict.Add("Regex", "None");
        }

        private void ArchiveExtractSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null) 
            { 
                if (toggleSwitch.IsOn == true) 
                { 
                    sui.archivedata.Archive_Extract = true;
                    ChangeCollectionData("Extract", "Enabled");
                } 
                else 
                {
                    sui.archivedata.Archive_Extract = false;
                    ChangeCollectionData("Extract", "Disabled");

                }
            }
        }



        private void ChangeCollectionData(string Key,string NewValue)
        {
            ArchiveConceptTaskDict.Remove(Key);
            ArchiveConceptTaskDict.Add(Key, NewValue);
            ArchiveTaskConceptGrid.ItemsSource = null;
            ArchiveTaskConceptGrid.ItemsSource = ArchiveConceptTaskDict;
        }

        private void ArchiveDumpSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveListSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveUncookSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveTGARadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveDDSRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveJPEGRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveJPGRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ArchivePNGRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveBMPRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ArchivePatternTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ArchiveRegexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ArchivePresetNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ArchiveSavePresetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveDeleteSelectedPresetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveLaunchTaskButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveSelectOutpathButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveSelectInputButton_Click(object sender, RoutedEventArgs e)
        {
            OMD Testing = new OMD(this,sui,General.TaskType.Archive, Data.General.OMD_Type.Multi);
            Testing.Show();
        }
    }


    public class Preset
    {
        public static General.TaskType Type { get; set; }
        public static string Name { get; set; }
        public static Dictionary<string,string> PresetData { get; set; }

        public Preset(General.TaskType taskType, string name, Dictionary<string,string> data)
        {
            Type = taskType;
            Name = name;
            PresetData = data;
        }

    }







}
