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
    /// WorkGroup.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.MainWindow, TabHeader = "工作组管理", Order = 20)]
    public partial class WorkGroup : UserControl
    {
        LR.Services.IWorkGroupService _service;

        public WorkGroup(LR.Services.IWorkGroupService service)
        {
            InitializeComponent();
            this._service = service;
            InitListView();
            this.lvwShow.SelectionChanged += LvwShow_SelectionChanged1;
        }

        Guid editID;
        private void LvwShow_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
            {
                this.txtName.Text = this.lvwShow.SelectedItem.GetValue("Name")?.ToString();
                this.editID = (Guid)this.lvwShow.SelectedItem.GetValue("ID");
                this.btnSave.Visibility = this.btnDelete.Visibility = Visibility.Visible;

                //加载成员
                this.lvwStaff.ItemsSource = this._service.GetMembers(editID);
            }
        }

        private void InitListView()
        {
            this.txtName.Text = "";
            this.btnAdd.IsEnabled = true;
            this.btnSave.Visibility = this.btnDelete.Visibility = Visibility.Hidden;
            this.lvwStaff.ItemsSource = null;
            this.lvwShow.ItemsSource = this._service.GetAll();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.btnDelete.Visibility = Visibility.Hidden;
            this.btnSave.Visibility = Visibility.Visible;
            this.editID = new Guid();
            this.lvwShow.SelectedItem = null;
            this.txtName.Text = "";
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                MessageBox.Show("未输入或输入无效", "提示");
                return;
            }
            if (this.editID == new Guid())
            {
                this._service.Insert(new Entity.WorkGroup { Name = this.txtName.Text });
            }
            else
            {
                this._service.Update(editID, new { Name = this.txtName.Text });
            }
            this.InitListView();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            this._service.Update(this.editID, new { State = (int)LR.Services.DataState.Delete });
            this.InitListView();
        }
    }
}
