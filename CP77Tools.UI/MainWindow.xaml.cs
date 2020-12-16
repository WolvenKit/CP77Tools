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

namespace CP77Tools.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine(list.Items.Count);
        }














        // Just some breh code for handling pages.
        private int CurrentPageIndex = 3;
        private int Pagehandler(bool plus)
        {



            if (CurrentPageIndex <= 0)
            {
                CurrentPageIndex = 3;
            }

            if (plus)
            {
                CurrentPageIndex += 1;

                if (CurrentPageIndex >= 0 && CurrentPageIndex <= 3)
                {
                    CurrentPageIndex = 3;
                }
                if (CurrentPageIndex >= 4 && CurrentPageIndex <= 7)
                {
                    CurrentPageIndex = 7;
                }
                if (CurrentPageIndex >= 8 && CurrentPageIndex <= 11)
                {
                    CurrentPageIndex = 11;
                }

                Trace.WriteLine(CurrentPageIndex);


                if (CurrentPageIndex >= list.Items.Count - 1)
                {
                    CurrentPageIndex = list.Items.Count - 1;
                }
                return CurrentPageIndex;
            }
            if (!plus)
            {
                CurrentPageIndex -= 1;
                if (CurrentPageIndex >= 0 && CurrentPageIndex <= 2)
                {
                    CurrentPageIndex = 0;
                }
                if (CurrentPageIndex >= 3 && CurrentPageIndex <=6)
                {
                    CurrentPageIndex = 0;
                }
                if (CurrentPageIndex >= 7 && CurrentPageIndex <= 10 || CurrentPageIndex == 9)
                {
                    CurrentPageIndex = 4;
                }

                Trace.WriteLine(CurrentPageIndex);

                return CurrentPageIndex;
            }


            else
            {
                Trace.WriteLine(CurrentPageIndex);

                return 3;
            }

        }



        private void UIFunc_DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void UIElement_CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UIElement_CloseButton.Source = ImageCache.CloseSelected;
        }

        private void UIElement_CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            UIElement_CloseButton.Source = ImageCache.Close;

        }

        private void UIElement_MinimizeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UIElement_MinimizeButton.Source = ImageCache.MinimizeSelected;

        }

        private void UIElement_MinimizeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            UIElement_MinimizeButton.Source = ImageCache.Minimize;

        }

        private void UIElement_CloseButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void UIElement_MinimizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void UIElement_PreviousItems_MouseEnter(object sender, MouseEventArgs e)
        {
            UIElement_PreviousItems.Source = ImageCache.PageMoveSelected;
        }

        private void UIElement_PreviousItems_MouseLeave(object sender, MouseEventArgs e)
        {
            UIElement_PreviousItems.Source = ImageCache.PageMove;
        }

        private void UIElement_PreviousItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                list.ScrollIntoView(list.Items[0]);
        }
        private void UIElement_Button_NextItems_MouseEnter(object sender, MouseEventArgs e)
        {
           UIElement_Button_NextItems.Opacity = 0.8;
        }

        private void UIElement_Button_NextItems_MouseLeave(object sender, MouseEventArgs e)
        {
            UIElement_Button_NextItems.Opacity = 0.5;

        }

        private void UIElement_Button_NextItems_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement_Button_NextItems.Opacity = 0.8;

        }

        private void UIElement_Button_NextItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           UIElement_Button_NextItems.Opacity = 1.0;
            list.ScrollIntoView(list.Items[Pagehandler(true)]);  
        }

        private void UIElement_Button_PreviousItems_MouseEnter(object sender, MouseEventArgs e)
        {
                UIElement_Button_PreviousItems.Opacity = 0.8;
        }

        private void UIElement_Button_PreviousItems_MouseLeave(object sender, MouseEventArgs e)
        {
            UIElement_Button_PreviousItems.Opacity = 0.5;

        }

        private void UIElement_Button_PreviousItems_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIElement_Button_PreviousItems.Opacity = 1.0;
            if (CurrentPageIndex > 0)
            {
                list.ScrollIntoView(list.Items[Pagehandler(false)]);
            }
        }

        private void UIElement_Button_PreviousItems_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement_Button_PreviousItems.Opacity = 0.8;
        }

        private void UIElement_Button_ArchiveSelectArchive_MouseEnter(object sender, MouseEventArgs e)
        {
            UIElement_Button_ArchiveSelectArchive.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void UIElement_Button_ArchiveSelectArchive_MouseLeave(object sender, MouseEventArgs e)
        {
            UIElement_Button_ArchiveSelectArchive.Foreground = new SolidColorBrush(Colors.Yellow);

        }

        private void UIElement_Button_ArchiveSelectOutputPath_MouseEnter(object sender, MouseEventArgs e)
        {
            UIElement_Button_ArchiveSelectOutputPath.Foreground = new SolidColorBrush(Colors.Black);

        }

        private void UIElement_Button_ArchiveSelectOutputPath_MouseLeave(object sender, MouseEventArgs e)
        {
            UIElement_Button_ArchiveSelectOutputPath.Foreground = new SolidColorBrush(Colors.Yellow);

        }


        private void UIElement_Button_ArchiveStart_MouseEnter(object sender, MouseEventArgs e)
        {
            UIElement_Button_ArchiveStart.Foreground = new SolidColorBrush(Colors.Black);

        }

        private void UIElement_Button_ArchiveStart_MouseLeave(object sender, MouseEventArgs e)
        {
            UIElement_Button_ArchiveStart.Foreground = new SolidColorBrush(Colors.Yellow);

        }
    }
}
