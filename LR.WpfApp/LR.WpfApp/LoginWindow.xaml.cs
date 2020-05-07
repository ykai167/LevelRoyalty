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
        LR.Services.AdminService _service = new LR.Services.AdminService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LR.Entity.Admin admin = new LR.Entity.Admin
            {
                Name = txtBoxUserName.Text,
                Password = txtBoxPwd.Password,
            };

            int i = this._service.Check(admin);
            if (txtBoxUserName.Text == "")
            {
                userNameTip.Visibility = Visibility.Visible;
                userNameTip.Content = "用户名不能为空!";
                return;
            }
            else if (txtBoxPwd.Password == "")
            {
                pwdTip.Visibility = Visibility.Visible;
                pwdTip.Content = "密码不能为空!";
                return;
            }
            else if (i == 1)
            {

                pwdTip.Visibility = Visibility.Visible;
                pwdTip.Content = "密码不正确!";
            }
            else if (i == 0)
            {
                userNameTip.Visibility = Visibility.Visible;
                userNameTip.Content = "没有此用户!";
            }
            else if (i == 3)
            {
                MessageBox.Show("系统错误!", "系统提示");
            }
            else
            {
                LR.Services.AdminType type = (Services.AdminType)this._service.Single(item => item.Name == admin.Name && item.Password == admin.Password).Type;
                LR.Services.Administrator.Current = new Services.Administrator
                {
                    Name = admin.Name,
                    Type = type
                };
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
        }
        /// <summary>
        /// 密码输入框改变
        /// </summary>
        private void txtBoxPwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            pwdTip.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 鼠标主键拖拽窗口
        /// </summary>
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
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
