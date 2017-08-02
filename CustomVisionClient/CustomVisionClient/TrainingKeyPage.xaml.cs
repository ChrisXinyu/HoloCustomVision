using Microsoft.Cognitive.CustomVision;
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
    /// TrainingKeyPage.xaml 的交互逻辑
    /// </summary>
    public partial class TrainingKeyPage : Page
    {
        public TrainingKeyPage()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.currentWindow.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TrainingKeyTextBox.Text))
            {
                tipTextBlock.Text = "请输入Training Key";
                return;
            }
            else
            {
                try
                {
                    TrainingApiCredentials trainingCredentials = new TrainingApiCredentials(TrainingKeyTextBox.Text.Trim());
                    TrainingApi trainingApi = new TrainingApi(trainingCredentials);
                    MainWindow.currentWindow.SetTrainingAPi(trainingApi);
                    MainWindow.currentWindow.GetProjects();
                    MainWindow.currentWindow.StartMainPage();
                }
                catch
                {
                    tipTextBlock.Text = "Training Key验证失败";
                    return;
                }
            }
        }

        private void TrainingKeyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tipTextBlock.Text = "";
        }
    }
}
