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
    /// RoomControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.MainWindow, TabHeader = "房间管理")]
    public partial class RoomControl : UserControl
    {
        LR.Services.RoomService service;
        LR.Services.RoomCategoryService cateservice = new Services.RoomCategoryService();

        public class RoomState
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public int Value { get; set; }
        }

        public RoomControl(LR.Services.IRoomService _service)
        {
            InitializeComponent();
            this.service = (LR.Services.RoomService)_service;
            List<RoomState> stateSource = new List<RoomState>()
            {
                new RoomState(){ Name = "正常", ID = 0, Value = 200},
                new RoomState() { Name = "删除", ID = 1, Value = 400}
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
            List<LR.Entity.Room> di = this.service.All();
            for (int i = 0; i < di.Count; i++)
            {
                lvwShow.Items.Add(new
                {
                    No = di[i].No,
                    Name = di[i].Name,
                    Type = "包间", // cateservice.Single(di[i].CategoryID).Name,
                    Summary = di[i].Summary,
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
                cboType.Text = ss[2].Split('=')[1].Trim();
                txtSummary.Text = ss[3].Split('=')[1].Trim();
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
                cboType,
                txtSummary,
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
            LR.Entity.Room room = new LR.Entity.Room();
            room.No = txtNo.Text;
            room.Name = txtName.Text;
            room.CategoryID = new Guid(); //TODO
            room.Summary = txtSummary.Text;
            room.State = int.Parse(cboState.Text);
            this.service.Insert(room);
            this.InitListView();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            #region 控件列表集合
            List<Control> con_list = new List<Control>()
            {
                txtNo,
                txtName,
                cboType,
                txtSummary,
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
            LR.Entity.Room room = new LR.Entity.Room();
            var id = this.service.Single(item => item.No == txtNo.Text).ID;
            room.No = txtNo.Text;
            room.Name = txtName.Text;
            room.CategoryID = new Guid(); //TODO
            room.Summary = txtSummary.Text;
            room.State = int.Parse(cboState.Text);
            this.service.Update(id , room);
            this.InitListView();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            LR.Entity.Room room = new LR.Entity.Room();
            room.State = 400;
            var id = this.service.Single(item => item.No == room.No).ID;
            this.service.Update(id, new { room.ID });
            this.InitListView();
        }
    }
}
