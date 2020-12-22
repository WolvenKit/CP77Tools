using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CP77Tools.UI.Data.Caching
{
    public static class ImageCache
    {

        public static BitmapImage CloseSelected = new BitmapImage(new Uri(@"/CP77Tools.UI;component/Resources/CloseSelected.png", UriKind.Relative));
        public static BitmapImage Close = new BitmapImage(new Uri(@"/CP77Tools.UI;component/Resources/Close.png", UriKind.Relative));
        
        public static BitmapImage MinimizeSelected = new BitmapImage(new Uri(@"/CP77Tools.UI;component/Resources/MinimizeSelected.png", UriKind.Relative));
        public static BitmapImage Minimize = new BitmapImage(new Uri(@"/CP77Tools.UI;component/Resources/Minimize.png", UriKind.Relative));

        public static BitmapImage PageMoveSelected = new BitmapImage(new Uri(@"/CP77Tools.UI;component/Resources/PageMoveSelected.png", UriKind.Relative));
        public static BitmapImage PageMove = new BitmapImage(new Uri(@"/CP77Tools.UI;component/Resources/PageMove.png", UriKind.Relative));
    }
}
