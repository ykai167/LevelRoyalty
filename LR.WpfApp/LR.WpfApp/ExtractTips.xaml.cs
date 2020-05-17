using LR.Tools;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace LR.WpfApp
{
    /// <summary>
    /// ExtractTips.xaml 的交互逻辑
    /// </summary>
    public partial class ExtractTips : Window
    {
        LR.Services.IConsumeDataService _service;

        public ExtractTips(LR.Services.IConsumeDataService service)
        {
            this._service = service;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = this.dtStart.SelectedDate??DateTime.Now;
            DateTime end = this.dtEnd.SelectedDate?? DateTime.Now;
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.DefaultExt = "xls";
            sfd.Filter = "Excel文件(*.xls)|*.xls";
            sfd.Title = "导出文件路径";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataTable dt = IEnumerableHelper.ToDataTable<LR.Services.ConsumeDataModel>(this._service.GetExtractList(start, end.AddDays(1)));
                String[] columns = { "StaffNo", "StaffName", "RoomNo", "RoomName", "Amount", "Admin", "CreateDate", "ModifyDate" };
                String[] names = { "员工号", "姓名", "房间号", "房间", "金额", "操作人", "创建时间", "修改时间" };
                DataView dv = dt.DefaultView;
                dt = dv.ToTable(true, columns);

                DataRow dr = dt.NewRow();
                dr["RoomName"] = "总计";
                dr["Amount"] = dt.Compute("sum(Amount)", "");
                dt.Rows.Add(dr);

                for(int i = 0; i < columns.Length; i++)                
                {
                    dt.Columns[columns[i]].ColumnName = names[i];
                }
                
                ExcelHelper.DataTableToExcel(dt, sfd.FileName);
            }
            this.Close();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
    }
}
