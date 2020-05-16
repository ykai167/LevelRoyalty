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
    /// ChangePSWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePSWindow : Window
    {
        Services.IAdminService _service;
        public ChangePSWindow(Services.IAdminService service)
        {
            this._service = service;
            InitializeComponent();
        }

        /// <summary>
        /// 密码输入框改变
        /// </summary>
        private void txtBoxPwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblTip.Visibility = Visibility.Hidden;
        }

        private void btnCommit_Click(object sender, RoutedEventArgs e)
        {
            string old = this.txtBoxPwdold.Password.Trim();
            string new1 = this.txtBoxPwd0.Password.Trim();
            string new2 = this.txtBoxPwd1.Password.Trim();
            lblTip.Visibility = Visibility.Visible;
            if (old.Length == 0)
            {
                lblTip.Content = "请输入原密码";
                return;
            }
            if (new1.Length == 0)
            {
                lblTip.Content = "请输入新密码";
                return;
            }
            if (new2.Length == 0)
            {
                lblTip.Content = "请输入确认密码";
                return;
            }
            if (new1 != new2)
            {
                lblTip.Content = "两次输入的新密码不一致";
                return;
            }

            var r = this._service.ChangePassword(old, new1);
            if (!r.Success)
            {
                lblTip.Content = r.Message;
                return;
            }
            MessageBox.Show("密码已修改,下次登录请使用新密码");
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
