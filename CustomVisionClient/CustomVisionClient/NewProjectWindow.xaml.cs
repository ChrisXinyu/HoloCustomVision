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
    /// NewProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        MainPage mainPage;
        private List<DomainModel> domains = new List<DomainModel>();
        public NewProjectWindow(Page page)
        {
            InitializeComponent();
            this.mainPage = (MainPage)page;

            domains = MainWindow.currentWindow.GetDomains();
            domainsComboBox.ItemsSource = domains;
            domainsComboBox.DisplayMemberPath = "Name";
            domainsComboBox.SelectedIndex = 0;
        }

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string projectName = projectNameTextBox.Text;
            if (String.IsNullOrEmpty(projectName))
            {
                tipTextBlock.Text = "请输入Project Name";
                return;
            }
            string projectDesc = projectDescTextBox.Text;
            Guid? domainId = ((DomainModel)domainsComboBox.SelectedItem).Id;
            mainPage.NewProject(projectName, projectDesc, domainId);
            this.Close();
        }

        private void projectNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tipTextBlock.Text = "";
        }
    }
}
