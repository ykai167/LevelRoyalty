using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LR.WpfApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            Tools.DIHelper.RegistTransient<LR.Services.IConsumeDataService, LR.Services.ConsumeDataService>();

            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

            //判断:未创建超级管理员,创建超级管理员及密码



            LoginWindow window = new LoginWindow();
            bool? dialogResult = window.ShowDialog();
            if ((dialogResult.HasValue == true) &&
                (dialogResult.Value == true))
            {
                if (LR.Services.Administrator.Current.Type == Services.AdminType.Super)
                {
                    this.MainWindow = new SuperAdminWindow();
                }
                else
                {
                    this.MainWindow = new MainWindow();
                }
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                this.MainWindow.Show();
            }
            else
            {
                this.Shutdown();
            }
        }
    }
}
