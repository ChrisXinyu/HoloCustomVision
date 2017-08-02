using CustomVisionClient.Models;
using Microsoft.Cognitive.CustomVision.Models;
using Microsoft.Rest;
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
    /// ProjectPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectPage : Page
    {
        private Guid projectId;
        private string projectName;

        public ProjectPage(Guid projectId, string projectName)
        {
            InitializeComponent();
            this.projectId = projectId;
            this.projectName = projectName;
            projectNameTextBlock.Text = projectName;
            GetAllTags();
        }

        private void HomeBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void TagListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImageTagModel imageTagModel = (ImageTagModel)TagListView.SelectedItem;
            if (imageTagModel != null)
            {
                List<string> imageTagList = new List<string>();
                imageTagList.Add(imageTagModel.Id.ToString());
                int take = imageTagModel.ImageCount;
                if (take > 0)
                {
                    List<ImageModel> imageList = MainWindow.currentWindow.GetTaggedImages(projectId, imageTagList, take);
                    ImageListBox.ItemsSource = imageList;
                }
                else
                {
                    ImageListBox.ItemsSource = new List<ImageModel>();
                }
            }
        }

        private void GetAllTags()
        {
            ImageTagListModel imageTagListModel = MainWindow.currentWindow.GetAllTags(projectId);
            TagListView.ItemsSource = imageTagListModel.Tags.ToList();
            TagsCountTextBlock.Text = "(" + imageTagListModel.TotalTaggedImages + ")";
            if (imageTagListModel.TotalTaggedImages > 0)
            {
                List<ImageModel> imageList = MainWindow.currentWindow.GetAllTaggedImages(projectId, imageTagListModel.TotalTaggedImages);
                ImageListBox.ItemsSource = imageList;
            }
            else
            {
                ImageListBox.ItemsSource = new List<ImageModel>();
            }
        }

        public Guid GetProjectId()
        {
            return projectId;
        }

        public void RefreshTags()
        {
            GetAllTags();
        }

        private void AllTagsPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GetAllTags();
        }

        private void NewTagImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NewTagWindow newTagWindow = new NewTagWindow(this);
            newTagWindow.Owner = MainWindow.currentWindow;
            newTagWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newTagWindow.ShowDialog();
        }

        private void AddImages_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NewImagesWindow newImagesWindow = new NewImagesWindow(projectId);
            newImagesWindow.Owner = MainWindow.currentWindow;
            newImagesWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newImagesWindow.ShowDialog();
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            IterationModel iterationModel = null;
            try
            {
                iterationModel = MainWindow.currentWindow.TrainProject(projectId);
            }
            catch(HttpOperationException ex)
            {
                string result = ex.Response.Content;
                TrainResponseModel response = Utils.Utils.JsonDeserialize<TrainResponseModel>(result);
                if (response != null && !String.IsNullOrEmpty(response.Code))
                {
                    switch (response.Code)
                    {
                        case "BadRequestTrainingNotNeeded":
                            MessageBox.Show("Nothing has changed since previous training.");
                            return;
                        case "BadRequestTrainingValidationFailed":
                            MessageBox.Show("Your project can't be trained just yet. Make sure you have at least 2 tags with at least 5 images in each.");
                            return;
                        default:
                            MessageBox.Show(response.Code);
                            return;
                    }
                }
                else
                {
                    RateLimitResponseModel limitResponse = Utils.Utils.JsonDeserialize<RateLimitResponseModel>(result);
                    if (limitResponse != null)
                    {
                        MessageBox.Show(limitResponse.message);
                        return;
                    }
                }
                MessageBox.Show("an error occurs");
                return;
            }
            
            IterationPage iterationPage = new IterationPage(projectId, projectName, iterationModel);
            this.NavigationService.Navigate(iterationPage);
        }

        private void IterationButton_Click(object sender, RoutedEventArgs e)
        {
            IterationPage iterationPage = new IterationPage(projectId, projectName);
            this.NavigationService.Navigate(iterationPage);
        }

        private void AccountImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string predictionKey = MainWindow.currentWindow.GetPredictionKey();
            KeysWindow keysWindow = new KeysWindow(projectId, predictionKey);
            keysWindow.Owner = MainWindow.currentWindow;
            keysWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            keysWindow.ShowDialog();
        }
    }
}
