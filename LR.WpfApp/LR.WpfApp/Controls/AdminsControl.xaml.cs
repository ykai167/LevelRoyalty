using LR.Services;
using LR.Tools;
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

namespace LR.WpfApp.Controls
{
    /// <summary>
    /// AdminsControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.SuperAdminWindow, TabHeader = "普通管理员")]
    public partial class AdminsControl : UserControl
    {
        IAdminService _service;

        public AdminsControl(IAdminService service)
        {
            InitializeComponent();
            this._service = service;

            InitListView();
            this.lvwShow.SelectionChanged += LvwShow_SelectionChanged1;

            this.btns.OnSave += Btns_OnSave;
            this.btns.OnReset += Btns_OnReset;
            this.btns.ShowDelButton = false;

            this.cbxState.ItemsSource = new[] {
                new {text=AdminState.Normal.GetName(),value=AdminState.Normal},
                new{ text=AdminState.Disable.GetName(),value=AdminState.Disable}
            };
            this.cbxState.DisplayMemberPath = "text";
            this.cbxState.SelectedValuePath = "value";
            this.cbxState.SelectionChanged += CbxState_SelectionChanged;
            this.Btns_OnReset();
        }

        private void CbxState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.cbxState && this.cbxState.SelectedValue != null)
            {
                this._service.Update(btns.DataID, new { State = (int)this.cbxState.SelectedValue });
                this.InitListView();
                e.Handled = true;
            }
        }

        private void Btns_OnReset()
        {
            this.lvwShow.SelectedItem = null;
            this.txtName.Text = "";
            this.txtUserName.Text = "";
            this.txtUserName.IsEnabled = true;
            this.lblState.Visibility = Visibility.Hidden;
            this.cbxState.Visibility = Visibility.Hidden;
        }

        private bool Btns_OnSave()
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text.Trim()) || string.IsNullOrWhiteSpace(this.txtUserName.Text.Trim()))
            {
                MessageBox.Show("未输入或输入无效", "提示");
                return false;
            }
            Entity.Admin admin = null;
            string temp = new Random().Next(100000, 999999).ToString();
            if (this.btns.IsAdd)
            {
                var r = this._service.Add(admin = new Entity.Admin
                {
                    Name = this.txtName.Text,
                    UserName = this.txtUserName.Text,
                    Password = temp
                });
                if (!r.Success)
                {
                    MessageBox.Show(r.Message);
                    return false;
                }
                MessageBox.Show($"已新增管理员\r\n登录名:{admin.UserName}\r\n密码:{temp}");
            }
            else
            {
                this._service.Update(this.btns.DataID, new { Name = this.txtName.Text });
            }
            this.InitListView();
            return true;
        }



        private void LvwShow_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
            {
                this.txtName.Text = this.lvwShow.SelectedItem.GetObjectValue<string>("Name");
                this.txtUserName.Text = this.lvwShow.SelectedItem.GetObjectValue<string>("UserName");
                this.txtUserName.IsEnabled = false;
                this.cbxState.Visibility = this.lblState.Visibility = Visibility.Visible;
                this.cbxState.SelectedValue = (AdminState)this.lvwShow.SelectedItem.GetObjectValue<int>("State");
                this.btns.SetEdit(this.lvwShow.SelectedItem.GetObjectValue<Guid>("ID"));
            }
        }

        private void InitListView()
        {
            this.lvwShow.ItemsSource = this._service.GetList();
        }
    }
}
