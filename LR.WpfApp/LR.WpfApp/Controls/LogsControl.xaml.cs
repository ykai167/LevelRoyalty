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
    /// LogsControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.SuperAdminWindow, TabHeader = "日志列表", Order = 30)]
    public partial class LogsControl : UserControl
    {
        LR.Services.ILogService _service;
        int curentPage = 1;
        int pageSize = Tools.ConfigHelper.AppSettings.PageSize;

        public LogsControl(LR.Services.ILogService service)
        {
            InitializeComponent();
            this._service = service;

            this.lvwShow.SelectionChanged += LvwShow_SelectionChanged;
            this.ucPager.FirstPage += UcPager_FirstPage;
            this.ucPager.LastPage += UcPager_LastPage;
            this.ucPager.PreviousPage += UcPager_PreviousPage;
            this.ucPager.NextPage += UcPager_NextPage;

            this.Loaded += LogsControl_Loaded;
        }

        private void LvwShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == lvwShow && lvwShow.SelectedItem != null)
            {
                this.lvwLog.ItemsSource = _service.GetHistory(lvwShow.SelectedItem.GetObjectValue<Guid>("DataID"));
            }
        }

        private void LogsControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitData();
        }

        private void UcPager_NextPage(object sender, RoutedEventArgs e)
        {
            this.curentPage = page.TotalPage > this.curentPage ? this.curentPage + 1 : this.curentPage;
            InitData();
        }

        private void UcPager_PreviousPage(object sender, RoutedEventArgs e)
        {
            this.curentPage = this.curentPage > 1 ? this.curentPage - 1 : this.curentPage;
            InitData();
        }

        private void UcPager_LastPage(object sender, RoutedEventArgs e)
        {
            this.curentPage = page.TotalPage;
            InitData();
        }

        private void UcPager_FirstPage(object sender, RoutedEventArgs e)
        {
            this.curentPage = 1;
            InitData();
        }

        Pager<object> page;
        void InitData()
        {
            page = this._service.GetPage(curentPage, pageSize);
            this.ucPager.TotalPage = page.TotalPage.ToString();
            this.ucPager.CurrentPage = this.curentPage.ToString();
            this.lvwShow.ItemsSource = page;
        }
    }
}
