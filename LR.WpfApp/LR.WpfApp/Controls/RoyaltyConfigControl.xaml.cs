using LR.Services;
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
    class MessageHelper
    {
        public string GroupName { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// RoyaltyConfiControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.SuperAdminWindow, TabHeader = "奖励参数设置")]
    public partial class RoyaltyConfigControl : UserControl
    {
        IRoyaltyConfigService _service;
        Dictionary<Guid, MessageHelper> lables = new Dictionary<Guid, MessageHelper>();
        public RoyaltyConfigControl(IRoyaltyConfigService service)
        {
            InitializeComponent();
            this._service = service;
            this.Loaded += RoyaltyConfiControl_Loaded;
        }

        private void RoyaltyConfiControl_Loaded(object sender, RoutedEventArgs e)
        {
            var groups = Enum.GetValues(typeof(RoyaltyType)).Cast<RoyaltyType>()
                .Select(item =>
                {
                    var groupBox = new GroupBox { Header = item.GetName() };
                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Pixel) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { });
                    groupBox.Content = grid;
                    int rowIndex = 0;
                    switch (item)
                    {
                        case RoyaltyType.Reservation:
                            var level = LR.Services.Level.Min;
                            while (level != null)
                            {
                                grid.RowDefinitions.Add(new RowDefinition());
                                var txb = GenTextBlock($"{level.Name}：");
                                grid.Children.Add(txb);
                                Grid.SetRow(txb, rowIndex);
                                Grid.SetColumn(txb, 0);

                                var txb2 = GenTextBlock($"本人订房消费额百分比", HorizontalAlignment.Left);
                                grid.Children.Add(txb2);
                                Grid.SetRow(txb2, rowIndex);
                                Grid.SetColumn(txb2, 2);

                                var royalty = this._service.GetConfig(item, level.ID, new Guid());
                                AddLabel(royalty.ID, item.GetName(), level.Name);

                                var txt = ToTextBox(royalty.ID, item, royalty.Percent);

                                grid.Children.Add(txt);
                                Grid.SetRow(txt, rowIndex++);
                                Grid.SetColumn(txt, 1);
                                level = level.Upper;
                            }
                            break;
                        case RoyaltyType.Administration:
                            var level2 = LR.Services.Level.Min.Upper;
                            while (level2 != null)
                            {
                                var downers = level2.Downer();
                                var txb = GenTextBlock($"{level2.Name}：");
                                grid.Children.Add(txb);
                                Grid.SetRow(txb, rowIndex);
                                Grid.SetColumn(txb, 0);
                                Grid.SetRowSpan(txb, downers.Length);
                                for (int index = 0; index < downers.Length; index++)
                                {
                                    grid.RowDefinitions.Add(new RowDefinition { });
                                    var txb2 = GenTextBlock($"名下[ {downers[index].Name} ]订房消费额百分比", HorizontalAlignment.Left);
                                    grid.Children.Add(txb2);
                                    Grid.SetRow(txb2, rowIndex);
                                    Grid.SetColumn(txb2, 2);

                                    var royalty = this._service.GetConfig(item, level2.ID, downers[index].ID);
                                    AddLabel(royalty.ID, item.GetName(), level2.Name);
                                    var txt = ToTextBox(royalty.ID, item, royalty.Percent);
                                    grid.Children.Add(txt);
                                    Grid.SetRow(txt, rowIndex++);
                                    Grid.SetColumn(txt, 1);
                                }
                                level2 = level2.Upper;
                            }
                            break;
                        case RoyaltyType.Cooperation:
                            var level3 = LR.Services.Level.Min.Upper;
                            while (level3 != null)
                            {
                                grid.RowDefinitions.Add(new RowDefinition());
                                var txb = GenTextBlock($"{level3.Name}：");
                                grid.Children.Add(txb);
                                Grid.SetRow(txb, rowIndex);
                                Grid.SetColumn(txb, 0);

                                var txb2 = GenTextBlock($"直推[ {level3.Name} ]团队订房消费额百分比", HorizontalAlignment.Left);
                                grid.Children.Add(txb2);
                                Grid.SetRow(txb2, rowIndex);
                                Grid.SetColumn(txb2, 2);

                                var royalty = this._service.GetConfig(item, level3.ID, level3.ID);
                                AddLabel(royalty.ID, item.GetName(), level3.Name);
                                var txt = ToTextBox(royalty.ID, item, royalty.Percent);
                                grid.Children.Add(txt);
                                Grid.SetRow(txt, rowIndex++);
                                Grid.SetColumn(txt, 1);
                                level3 = level3.Upper;
                            }
                            break;
                        case RoyaltyType.Transcend:
                            var level4 = LR.Services.Level.Min.Upper;
                            while (level4.Upper != null)
                            {
                                grid.RowDefinitions.Add(new RowDefinition());
                                var txb = GenTextBlock($"{level4.Name}：");
                                grid.Children.Add(txb);
                                Grid.SetRow(txb, rowIndex);
                                Grid.SetColumn(txb, 0);

                                var txb2 = GenTextBlock($"直推[ {level4.Upper.Name} ]团队订房消费额百分比", HorizontalAlignment.Left);
                                grid.Children.Add(txb2);
                                Grid.SetRow(txb2, rowIndex);
                                Grid.SetColumn(txb2, 2);
                                var royalty = this._service.GetConfig(item, level4.ID, level4.Upper.ID);
                                AddLabel(royalty.ID, item.GetName(), level4.Name);
                                var txt = ToTextBox(royalty.ID, item, royalty.Percent);
                                grid.Children.Add(txt);
                                Grid.SetRow(txt, rowIndex++);
                                Grid.SetColumn(txt, 1);
                                level4 = level4.Upper;
                            }
                            break;
                        case RoyaltyType.WorkGroupRoyalty:
                            grid.RowDefinitions.Add(new RowDefinition());
                            string label = "总奖励";
                            var txbw = GenTextBlock($"{label}：");
                            grid.Children.Add(txbw);
                            Grid.SetRow(txbw, rowIndex);
                            Grid.SetColumn(txbw, 0);

                            var txbw2 = GenTextBlock($"组消费总额百分比", HorizontalAlignment.Left);
                            grid.Children.Add(txbw2);
                            Grid.SetRow(txbw2, rowIndex);
                            Grid.SetColumn(txbw2, 2);
                            var royaltyW = this._service.GetConfig(item, new Guid(), new Guid());
                            AddLabel(royaltyW.ID, item.GetName(), label);
                            var txtw = ToTextBox(royaltyW.ID, item, royaltyW.Percent);
                            grid.Children.Add(txtw);
                            Grid.SetRow(txtw, rowIndex++);
                            Grid.SetColumn(txtw, 1);
                            break;
                        case RoyaltyType.WorkGroup:
                            var wgmc = LR.Services.WorkGroupManagerCategory.WorkGroupManagerCategories;
                            for (int i = 0; i < wgmc.Length; i++)
                            {
                                var category = wgmc[i];
                                grid.RowDefinitions.Add(new RowDefinition());
                                var txb = GenTextBlock($"{category.Name}：");
                                grid.Children.Add(txb);
                                Grid.SetRow(txb, rowIndex);
                                Grid.SetColumn(txb, 0);

                                var txb2 = GenTextBlock($"奖励总额的百分比", HorizontalAlignment.Left);
                                grid.Children.Add(txb2);
                                Grid.SetRow(txb2, rowIndex);
                                Grid.SetColumn(txb2, 2);
                                var royalty = this._service.GetConfig(item, category.ID, new Guid());
                                AddLabel(royalty.ID, item.GetName(), category.Name);
                                var txt = ToTextBox(royalty.ID, item, royalty.Percent);
                                grid.Children.Add(txt);
                                Grid.SetRow(txt, rowIndex++);
                                Grid.SetColumn(txt, 1);
                            }
                            break;
                        default:
                            break;
                    }
                    return groupBox;
                }).ToArray();
            for (int index = 0; index < groups.Length; index++)
            {
                this.gridMain.RowDefinitions.Add(new RowDefinition { });
                var group = groups[index];
                this.gridMain.Children.Add(group);
                Grid.SetRow(group, index);
            }
        }

        TextBlock GenTextBlock(string text, HorizontalAlignment hAling = HorizontalAlignment.Right)
        {
            return new TextBlock
            {
                Text = $"% ({text})",
                HorizontalAlignment = hAling,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
        TextBox ToTextBox(Guid id, LR.Services.RoyaltyType type, decimal value)
        {
            var txt = new TextBox()
            {
                Name = $"txt_{id.ToString().Replace("-", "_")}",
                Margin = new Thickness(10, 0, 10, 0),
                MaxHeight = 30,
                Text = value.ToString()
            };
            txt.GotFocus += Txt_GotFocus;
            txt.LostFocus += Txt_LostFocus;// += Txt_TextChanged;
            return txt;
        }

        decimal oldValue = 0;
        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            var value = txt.Text;
            decimal.TryParse(value, out oldValue);
            e.Handled = true;
        }

        private async void Txt_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            var value = txt.Text;
            if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d+(\.\d+)?$"))
            {
                Guid id = Guid.Parse(txt.Name.Substring(4).Replace("_", "-"));
                decimal newValue = decimal.Parse(value);
                if (newValue <= 100 && oldValue != newValue)
                {
                    var result = MessageBox.Show($"{lables[id].GroupName}[{lables[id].Name}]\r\n由[{oldValue}]改为[{newValue}],是否保存?", "提示", MessageBoxButton.YesNo);

                    txt.Text = await Task.Run<string>(() =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            this._service.Update(id, new { Percent = newValue });
                            return newValue.ToString();
                        }
                        else
                        {
                            return oldValue.ToString();
                        }
                    });
                }
                else
                {
                    txt.Text = await Task.Run<string>(() => newValue <= 100 ? newValue.ToString() : "0");
                }
            }
            else
            {
                MessageBox.Show("请输入100以内数字", "提示");
                txt.Text = await Task.Run<string>(() => oldValue <= 100 ? oldValue.ToString() : "0");
            }
            e.Handled = true;
        }

        void AddLabel(Guid id, string group, string label)
        {
            lables[id] = new MessageHelper { GroupName = group, Name = label };
        }
    }
}
