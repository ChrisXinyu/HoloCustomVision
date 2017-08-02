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

namespace CustomVisionClient
{
    /// <summary>
    /// NewImagesWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewImagesWindow : Window
    {
        public NewImagesWindow(Guid projectId)
        {
            InitializeComponent();
            BrowseImagePage browseImagesPage = new BrowseImagePage(projectId);
            mainFrame.Navigate(browseImagesPage);
        }

        public NewImagesWindow()
        {
            InitializeComponent();
        }
    }
}
