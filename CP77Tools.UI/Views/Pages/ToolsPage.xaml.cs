using ControlzEx.Theming;
using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Functionality.Customs;
using CP77Tools.UI.Functionality.ToolsWindow;
using CP77Tools.UI.Views.Tabs.Archive;
using CP77Tools.UI.Views.Tasks;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WolvenKit.Common.Tools.DDS;
using static CP77Tools.UI.Data.General;

namespace CP77Tools.UI.Views.Pages
{
    /// <summary>
    /// Interaction logic for Tools.xaml
    /// </summary>
    public partial class Tools : UserControl
    {
        public static int darklight { get; set; }
        public Tools()
        {
            
                InitializeComponent();




                SUI.sui.generaldata.ToolsInstance = this;
                // ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());
                this.DataContext = this;
            
       

            
     

        }


        public void RefreshTheme()
        {

            ThemeManager.Current.ChangeTheme(SUI.sui.tools, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.main, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.settings, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.about, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.ArchiveCustomTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.ArchiveDumpTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.ArchiveSingleTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.ArchiveExtractTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.ArchiveListTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.ArchiveUncookTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.Cr2wCustomTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.Cr2WClassInfoTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.Cr2wAllInfoTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.DumpClassInfoTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.DumpCustomTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.DumpImportsTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.DumpMissingTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.DumpXbmTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.HashHashTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.OodleDecompressTab, SUI.sui.generaldata.ThemeFinder());
            ThemeManager.Current.ChangeTheme(SUI.sui.RepackRepackTab, SUI.sui.generaldata.ThemeFinder());
        }

        public static CustomTab _CustomTab;
        private void ArhiveBottomTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not MetroTabControl)
                return;
            if (sender == ArhiveBottomTab)
            {
                TabItem CurrentMain = (TabItem)ToolsTab.SelectedItem;
                switch (CurrentMain.Header)
                {
                    case "Archive":
                        TabItem Current = (TabItem)ArhiveBottomTab.SelectedItem;
                        switch (Current.Header)
                        {
                            case "New Task": Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Custom; break;
                            case "Extract": Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Extract; break;
                            case "Uncook": Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Uncook; break;
                            case "Dump": Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Dump; break;
                            case "List": Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.List; break;
                            case "Extract Single": Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Single; break;
                        }
                        break;
                    case "CR2W":
                        TabItem Current2 = (TabItem)ArhiveBottomTab_Copy.SelectedItem;
                        switch (Current2.Header)
                        {
                            case "New Task": Functionality.UserInterfaceLogic.selectedCR2WTaskType = CR2WData.CR2WTaskType.Custom; break;
                            case "All Information": Functionality.UserInterfaceLogic.selectedCR2WTaskType = CR2WData.CR2WTaskType.All; break;
                            case "Class Information": Functionality.UserInterfaceLogic.selectedCR2WTaskType = CR2WData.CR2WTaskType.Chunks; break;

                        }
                        break;
                    case "Repack":
                        TabItem Current3 = (TabItem)ArhiveBottomTab_Copy2.SelectedItem;
                        switch (Current3.Header) { case "Repack": Functionality.UserInterfaceLogic.selectedRepackTaskType = RepackData.RepackTaskType.Repack; break; }
                        break;
                    case "Dump":
                        TabItem Current5 = (TabItem)ArhiveBottomTab_Copy3.SelectedItem;
                        switch (Current5.Header)
                        {
                            case "New Task": Functionality.UserInterfaceLogic.selectedDumpTaskType = DumpData.DumpTaskType.Custom; break;
                            case "Imports": Functionality.UserInterfaceLogic.selectedDumpTaskType = DumpData.DumpTaskType.Imports; break;
                            case "Class Information": Functionality.UserInterfaceLogic.selectedDumpTaskType = DumpData.DumpTaskType.ClassInfo; break;
                            case "XBM Information": Functionality.UserInterfaceLogic.selectedDumpTaskType = DumpData.DumpTaskType.Info; break;
                            case "Missing Hashes": Functionality.UserInterfaceLogic.selectedDumpTaskType = DumpData.DumpTaskType.MissingHashes; break;
                        }
                        break;
                    case "Hash":
                        break;
                    case "Oodle":
                        TabItem Current4 = (TabItem)ArhiveBottomTab_Copy32.SelectedItem;
                        switch (Current4.Header) { case "Decompress": Functionality.UserInterfaceLogic.selectedOodleTaskType = OodleData.OodleTaskType.Decompress; break; }
                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
      
        }

        private void PackIconFeatherIcons_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToolsLogFlyOut.IsOpen = true;
        }

        private void ToolsLogFlyOut_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (ToolsLogFlyOut.IsOpen) { LogsLabel.Visibility = Visibility.Hidden; }
            else { LogsLabel.Visibility = Visibility.Visible; }
        }


    }









}
