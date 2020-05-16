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
    /// RoomControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.MainWindow, TabHeader = "房间管理", Order = 15)]
    public partial class RoomControl : UserControl
    {
        LR.Services.IRoomService _service;
        LR.Services.IRoomCategoryService _cateservice;

        public RoomControl(LR.Services.IRoomService service, LR.Services.IRoomCategoryService cateservice)
        {
            InitializeComponent();
            this._service = service;
            this._cateservice = cateservice;

            this.cbxCategory.ItemsSource = cateservice.List();
            this.cbxCategory.DisplayMemberPath = "Name";

            this.InitListView();

            this.btns.OnSave += Btns_OnSave;
            this.btns.OnDelete += Btns_OnDelete;
            this.btns.OnReset += Btns_OnReset;
            this.lvwShow.SelectionChanged += LvwShow_SelectionChanged1;
        }
        private bool Btns_OnDelete()
        {
            this._service.Update(btns.DataID, new { State = (int)Services.StaffState.Delete });
            this.InitListView();
            return true;
        }

        private void Btns_OnReset()
        {
            this.txtNo.Text = "";
            this.txtName.Text = "";
            this.cbxCategory.SelectedItem = null;
        }

        private bool Btns_OnSave()
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text) || string.IsNullOrWhiteSpace(this.txtNo.Text))
            {
                MessageBox.Show("输入不完整");
                return false;
            }
            if (this.cbxCategory.SelectedItem == null)
            {
                MessageBox.Show("未选择房间类型", "提示");
                return false;
            }
            LR.Entity.Room room = new LR.Entity.Room()
            {
                No = this.txtNo.Text,
                Name = this.txtName.Text,
                Summary = this.txtSummary.Text,
                CategoryID = (this.cbxCategory.SelectedItem as Entity.RoomCategory).ID
            };
            if (btns.IsAdd)
            {
                this._service.Insert(room);
            }
            else
            {
                this._service.Update(btns.DataID, new
                {
                    room.Name,
                    room.No,
                    room.Summary,
                    room.CategoryID
                });
            }
            this.InitListView();
            return true;
        }

        private void InitListView()
        {
            this.lvwShow.ItemsSource = this._service.GetAll();
        }
        private void LvwShow_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
            {
                this.txtNo.Text = this.lvwShow.SelectedItem.GetObjectValue<string>("No");
                this.txtName.Text = this.lvwShow.SelectedItem.GetObjectValue<string>("Name");
                this.txtSummary.Text = this.lvwShow.SelectedItem.GetObjectValue<string>("Summary");

                this.btns.SetEdit(this.lvwShow.SelectedItem.GetObjectValue<Guid>("ID"));
            }
        }
    }
}
