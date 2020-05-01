using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class LayOutViewModel : INotifyPropertyChanged
    {
        public string AdminName { get; set; }
        DateTime dateTime;
        public DateTime DateTime
        {
            get
            {
                return dateTime;
            }
            set
            {
                this.dateTime = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateTime)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    /// <summary>
    /// LayoutControl.xaml 的交互逻辑
    /// </summary>
    public partial class LayoutControl : UserControl
    {
        LayOutViewModel vm;
        public LayoutControl()
        {
            InitializeComponent();

            vm = new LayOutViewModel()
            {
                AdminName = LR.Services.Administrator.Current.Name,
                DateTime = DateTime.Now
            };
            this.DataContext = vm;


            this.Loaded += LayoutControl_Loaded;
        }

        private void LayoutControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                while (true)
                {
                    vm.DateTime = await System.Threading.Tasks.Task.Delay(1000).ContinueWith<DateTime>(t =>
                    {
                        return DateTime.Now;
                    });
                }
            });
        }

        public TabControl TabControl
        {
            get
            {
                return this.TabMain;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
