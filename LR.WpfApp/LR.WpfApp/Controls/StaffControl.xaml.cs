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
    [UserControlUse(UseTo.MainWindow, TabHeader = "员工管理")]
    public partial class StaffControl : UserControl
    {
        LR.Services.StaffService service;

        public class StaffState
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public int Value { get; set; }
        }

        public StaffControl(LR.Services.IStaffService _service)
        {
            InitializeComponent();      
            this.service = (LR.Services.StaffService)_service;
            List<StaffState> stateSource = new List<StaffState>()
            {
                new StaffState(){ Name = "正常", ID = 0, Value = 200},
                new StaffState() { Name = "离职", ID = 1, Value = 400}
            };
            cboState.ItemsSource = stateSource;
            cboState.DisplayMemberPath = "Name";
            cboState.SelectedValuePath = "Value";
            cboState.SelectedIndex = 0;
            this.Loaded += StaffControl_Loaded;
        }

        private void StaffControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitListView();
        }        

        private void InitListView()
        {
            lvwShow.Items.Clear();
            List<LR.Entity.Staff> di = this.service.List();
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
                txtWorkGroup.Text = ss[5].Split('=')[1].Trim();
                txtLevel.Text = ss[6].Split('=')[1].Trim();
                dpEntryTime.Text = ss[7].Split('=')[1].Trim();
                cboState.SelectedValue = ss[4].Split('=')[1].Trim('}').Trim();
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
                txtWorkGroup,
                txtLevel,
                dpEntryTime,
                cboState
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
            staff.WorkGroupID = Guid.Parse(txtWorkGroup.Text); //TODO
            staff.StaffLevelID = Guid.Parse(txtLevel.Text);
            staff.State = int.Parse(cboState.Text);
            this.service.Update(staff);
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
                txtWorkGroup,
                txtLevel,
                dpEntryTime,
                cboState
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
            staff.WorkGroupID = Guid.Parse(txtWorkGroup.Text); //TODO
            staff.StaffLevelID = Guid.Parse(txtLevel.Text);
            staff.State = int.Parse(cboState.Text);
            this.service.Update(staff);            
            this.InitListView();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            LR.Entity.Staff staff = new LR.Entity.Staff();
            staff.State = 400;
            staff.ID = this.service.Single(item => item.No == staff.No).ID;
            this.service.Update(staff);
            this.InitListView();
        }
    }
}
