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

namespace LR.WpfApp
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            LR.Services.Administrator.Current = new Services.Administrator
            {
                ID = Guid.NewGuid(),
                Name = "张三",
                Type = Services.AdminType.Ordin
            };
            DialogResult = true;
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LR.Services.Administrator.Current = new Services.Administrator
            {
                ID = Guid.NewGuid(),
                Name = "超级管理员",
                Type = Services.AdminType.Super
            };
            DialogResult = true;
            this.Close();
        }
    }
}
