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
    /// StaffCategoryControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.SuperAdminWindow, TabHeader = "员工级别", Order = 10)]
    public partial class StaffLevelControl : UserControl
    {
        LR.Services.IStaffLevelService _service;

        public StaffLevelControl(IStaffLevelService service)
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
            this.txtOrder.Text = "";
            this.txtOrder.Text = "";
        }

        private bool Btns_OnSave()
        {
            int min;
            int order;
            if (!int.TryParse(this.txtMin.Text.Trim(), out min) || !int.TryParse(this.txtOrder.Text.Trim(), out order))
            {
                MessageBox.Show("数字输入错误", "提示");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.txtName.Text.Trim()))
            {
                MessageBox.Show("未输入名称", "提示");
                return false;
            }
            if (this.btns.IsAdd)
            {
                var r = this._service.Insert(new Entity.StaffLevel
                {
                    Name = this.txtName.Text.Trim(),
                    Order = order,
                    MinCount = min
                });
            }
            else
            {
                this._service.Update(this.btns.DataID, new
                {
                    Name = this.txtName.Text.Trim(),
                    MinCharge = min,
                    Order = order
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
                this.txtMin.Text = this.lvwShow.SelectedItem.GetObjectValue("MinCount")?.ToString();
                this.txtOrder.Text = this.lvwShow.SelectedItem.GetObjectValue("Order")?.ToString();
                this.btns.SetEdit(this.lvwShow.SelectedItem.GetObjectValue<Guid>("ID"));
            }
        }

        private void InitListView()
        {
            this.lvwShow.ItemsSource = this._service.List();
        }
    }
}
