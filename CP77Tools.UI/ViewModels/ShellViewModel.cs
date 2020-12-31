using CP77Tools.UI.Views.Pages;
using MahApps.Metro.IconPacks;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace CP77Tools.UI.ViewModels
{
    class ShellViewModel : BindableBase
    {
        private static readonly ObservableCollection<MenuItem> AppMenu = new ObservableCollection<MenuItem>();
        private static readonly ObservableCollection<MenuItem> AppOptionsMenu = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> Menu => AppMenu;

        public ObservableCollection<MenuItem> OptionsMenu => AppOptionsMenu;

        public ShellViewModel()
        {
            // Build the menus
            this.Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Tool },
                Label = "Tools",
         //       NavigationType = typeof(Tools),
          //      NavigationDestination = new Uri("Views/Pages/ToolsPage.xaml", UriKind.RelativeOrAbsolute)

            });


            this.OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Settings },
                Label = "Settings",
           //     NavigationType = typeof(SettingsPage),
           //     NavigationDestination = new Uri("Views/Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute)


            });
            this.OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Info },
                Label = "About",
            //    NavigationType = typeof(AboutPage),
            //    NavigationDestination = new Uri("Views/Pages/AboutPage.xaml", UriKind.RelativeOrAbsolute)
            });




        }


    }
}