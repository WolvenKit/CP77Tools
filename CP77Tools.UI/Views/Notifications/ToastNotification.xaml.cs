using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CP77Tools.UI.Views.Notifications
{
    /// <summary>
    /// Interaction logic for ToastNotification.xaml
    /// </summary>
    public partial class ToastNotification : MetroWindow
    {
        public ToastNotification(string Title, string Text, int NotiType)
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, SUI.sui.generaldata.ThemeFinder());

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
            NoteTExt.Text = Text;
            Header.Header = Title;
            SetColor(NotiType);
        }

        private void SetColor(int type)
        {

            switch (type)
            {
                case 1:
                    BackGround.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7C0000")); // Error
                    break;
                case 2:
                    BackGround.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF1F4D1A")); // Succes
                    break;
            }
        }
        private void Storyboard_Completed(object sender, EventArgs e) { this.Close(); }
    }
}
