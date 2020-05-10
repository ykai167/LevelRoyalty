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
    /// WorkGroup.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.MainWindow, TabHeader = "工作组管理", Order = 20)]
    public partial class WorkGroup : UserControl
    {
        IWorkGroupService _service;
        IWorkGroupManagerCategoryService _wService;
        IWorkGroupService _mService;

        public WorkGroup(IWorkGroupService service,
                IWorkGroupManagerCategoryService wService,
                IWorkGroupService mService)
        {
            InitializeComponent();
            this._wService = wService;
            this._mService = mService;
            this._service = service;
            this.cbxManager.ItemsSource = _wService.List();
            InitListView();
            this.lvwShow.SelectionChanged += LvwShow_SelectionChanged1;
            this.lvwStaff.SelectionChanged += LvwStaff_SelectionChanged;
            this.btnSet.Click += Btn_Click;
            this.btnCancel.Click += Btn_Click;
            this.btnRemove.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (MemberID == new Guid())
            {
                MessageBox.Show("未选择组员", "提示");
                return;
            }
            bool isTrue = false;
            LR.Models.OperateResult result = null;
            if (sender == this.btnSet)
            {
                if (this.cbxManager.SelectedItem == null)
                {
                    MessageBox.Show("未选择管理员类别", "提示");
                    return;
                }
                isTrue = true;
                result = _service.SetManager(MemberID, (Guid)this.cbxManager.SelectedValue);
            }
            else if (sender == this.btnCancel)
            {
                isTrue = true;
                result = _service.CancelManager(MemberID);
            }
            else if (sender == this.btnRemove)
            {
                isTrue = true;
                result = _service.RemoveMember(MemberID);
            }
            if (isTrue)
            {
                if (!result.Success)
                {
                    MessageBox.Show(result.Message, "提示");
                }
                else
                {
                    //加载成员
                    this.lvwStaff.ItemsSource = this._service.GetMembers(groupID);
                }
            }
        }

        Guid MemberID;
        private void LvwStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwStaff && this.lvwStaff.SelectedItem != null)
            {
                MemberID = (Guid)this.lvwStaff.SelectedItem.GetValue(nameof(MemberID));
            }
        }

        Guid groupID;
        private void LvwShow_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
            {
                this.txtName.Text = this.lvwShow.SelectedItem.GetValue("Name")?.ToString();
                this.groupID = (Guid)this.lvwShow.SelectedItem.GetValue("ID");
                this.btnSave.Visibility = this.btnDelete.Visibility = Visibility.Visible;

                //加载成员
                this.lvwStaff.ItemsSource = this._service.GetMembers(groupID);
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
            if (sender == this.btnAdd)
            {
                this.btnDelete.Visibility = Visibility.Hidden;
                this.btnSave.Visibility = Visibility.Visible;
                this.groupID = new Guid();
                this.lvwShow.SelectedItem = null;
                this.txtName.Text = "";
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.btnSave)
            {
                if (string.IsNullOrWhiteSpace(this.txtName.Text))
                {
                    MessageBox.Show("未输入或输入无效", "提示");
                    return;
                }
                if (this.groupID == new Guid())
                {
                    this._service.Insert(new Entity.WorkGroup { Name = this.txtName.Text });
                }
                else
                {
                    this._service.Update(groupID, new { Name = this.txtName.Text });
                }
                this.InitListView();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.btnDelete)
            {
                this._service.Update(this.groupID, new { State = (int)DataState.Delete });
                this.InitListView();
            }
        }
    }
}
