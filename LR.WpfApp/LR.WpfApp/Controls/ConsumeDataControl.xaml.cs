using LR.Tools;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// ConsumeDataControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.MainWindow, TabHeader = "消费记录", Order = 0)]
    public partial class ConsumeDataControl : UserControl
    {
        LR.Services.IConsumeDataService _service;
        int curentPage = 1;
        int pageSize = Tools.ConfigHelper.AppSettings.PageSize;
        public ConsumeDataControl(
            LR.Services.IConsumeDataService service,
            LR.Services.IStaffService _sService,
            LR.Services.IRoomService _rService)
        {
            this._service = service;
            InitializeComponent();
            InitData();

            this.cbxRoom.ItemsSource = _rService.List().Select(p => new { name = p.Name, id = p.ID });
            this.cbxRoom.DisplayMemberPath = "name";
            this.cbxRoom.SelectedValuePath = "id";
            this.cbxRoom.SelectedIndex = 0;

            this.cbxStaff.ItemsSource = _sService.List().Select(p => new { name = p.Name, id = p.ID });
            this.cbxStaff.DisplayMemberPath = "name";
            this.cbxStaff.SelectedValuePath = "id";
            this.cbxStaff.SelectedIndex = 0;

            this.btns.OnReset += Btns_OnAdd;
            this.btns.OnDelete += Btns_OnDelete; ;
            this.btns.OnSave += Btns_OnSave; ;

            this.ucPager.FirstPage += UcPager_FirstPage;
            this.ucPager.LastPage += UcPager_LastPage;
            this.ucPager.PreviousPage += UcPager_PreviousPage;
            this.ucPager.NextPage += UcPager_NextPage;
        }

        private bool Btns_OnSave()
        {
            decimal amount;
            if (!decimal.TryParse(this.txtAmount.Text, out amount))
            {
                MessageBox.Show("金额输入错误", "错误");
                return false;
            }
            if (this.cbxRoom.SelectedValue == null || this.cbxStaff.SelectedValue == null)
            {
                MessageBox.Show("未选择房间或员工", "错误");
                return false;
            }

            var entity = new Entity.ConsumeData
            {
                Amount = amount,
                RoomID = (Guid)this.cbxRoom.SelectedValue,
                StaffID = (Guid)this.cbxStaff.SelectedValue
            };
            if (entity.RoomID == Guid.Empty || entity.StaffID == Guid.Empty)
            {
                MessageBox.Show("数据不完整", "错误");
                return false;
            }
            try
            {
                if (this.btns.IsAdd)
                {
                    this._service.Insert(entity);
                }
                else
                {
                    this._service.Update(this.btns.DataID, new { entity.RoomID, entity.StaffID, entity.Amount });
                }
                InitData();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        private bool Btns_OnDelete()
        {
            this._service.Delete(this.btns.DataID);
            InitData();
            return true;
        }

        private void Btns_OnAdd()
        {
            this.txtAmount.Text = "";
            this.cbxRoom.SelectedValue = null;
            this.cbxStaff.SelectedValue = null;
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            ExtractTips p = new ExtractTips(this._service);
            p.ShowDialog();
        }

        private void UcPager_NextPage(object sender, RoutedEventArgs e)
        {
            this.curentPage = page.TotalPage > this.curentPage ? this.curentPage + 1 : this.curentPage;
            InitData();
        }

        private void UcPager_PreviousPage(object sender, RoutedEventArgs e)
        {
            this.curentPage = this.curentPage > 1 ? this.curentPage - 1 : this.curentPage;
            InitData();
        }

        private void UcPager_LastPage(object sender, RoutedEventArgs e)
        {
            this.curentPage = page.TotalPage;
            InitData();
        }

        private void UcPager_FirstPage(object sender, RoutedEventArgs e)
        {
            this.curentPage = 1;
            InitData();
        }

        private void LvwShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
            {
                this.txtAmount.Text = this.lvwShow.SelectedItem.GetObjectValue("Amount")?.ToString();
                this.cbxStaff.SelectedValue = this.lvwShow.SelectedItem.GetObjectValue("StaffID");
                this.cbxRoom.SelectedValue = this.lvwShow.SelectedItem.GetObjectValue("RoomID");
                this.btns.SetEdit(this.lvwShow.SelectedItem.GetObjectValue<Guid>("ID"));
            }
        }

        Pager<object> page;
        void InitData()
        {
            page = this._service.GetPage(curentPage, pageSize);
            this.ucPager.TotalPage = page.TotalPage.ToString();
            this.ucPager.CurrentPage = this.curentPage.ToString();
            this.lvwShow.ItemsSource = page;
        }
    }
}
