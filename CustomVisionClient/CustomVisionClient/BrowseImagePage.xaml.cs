using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomVisionClient
{
    /// <summary>
    /// BrowseImagePage.xaml 的交互逻辑
    /// </summary>
    public partial class BrowseImagePage : Page
    {
        Guid projectId;
        public BrowseImagePage(Guid projectId)
        {
            InitializeComponent();
            this.projectId = projectId;
        }

        private void BrowseImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.jpg,*.png,*.jpeg)|*.jpg;*.png;*.jpeg";
            openFileDialog.Multiselect = true;
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                if (openFileDialog.FileNames.Length > 0)
                {
                    UploadImagePage uploadImagePage = new UploadImagePage(projectId, openFileDialog.FileNames);
                    this.NavigationService.Navigate(uploadImagePage);
                }
            }
        }

        private void BrowseOnlineImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UploadOnlineImage uploadOnlineImagePage = new UploadOnlineImage(projectId);
            this.NavigationService.Navigate(uploadOnlineImagePage);
        }
    }
}
