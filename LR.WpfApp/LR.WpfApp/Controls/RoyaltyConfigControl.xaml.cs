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
    /// <summary>
    /// RoyaltyConfiControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.SuperAdminWindow, TabHeader = "奖励参数设置")]
    public partial class RoyaltyConfigControl : UserControl
    {
        IRoyaltyConfigService _service;
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
                                    var txb2 = GenTextBlock($"名下[{downers[index].Name}]订房消费额百分比", HorizontalAlignment.Left);
                                    grid.Children.Add(txb2);
                                    Grid.SetRow(txb2, rowIndex);
                                    Grid.SetColumn(txb2, 2);

                                    var royalty = this._service.GetConfig(item, level2.ID, downers[index].ID);
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

                                var txb2 = GenTextBlock($"直推[{level3.Name}]团队订房消费额百分比", HorizontalAlignment.Left);
                                grid.Children.Add(txb2);
                                Grid.SetRow(txb2, rowIndex);
                                Grid.SetColumn(txb2, 2);

                                var royalty = this._service.GetConfig(item, level3.ID, level3.ID);
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

                                var txb2 = GenTextBlock($"直推[{level4.Upper.Name}]团队订房消费额百分比", HorizontalAlignment.Left);
                                grid.Children.Add(txb2);
                                Grid.SetRow(txb2, rowIndex);
                                Grid.SetColumn(txb2, 2);
                                var royalty = this._service.GetConfig(item, level4.ID, level4.Upper.ID);
                                var txt = ToTextBox(royalty.ID, item, royalty.Percent);
                                grid.Children.Add(txt);
                                Grid.SetRow(txt, rowIndex++);
                                Grid.SetColumn(txt, 1);
                                level4 = level4.Upper;
                            }
                            break;
                        case RoyaltyType.WorkGroupRoyalty:
                            grid.RowDefinitions.Add(new RowDefinition());
                            var txbw = GenTextBlock($"总奖励：");
                            grid.Children.Add(txbw);
                            Grid.SetRow(txbw, rowIndex);
                            Grid.SetColumn(txbw, 0);

                            var txbw2 = GenTextBlock($"组消费总额百分比", HorizontalAlignment.Left);
                            grid.Children.Add(txbw2);
                            Grid.SetRow(txbw2, rowIndex);
                            Grid.SetColumn(txbw2, 2);
                            var royaltyW = this._service.GetConfig(item, new Guid(), new Guid());
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
                Text = text,
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
            txt.LostFocus += Txt_LostFocus;// += Txt_TextChanged;
            return txt;
        }

        private void Txt_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
            var value = txt.Text;
            if (!System.Text.RegularExpressions.Regex.IsMatch(@"\d+.?\d?", value))
            {

            }
            Guid id = Guid.Parse(txt.Name.Substring(4).Replace("_", "-"));
        }
    }
}
