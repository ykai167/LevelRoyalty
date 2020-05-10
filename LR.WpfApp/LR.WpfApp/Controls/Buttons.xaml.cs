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
    /// Buttons.xaml 的交互逻辑
    /// </summary>
    public partial class Buttons : UserControl
    {
        public Buttons()
        {
            InitializeComponent();
            this.btnSave.Visibility = this.btnDelete.Visibility = Visibility.Hidden;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.btnAdd)
            {
                this.OnAdd?.Invoke(this, e);
                this.btnDelete.Visibility = Visibility.Hidden;
                this.btnSave.Visibility = Visibility.Visible;
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.btnSave)
            {
                this.OnSave?.Invoke(this, e);
                this.btnSave.Visibility = this.btnDelete.Visibility = Visibility.Hidden;
                e.Handled = true;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.btnDelete)
            {
                this.OnDelete?.Invoke(this, e);
                this.btnSave.Visibility = this.btnDelete.Visibility = Visibility.Hidden;
                e.Handled = true;
            }
        }

        public event EventHandler OnAdd;
        public event EventHandler OnDelete;
        public event EventHandler OnSave;

        public void SetEdit()
        {
            this.btnSave.Visibility = this.btnDelete.Visibility = Visibility.Visible;
        }
    }
}
