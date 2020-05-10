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

        public class StaffState
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public int Value { get; set; }
        }

        public StaffControl(LR.Services.IStaffService service)
        {
            InitializeComponent();
            this._service = service;
           
            this.Loaded += StaffControl_Loaded;
        }

        private void StaffControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitListView();
        }

        private void InitListView()
        {
            lvwShow.Items.Clear();
            List<LR.Entity.Staff> di = this._service.List();
            for (int i = 0; i < di.Count; i++)
            {
                lvwShow.Items.Add(new
                {
                    No = di[i].No,
                    Name = di[i].Name,
                    IdenNo = di[i].IdenNo,
                    MobileNo = di[i].MobileNo,
                    Referrer = di[i].ReferrerID,
                    WorkGrop = di[i].WorkGroupID,
                    Level = di[i].StaffLevelID,
                    EntryTime = di[i].EntryTime,
                    State = di[i].State
                });
            }
        }
        private void RoomControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitListView();
        }

        private void LvwShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvwShow.SelectedItems.Count > 0)
            {
                string s = lvwShow.Items[lvwShow.SelectedIndex].ToString();
                string[] ss = s.Split(',');
                txtNo.Text = ss[0].Split('=')[1].Trim();
                txtName.Text = ss[1].Split('=')[1].Trim();
                txtIdenNo.Text = ss[2].Split('=')[1].Trim();
                txtMobileNo.Text = ss[3].Split('=')[1].Trim();
                txtReferrer.Text = ss[4].Split('=')[1].Trim();
                dpEntryTime.Text = ss[7].Split('=')[1].Trim();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region 控件列表集合
            List<Control> con_list = new List<Control>()
            {
                txtNo,
                txtName,
                txtIdenNo,
                txtMobileNo,
                txtReferrer,
                dpEntryTime,
            };
            #endregion
            foreach (Control item in con_list)
            {
                if (item is TextBox)
                    if (((TextBox)item).Text == "")
                    {
                        Tip p = new Tip("请把信息填写完整 !");
                        p.ShowDialog();
                        return;
                    }
                if (item is ComboBox)
                    if (((ComboBox)item).Text == "")
                    {
                        Tip p = new Tip("请把信息填写完整 !");
                        p.ShowDialog();
                        return;
                    }
            }
            LR.Entity.Staff staff = new LR.Entity.Staff();
            staff.No = txtNo.Text;
            staff.Name = txtName.Text;
            staff.IdenNo = txtIdenNo.Text;
            staff.MobileNo = txtMobileNo.Text;
            staff.ReferrerID = Guid.Parse(txtReferrer.Text); //TODO
            this._service.Insert(staff);
            this.InitListView();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            #region 控件列表集合
            List<Control> con_list = new List<Control>()
            {
                txtNo,
                txtName,
                txtIdenNo,
                txtMobileNo,
                txtReferrer,
                dpEntryTime,
            };
            #endregion
            foreach (Control item in con_list)
            {
                if (item is TextBox)
                    if (((TextBox)item).Text == "")
                    {
                        Tip p = new Tip("请把信息填写完整 !");
                        p.ShowDialog();
                        return;
                    }
                if (item is ComboBox)
                    if (((ComboBox)item).Text == "")
                    {
                        Tip p = new Tip("请把信息填写完整 !");
                        p.ShowDialog();
                        return;
                    }
            }
            LR.Entity.Staff staff = new LR.Entity.Staff();
            var id = this._service.Single(item => item.No == txtNo.Text).ID;
            staff.No = txtNo.Text;
            staff.Name = txtName.Text;
            staff.IdenNo = txtIdenNo.Text;
            staff.MobileNo = txtMobileNo.Text;
            staff.ReferrerID = Guid.Parse(txtReferrer.Text); //TODO
            this._service.Update(id, staff);
            this.InitListView();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            LR.Entity.Staff staff = new LR.Entity.Staff();
            var id = this._service.Single(item => item.No == staff.No).ID;
            staff.State = 400;
            staff.ID = this._service.Single(item => item.No == staff.No).ID;
            this._service.Update(id, staff);
            this.InitListView();
        }
    }
}
