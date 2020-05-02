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
    public partial class RoyaltyConfiControl : UserControl
    {
        public RoyaltyConfiControl()
        {
            InitializeComponent();

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

                                var txt = ToTextBox(level, item);
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

                                    var txt = ToTextBox(level2, item);
                                    grid.Children.Add(txt);
                                    Grid.SetRow(txt, rowIndex++);
                                    Grid.SetColumn(txt, 1);
                                }
                                level2 = level2.Upper;
                            }
                            break;
                        case RoyaltyType.Cooperation:
                            break;
                        case RoyaltyType.Transcend:
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
        TextBox ToTextBox(LR.Services.Level level, LR.Services.RoyaltyType type)
        {
            var txt = new TextBox()
            {
                Name = $"{type.ToString()}_{level.ID.ToString().Replace("-", "_")}",
                Margin = new Thickness(10, 0, 10, 0)
            };
            txt.TextChanged += Txt_TextChanged;
            return txt;
        }

        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageBox.Show(string.Join(",", e.Changes.Select(item => item.AddedLength)) + (sender as TextBox).Text);
        }
    }
}
