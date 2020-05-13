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
    /// RoyaltyControl.xaml 的交互逻辑
    /// </summary>    
    //[UserControlUse(UseTo.MainWindow, TabHeader = "员工管理")]
    public partial class RoyaltyControl : UserControl
    {
        LR.Services.IRoyaltyService _service;
        LR.Services.IStaffService _staffservice;

        public class RoyaltyState
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public int Value { get; set; }
        }

        public RoyaltyControl(LR.Services.IRoyaltyService service, LR.Services.IStaffService staffservice)
        {
            InitializeComponent();
            this._service = service;
            this._staffservice = staffservice;
            List<RoyaltyState> stateSource = new List<RoyaltyState>()
            {
                new RoyaltyState(){ Name = LR.Services.Extends.GetName(LR.Entity.Royalty.RoyaltyState.Normal), ID = 0, Value = (int)LR.Entity.Royalty.RoyaltyState.Normal},
                new RoyaltyState() { Name = LR.Services.Extends.GetName(LR.Entity.Royalty.RoyaltyState.Abandon), ID = 1, Value = (int)LR.Entity.Royalty.RoyaltyState.Abandon}
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
            List<LR.Entity.Royalty> di = this._service.List();
            for (int i = 0; i < di.Count; i++)
            {
                lvwShow.Items.Add(new
                {
                    Staff = di[i].StaffID,
                    ConsumeDataID = di[i].ConsumeDataID,
                    RoyaltyType = di[i].RoyaltyType,
                    Percent = di[i].Percent,
                    SettleNum = di[i].SettleNum,
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
                txtStaff.Text = ss[0].Split('=')[1].Trim();
                txtConsumeData.Text = ss[1].Split('=')[1].Trim();
                txtRoyaltyType.Text = ss[2].Split('=')[1].Trim();
                txtPercent.Text = ss[3].Split('=')[1].Trim();
                txtSettleNum.Text = ss[4].Split('=')[1].Trim();
                cboState.SelectedValue = ss[5].Split('=')[1].Trim('}').Trim();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region 控件列表集合
            List<Control> con_list = new List<Control>()
            {
                txtStaff,
                txtConsumeData,
                txtRoyaltyType,
                txtPercent,
                txtSettleNum,
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
            LR.Entity.Royalty royalty = new LR.Entity.Royalty();
            royalty.StaffID = this._staffservice.Single(item => item.Name == txtStaff.Text).ID;
            royalty.ConsumeDataID = Guid.Parse(txtConsumeData.Text); //TODO
            royalty.RoyaltyType = int.Parse(txtRoyaltyType.Text); //TODO
            royalty.Percent = decimal.Parse(txtPercent.Text);
            royalty.SettleNum = int.Parse(txtSettleNum.Text); //TODO            
            royalty.State = int.Parse(cboState.Text);
            //this._service.Insert(royalty);
            this.InitListView();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            #region 控件列表集合
            List<Control> con_list = new List<Control>()
            {
                 txtStaff,
                txtConsumeData,
                txtRoyaltyType,
                txtPercent,
                txtSettleNum,
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
            LR.Entity.Royalty royalty = new LR.Entity.Royalty();
            royalty.ID = this._service.Single(item => item.ConsumeDataID == royalty.ConsumeDataID).ID;
            royalty.StaffID = this._staffservice.Single(item => item.Name == txtStaff.Text).ID;
            royalty.ConsumeDataID = Guid.Parse(txtConsumeData.Text); //TODO
            royalty.RoyaltyType = int.Parse(txtRoyaltyType.Text); //TODO
            royalty.Percent = decimal.Parse(txtPercent.Text);
            royalty.SettleNum = int.Parse(txtSettleNum.Text); //TODO            
            royalty.State = int.Parse(cboState.Text);
            //this._service.Update(royalty.ID, royalty);
            this.InitListView();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            LR.Entity.Royalty royalty = new LR.Entity.Royalty();
            royalty.State = 400;
            royalty.ID = this._service.Single(item => item.ConsumeDataID == royalty.ConsumeDataID).ID;
            //this._service.Update(royalty.ID, royalty);
            this.InitListView();
        }
    }
}
