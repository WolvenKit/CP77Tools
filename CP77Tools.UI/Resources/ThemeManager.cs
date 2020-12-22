using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CP77Tools.UI.Resources
{
    public class ThemeManager
    {
        public Color TextFG = (Color)ColorConverter.ConvertFromString("#FFE5D90C");
        public Color TextFG_Selected_Button = (Color)ColorConverter.ConvertFromString("#FFE5D90C");


        public void ApplyThemeDark(MainWindow m)
        {
            TextFG = (Color)ColorConverter.ConvertFromString("#FF02061B");
            TextFG_Selected_Button = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(m)) { tb.Background = new SolidColorBrush(TextFG); }
            foreach (Label tb in FindVisualChildren<Label>(m)) { tb.Background = new SolidColorBrush(TextFG); tb.BorderBrush = null; }
            foreach (Rectangle tb in FindVisualChildren<Rectangle>(m)) { if (tb.Name == "Main_TopHeader_UIElement_Rectangle" || tb.Name == "Main_CP77Logo_UIElement_Image") { } else { tb.Fill = new SolidColorBrush(TextFG); tb.Stroke = null; } }
            foreach (Control tb in FindVisualChildren<Control>(m)) { tb.Background = new SolidColorBrush(TextFG); tb.BorderBrush = null; }
            foreach (Expander tb in FindVisualChildren<Expander>(m)) { tb.Background = new SolidColorBrush(TextFG); tb.BorderBrush = null; }
            foreach (Grid tb in FindVisualChildren<Grid>(m)) { if (tb.Name == "ContainerGrid") { } else { tb.Background = new SolidColorBrush(TextFG); } }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i); if (child != null && child is T) { yield return (T)child; }
                    foreach (T childOfChild in FindVisualChildren<T>(child)) { yield return childOfChild; }
                }
            }
        }

    }
}
