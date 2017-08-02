using Microsoft.Cognitive.CustomVision.Models;
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
    /// UploadOnlineImage.xaml 的交互逻辑
    /// </summary>
    public partial class UploadOnlineImage : Page
    {
        private Guid projectId;
        List<string> urls = new List<string>();
        public UploadOnlineImage(Guid projectId)
        {
            InitializeComponent();
            this.projectId = projectId;

            ImageTagListModel taglist = MainWindow.currentWindow.GetAllTags(this.projectId);
            tagsComboBox.ItemsSource = taglist.Tags;
            tagsComboBox.DisplayMemberPath = "Name";
            tagsComboBox.SelectedIndex = 0;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(urlTextBox.Text))
            {
                return;
            }
            urls.Add(urlTextBox.Text);
            ImageListBox.Items.Add(urlTextBox.Text);
            urlTextBox.Text = "";
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseImagePage browseImagePage = new BrowseImagePage(projectId);
            this.NavigationService.Navigate(browseImagePage);
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (urls.Count > 0)
            {
                ImageUrlCreateBatch createBatch = new ImageUrlCreateBatch();
                ImageTagModel imageTagModel = (ImageTagModel)tagsComboBox.SelectedItem;
                createBatch.TagIds = new List<Guid> { imageTagModel.Id };
                createBatch.Urls = urls;
                await MainWindow.currentWindow.UploadOnlineImage(projectId, createBatch);

                ProjectPage projectPage = (ProjectPage)MainWindow.currentWindow.mainFrame.Content;
                projectPage.RefreshTags();
                Window.GetWindow(this).Close();
            }
        }
    }
}
