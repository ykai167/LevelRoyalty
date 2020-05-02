using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.WpfApp.Controls
{
    /*普通管理员tabs
     * <TabItem Header="员工">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="房间">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="工作组">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="消费记录">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="奖励数据">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="奖励发放">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="日志">
                <Controls:AdminsControl />
            </TabItem>*/

    /*超级管理员tabs
     * <TabItem Header="系统管理员设置">
            <Controls:AdminsControl />
        </TabItem>
        <TabItem Header="房间类型设置">
            <Controls:AdminsControl />
        </TabItem>
        <TabItem Header="员工级别设置">
            <Controls:AdminsControl />
        </TabItem>
        <TabItem Header="工作组管理员类别设置">
            <Controls:AdminsControl />
        </TabItem>
        <TabItem Header="员工奖励参数设置">
            <Controls:AdminsControl />
        </TabItem>
        <TabItem Header="超级管理员日志">
            <Controls:AdminsControl />
        </TabItem>*/
    public class UserControlUseAttribute : Attribute
    {
        public UserControlUseAttribute(UseTo useTo)
        {
            this.UseTo = useTo;
        }
        public UseTo UseTo { get; }
        public string TabHeader { get; set; }
        public int Order { get; set; }
    }

    public enum UseTo
    {
        MainWindow,
        SuperAdminWindow
    }

   
}
