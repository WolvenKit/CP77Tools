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
using CP77Tools.UI.Data.Caching;
using CP77Tools;
using WolvenKit.Common.Tools.DDS;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Catel.IoC;
using CP77Tools.Services;
using WolvenKit.Common.Services;
using System.ComponentModel;

namespace CP77Tools.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        //Archive 
        private string ToolTipArchive = "Target an archive to extract files or dump information.";
        private string ToolTipArchive_Path = "Input path to .archive.";
        private string ToolTipArchive_OutputPath = "Output directory to extract files to.";
        private string ToolTipArchive_Extract = "Extract files from archive.";
        private string ToolTipArchive_Dump = "Dump archive information.";
        private string ToolTipArchive_List = "List contents of archive.";
        private string ToolTipArchive_Uncook = "Uncooks textures from archive.";
        private string ToolTipArchive_Uext = "Uncook extension (tga, bmp, jpg, png, dds). Default is tga.";
        private string ToolTipArchive_Hash = "Extract single file with given hash.";

        private string Archive_Path = "";
        private string Archive_OutPath = "";
        private bool Archive_Extract = false;
        private bool Archive_Dump = false;
        private bool Archive_List = false;
        private bool Archive_Uncook = false;
        private EUncookExtension Archive_UncookFileType = EUncookExtension.tga;
        private ulong Archive_Hash = 0;

        //Dump
        private string ToolTipDump = "Target an archive or a directory to dump archive information.";
        private string ToolTipDump_Path = "Input path to .archive or to a directory (runs over all archives in directory).";
        private string ToolTipDump_OutputPath = "Output directory";
        private string ToolTipDump_Imports = "Dump all imports (all filenames that are referenced by all files in the archive).";
        private string ToolTipDump_MissingHashes = "List all missing hashes of all input archives.";
        private string ToolTipDump_Info = "Dump all xbm info.";

        private string Dump_Path = "";
        private string Dump_OutPath = "";
        private bool Dump_Imports = false;
        private bool Dump_MissingHashes = false;
        private bool Dump_Info = false;

        //CR2W
        private string ToolTipCR2W = "Target a specific cr2w (extracted) file and dumps file information.";
        private string ToolTipCR2W_Path = "Input path to a cr2w file.";
        private string ToolTipCR2W_OutputPath = "Output directory";
        private string ToolTipCR2W_All = "Dump all information.";
        private string ToolTipCR2W_Chunks = "Dump all class information of file.";

        private string CR2W_Path = "";
        private string CR2W_OutPath = "";
        private bool CR2W_All = false;
        private bool CR2W_Chunks = false;

        //Hash
        private string ToolTipHash = "Some helper functions related to hashes.";
        private string ToolTipHash_Input = "Create FNV1A hash of given string";
        private string ToolTipHash_OutputPath = "Output directory";
        private string ToolTipHash_Missing = "";

        private string Hash_Input = "";
        private string Hash_Output = "";
        private bool Hash_Missing = false;

        //Oodle
        private string ToolTipOodle = "Some helper functions related to oodle compression.";
        private string ToolTipOodle_Path = "";
        private string ToolTipOodle_Decompress = "";

        private string Oodle_Path;
        private string Oodle_OutPath;
        private bool Oodle_Decompress = false;


        public MainWindow()
        {
            InitializeComponent();
            ServiceLocator.Default.RegisterType<IMainController, MainController>();
            ServiceLocator.Default.RegisterType<ILoggerService, LoggerService>();
            var UI_Logger = ServiceLocator.Default.ResolveType<ILoggerService>();

            SetToolTips();

            UI_Logger.PropertyChanged += UI_Logger_PropertyChanged;
            UI_Logger.OnStringLogged += UI_Logger_OnStringLogged;
         



        }

        private void UI_Logger_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Trace.Write(e.PropertyName);
        }

        private void UI_Logger_OnStringLogged(object sender, LogStringEventArgs e)
        {
            Trace.Write("Fuck me" + e.Message + e.Logtype);
        }

     


        // TooltipsSetter
        private void SetToolTips()
        {
            //Archive
            UIElement_Button_ArchiveSelectArchive.ToolTip = ToolTipArchive_Path; UIElement_Button_ArchiveSelectOutputPath.ToolTip = ToolTipArchive_OutputPath;
            UIElement_Checkbox_ArchiveDump.ToolTip = ToolTipArchive_Dump; UIElement_Checkbox_ArchiveExtract.ToolTip = ToolTipArchive_Extract;
            UIElement_Checkbox_ArchiveList.ToolTip = ToolTipArchive_List; UIElement_Checkbox_ArchiveUncook.ToolTip = ToolTipArchive_Uncook;
            UIElement_TextBox_ArchiveHash.ToolTip = ToolTipArchive_Hash; UIElement_Button_ArchiveStart.ToolTip = ToolTipArchive;
            //Dump
            UIElement_Dump_PathIndicator_Selected.ToolTip = ToolTipDump_Path; UIElement_Checkbox_DumpImports.ToolTip = ToolTipDump_Imports;
            UIElement_Checkbox_DumpMissingHashes.ToolTip = ToolTipDump_MissingHashes; UIElement_Checkbox_DumpInfo.ToolTip = ToolTipDump_Info;
            //CR2W
            //Hash
            //Oodle


        }





        // Just some breh code for handling pages.
        private int CurrentPageIndex = 3;
        private int Pagehandler(bool plus)
        {
            if (CurrentPageIndex <= 0) { CurrentPageIndex = 3; }
            if (plus)
            {
                CurrentPageIndex += 1;
                if (CurrentPageIndex >= 0 && CurrentPageIndex <= 3) { CurrentPageIndex = 3; }
                if (CurrentPageIndex >= 4 && CurrentPageIndex <= 7) { CurrentPageIndex = 7; }
                if (CurrentPageIndex >= 8 && CurrentPageIndex <= 11) { CurrentPageIndex = 11; }
                if (CurrentPageIndex >= UIElement_ItemList.Items.Count - 1) { CurrentPageIndex = UIElement_ItemList.Items.Count - 1; }
                return CurrentPageIndex;
            }
            if (!plus)
            {
                CurrentPageIndex -= 1;
                if (CurrentPageIndex >= 0 && CurrentPageIndex <= 2) { CurrentPageIndex = 0; }
                if (CurrentPageIndex >= 3 && CurrentPageIndex <= 6) { CurrentPageIndex = 0; }
                if (CurrentPageIndex >= 7 && CurrentPageIndex <= 10 || CurrentPageIndex == 9) { CurrentPageIndex = 4; }
                return CurrentPageIndex;
            }
            else { return 3; }
        }



        private void UIFunc_DragWindow(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) this.DragMove(); }
        // General Events
        private void UIElement_CloseButton_MouseEnter(object sender, MouseEventArgs e) { UIElement_CloseButton.Source = ImageCache.CloseSelected; }
        private void UIElement_CloseButton_MouseLeave(object sender, MouseEventArgs e) { UIElement_CloseButton.Source = ImageCache.Close; }
        private void UIElement_MinimizeButton_MouseEnter(object sender, MouseEventArgs e) { UIElement_MinimizeButton.Source = ImageCache.MinimizeSelected; }
        private void UIElement_MinimizeButton_MouseLeave(object sender, MouseEventArgs e) { UIElement_MinimizeButton.Source = ImageCache.Minimize; }
        private void UIElement_PreviousItems_MouseEnter(object sender, MouseEventArgs e) { UIElement_PreviousItems.Source = ImageCache.PageMoveSelected; }
        private void UIElement_PreviousItems_MouseLeave(object sender, MouseEventArgs e) { UIElement_PreviousItems.Source = ImageCache.PageMove; }
        private void UIElement_Button_NextItems_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_NextItems.Opacity = 0.75; }
        private void UIElement_Button_NextItems_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_NextItems.Opacity = 0.9; }
        private void UIElement_Button_NextItems_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { UIElement_Button_NextItems.Opacity = 0.9; }
        private void UIElement_Button_PreviousItems_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_PreviousItems.Opacity = 0.75; }
        private void UIElement_Button_PreviousItems_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_PreviousItems.Opacity = 0.9; }
        private void UIElement_Button_PreviousItems_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { UIElement_Button_PreviousItems.Opacity = 0.9; }
        // OnExit
        private void UIElement_CloseButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { System.Windows.Application.Current.Shutdown(); }
        // Minimize
        private void UIElement_MinimizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { this.WindowState = WindowState.Minimized; }
        // Prev/Next Items Click events
        private void UIElement_Button_PreviousItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { UIElement_Button_PreviousItems.Opacity = 0.6; if (CurrentPageIndex > 0) { UIElement_ItemList.ScrollIntoView(UIElement_ItemList.Items[Pagehandler(false)]); } }
        private void UIElement_Button_NextItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { UIElement_Button_NextItems.Opacity = 0.6; UIElement_ItemList.ScrollIntoView(UIElement_ItemList.Items[Pagehandler(true)]); }
        private void UIElement_PreviousItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { UIElement_ItemList.ScrollIntoView(UIElement_ItemList.Items[0]); }
        //Archive events
        private void UIElement_Checkbox_ArchiveExtract_Checked(object sender, RoutedEventArgs e) { Archive_Extract = true; }
        private void UIElement_Checkbox_ArchiveDump_Checked(object sender, RoutedEventArgs e) { Archive_Dump = true; }
        private void UIElement_Checkbox_ArchiveList_Checked(object sender, RoutedEventArgs e) { Archive_List = true; }
        private void UIElement_Checkbox_ArchiveUncook_Checked(object sender, RoutedEventArgs e) { Archive_Uncook = true; }
        private void UIElement_Checkbox_ArchiveUncook_Unchecked(object sender, RoutedEventArgs e) { Archive_Uncook = false; }
        private void UIElement_Checkbox_ArchiveList_Unchecked(object sender, RoutedEventArgs e) { Archive_List = false; }
        private void UIElement_Checkbox_ArchiveExtract_Unchecked(object sender, RoutedEventArgs e) { Archive_Extract = false; }
        private void UIElement_Checkbox_ArchiveDump_Unchecked(object sender, RoutedEventArgs e) { Archive_Dump = false; }
        private void UIElement_Button_ArchiveStart_Click(object sender, RoutedEventArgs e) { UISender(0); }
        private void UIElement_TextBox_ArchiveHash_TextChanged(object sender, TextChangedEventArgs e) { Archive_Hash = Convert.ToUInt64(UIElement_TextBox_ArchiveHash.Text); }
        private void UIElement_Button_ArchiveSelectArchive_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_ArchiveSelectArchive.Foreground = new SolidColorBrush(Colors.Black); }
        private void UIElement_Button_ArchiveSelectArchive_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_ArchiveSelectArchive.Foreground = new SolidColorBrush(Colors.Yellow); }
        private void UIElement_Button_ArchiveSelectOutputPath_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_ArchiveSelectOutputPath.Foreground = new SolidColorBrush(Colors.Black); }
        private void UIElement_Button_ArchiveSelectOutputPath_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_ArchiveSelectOutputPath.Foreground = new SolidColorBrush(Colors.Yellow); }
        private void UIElement_Button_ArchiveStart_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_ArchiveStart.Foreground = new SolidColorBrush(Colors.Black); }
        private void UIElement_Button_ArchiveStart_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_ArchiveStart.Foreground = new SolidColorBrush(Colors.Yellow); }
        //Dump Events
        private void UIElement_Button_DumpSelectArchiveOrDir_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_DumpSelectArchiveOrDir.Foreground = new SolidColorBrush(Colors.Black); }
        private void UIElement_Button_DumpSelectArchiveOrDir_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_DumpSelectArchiveOrDir.Foreground = new SolidColorBrush(Colors.Yellow); }
        private void UIElement_Button_DumpSelectOutputPath_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_DumpSelectOutputPath.Foreground = new SolidColorBrush(Colors.Black); }
        private void UIElement_Button_DumpSelectOutputPath_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_DumpSelectOutputPath.Foreground = new SolidColorBrush(Colors.Yellow); }
        private void UIElement_Checkbox_DumpImports_Checked(object sender, RoutedEventArgs e) { Dump_Imports = true; }
        private void UIElement_Checkbox_DumpImports_Unchecked(object sender, RoutedEventArgs e) { Dump_Imports = false; }
        private void UIElement_Checkbox_DumpMissingHashes_Checked(object sender, RoutedEventArgs e) { Dump_MissingHashes = true; }
        private void UIElement_Checkbox_DumpMissingHashes_Unchecked(object sender, RoutedEventArgs e) { Dump_MissingHashes = false; }
        private void UIElement_Checkbox_DumpInfo_Checked(object sender, RoutedEventArgs e) { Dump_Info = true; }
        private void UIElement_Checkbox_DumpInfo_Unchecked(object sender, RoutedEventArgs e) { Dump_Info = false; }
        private void UIElement_Button_DumpStart_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_DumpStart.Foreground = new SolidColorBrush(Colors.Black); }
        private void UIElement_Button_DumpStart_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_DumpStart.Foreground = new SolidColorBrush(Colors.Yellow); }


        private void UISender(int item)
        {
            switch (item)
            {
                case 0:

                    if (Archive_Path != "" && Archive_OutPath != "") { ConsoleFunctions.ArchiveTask(Archive_Path, Archive_OutPath, Archive_Extract, Archive_Dump, Archive_List, Archive_Uncook, Archive_UncookFileType, Archive_Hash); }
                    break;

                case 1:
                  //  if (Dump_Path != "" && Dump_OutPath != "") { ConsoleFunctions.DumpTask(Dump_Path, Dump_OutPath, Dump_Imports, Dump_MissingHashes, Dump_Info); }
                    break;

                case 2:
                  //  if (CR2W_Path != "" && CR2W_OutPath != "") { ConsoleFunctions.Cr2wTask(CR2W_Path, CR2W_OutPath, CR2W_All, CR2W_Chunks); }
                    break;

                case 3:
                    ConsoleFunctions.HashTask(Hash_Input, Hash_Missing);
                    break;

                case 4:
                   // if (CR2W_Path != "" && CR2W_OutPath != "") { ConsoleFunctions.OodleTask(Oodle_Path, Oodle_OutPath, Oodle_Decompress); }
                    break;
            }
        }

        private void UIElement_Button_ArchiveSelectArchive_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); openFileDialog.Multiselect = false; openFileDialog.Filter = "Archives (*.archive)|*.archive"; var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value) { UIElement_Archive_PathIndicator_Selected.Text = openFileDialog.SafeFileName; Archive_Path = openFileDialog.FileName; }
        }

        private void UIElement_Button_ArchiveSelectOutputPath_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog(); dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok) { UIElement_Archive_PathIndicator_Output.Text = dialog.FileName.ReverseTruncate(34); Archive_OutPath = dialog.FileName; }
        }

    }

    public static class StringExt
    {
        public static string ReverseTruncate(this string value, int maxLength)
        {
            var a = value.Length - maxLength;
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(a, maxLength);
        }
    }
}
