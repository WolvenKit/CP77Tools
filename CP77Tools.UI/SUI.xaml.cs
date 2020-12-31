using Catel.IoC;
using ControlzEx.Theming;
using CP77.Common.Services;
using CP77Tools.UI.Data;
using CP77Tools.UI.Functionality;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using CP77Tools.UI.Data.Tasks;
using CP77Tools.UI.Views;
using CP77Tools.UI.Views.Pages;
using CP77.CR2W.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using CP77Tools.UI.Views.Notifications;
using System.Diagnostics;

namespace CP77Tools.UI
{
    /// <summary>
    /// Interaction logic for SUI.xaml
    /// </summary>
    /// 

   public class NewerMenuItem
    {

    }

    
    public partial class SUI : MetroWindow
    {
        public ILoggerService UI_Logger;
  
        public General generaldata;

        public static SUI sui; // Pfft
        public ArchiveData archivedata;
        public DumpData dumpdata;
        public OodleData oodledata;
        public CR2WData cr2wdata;
        public HashData hashdata;
        public RepackData repackdata;
        public Logging log;
        public UserInterfaceLogic ui;


        //tabs
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




        //

        public Tools tools;
        public AboutPage about;
        public SettingsPage settings;
        public MainPage main;
        public SUI()
        {



            InitializeComponent();
            sui = this;

            generaldata = new General();

            archivedata = new ArchiveData();
            dumpdata = new DumpData();
            oodledata = new OodleData();
            cr2wdata = new CR2WData();
            hashdata = new HashData();
            repackdata = new RepackData();
            log = new Logging(sui);
            ui = new UserInterfaceLogic(sui);



             tools = new Tools();
             settings = new SettingsPage();

             about = new AboutPage();
            main = (MainPage)tab1.Content;
            tab2.Content = tools;
            tab3.Content = settings;
            tab4.Content = about;
            TabControlMain.SelectedIndex = 1;


           

            //  tabItem.Content = tools;
            //  MetroTabItem tabItem = new MetroMenuTabItem;
            //   TabItem tabItem = new TabItem();

            //   tabItem.Header = "Tools";
            //   tabItem.HeaderTemplate = MetroMenuTabItem.HeaderTemplate;

            //   Tools tools = new Tools();

            //  tabItem.Content = tools;

            //   TabControlMain.Items.Add(tabItem);

            // Properties.Settings.Default.Save();
            ItemContainerTemplate a = new ItemContainerTemplate();


            ThemeManager.Current.ChangeTheme(this, (Properties.Settings.Default.Theme + "." + Properties.Settings.Default.ThemeAccent));
            tools.RefreshTheme();

            //  this.navigationServiceEx = new NavigationServiceEx();
            // this.navigationServiceEx.Navigated += this.NavigationServiceEx_OnNavigated;
            // this.HamburgerMenuControl.Content = this.navigationServiceEx.Frame;
            ServiceLocator.Default.RegisterType<ILoggerService, LoggerService>();
            ServiceLocator.Default.RegisterType<IHashService, HashService>();
            ServiceLocator.Default.RegisterType<IAppSettingsService, AppSettingsService>();

            UI_Logger = ServiceLocator.Default.ResolveType<ILoggerService>();
            var hashService = ServiceLocator.Default.ResolveType<IHashService>();
            hashService.ReloadLocally();

            UI_Logger.PropertyChanged += log.UI_Logger_PropertyChanged;
            UI_Logger.OnStringLogged += log.UI_Logger_OnStringLogged;
            UI_Logger.PropertyChanging += log.UI_Logger_PropertyChanging;


            this.ShowMessageAsync("Happy New Year!", 
                "The CP77Tools and CDPR Modding Community wish you a happy new year!");



         // backgroundworker();





            // Navigate to the home page.
            //  this.Loaded += (sender, args) => this.navigationServiceEx.Navigate(new Uri("Views/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }


        
           
        



        public async void ProgressDialogHelper(string title, string message)
        {

            Application.Current.Dispatcher.BeginInvoke(new Action(async() =>
            {
                UserInterfaceLogic.controller = await this.ShowProgressAsync(title,message, false);
                

            }));


        }



        private void NavigationServiceEx_OnNavigated(object sender, NavigationEventArgs e)
        {
            //// select the menu item
            //this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
            //                                             .Items
            //                                             .OfType<MenuItem>()
            //                                             .FirstOrDefault(x => x.NavigationDestination == e.Uri);
            //this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
            //                                                    .OptionsItems
            //                                                    .OfType<MenuItem>()
            //                                                    .FirstOrDefault(x => x.NavigationDestination == e.Uri);

         //    this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
          //                                                .Items
           //                                               .OfType<MenuItem>()
          //                                                .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());
        //    this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
        //                                                        .OptionsItems
       //                                                         .OfType<MenuItem>()
        //                                                        .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());
//
       //     this.GoBackButton.Visibility = this.navigationServiceEx.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            //this.navigationServiceEx.GoBack();
        }

        private void HamburgerMenuControl_ItemClick(object sender, ItemClickEventArgs args)
        {
       
        }

        private void tab45_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
         
        }

   

        private void Button_Click_1()
        {


            foreach (MetroTabItem a in TabControlMain.Items)
            {
                if (a.Header == "Home")

                {
                    a.MaxHeight = 89;
                    a.Height = 89;

                }
                else
                {
                    a.MaxHeight = 74;
                    a.Height = 74;

                }


                if (a.Width == 50)
                {
                    a.MinWidth = 154; a.Width = 154;

                }
                else
                {
                    a.MinWidth = 50; a.Width = 50;

                }


            }
        }

        private int LastSelectedPageIndex;
        private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControlMain.SelectedIndex == 0)
            {

                Button_Click_1();
                MetroTabItem b = (MetroTabItem)TabControlMain.Items[1];

                MetroTabItem a = (MetroTabItem)TabControlMain.SelectedItem;
                a.Content = b.Content;


             
            }
            else
            {
                LastSelectedPageIndex = TabControlMain.SelectedIndex;

            }

        }

        private void tab45_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TabControlMain.SelectedIndex == 0)
            {

                Button_Click_1();
                MetroTabItem b = (MetroTabItem)TabControlMain.Items[1];

                MetroTabItem a = (MetroTabItem)TabControlMain.SelectedItem;
                a.Content = b.Content;



            }
            else
            {
                LastSelectedPageIndex = TabControlMain.SelectedIndex;

            }
        }

        private void PackIconFeatherIcons_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (TabControlMain.SelectedIndex == 0)
            {

                Button_Click_1();
                MetroTabItem b = (MetroTabItem)TabControlMain.Items[1];

                MetroTabItem a = (MetroTabItem)TabControlMain.SelectedItem;
                a.Content = b.Content;



            }
            else
            {
                LastSelectedPageIndex = TabControlMain.SelectedIndex;

            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Button_Click_1();
        }
    }
}