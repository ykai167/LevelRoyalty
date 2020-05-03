using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.WpfApp.Models
{
    public class LayOutViewModel : INotifyPropertyChanged
    {
        public LayOutViewModel()
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                while (true)
                {
                    this.DateTime = await System.Threading.Tasks.Task.Delay(1000).ContinueWith<DateTime>(t =>
                    {
                        return DateTime.Now;
                    });
                }
            });
            this.DateTime = DateTime.Now;
        }
        string adminName;
        public string AdminName
        {
            get
            {
                return adminName;
            }

            set
            {
                this.adminName = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdminName)));
            }
        }

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
}
