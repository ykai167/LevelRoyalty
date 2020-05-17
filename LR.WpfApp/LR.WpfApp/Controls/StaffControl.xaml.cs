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
    /// StaffControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.MainWindow, TabHeader = "员工管理", Order = 10)]
    public partial class StaffControl : UserControl
    {
        LR.Services.IStaffService _service;
        LR.Services.IWorkGroupService _wService;

        public StaffControl(LR.Services.IStaffService service,
            LR.Services.IWorkGroupService wService)
        {
            InitializeComponent();
            this._service = service;
            this._wService = wService;
            InitListView();
            this.btns.OnSave += Btns_OnSave;
            this.btns.OnReset += Btns_OnReset;
            this.btns.OnDelete += Btns_OnDelete;

            this.cbxWorkGroupID.ItemsSource = _wService.List().Select(p => new { p.Name, p.ID });
            this.cbxWorkGroupID.DisplayMemberPath = "Name";
            this.cbxWorkGroupID.SelectedValuePath = "ID";

            this.cbxState.ItemsSource = new[] { new {
                name=LR.Services.StaffState.Normal.GetName(),
                value=(int)LR.Services.StaffState.Normal
                } ,new {
                name=LR.Services.StaffState.Quit.GetName(),
                value=(int)LR.Services.StaffState.Quit
            }};
            this.cbxState.SelectedValuePath = "value";
            this.cbxState.DisplayMemberPath = "name";

            this.cbxRefererID.SelectedValuePath = "id";
            this.cbxRefererID.DisplayMemberPath = "name";

            this.cbxState.SelectionChanged += Cbx_SelectionChanged;
            this.cbxRefererID.SelectionChanged += Cbx_SelectionChanged;
            this.cbxWorkGroupID.SelectionChanged += Cbx_SelectionChanged;

            Btns_OnReset();
        }

        private void Cbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!listSelecting && sender is ComboBox)
            {
                var cbx = sender as ComboBox;
                if (cbx.SelectedValue != null)
                {
                    var value = cbx.SelectedValue;
                    if (cbx == this.cbxState)
                    {
                        this._service.Update(this.btns.DataID, new { State = value });
                    }
                    else if (cbx == this.cbxRefererID)
                    {
                        this._service.Update(this.btns.DataID, new { ReferrerID = value });
                    }
                    else if (cbx == this.cbxWorkGroupID)
                    {
                        this._wService.AddMember((Guid)value, this.btns.DataID);
                    }
                    InitListView();
                    this.lvwShow.SelectedValue = this.btns.DataID;
                }
                e.Handled = true;
            }
        }

        private bool Btns_OnDelete()
        {
            this._service.Delete(this.btns.DataID);
            this.InitListView();
            return true;
        }

        private void Btns_OnReset()
        {
            this.txtIdenNo.Text = "";
            this.txtNo.Text = "";
            this.txtName.Text = "";
            this.txtMobileNo.Text = "";
            this.dpEntryTime.SelectedDate = DateTime.Now;
            cbxHide();
        }

        private bool Btns_OnSave()
        {
            LR.Entity.Staff staff = new LR.Entity.Staff()
            {
                No = this.txtNo.Text,
                IdenNo = this.txtIdenNo.Text,
                MobileNo = this.txtMobileNo.Text,
                Name = this.txtName.Text,
                EntryTime = this.dpEntryTime.SelectedDate
            };
            if (string.IsNullOrEmpty(staff.No)
                || string.IsNullOrEmpty(staff.IdenNo)
                || string.IsNullOrEmpty(staff.MobileNo)
                || string.IsNullOrEmpty(staff.Name))
            {
                MessageBox.Show("输入不完整", "提示");
                return false;
            }
            if (this.btns.IsAdd)
            {
                this._service.Insert(staff);
            }
            else
            {
                this._service.Update(this.btns.DataID, new
                {
                    staff.Name,
                    staff.No,
                    staff.IdenNo,
                    staff.MobileNo,
                    staff.EntryTime
                });
            }
            this.InitListView();
            return true;
        }

        private void InitListView()
        {
            this.lvwShow.ItemsSource = this._service.GetStaffs();
        }

        bool listSelecting = false;
        private void LvwShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && lvwShow.SelectedItem != null)
            {
                listSelecting = true;
                var selectedItem = lvwShow.SelectedItem;
                Guid id = selectedItem.GetObjectValue<Guid>("ID");
                this.txtIdenNo.Text = selectedItem.GetObjectValue("IdenNo") as string;
                this.txtNo.Text = selectedItem.GetObjectValue("No") as string;
                this.txtName.Text = selectedItem.GetObjectValue("Name") as string;
                this.txtMobileNo.Text = selectedItem.GetObjectValue("MobileNo") as string;
                this.dpEntryTime.SelectedDate = selectedItem.GetObjectValue<DateTime?>("EntryTime");
                this.btns.SetEdit(id);

                this.cbxWorkGroupID.SelectedValue = selectedItem.GetObjectValue("WorkGroupID");

                int state = selectedItem.GetObjectValue<int>("State");
                this.cbxState.SelectedValue = state;
                if (state == (int)StaffState.Quit)
                {
                    this.cbxRefererID.IsEnabled = this.cbxWorkGroupID.IsEnabled = false;
                }
                else
                {
                    this.cbxRefererID.IsEnabled = this.cbxWorkGroupID.IsEnabled = true;
                }
                this.cbxRefererID.ItemsSource =
                    new[] { new { name = "无", id = new Guid() } }.Concat(
                    LR.Services.MemoryData.Current
                    .Staffs
                    .GetReferrers(id)
                    .Select(item => new
                    {
                        name = item.Name,
                        id = item.ID
                    }));

                this.cbxRefererID.SelectedValue = selectedItem.GetObjectValue("ReferrerID");
                cbxShow();
                listSelecting = false;
                this.btns.SetEdit(id);
            }
        }

        void cbxShow()
        {
            this.cbxWorkGroupID.Visibility
                = this.cbxRefererID.Visibility
                = this.cbxState.Visibility
                = this.lblGroup.Visibility
                = this.lblReferer.Visibility
                = this.lblState.Visibility
                = Visibility.Visible;
        }

        void cbxHide()
        {
            this.cbxWorkGroupID.Visibility
                = this.cbxRefererID.Visibility
                = this.cbxState.Visibility
                = this.lblGroup.Visibility
                = this.lblReferer.Visibility
                = this.lblState.Visibility
                = Visibility.Hidden;
        }
    }
}
