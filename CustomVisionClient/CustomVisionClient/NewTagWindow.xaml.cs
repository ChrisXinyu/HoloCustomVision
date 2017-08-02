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
using System.Windows.Shapes;

namespace CustomVisionClient
{
    /// <summary>
    /// NewTagWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewTagWindow : Window
    {
        ProjectPage page;

        public NewTagWindow(Page page)
        {
            InitializeComponent();
            this.page = (ProjectPage)page;
        }

        public NewTagWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tagNameTextBox.Text))
            {
                tipTextBlock.Text = "请输入Tag Name";
                return;
            }
            try
            {
                ImageTagModel tagModel = MainWindow.currentWindow.NewTag(page.GetProjectId(), tagNameTextBox.Text);
            }
            catch
            {
                tipTextBlock.Text = "新增Tag失败，检查是否已经存在该Tag";
                return;
            }
            
            page.RefreshTags();
            this.Close();
        }

        private void tagNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tipTextBlock.Text = "";
        }
    }
}
