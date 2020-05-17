using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
            var name = AppDomain.CurrentDomain.FriendlyName;
            var old = System.Diagnostics.Process.GetProcessesByName(name.Replace(".exe", ""));
            if (old.Length > 1)
            {
                var r = MessageBox.Show("程序启动, 是否关闭后重新启动?", "提示", MessageBoxButton.YesNo);
                if (r == MessageBoxResult.Yes)
                {
                    old.OrderBy(o => o.StartTime).First().Kill();
                }
                else
                {
                    Process.GetCurrentProcess().Kill();
                }
            }



            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            var baseType = typeof(LR.Services.IQueryService<>);
            var types = baseType.Assembly.GetTypes()
                .Where(t => !t.IsInterface && !t.IsGenericType && t.GetInterfaces().Any(p => p.Name == baseType.Name));
            foreach (var type in types)
            {
                Tools.DIHelper.RegistTransient(type.GetInterfaces().Last(), type);
            }
            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

            //判断:未创建超级管理员,创建超级管理员及密码

            if (!Services.Initer.IsInit)
            {
                var result = new InitWindow().ShowDialog();
                if (!result.HasValue || !result.Value)
                {
                    this.Shutdown();
                }
            }

            LoginWindow window = Tools.DIHelper.GetInstance<LoginWindow>();
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

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出      
                MessageBox.Show("UI线程异常:" + e.Exception.Message + "\r\n" + e.Exception?.InnerException?.Message + "\r\n" + e.Exception.StackTrace);
            }
            catch (Exception)
            {
                //此时程序出现严重异常，将强制结束退出
                MessageBox.Show("UI线程发生致命错误！");
            }

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("非UI线程发生致命错误");
            }
            sbEx.Append("非UI线程异常：");
            if (e.ExceptionObject is Exception)
            {
                sbEx.Append(((Exception)e.ExceptionObject).Message);
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }
            MessageBox.Show(sbEx.ToString());
        }

        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //task线程内未处理捕获
            MessageBox.Show("Task线程异常：" + e.Exception.Message);
            e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }
    }
}
