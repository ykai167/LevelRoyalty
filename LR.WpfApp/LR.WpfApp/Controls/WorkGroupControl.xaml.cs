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
    public partial class WorkGroupControl : UserControl
    {
        IWorkGroupService _service;
        IWorkGroupManagerCategoryService _wService;
        IWorkGroupService _mService;

        public WorkGroupControl(IWorkGroupService service,
                IWorkGroupManagerCategoryService wService,
                IWorkGroupService mService)
        {
            InitializeComponent();
            this._wService = wService;
            this._mService = mService;
            this._service = service;
            var managerC = _wService.List();
            if (managerC.Count > 0)
            {
                this.cbxManager.ItemsSource = managerC;
                this.cbxManager.SelectedIndex = 0;
            }
            InitListView();
            this.lvwShow.SelectionChanged += LvwShow_SelectionChanged1;
            this.lvwStaff.SelectionChanged += LvwStaff_SelectionChanged;
            this.btnSet.Click += Btn_Click;
            this.btnCancel.Click += Btn_Click;
            this.btnRemove.Click += Btn_Click;

            this.btns.OnSave += Btns_OnSave;
            this.btns.OnDelete += Btns_OnDelete;
            this.btns.OnReset += Btns_OnAdd;
        }

        private void Btns_OnAdd()
        {
            this.lvwShow.SelectedItem = null;
            this.txtName.Text = "";
        }

        private bool Btns_OnDelete()
        {
            var r = this._service.Delete(this.btns.DataID);
            if (r.Success)
            {
                this.InitListView();
                return true;
            }
            else
            {
                MessageBox.Show(r.Message, "错误");
                return false;
            }
        }

        private bool Btns_OnSave()
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                MessageBox.Show("未输入或输入无效", "提示");
                return false;
            }
            if (this.btns.IsAdd)
            {
                this._service.Insert(new Entity.WorkGroup { Name = this.txtName.Text });
            }
            else
            {
                this._service.Update(this.btns.DataID, new { Name = this.txtName.Text });
            }
            this.InitListView();
            return true;
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
                    this.lvwStaff.ItemsSource = this._service.GetMembers(this.btns.DataID);
                }
            }
        }

        Guid MemberID;
        private void LvwStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwStaff && this.lvwStaff.SelectedItem != null)
            {
                MemberID = this.lvwStaff.SelectedItem.GetObjectValue<Guid>(nameof(MemberID));
            }
        }

        private void LvwShow_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
            {
                this.txtName.Text = this.lvwShow.SelectedItem.GetObjectValue("Name")?.ToString();
                var dataId = (Guid)this.lvwShow.SelectedItem.GetObjectValue("ID");

                //加载成员
                this.lvwStaff.ItemsSource = this._service.GetMembers(dataId);
                this.btns.SetEdit(dataId);
            }
        }

        private void InitListView()
        {
            this.txtName.Text = "";
            this.lvwStaff.ItemsSource = null;
            this.lvwShow.ItemsSource = this._service.GetAll();
        }
    }
}
