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
    /// WorkGroup.xaml 的交互逻辑
    /// </summary>

    [UserControlUse(UseTo.MainWindow, TabHeader = "工作组管理", Order = 20)]
    public partial class WorkGroup : UserControl
    {
        LR.Services.IWorkGroupService _service;
        LR.Services.StaffService staffService = new Services.StaffService();

        public class WorkGroupState
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public int Value { get; set; }
        }

        public WorkGroupControl(LR.Services.IWorkGroupService service)
        {
            InitializeComponent();
            this._service = service;
            List<WorkGroupState> stateSource = new List<WorkGroupState>()
            {
                new WorkGroupState(){ Name = LR.Services.Extends.GetName(LR.Entity.WorkGroup.WorkGroupState.Normal), ID = 0, Value = (int)LR.Entity.WorkGroup.WorkGroupState.Normal},
                new WorkGroupState() { Name = LR.Services.Extends.GetName(LR.Entity.WorkGroup.WorkGroupState.Dismiss), ID = 1, Value = (int)LR.Entity.WorkGroup.WorkGroupState.Dismiss}
            };
            cboState.ItemsSource = stateSource;
            cboState.DisplayMemberPath = "Name";
            cboState.SelectedValuePath = "Value";
            cboState.SelectedIndex = 0;
            this.Loaded += RoomControl_Loaded;
        }

        private void InitListView()
        {
            lvwShow.Items.Clear();
            List<LR.Entity.WorkGroup> di = this._service.List();
            for (int i = 0; i < di.Count; i++)
            {
                lvwShow.Items.Add(new
                {
                    Name = di[i].Name,
                    Manager = di[i].ManagerID,
                    Assistant = di[i].AssistantID,
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
                txtName.Text = ss[0].Split('=')[1].Trim();
                txtManager.Text = ss[1].Split('=')[1].Trim();
                txtAssitant.Text = ss[2].Split('=')[1].Trim();
                cboState.SelectedValue = ss[4].Split('=')[1].Trim('}').Trim();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region 控件列表集合
            List<Control> con_list = new List<Control>()
            {
                txtName,
                txtManager,
                txtAssitant,
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
            LR.Entity.WorkGroup workgroup = new LR.Entity.WorkGroup();
            workgroup.Name = txtName.Text;
            workgroup.ManagerID = staffService.Single(item => item.Name == txtManager.Text).ID;
            workgroup.AssistantID = staffService.Single(item => item.Name == txtAssitant.Text).ID;
            workgroup.State = int.Parse(cboState.Text);
            this._service.Insert(workgroup);
            this.InitListView();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            #region 控件列表集合
            List<Control> con_list = new List<Control>()
            {
                txtName,
                txtManager,
                txtAssitant,
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
            LR.Entity.WorkGroup workgroup = new LR.Entity.WorkGroup();
            workgroup.ID = this._service.Single(item => item.Name == txtName.Text).ID;
            workgroup.Name = txtName.Text;
            workgroup.ManagerID = staffService.Single(item => item.Name == txtManager.Text).ID;
            workgroup.AssistantID = staffService.Single(item => item.Name == txtAssitant.Text).ID;
            workgroup.State = int.Parse(cboState.Text);
            this._service.Update(workgroup.ID, workgroup);
            this.InitListView();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            LR.Entity.WorkGroup workgroup = new LR.Entity.WorkGroup();
            workgroup.State = 400;
            workgroup.ID = this._service.Single(item => item.Name == workgroup.Name).ID;
            this._service.Update(workgroup.ID, workgroup);
            this.InitListView();
        }
    }
}
