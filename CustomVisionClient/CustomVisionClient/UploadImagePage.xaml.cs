using Microsoft.Cognitive.CustomVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// UploadImagePage.xaml 的交互逻辑
    /// </summary>
    public partial class UploadImagePage : Page
    {
        private Guid projectId;
        public UploadImagePage(Guid projectId, string[] fileNames)
        {
            InitializeComponent();
            this.projectId = projectId;
            ImageListBox.ItemsSource = fileNames;
            ImageTagListModel taglist = MainWindow.currentWindow.GetAllTags(this.projectId);
            tagsComboBox.ItemsSource = taglist.Tags;
            tagsComboBox.DisplayMemberPath = "Name";
            tagsComboBox.SelectedIndex = 0;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseImagePage browseImagePage = new BrowseImagePage(projectId);
            this.NavigationService.Navigate(browseImagePage);
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            string[] fileNames = (string[])ImageListBox.ItemsSource;
            Task[] tasks = new Task[fileNames.Length];
            ImageTagModel imageTagModel = (ImageTagModel)tagsComboBox.SelectedItem;
            for (int i = 0; i < fileNames.Length; i++)
            {
                FileStream fileStream = File.Open(fileNames[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                tasks[i] = MainWindow.currentWindow.UploadImage(projectId, fileStream, new List<string> { imageTagModel.Id.ToString() });
            }
            Task.WaitAll(tasks);
            ProjectPage projectPage = (ProjectPage)MainWindow.currentWindow.mainFrame.Content;
            projectPage.RefreshTags();
            Window.GetWindow(this).Close();
        }
    }
}
