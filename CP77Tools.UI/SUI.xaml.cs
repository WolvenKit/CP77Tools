using Catel.IoC;
using ControlzEx.Theming;
using CP77.Common.Services;
using CP77Tools.UI.Data;
using CP77Tools.UI.Navigation;
using CP77Tools.UI.ViewModels;
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

namespace CP77Tools.UI
{
    /// <summary>
    /// Interaction logic for SUI.xaml
    /// </summary>
    public partial class SUI : MetroWindow
    {
        private readonly NavigationServiceEx navigationServiceEx;
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




        public SUI()
        {
            InitializeComponent();
            //Tools a = new Tools();
            sui = this;
            log = new Logging(sui);
            generaldata = new General();
            Properties.Settings.Default.Save();

            ThemeManager.Current.ChangeTheme(this, generaldata.ThemeHelper("Dark.Steel"));


            this.navigationServiceEx = new NavigationServiceEx();
            this.navigationServiceEx.Navigated += this.NavigationServiceEx_OnNavigated;
            this.HamburgerMenuControl.Content = this.navigationServiceEx.Frame;
            ServiceLocator.Default.RegisterType<ILoggerService, LoggerService>();
            ServiceLocator.Default.RegisterType<IHashService, HashService>();
            ServiceLocator.Default.RegisterType<IAppSettingsService, AppSettingsService>();

            UI_Logger = ServiceLocator.Default.ResolveType<ILoggerService>();
            var hashService = ServiceLocator.Default.ResolveType<IHashService>();
            hashService.ReloadLocally();

            UI_Logger.PropertyChanged += log.UI_Logger_PropertyChanged;
            UI_Logger.OnStringLogged += log.UI_Logger_OnStringLogged;
            UI_Logger.PropertyChanging += log.UI_Logger_PropertyChanging;

            archivedata = new ArchiveData();
            dumpdata = new DumpData();
            oodledata = new OodleData();
            cr2wdata = new CR2WData();
            hashdata = new HashData();
            repackdata = new RepackData();
       
            ui = new UserInterfaceLogic(sui);

            Thread worker = new Thread(backgroundworker); worker.IsBackground = true; worker.Start();

            // Navigate to the home page.
            this.Loaded += (sender, args) => this.navigationServiceEx.Navigate(new Uri("Views/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }
        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            if (e.InvokedItem is MenuItem menuItem && menuItem.IsNavigation)
            {
                this.navigationServiceEx.Navigate(menuItem.NavigationDestination);
            }
        }


        public void backgroundworker()
        {
            while (true)
            {
            
                    Thread.Sleep(100);
                    if (General.cq.IsEmpty)
                    {

                    }
                    else
                    {
                        Task retvalue;
                        var a = General.cq.TryDequeue(out retvalue);
                        retvalue.Start();
                        retvalue.Wait();
                    }
                
                
              
            }
        }

        private void NavigationServiceEx_OnNavigated(object sender, NavigationEventArgs e)
        {
            // select the menu item
            this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
                                                         .Items
                                                         .OfType<MenuItem>()
                                                         .FirstOrDefault(x => x.NavigationDestination == e.Uri);
            this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
                                                                .OptionsItems
                                                                .OfType<MenuItem>()
                                                                .FirstOrDefault(x => x.NavigationDestination == e.Uri);

            // or when using the NavigationType on menu item
            // this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
            //                                              .Items
            //                                              .OfType<MenuItem>()
            //                                              .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());
            // this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
            //                                                     .OptionsItems
            //                                                     .OfType<MenuItem>()
            //                                                     .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());

            // update back button
            this.GoBackButton.Visibility = this.navigationServiceEx.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.navigationServiceEx.GoBack();
        }
    }
}