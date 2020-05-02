using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LR.WpfApp.Models
{
    public class TabSource
    {
        public string Header { get; set; }
        public Type ControlType { get; set; }
        public int Order { get; set; }
        public LR.WpfApp.Controls.UseTo UserTo { get; set; }
        public static TabSource[] GetTabSources(LR.WpfApp.Controls.UseTo useTo)
        {
            return typeof(TabSource).Assembly.GetTypes().Where(item => item.GetCustomAttributes(typeof(LR.WpfApp.Controls.UserControlUseAttribute), true).Length == 1).Select(item =>
            {

                var attr = item.GetCustomAttributes(typeof(LR.WpfApp.Controls.UserControlUseAttribute), true)[0] as LR.WpfApp.Controls.UserControlUseAttribute;

                return new TabSource
                {
                    ControlType = item,
                    Header = attr.TabHeader,
                    Order = attr.Order,
                    UserTo = attr.UseTo
                };
            }).Where(item => item.UserTo == useTo).OrderBy(item => item.Order)
            .ToArray();
        }
    }
}
