using LR.Services;
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
        IAdminService _service;
        public LoginWindow(IAdminService service)
        {
            this._service = service;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string name = txtBoxUserName.Text.Trim();
            string ps = txtBoxPwd.Password.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                userNameTip.Visibility = Visibility.Visible;
                userNameTip.Content = "用户名不能为空!";
                return;
            }
            else if (string.IsNullOrWhiteSpace(ps))
            {
                pwdTip.Visibility = Visibility.Visible;
                pwdTip.Content = "密码不能为空!";
                return;
            }

            var result = this._service.Login(name, ps);
            if (!result.Success)
            {
                loginTip.Content = result.Message;
                return;
            }
            DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// “取消”点击事件
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 用户名输入框改变
        /// </summary>
        private void txtBoxUserName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            userNameTip.Visibility = Visibility.Hidden;
            loginTip.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 密码输入框改变
        /// </summary>
        private void txtBoxPwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            pwdTip.Visibility = Visibility.Hidden;
            loginTip.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LR.Services.Administrator.Current = new Services.Administrator
            {
                Name = "超级",
                Type = Services.AdminType.Super
            };

            DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LR.Services.Administrator.Current = new Services.Administrator
            {
                Name = "阿三",
                Type = Services.AdminType.Ordin
            };

            DialogResult = true;
            this.Close();
        }
    }
}
