using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ControlzEx.Theming;
using CP77Tools.UI.Data;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Views;
using CP77Tools.UI.Views.Pages;
using CP77Tools.UI.Views.Tabs.Archive;
using CP77Tools.UI.Views.Tabs.CR2W;

using MahApps.Metro.Controls;
using Color = System.Windows.Media.Color;

namespace CP77Tools.UI.Functionality.Customs
{
    /// <summary>
    /// Interaction logic for OMD.xaml
    /// </summary>
    public partial class OMD : MetroWindow
    {



        public General.OMD_Type OMD_CurrentOMDType;  // Multi Or Single
        public Views.Tabs.Archive.CustomTab ArchiveCustomTab;
        public Views.Tabs.Archive.DumpTab ArchiveDumpTab;
        public Views.Tabs.Archive.ExtractSingleTab ArchiveSingleTab;
        public Views.Tabs.Archive.ExtractTab ArchiveExtractTab;
        public Views.Tabs.Archive.ListTab ArchiveListTab;
        public Views.Tabs.Archive.UncookTab ArchiveUncookTab;

        public Views.Tabs.CR2W.CustomTab Cr2wCustomTab;
        public Views.Tabs.CR2W.ClassInfoTab Cr2WClassInfoTab;
        public Views.Tabs.CR2W.AllInfoTab Cr2wAllInfoTab;


        public Views.Tabs.Dump.ClassInfoTab DumpClassInfoTab;
        public Views.Tabs.Dump.CustomTab DumpCustomTab;
        public Views.Tabs.Dump.ImportsTab DumpImportsTab;
        public Views.Tabs.Dump.MissingHashesTab DumpMissingTab;
        public Views.Tabs.Dump.XbmInfoTab DumpXbmTab;


        public Views.Tabs.Hash.HashTab HashHashTab;
        public Views.Tabs.Oodle.DecompressTab OodleDecompressTab;
        public Views.Tabs.Repack.RepackTab RepackRepackTab;


        public enum ForF { File, Folder }
        private void UIFunc_DragWindow(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) this.DragMove(); }
        public Color ForeGroundTextColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFE5D90C");
        private General.TaskType CurrentTaskType;
        private SUI sui;
        private bool OutputSelector;
        private General.OMD_Type CurrentOMDType;

        public OMD(General.TaskType GivenTaskType,General.OMD_Type _OMD_Type, bool _OutputSelector)
        {

            CurrentTaskType = GivenTaskType;
            CurrentOMDType = _OMD_Type;
            OutputSelector = _OutputSelector;
            InitializeComponent();
            InitializeFileSystemObjects();
            ThemeManager.Current.ChangeTheme(this, "Dark.Steel");
        }


        private void InitializeFileSystemObjects()
        {
            var drives = DriveInfo.GetDrives();
            DriveInfo.GetDrives().ToList().ForEach(drive =>
            {
                var fileSystemObject = new FileSystemObjectInfo(drive);
                fileSystemObject.BeforeExplore += FileSystemObject_BeforeExplore;
                fileSystemObject.AfterExplore += FileSystemObject_AfterExplore;
                OMD_FileTreeView.Items.Add(fileSystemObject);
            });
        }
        private void FileSystemObject_AfterExplore(object sender, System.EventArgs e) { Cursor = Cursors.Arrow; }
        private void FileSystemObject_BeforeExplore(object sender, System.EventArgs e) { Cursor = Cursors.Wait; }

