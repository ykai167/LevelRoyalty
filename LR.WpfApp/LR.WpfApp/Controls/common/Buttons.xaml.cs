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
        const string Add = "[添加数据]";
        const string Edit = "[编辑数据]";
        public Buttons()
        {
            InitializeComponent();
            this.Reset();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.btnSave)
            {
                var success = this.OnSave?.Invoke() ?? false;
                if (success)
                {
                    this.tbxInfo.Text = Add;
                    this.Reset();
                    this.OnReset?.Invoke();
                }
                e.Handled = true;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.btnDelete)
            {
                var success = this.OnDelete?.Invoke() ?? false;
                if (success)
                {
                    this.Reset();
                    this.OnReset?.Invoke();
                }
                e.Handled = true;
            }
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.btnReset)
            {
                this.Reset();
                this.OnReset?.Invoke();
                e.Handled = true;
            }
        }

        public event Action OnReset;
        public event Func<bool> OnDelete;
        public event Func<bool> OnSave;

        public Guid DataID { get; private set; }
        public bool IsAdd { get { return DataID == Guid.Empty; } }
        public void SetEdit(Guid id)
        {
            this.tbxInfo.Text = id == Guid.Empty ? Add : Edit;
            this.DataID = id;
            if (ShowDelButton)
            {
                this.btnDelete.Visibility = Visibility.Visible;
            }
        }

        void Reset()
        {
            this.DataID = Guid.Empty;
            this.btnDelete.Visibility = Visibility.Collapsed;
            this.tbxInfo.Text = Add;
        }
        bool showDel = true;
        public bool ShowDelButton
        {
            set
            {
                showDel = value;
                if (!value)
                {
                    this.btnDelete.Visibility = Visibility.Collapsed;
                }
            }
            get
            {
                return showDel;
            }
        }
    }
}
