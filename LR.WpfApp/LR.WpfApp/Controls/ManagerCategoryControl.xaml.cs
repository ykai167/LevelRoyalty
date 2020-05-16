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
    /// ManagerCategoryControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.SuperAdminWindow, TabHeader = "工作组管理员类别", Order = 20)]
    public partial class ManagerCategoryControl : UserControl
    {
        LR.Services.IWorkGroupManagerCategoryService _service;

        public ManagerCategoryControl(IWorkGroupManagerCategoryService service)
        {
            InitializeComponent();
            this._service = service;

            InitListView();
            this.lvwShow.SelectionChanged += LvwShow_SelectionChanged1;

            this.btns.OnSave += Btns_OnSave;
            this.btns.OnReset += Btns_OnReset;
            this.btns.OnDelete += Btns_OnDelete;

            this.Btns_OnReset();
        }

        private bool Btns_OnDelete()
        {
            this._service.Delete(this.btns.DataID);
            InitListView();
            return true;
        }


        private void Btns_OnReset()
        {
            this.lvwShow.SelectedItem = null;
            this.txtName.Text = "";
        }

        private bool Btns_OnSave()
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text.Trim()))
            {
                MessageBox.Show("未输入名称", "提示");
                return false;
            }
            if (this.btns.IsAdd)
            {
                var r = this._service.Insert(new Entity.WorkGroupManagerCategory
                {
                    Name = this.txtName.Text.Trim()
                });
            }
            else
            {
                this._service.Update(this.btns.DataID, new
                {
                    Name = this.txtName.Text.Trim()
                });
            }
            this.InitListView();
            return true;
        }

        private void LvwShow_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
            {
                this.txtName.Text = this.lvwShow.SelectedItem.GetObjectValue<string>("Name");
                this.btns.SetEdit(this.lvwShow.SelectedItem.GetObjectValue<Guid>("ID"));
            }
        }

        private void InitListView()
        {
            this.lvwShow.ItemsSource = this._service.List();
        }
    }
}