        private void OMD_FileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var q = OMD_FileTreeView.SelectedItem as FileSystemObjectInfo;
            if (CurrentOMDType == General.OMD_Type.Multi) { OMD_ListboxSelected.Items.Add(q.FileSystemInfo.FullName); }
            else if (CurrentOMDType == General.OMD_Type.Single) { OMD_ListboxSelected.Items.Clear(); OMD_ListboxSelected.Items.Add(q.FileSystemInfo.FullName); }
        }

        private void OMD_ListboxSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OMD_ListboxSelected.Items.Remove(OMD_ListboxSelected.SelectedItem);
        }

        public ArchiveData.TaskType archivesubtasktype;
        public DumpData.DumpTaskType dumpsubtasktype;
        public OodleData.OodleTaskType oodlesubtasktype;
        public CR2WData.CR2WTaskType cr2wsubtasktype;
        public RepackData.RepackTaskType repacksubtasktype;
        public HashData.HashTaskType hashsubtrasktype;

        private void OMD_ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
           

            string[] clist = OMD_ListboxSelected.Items.OfType<string>().ToArray();
            var d = clist.Distinct().ToArray();
            General.OMD_Output = d;

            switch (CurrentTaskType)
            {
                case General.TaskType.Archive:

                    switch (archivesubtasktype)
                    {
                        case ArchiveData.TaskType.Custom:
                            if (OutputSelector)
                            {
                                SUI.sui.archivedata.Archive_OutPath = d[0];
                                ArchiveCustomTab.ChangeCollectionData("Selected Outpath", d[0]);
                                ArchiveCustomTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                if (ArchiveData.ArchiveConceptTaskDict.ContainsKey("Selected Input"))
                                {
                                    ArchiveData.ArchiveConceptTaskDict.Remove("Selected Input");
                                }
                                SUI.sui.archivedata.Archive_Path = d;
                                ArchiveData.ArchiveConceptTaskDict.Add("Selected Input", d[0] + " (+" + (d.Length - 1) + ")");
                                ArchiveCustomTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";

                                ArchiveCustomTab.ArchiveTaskConceptGrid.ItemsSource = null;
                                ArchiveCustomTab.ArchiveTaskConceptGrid.ItemsSource = ArchiveData.ArchiveConceptTaskDict;

                                ArchiveCustomTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                ArchiveCustomTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                ArchiveCustomTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case ArchiveData.TaskType.Dump:
                            if (OutputSelector)
                            {
                                SUI.sui.archivedata.Archive_OutPath = d[0];
                                ArchiveDumpTab.OutpathLabel.Content = d[0];
                            }
                            else
                            {                
                                SUI.sui.archivedata.Archive_Path = d;
                                ArchiveDumpTab.InputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                ArchiveDumpTab.DumpSelectedInputConceptDropDown1.Items.Clear();
                                ArchiveDumpTab.DumpSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                ArchiveDumpTab.DumpSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case ArchiveData.TaskType.Extract:
                            if (OutputSelector)
                            {
                                SUI.sui.archivedata.Archive_OutPath = d[0];
                                ArchiveExtractTab.OutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.archivedata.Archive_Path = d;
                                ArchiveExtractTab.InputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                ArchiveExtractTab.SelectedInputConceptDropDown1.Items.Clear();
                                ArchiveExtractTab.SelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                ArchiveExtractTab.SelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case ArchiveData.TaskType.List:
                            if (OutputSelector)
                            {
                                SUI.sui.archivedata.Archive_OutPath = d[0];
                                ArchiveListTab.OutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.archivedata.Archive_Path = d;
                                ArchiveListTab.InputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                ArchiveListTab.SelectedInputConceptDropDown1.Items.Clear();
                                ArchiveListTab.SelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                ArchiveListTab.SelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case ArchiveData.TaskType.Single:
                            if (OutputSelector)
                            {
                                SUI.sui.archivedata.Archive_OutPath = d[0];
                                ArchiveSingleTab.OutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.archivedata.Archive_Path = d;
                                ArchiveSingleTab.InputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                ArchiveSingleTab.SelectedInputConceptDropDown1.Items.Clear();
                                ArchiveSingleTab.SelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                ArchiveSingleTab.SelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case ArchiveData.TaskType.Uncook:
                            if (OutputSelector)
                            {
                                SUI.sui.archivedata.Archive_OutPath = d[0];
                                ArchiveUncookTab.OutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.archivedata.Archive_Path = d;
                                ArchiveUncookTab.InputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                ArchiveUncookTab.SelectedInputConceptDropDown1.Items.Clear();
                                ArchiveUncookTab.SelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                ArchiveUncookTab.SelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                    }
                   

                    break;
                case General.TaskType.CR2W:

                    switch (cr2wsubtasktype)
                    {
                        case CR2WData.CR2WTaskType.All:
                            if (OutputSelector)
                            {
                                SUI.sui.cr2wdata.CR2W_OutPath = d[0];
                                Cr2wAllInfoTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.cr2wdata.CR2W_Path = d;

                                Cr2wAllInfoTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                Cr2wAllInfoTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                Cr2wAllInfoTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                Cr2wAllInfoTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }

                            break;
                        case CR2WData.CR2WTaskType.Chunks:
                            if (OutputSelector)
                            {
                                SUI.sui.cr2wdata.CR2W_OutPath = d[0];
                                Cr2WClassInfoTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.cr2wdata.CR2W_Path = d;

                                Cr2WClassInfoTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                Cr2WClassInfoTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                Cr2WClassInfoTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                Cr2WClassInfoTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case CR2WData.CR2WTaskType.Custom:
                            if (OutputSelector)
                            {
                                SUI.sui.cr2wdata.CR2W_OutPath = d[0];
                                Cr2wCustomTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.cr2wdata.CR2W_Path = d;

                                Cr2wCustomTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                Cr2wCustomTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                Cr2wCustomTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                Cr2wCustomTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                    }
            
                    break;
                case General.TaskType.Dump:
                    switch (dumpsubtasktype)
                    {
                        case DumpData.DumpTaskType.ClassInfo:
                            if (OutputSelector)
                            {
                                SUI.sui.dumpdata.Dump_OutPath = d[0];
                                DumpClassInfoTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.dumpdata.Dump_Path = d;
                                DumpClassInfoTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                DumpClassInfoTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                DumpClassInfoTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                DumpClassInfoTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case DumpData.DumpTaskType.Custom:
                            if (OutputSelector)
                            {
                                SUI.sui.dumpdata.Dump_OutPath = d[0];
                                DumpCustomTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.dumpdata.Dump_Path = d;
                                DumpCustomTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                DumpCustomTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                DumpCustomTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                DumpCustomTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case DumpData.DumpTaskType.Imports:
                            if (OutputSelector)
                            {
                                SUI.sui.dumpdata.Dump_OutPath = d[0];
                                DumpImportsTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.dumpdata.Dump_Path = d;
                                DumpImportsTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                DumpImportsTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                DumpImportsTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                DumpImportsTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case DumpData.DumpTaskType.Info:
                            if (OutputSelector)
                            {
                                SUI.sui.dumpdata.Dump_OutPath = d[0];
                                DumpXbmTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.dumpdata.Dump_Path = d;
                                DumpXbmTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                DumpXbmTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                DumpXbmTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                DumpXbmTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                        case DumpData.DumpTaskType.MissingHashes:
                            if (OutputSelector)
                            {
                                SUI.sui.dumpdata.Dump_OutPath = d[0];
                                DumpMissingTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.dumpdata.Dump_Path = d;
                                DumpMissingTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                DumpMissingTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                DumpMissingTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                DumpMissingTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                    }
                
                    break;



                case General.TaskType.Hash:
                    switch (hashsubtrasktype)
                    {
                        case HashData.HashTaskType.Custom:
                            if (OutputSelector)
                            {
                            }
                            else
                            {

                            }
                            break;              
                    }
                    /////// HASH DO SOMETHING HERE
                    break;






                case General.TaskType.Oodle:
                    switch (oodlesubtasktype)
                    {               
                        case OodleData.OodleTaskType.Decompress:
                            if (OutputSelector)
                            {
                                SUI.sui.oodledata.Oodle_OutPath = d[0];
                                OodleDecompressTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                SUI.sui.oodledata.Oodle_Path = d;
                                OodleDecompressTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                OodleDecompressTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                OodleDecompressTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                OodleDecompressTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                    }
                 
                    break;



                case General.TaskType.Repack:
                    switch (repacksubtasktype)
                    {
                        case RepackData.RepackTaskType.Repack:
                            if (OutputSelector)
                            {
                                SUI.sui.repackdata.Repack_OutPath = d[0];
                                RepackRepackTab.ArchiveOutpathLabel.Content = d[0];
                            }
                            else
                            {
                                                    SUI.sui.repackdata.Repack_Path = d;
                                RepackRepackTab.ArchiveInputLabel.Content = d[0] + " (+" + (d.Length - 1) + ")";
                                RepackRepackTab.ArchiveSelectedInputConceptDropDown1.Items.Clear();
                                RepackRepackTab.ArchiveSelectedInputConceptDropDown1.ItemsSource = d.ToList();
                                RepackRepackTab.ArchiveSelectedInputConceptDropDown1.SelectedIndex = 0;
                            }
                            break;
                    }
                    
                    break;
            }
            this.Close();


        }

        private void OMD_ClearButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }




    [Serializable]
    public abstract class PropertyNotifier : INotifyPropertyChanged
    {
        public PropertyNotifier() : base() { }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }
    [Serializable]
    public abstract class BaseObject : PropertyNotifier
    {
        private IDictionary<string, object> m_values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        public T GetValue<T>(string key) { var value = GetValue(key); return (value is T) ? (T)value : default(T); }
        private object GetValue(string key) { if (string.IsNullOrEmpty(key)) { return null; } return m_values.ContainsKey(key) ? m_values[key] : null; }
        public void SetValue(string key, object value)
        {
            if (!m_values.ContainsKey(key)) { m_values.Add(key, value); }
            else { m_values[key] = value; }
            OnPropertyChanged(key);
        }
    }
    public static class Interop
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string path,
            uint attributes,
            out ShellFileInfo fileInfo,
            uint size,
            uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr pointer);
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct ShellFileInfo
    {
        public IntPtr hIcon; public int iIcon; public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }
    public enum FileAttribute : uint { Directory = 16, File = 256 }
    [Flags]
    public enum ShellAttribute : uint
    {
        LargeIcon = 0, SmallIcon = 1, OpenIcon = 2,
        ShellIconSize = 4, Pidl = 8, UseFileAttributes = 16, AddOverlays = 32, OverlayIndex = 64, Others = 128, Icon = 256, DisplayName = 512, TypeName = 1024, Attributes = 2048, IconLocation = 4096, ExeType = 8192, SystemIconIndex = 16384, LinkOverlay = 32768, Selected = 65536, AttributeSpecified = 131072
    }
    public enum IconSize : short { Small, Large }
    public enum ItemState : short { Undefined, Open, Close }
    public enum ItemType { Drive, Folder, File }
    public class FileSystemObjectInfo : BaseObject
    {
        public FileSystemObjectInfo(DriveInfo drive) : this(drive.RootDirectory) { }
        public FileSystemObjectInfo(FileSystemInfo info)
        {
            if (this is DummyFileSystemObjectInfo) { return; }
            Children = new ObservableCollection<FileSystemObjectInfo>(); FileSystemInfo = info;
            if (info is DirectoryInfo) { ImageSource = FolderManager.GetImageSource(info.FullName, ItemState.Close); AddDummy(); }
            else if (info is FileInfo) { ImageSource = FileManager.GetImageSource(info.FullName); }
            PropertyChanged += new PropertyChangedEventHandler(FileSystemObjectInfo_PropertyChanged);
            void FileSystemObjectInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (FileSystemInfo is DirectoryInfo)
                {
                    if (string.Equals(e.PropertyName, "IsExpanded", StringComparison.CurrentCultureIgnoreCase))
                    {
                        RaiseBeforeExpand();
                        if (IsExpanded)
                        {
                            ImageSource = FolderManager.GetImageSource(FileSystemInfo.FullName, ItemState.Open);
                            if (HasDummy()) { RaiseBeforeExplore(); RemoveDummy(); ExploreDirectories(); ExploreFiles(); RaiseAfterExplore(); }
                        }
                        else { ImageSource = FolderManager.GetImageSource(FileSystemInfo.FullName, ItemState.Close); }
                        RaiseAfterExpand();
                    }
                }
            }
        }
        public event EventHandler BeforeExpand;
        public event EventHandler AfterExpand;
        public event EventHandler BeforeExplore;
        public event EventHandler AfterExplore;
        private void RaiseBeforeExpand() { BeforeExpand?.Invoke(this, EventArgs.Empty); }
        private void RaiseAfterExpand() { AfterExpand?.Invoke(this, EventArgs.Empty); }
        private void RaiseBeforeExplore() { BeforeExplore?.Invoke(this, EventArgs.Empty); }
        private void RaiseAfterExplore() { AfterExplore?.Invoke(this, EventArgs.Empty); }
        public ObservableCollection<FileSystemObjectInfo> Children { get { return base.GetValue<ObservableCollection<FileSystemObjectInfo>>("Children"); } private set { base.SetValue("Children", value); } }
        public ImageSource ImageSource { get { return base.GetValue<ImageSource>("ImageSource"); } private set { base.SetValue("ImageSource", value); } }
        public bool IsExpanded { get { return base.GetValue<bool>("IsExpanded"); } set { base.SetValue("IsExpanded", value); } }
        public FileSystemInfo FileSystemInfo { get { return base.GetValue<FileSystemInfo>("FileSystemInfo"); } private set { base.SetValue("FileSystemInfo", value); } }
        private DriveInfo Drive { get { return base.GetValue<DriveInfo>("Drive"); } set { base.SetValue("Drive", value); } }
        private void AddDummy() { this.Children.Add(new DummyFileSystemObjectInfo()); }
        private bool HasDummy() { return !object.ReferenceEquals(this.GetDummy(), null); }
        private DummyFileSystemObjectInfo GetDummy() { var list = this.Children.OfType<DummyFileSystemObjectInfo>().ToList(); if (list.Count > 0) return list.First(); return null; }
        private void RemoveDummy() { this.Children.Remove(this.GetDummy()); }
        private void ExploreDirectories()
        {
            if (Drive?.IsReady == false) { return; }
            if (FileSystemInfo is DirectoryInfo)
            {
                try
                {
                    var directories = ((DirectoryInfo)FileSystemInfo).GetDirectories();
                    foreach (var directory in directories.OrderBy(d => d.Name))
                    {
                        if ((directory.Attributes & FileAttributes.System) != FileAttributes.System &&
                            (directory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                        {
                            var fileSystemObject = new FileSystemObjectInfo(directory);
                            fileSystemObject.BeforeExplore += FileSystemObject_BeforeExplore;
                            fileSystemObject.AfterExplore += FileSystemObject_AfterExplore;
                            Children.Add(fileSystemObject);
                        }
                    }
                }
                catch (UnauthorizedAccessException e) { }
            }
        }

        private void FileSystemObject_AfterExplore(object sender, EventArgs e) { RaiseAfterExplore(); }
        private void FileSystemObject_BeforeExplore(object sender, EventArgs e) { RaiseBeforeExplore(); }
        private void ExploreFiles()
        {
            if (Drive?.IsReady == false) { return; }
            if (FileSystemInfo is DirectoryInfo)
            {
                try
                {
                    var files = ((DirectoryInfo)FileSystemInfo).GetFiles();
                    foreach (var file in files.OrderBy(d => d.Name))
                    {
                        if ((file.Attributes & FileAttributes.System) != FileAttributes.System && (file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) { Children.Add(new FileSystemObjectInfo(file)); }
                    }
                }
                catch (UnauthorizedAccessException e) { }
            }
        }
    }
    internal class DummyFileSystemObjectInfo : FileSystemObjectInfo { public DummyFileSystemObjectInfo() : base(new DirectoryInfo("DummyFileSystemObjectInfo")) { } }
    public class ShellManager
    {
        public static Icon GetIcon(string path, ItemType type, IconSize iconSize, ItemState state)
        {
            var attributes = (uint)(type == ItemType.Folder ? FileAttribute.Directory : FileAttribute.File);
            var flags = (uint)(ShellAttribute.Icon | ShellAttribute.UseFileAttributes);
            if (type == ItemType.Folder && state == ItemState.Open) { flags = flags | (uint)ShellAttribute.OpenIcon; }
            if (iconSize == IconSize.Small) { flags = flags | (uint)ShellAttribute.SmallIcon; }
            else { flags = flags | (uint)ShellAttribute.LargeIcon; }
            var fileInfo = new ShellFileInfo();
            var size = (uint)Marshal.SizeOf(fileInfo);
            var result = Interop.SHGetFileInfo(path, attributes, out fileInfo, size, flags);
            if (result == IntPtr.Zero) { throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()); }
            try { return (Icon)Icon.FromHandle(fileInfo.hIcon).Clone(); }
            catch { throw; }
            finally { Interop.DestroyIcon(fileInfo.hIcon); }
        }
    }

    public static class FolderManager
    {
        public static ImageSource GetImageSource(string directory, ItemState folderType) { return GetImageSource(directory, new System.Drawing.Size(16, 16), folderType); }
        public static ImageSource GetImageSource(string directory, System.Drawing.Size size, ItemState folderType)
        {
            using (var icon = ShellManager.GetIcon(directory, ItemType.Folder, IconSize.Large, folderType))
            {
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(size.Width, size.Height));
            }
        }
    }
    public static class FileManager
    {
        public static ImageSource GetImageSource(string filename) { return GetImageSource(filename, new System.Drawing.Size(16, 16)); }
        public static ImageSource GetImageSource(string filename, System.Drawing.Size size)
        {
            using (var icon = ShellManager.GetIcon(System.IO.Path.GetExtension(filename), ItemType.File, IconSize.Small, ItemState.Undefined))
            {
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(size.Width, size.Height));
            }
        }
    }
}
