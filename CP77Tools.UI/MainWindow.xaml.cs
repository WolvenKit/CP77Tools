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
using WolvenKit.Common.Services;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using CP77Tools.UI.Functionality;
using CP77.Common.Services;
using System.Runtime.InteropServices;
using CP77Tools.UI.Functionality.Customs;

namespace CP77Tools.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 0 - Archive (EndUser)
    /// 1 - CR2W (EndUser)
    /// 2 - Repack (EndUser)
    /// 3 - Dump (Dev)
    /// 4 - Hash (Dev)
    /// 5 - Oodle (Dev)
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("lib/kraken.dll")]
        static extern int Kraken_Decompress(byte[] buffer, long bufferSize, byte[] outputBuffer, long outputBufferSize);

        [DllImport("lib/kraken.dll")]
        static extern int Kraken_Compress(byte[] buffer, long bufferSize, byte[] outputBuffer);

        //Other
        public ILoggerService UI_Logger;
        public Functionality.Logging log;
        public Functionality.UI ui;
        public Data.General data;
        public string[] InputFileTypes = { "Archives (*.archive)|*.archive" };

        public enum TaskType { Archive, Dump, CR2W, Hash, Oodle, Repack, }

        public Color ForeGroundTextColor = (Color)ColorConverter.ConvertFromString("#FFE5D90C");

   

        public MainWindow()
        {

            InitializeComponent();
            ServiceLocator.Default.RegisterType<IMainController, MainController>();
            ServiceLocator.Default.RegisterType<ILoggerService, LoggerService>();
            UI_Logger = ServiceLocator.Default.ResolveType<ILoggerService>();


            log = new Functionality.Logging(this);
            ui = new Functionality.UI(this);
            data = new Data.General();
            ui.SetToolTips();

            UI_Logger.PropertyChanged += log.UI_Logger_PropertyChanged;
            UI_Logger.OnStringLogged += log.UI_Logger_OnStringLogged;
            UI_Logger.PropertyChanging += log.UI_Logger_PropertyChanging;




            InitializeDefaults();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //thm.ApplyThemeDark(this);

        }

        private void InitializeDefaults()
        {
            data.Hash_Input = Hash_InputString_UIElement_TextBox.Text;

        }

        private void UIFunc_DragWindow(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) this.DragMove(); }

        // General Events
        private void Main_CloseButton_MouseEnter(object sender, MouseEventArgs e) { Main_CloseApp_UIElement_Image.Source = ImageCache.CloseSelected; }
        private void Main_CloseButton_MouseLeave(object sender, MouseEventArgs e) { Main_CloseApp_UIElement_Image.Source = ImageCache.Close; }
        private void Main_MinimizeButton_MouseEnter(object sender, MouseEventArgs e) { Main_MinimizeApp_UIElement_Image.Source = ImageCache.MinimizeSelected; }
        private void Main_MinimizeButton_MouseLeave(object sender, MouseEventArgs e) { Main_MinimizeApp_UIElement_Image.Source = ImageCache.Minimize; }
        private void Main_PreviousItems_MouseEnter(object sender, MouseEventArgs e) { Main_PreviousItems_UIElement_Image.Source = ImageCache.PageMoveSelected; }
        private void Main_PreviousItems_MouseLeave(object sender, MouseEventArgs e) { Main_PreviousItems_UIElement_Image.Source = ImageCache.PageMove; }
        private void Main_Button_NextItems_MouseEnter(object sender, MouseEventArgs e) { Main_NextItems_UIElement_Button.Opacity = 0.75; }
        private void Main_Button_NextItems_MouseLeave(object sender, MouseEventArgs e) { Main_NextItems_UIElement_Button.Opacity = 0.9; }
        private void Main_Button_NextItems_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { Main_NextItems_UIElement_Button.Opacity = 0.9; }
        private void Main_Button_PreviousItems_MouseEnter(object sender, MouseEventArgs e) { Main_PreviousItems_UIElement_Button.Opacity = 0.75; }
        private void Main_Button_PreviousItems_MouseLeave(object sender, MouseEventArgs e) { Main_PreviousItems_UIElement_Button.Opacity = 0.9; }
        private void Main_Button_PreviousItems_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { Main_PreviousItems_UIElement_Button.Opacity = 0.9; }

        // OnExit
        private void Main_CloseButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { System.Windows.Application.Current.Shutdown(); }

        // Minimize
        private void Main_MinimizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { this.WindowState = WindowState.Minimized; }

        // Prev/Next Items Click events
        private void Main_Button_PreviousItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { ui.PreviousItemsInView(); }
        private void Main_Button_NextItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { ui.NextItemsInView(); }
        private void Main_PreviousItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { ui.GoToFirstItemInView(); }

        //Archive events
        private void Archive_Button_ArchiveSelectArchive_Click(object sender, RoutedEventArgs e) { ui.OpenF(Data.General.OMD_Type.Multi, TaskType.Archive); }
        private void Archive_Button_ArchiveSelectOutputPath_Click(object sender, RoutedEventArgs e) { ui.OpenFolder(0); } // Will become OpenF after singlepicker update.
        private void Archive_Checkbox_ArchiveExtract_Checked(object sender, RoutedEventArgs e) { data.Archive_Extract = true; }
        private void Archive_Checkbox_ArchiveDump_Checked(object sender, RoutedEventArgs e) { data.Archive_Dump = true; }
        private void Archive_Checkbox_ArchiveList_Checked(object sender, RoutedEventArgs e) { data.Archive_List = true; }
        private void Archive_Checkbox_ArchiveUncook_Checked(object sender, RoutedEventArgs e) { data.Archive_Uncook = true; }
        private void Archive_Checkbox_ArchiveUncook_Unchecked(object sender, RoutedEventArgs e) { data.Archive_Uncook = false; }
        private void Archive_Checkbox_ArchiveList_Unchecked(object sender, RoutedEventArgs e) { data.Archive_List = false; }
        private void Archive_Checkbox_ArchiveExtract_Unchecked(object sender, RoutedEventArgs e) { data.Archive_Extract = false; }
        private void Archive_Checkbox_ArchiveDump_Unchecked(object sender, RoutedEventArgs e) { data.Archive_Dump = false; }
        private void Archive_Button_ArchiveStart_Click(object sender, RoutedEventArgs e) { ui.ThreadedTaskSender(0); Main_OutputBox_UIElement_ComboBox.Items.Clear(); }
        private void Archive_TextBox_ArchiveHash_TextChanged(object sender, TextChangedEventArgs e) { if (Archive_Hash_UIElement_TextBox.Text != null && data != null) { try { data.Archive_Hash = Convert.ToUInt64(Archive_Hash_UIElement_TextBox.Text); } catch { } } }
        private void Archive_Button_ArchiveSelectArchive_MouseEnter(object sender, MouseEventArgs e) { Archive_SelectArchive_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Archive_Button_ArchiveSelectArchive_MouseLeave(object sender, MouseEventArgs e) { Archive_SelectArchive_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Archive_Button_ArchiveSelectOutputPath_MouseEnter(object sender, MouseEventArgs e) { Archive_SelectOutputPath_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Archive_Button_ArchiveSelectOutputPath_MouseLeave(object sender, MouseEventArgs e) { Archive_SelectOutputPath_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Archive_Button_ArchiveStart_MouseEnter(object sender, MouseEventArgs e) { Archive_Start_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Archive_Button_ArchiveStart_MouseLeave(object sender, MouseEventArgs e) { Archive_Start_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Archive_RadioButton_ArchiveJPG_Checked(object sender, RoutedEventArgs e) { data.Archive_UncookFileType = EUncookExtension.jpg; }
        private void Archive_RadioButton_ArchiveBMP_Checked(object sender, RoutedEventArgs e) { data.Archive_UncookFileType = EUncookExtension.bmp; }
        private void Archive_RadioButton_ArchiveJPEG_Checked(object sender, RoutedEventArgs e) { data.Archive_UncookFileType = EUncookExtension.jpeg; }
        private void Archive_RadioButton_ArchivePNG_Checked(object sender, RoutedEventArgs e) { data.Archive_UncookFileType = EUncookExtension.png; }
        private void Archive_RadioButton_ArchiveDDS_Checked(object sender, RoutedEventArgs e) { data.Archive_UncookFileType = EUncookExtension.dds; }
        private void Archive_RadioButton_ArchiveTGA_Checked(object sender, RoutedEventArgs e) { if (data != null) data.Archive_UncookFileType = EUncookExtension.tga; }
        private void UIElement_TextBox_ArchivePattern_TextChanged(object sender, TextChangedEventArgs e) { if (Archive_Pattern_UIElement_TextBox.Text != null && data != null) { data.Archive_Pattern = Archive_Pattern_UIElement_TextBox.Text; } }
        private void UIElement_TextBox_ArchiveRegex_TextChanged(object sender, TextChangedEventArgs e) { if (Archive_Regex_UIElement_TextBox.Text != null && data != null) { data.Archive_Regex = Archive_Regex_UIElement_TextBox.Text; } }

        //Dump Events (DEV ONLY)
        private void DumpSelectArchiveOrDir_MouseEnter(object sender, MouseEventArgs e) { Dump_SelectArchiveOrDirectory_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void DumpSelectArchiveOrDir_MouseLeave(object sender, MouseEventArgs e) { Dump_SelectArchiveOrDirectory_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void DumpSelectOutputPath_MouseEnter(object sender, MouseEventArgs e) { Dump_SelectOutputPath_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void DumpSelectOutputPath_MouseLeave(object sender, MouseEventArgs e) { Dump_SelectOutputPath_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void DumpImports_Checked(object sender, RoutedEventArgs e) { data.Dump_Imports = true; }
        private void DumpImports_Unchecked(object sender, RoutedEventArgs e) { data.Dump_Imports = false; }
        private void DumpMissingHashes_Checked(object sender, RoutedEventArgs e) { data.Dump_MissingHashes = true; }
        private void DumpMissingHashes_Unchecked(object sender, RoutedEventArgs e) { data.Dump_MissingHashes = false; }
        private void DumpInfo_Checked(object sender, RoutedEventArgs e) { data.Dump_Info = true; }
        private void DumpInfo_Unchecked(object sender, RoutedEventArgs e) { data.Dump_Info = false; }
        private void DumpStart_MouseEnter(object sender, MouseEventArgs e) { Dump_Start_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void DumpStart_MouseLeave(object sender, MouseEventArgs e) { Dump_Start_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Dump_ClassInfo_UIElement_CheckBox_Checked(object sender, RoutedEventArgs e) { data.Dump_ClassInfo = true; }
        private void Dump_ClassInfo_UIElement_CheckBox_Unchecked(object sender, RoutedEventArgs e) { data.Dump_ClassInfo = false; }
        private void Dump_Start_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.ThreadedTaskSender(3); Main_OutputBox_UIElement_ComboBox.Items.Clear(); }
        private void Dump_SelectArchiveOrDirectory_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.OpenF(Data.General.OMD_Type.Single, TaskType.Dump); }
        private void Dump_SelectOutputPath_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.OpenFolder(3); }

        //CR2W
        private void CR2W_SelectFile_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.OpenF(Data.General.OMD_Type.Single, TaskType.CR2W); }
        private void CR2W_SelectFile_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { CR2W_SelectFile_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void CR2W_SelectFile_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { CR2W_SelectFile_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void CR2W_SelectOutputPath_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.OpenFolder(1); }
        private void CR2W_SelectOutputPath_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { CR2W_SelectOutputPath_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void CR2W_SelectOutputPath_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { CR2W_SelectOutputPath_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void CR2W_DumpChunks_UIElement_Checkbox_Checked(object sender, RoutedEventArgs e) { data.CR2W_Chunks = true; }
        private void CR2W_DumpChunks_UIElement_Checkbox_Unchecked(object sender, RoutedEventArgs e) { data.CR2W_Chunks = false; }
        private void CR2W_DumpAll_UIElement_Checkbox_Checked(object sender, RoutedEventArgs e) { data.CR2W_All = true; }
        private void CR2W_DumpAll_UIElement_Checkbox_Unchecked(object sender, RoutedEventArgs e) { data.CR2W_All = false; }
        private void CR2W_Start_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.ThreadedTaskSender(1); Main_OutputBox_UIElement_ComboBox.Items.Clear(); }
        private void CR2W_Start_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { CR2W_Start_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void CR2W_Start_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { CR2W_Start_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }

        //Repack
        private void Repack_SelectOutputPath_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.OpenFolder(2); }
        private void Repack_SelectOutputPath_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { Repack_SelectOutputPath_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Repack_SelectOutputPath_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { Repack_SelectOutputPath_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Repack_SelectArchiveOrDirectory_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.OpenF(Data.General.OMD_Type.Multi, TaskType.Repack); }
        private void Repack_SelectArchiveOrDirectory_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { Repack_SelectArchiveOrDirectory_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Repack_SelectArchiveOrDirectory_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { Repack_SelectArchiveOrDirectory_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Repack_Start_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.ThreadedTaskSender(2); Main_OutputBox_UIElement_ComboBox.Items.Clear(); }
        private void Repack_Start_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { Repack_Start_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Repack_Start_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { Repack_Start_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }

        // Hash
        private void Hash_Start_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.ThreadedTaskSender(4); Main_OutputBox_UIElement_ComboBox.Items.Clear(); }
        private void Hash_Start_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { Hash_Start_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Hash_Start_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { Hash_Start_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Hash_InputString_UIElement_TextBox_TextChanged(object sender, TextChangedEventArgs e) { if (data != null) { try { data.Hash_Input = Hash_InputString_UIElement_TextBox.Text; } catch { } } }

        //Oodle
        private void Oodle_SelectFile_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.OpenF(Data.General.OMD_Type.Single, TaskType.Oodle); }
        private void Oodle_SelectFile_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { Oodle_SelectFile_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Oodle_SelectFile_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { Oodle_SelectFile_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Oodle_SelectOut_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { Oodle_SelectOut_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Oodle_SelectOut_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { Oodle_SelectOut_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }
        private void Oodle_SelectOut_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.OpenFolder(5); }
        private void Oodle_Decompress_UIElement_CheckBox_Checked(object sender, RoutedEventArgs e) { data.Oodle_Decompress = true; }
        private void Oodle_Decompress_UIElement_CheckBox_Unchecked(object sender, RoutedEventArgs e) { data.Oodle_Decompress = false; }
        private void Oodle_Start_UIElement_Button_Click(object sender, RoutedEventArgs e) { ui.ThreadedTaskSender(5); Main_OutputBox_UIElement_ComboBox.Items.Clear(); }
        private void Oodle_Start_UIElement_Button_MouseEnter(object sender, MouseEventArgs e) { Oodle_Start_UIElement_Button.Foreground = new SolidColorBrush(Colors.Black); }
        private void Oodle_Start_UIElement_Button_MouseLeave(object sender, MouseEventArgs e) { Oodle_Start_UIElement_Button.Foreground = new SolidColorBrush(ForeGroundTextColor); }


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
