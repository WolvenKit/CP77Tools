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
                            case "New Task":                         
                                Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Custom;
                                break;
                            case "Extract":                              
                                Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Extract;
                                break;
                            case "Uncook":                         
                                Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Uncook;
                                break;
                            case "Dump":                             
                                Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Dump;
                                break;
                            case "List":                            
                                Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.List;
                                break;
                            case "Extract Single":                             
                                Functionality.UserInterfaceLogic.selectedArchiveTaskType = ArchiveData.TaskType.Single;
                                break;
                        }
                        break;
                    case "CR2W":
                        break;
                    case "Repack":
                        break;
                    case "Dump":
                        break;
                    case "Hash":
                        break;
                    case "Oodle":
                        break;
                }

               
                Trace.Write(Functionality.UserInterfaceLogic.selectedArchiveTaskType);
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
            if (ToolsLogFlyOut.IsOpen)
            {
                LogsLabel.Visibility = Visibility.Hidden;

            }
            else
            {
                LogsLabel.Visibility = Visibility.Visible;

            }
        }


    }









}
