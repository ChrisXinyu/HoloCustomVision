using Microsoft.Cognitive.CustomVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// IterationPage.xaml 的交互逻辑
    /// </summary>
    public partial class IterationPage : Page
    {
        private Guid projectId;
        private string projectName;

        public IterationPage(Guid projectId, string projectName, IterationModel iterationModel)
        {
            InitializeComponent();
            this.projectId = projectId;
            this.projectName = projectName;

            List<IterationModel> iterationModels = MainWindow.currentWindow.GetIterations(projectId).Where(Entity => Entity.Status != "New").ToList();
            IterationListView.ItemsSource = iterationModels;
            Thread thread = new Thread(new ParameterizedThreadStart(UpdateIterationState));
            thread.Start(iterationModel);
        }

        public IterationPage(Guid projectId, string projectName)
        {
            InitializeComponent();
            this.projectId = projectId;
            this.projectName = projectName;
            List<IterationModel> iterationModels = MainWindow.currentWindow.GetIterations(projectId).Where(Entity => Entity.Status != "New").ToList();
            IterationListView.ItemsSource = iterationModels;
        }

        private void UpdateIterationState(object obj)
        {
            IterationModel iterationModel = (IterationModel)obj;
            if (iterationModel != null)
            {
                while (iterationModel.Status == "Training")
                {
                    Thread.Sleep(1000);
                    iterationModel = MainWindow.currentWindow.GetIteration(projectId, iterationModel.Id);
                }
                iterationModel.IsDefault = true;
                MainWindow.currentWindow.UpdateIteration(projectId, iterationModel.Id, iterationModel);

                this.Dispatcher.Invoke(new Action(() => {
                    List<IterationModel> iterationModels = MainWindow.currentWindow.GetIterations(projectId).Where(Entity => Entity.Status != "New").ToList();
                    IterationListView.ItemsSource = iterationModels;
                }));
            }
        }

        private void BackImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProjectPage projectPage = new ProjectPage(projectId, projectName);
            this.NavigationService.Navigate(projectPage);
        }

        private void IterationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
