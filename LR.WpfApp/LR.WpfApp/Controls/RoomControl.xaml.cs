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
        LR.Services.IRoomService service;


        public RoomControl(LR.Services.IRoomService _service)
        {
            this.service = _service;

            InitializeComponent();
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
                    Type = di[i].CategoryID,
                    Summary = di[i].Summary,
                    Status = di[i].State
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
                txtNo.Text = ss[0].Substring(10).Replace("=", "").Trim();
                txtName.Text = ss[1].Substring(6).Replace("=", "").Trim();
                cboType.Text = ss[2].Substring(6).Replace("=", "").Trim();
                txtState.Text = ss[3].Substring(10).Replace("=", "").Trim();
                txtSummary.Text = ss[4].Substring(6).Replace("=", "").Trim();
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
                txtState
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
            room.State = int.Parse(txtState.Text);
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
                txtState
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
            room.ID = this.service.Single(item => item.No == txtNo.Text).ID;
            room.No = txtNo.Text;
            room.Name = txtName.Text;
            room.CategoryID = new Guid(); //TODO
            room.Summary = txtSummary.Text;
            room.State = int.Parse(txtState.Text);
            this.service.Update(room.ID, new { room.No, room.Name, room.CategoryID, room.Summary, room.State });
            this.InitListView();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            LR.Entity.Room room = new LR.Entity.Room();
            room.State = 400;
            room.ID = this.service.Single(item => item.No == room.No).ID;
            this.service.Update(room.ID, new { room.ID });
            this.InitListView();
        }
    }
}
