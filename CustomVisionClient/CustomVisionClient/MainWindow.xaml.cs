using Microsoft.Cognitive.CustomVision;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ProjectModel> projectlist = new List<ProjectModel>();
        private List<DomainModel> domainList = new List<DomainModel>();
        public static MainWindow currentWindow;
        TrainingApi trainingApi;

        public MainWindow()
        {
            InitializeComponent();
            currentWindow = this;
            TrainingKeyPage trainingKeyPage = new TrainingKeyPage();
            mainFrame.Navigate(trainingKeyPage);
        }

        /// <summary>
        /// 设置TrainingAPI
        /// </summary>
        /// <param name="trainingApi"></param>
        public void SetTrainingAPi(TrainingApi trainingApi)
        {
            this.trainingApi = trainingApi;
        }

        public void StartMainPage()
        {
            MainPage mainPage = new MainPage();
            mainFrame.Navigate(mainPage);
        }

        /// <summary>
        /// 获取Project列表
        /// </summary>
        /// <returns></returns>
        public List<ProjectModel> GetProjects()
        {
            projectlist = trainingApi.GetProjects().ToList();
            return projectlist;
        }

        /// <summary>
        /// 根据Project Id获取所有Tags
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ImageTagListModel GetAllTags(Guid projectId)
        {
            ImageTagListModel taglist = trainingApi.GetTags(projectId);
            return taglist;
        }

        /// <summary>
        /// 获取所有有Tag的图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<ImageModel> GetAllTaggedImages(Guid projectId, int take)
        {
            List<ImageModel> images = trainingApi.GetAllTaggedImages(projectId, null, null, take).ToList();
            return images;
        }

        /// <summary>
        /// 根据TagIds 获取图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="tagsIds"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<ImageModel> GetTaggedImages(Guid projectId, List<string> tagsIds, int take)
        {
            List<ImageModel> images = trainingApi.GetImagesByTags(projectId, null, tagsIds, null, take).ToList();
            return images;
        }

        /// <summary>
        /// 获取所有Domains
        /// </summary>
        /// <returns></returns>
        public List<DomainModel> GetDomains()
        {
            domainList = trainingApi.GetDomains().ToList();
            return domainList;
        }

        /// <summary>
        /// 创建Tag
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ImageTagModel NewTag(Guid projectId, string tagName)
        {
            return trainingApi.CreateTag(projectId, tagName);
        }

        /// <summary>
        /// 创建Project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="description"></param>
        /// <param name="domainId"></param>
        public void NewProject(string projectName, string description, Guid? domainId)
        {
            var project = trainingApi.CreateProject(projectName, description, domainId);
        }

        /// <summary>
        /// 训练Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IterationModel TrainProject(Guid projectId)
        {
            return trainingApi.TrainProject(projectId);
        }

        /// <summary>
        /// 获取Iteration信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="iterationId"></param>
        /// <returns></returns>
        public IterationModel GetIteration(Guid projectId, Guid iterationId)
        {
            return trainingApi.GetIteration(projectId, iterationId);
        }

        /// <summary>
        /// 更新Iteration信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="iterationId"></param>
        /// <param name="iteration"></param>
        public void UpdateIteration(Guid projectId, Guid iterationId, IterationModel iteration)
        {
            trainingApi.UpdateIteration(projectId, iterationId, iteration);
        }

        /// <summary>
        /// 获取所有Iteration信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<IterationModel> GetIterations(Guid projectId)
        {
            return trainingApi.GetIterations(projectId).ToList();
        }

        /// <summary>
        /// 获取PredictionKey
        /// </summary>
        /// <returns></returns>
        public string GetPredictionKey()
        {
            return trainingApi.GetAccountInfo().Keys.PredictionKeys.PrimaryKey;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fileStream"></param>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        public Task UploadImage(Guid projectId, Stream fileStream, List<string> tagIds)
        {
            return Task.Run(() => {
                trainingApi.CreateImagesFromData(projectId, fileStream, tagIds);
            });
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="createBatch"></param>
        /// <returns></returns>
        public Task UploadOnlineImage(Guid projectId, ImageUrlCreateBatch createBatch)
        {
            return Task.Run(() => {
                trainingApi.CreateImagesFromUrls(projectId, createBatch);
            });
        }
    }
}
