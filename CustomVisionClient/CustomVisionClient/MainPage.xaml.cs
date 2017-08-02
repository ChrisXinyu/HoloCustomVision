using Microsoft.Cognitive.CustomVision;
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
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            List<ProjectModel>  projectlist = MainWindow.currentWindow.GetProjects();
            ProjectListBox.ItemsSource = projectlist;
        }

        private void RefreshProjectList()
        {
            List<ProjectModel> projectlist = MainWindow.currentWindow.GetProjects();
            ProjectListBox.ItemsSource = projectlist;
        }

        private void ProjectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProjectModel project = (ProjectModel)ProjectListBox.SelectedItem;
            this.NavigationService.Navigate(new ProjectPage(project.Id, project.Name));
        }

        private void NewProjectImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NewProjectWindow newProjectWindow = new NewProjectWindow(this);
            newProjectWindow.Owner = MainWindow.currentWindow;
            newProjectWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newProjectWindow.ShowDialog();
        }

        public void NewProject(string projectName, string projectDesc, Guid? domainId )
        {
            MainWindow mainWindow = MainWindow.currentWindow;
            mainWindow.NewProject(projectName, projectDesc, domainId);
            RefreshProjectList();
        }
    }
}
