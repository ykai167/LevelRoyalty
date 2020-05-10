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
            this.btns.OnAdd += Btns_OnAdd;
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
                        this._service.Update(ID, new { State = value });
                    }
                    else if (cbx == this.cbxRefererID)
                    {
                        this._service.Update(ID, new { ReferrerID = value });
                    }
                    else if (cbx == this.cbxWorkGroupID)
                    {
                        this._wService.AddMember((Guid)value, ID);
                    }
                    InitListView();
                }
            }
        }

        private void Btns_OnDelete(object sender, EventArgs e)
        {
            this._service.Update(ID, new { State = (int)Services.StaffState.Delete });
            this.InitListView();
        }

        private void Btns_OnAdd(object sender, EventArgs e)
        {
            this.ID = new Guid();
            this.txtIdenNo.Text = "";
            this.txtNo.Text = "";
            this.txtName.Text = "";
            this.txtMobileNo.Text = "";
            this.dpEntryTime.Text = "";
            cbxHide();
        }

        private void Btns_OnSave(object sender, EventArgs e)
        {
            LR.Entity.Staff staff = new LR.Entity.Staff()
            {
                No = this.txtNo.Text,
                IdenNo = this.txtIdenNo.Text,
                MobileNo = this.txtMobileNo.Text,
                Name = this.txtName.Text,
                EntryTime = this.dpEntryTime.SelectedDate
            };
            if (this.ID == new Guid())
            {
                this._service.Insert(staff);
            }
            else
            {
                this._service.Update(ID, new
                {
                    staff.Name,
                    staff.No,
                    staff.IdenNo,
                    staff.MobileNo,
                    staff.EntryTime
                });
            }
            this.InitListView();
        }

        private void InitListView()
        {
            this.lvwShow.ItemsSource = this._service.GetStaffs();
            cbxHide();
        }

        Guid ID;
        bool listSelecting = false;
        private void LvwShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && lvwShow.SelectedItem != null)
            {
                listSelecting = true;
                var selectedItem = lvwShow.SelectedItem;
                this.ID = selectedItem.GetObjectValue<Guid>(nameof(ID));
                this.txtIdenNo.Text = selectedItem.GetObjectValue("IdenNo") as string;
                this.txtNo.Text = selectedItem.GetObjectValue("No") as string;
                this.txtName.Text = selectedItem.GetObjectValue("Name") as string;
                this.txtMobileNo.Text = selectedItem.GetObjectValue("MobileNo") as string;
                this.dpEntryTime.SelectedDate = selectedItem.GetObjectValue<DateTime?>("EntryTime");
                this.btns.SetEdit();

                this.cbxWorkGroupID.SelectedValue = selectedItem.GetObjectValue("WorkGroupID");
                this.cbxState.SelectedValue = selectedItem.GetObjectValue("State");
                this.cbxRefererID.ItemsSource =
                    new[] { new { name = "无", id = new Guid() } }.Concat(
                    LR.Services.MemoryData.Current
                    .Staffs
                    .GetReferrers(this.ID)
                    .Select(item => new
                    {
                        name = item.Name,
                        id = item.ID
                    }));

                this.cbxRefererID.SelectedValue = selectedItem.GetObjectValue("ReferrerID");
                cbxShow();
                listSelecting = false;
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
