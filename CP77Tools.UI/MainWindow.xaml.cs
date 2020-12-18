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
using System.Threading;
using System.Windows.Threading;
using CP77Tools.UI.Functionality;

namespace CP77Tools.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     

        //Other
        public ILoggerService UI_Logger;
        public Functionality.Logging _Logging;
        public Functionality.UI _UI;
        public Data.General _General;
        public static MainWindow PuBWindow;
        public string[] InputFileTypes = { "Archives (*.archive)|*.archive" };

        public enum TaskType
        {
            Archive,
            Dump,
            CR2W,
            Hash,
            Oodle
        }

 




        public MainWindow()
        {
            InitializeComponent();
            ServiceLocator.Default.RegisterType<IMainController, MainController>();
            ServiceLocator.Default.RegisterType<ILoggerService, LoggerService>();
            UI_Logger = ServiceLocator.Default.ResolveType<ILoggerService>();

            PuBWindow = this;

            _Logging = new Functionality.Logging(this);
            _UI = new Functionality.UI(this);
            _General = new Data.General();

            _UI.SetToolTips();

            UI_Logger.PropertyChanged += _Logging.UI_Logger_PropertyChanged;
            UI_Logger.OnStringLogged += _Logging.UI_Logger_OnStringLogged;
            UI_Logger.PropertyChanging += _Logging.UI_Logger_PropertyChanging;


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
        private void UIElement_Button_PreviousItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { UIElement_Button_PreviousItems.Opacity = 0.6; if (_UI.CurrentPageIndex > 0) { UIElement_ItemList.ScrollIntoView(UIElement_ItemList.Items[_UI.Pagehandler(false)]); } }
        private void UIElement_Button_NextItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { UIElement_Button_NextItems.Opacity = 0.6; UIElement_ItemList.ScrollIntoView(UIElement_ItemList.Items[_UI.Pagehandler(true)]); }
        private void UIElement_PreviousItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { UIElement_ItemList.ScrollIntoView(UIElement_ItemList.Items[0]);}

        //Archive events
        private void UIElement_Button_ArchiveSelectArchive_Click(object sender, RoutedEventArgs e) { _UI.OpenFile(0); }
        private void UIElement_Button_ArchiveSelectOutputPath_Click(object sender, RoutedEventArgs e) { _UI.OpenFolder(0); }
        private void UIElement_Checkbox_ArchiveExtract_Checked(object sender, RoutedEventArgs e) { _General.Archive_Extract = true; }
        private void UIElement_Checkbox_ArchiveDump_Checked(object sender, RoutedEventArgs e) { _General.Archive_Dump = true; }
        private void UIElement_Checkbox_ArchiveList_Checked(object sender, RoutedEventArgs e) { _General.Archive_List = true; }
        private void UIElement_Checkbox_ArchiveUncook_Checked(object sender, RoutedEventArgs e) { _General.Archive_Uncook = true; }
        private void UIElement_Checkbox_ArchiveUncook_Unchecked(object sender, RoutedEventArgs e) { _General.Archive_Uncook = false; }
        private void UIElement_Checkbox_ArchiveList_Unchecked(object sender, RoutedEventArgs e) { _General.Archive_List = false; }
        private void UIElement_Checkbox_ArchiveExtract_Unchecked(object sender, RoutedEventArgs e) { _General.Archive_Extract = false; }
        private void UIElement_Checkbox_ArchiveDump_Unchecked(object sender, RoutedEventArgs e) { _General.Archive_Dump = false; }
        private void UIElement_Button_ArchiveStart_Click(object sender, RoutedEventArgs e) { _UI.ThreadedTaskSender(0); }
        private void UIElement_TextBox_ArchiveHash_TextChanged(object sender, TextChangedEventArgs e) { _General.Archive_Hash = Convert.ToUInt64(UIElement_TextBox_ArchiveHash.Text); }
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
        private void UIElement_Checkbox_DumpImports_Checked(object sender, RoutedEventArgs e) { _General.Dump_Imports = true; }
        private void UIElement_Checkbox_DumpImports_Unchecked(object sender, RoutedEventArgs e) { _General.Dump_Imports = false; }
        private void UIElement_Checkbox_DumpMissingHashes_Checked(object sender, RoutedEventArgs e) { _General.Dump_MissingHashes = true; }
        private void UIElement_Checkbox_DumpMissingHashes_Unchecked(object sender, RoutedEventArgs e) { _General.Dump_MissingHashes = false; }
        private void UIElement_Checkbox_DumpInfo_Checked(object sender, RoutedEventArgs e) { _General.Dump_Info = true; }
        private void UIElement_Checkbox_DumpInfo_Unchecked(object sender, RoutedEventArgs e) { _General.Dump_Info = false; }
        private void UIElement_Button_DumpStart_MouseEnter(object sender, MouseEventArgs e) { UIElement_Button_DumpStart.Foreground = new SolidColorBrush(Colors.Black); }
        private void UIElement_Button_DumpStart_MouseLeave(object sender, MouseEventArgs e) { UIElement_Button_DumpStart.Foreground = new SolidColorBrush(Colors.Yellow); }
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
