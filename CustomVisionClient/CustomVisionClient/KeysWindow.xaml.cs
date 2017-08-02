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
    /// KeysWindow.xaml 的交互逻辑
    /// </summary>
    public partial class KeysWindow : Window
    {
        public KeysWindow(Guid projectId, string predictionKey)
        {
            InitializeComponent();
            projectIdTextBox.Text = projectId.ToString();
            predictionKeyTextBox.Text = predictionKey;
        }
    }
}
