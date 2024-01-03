using System;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using IRMMAUI.Entity;
using Microsoft.Maui.Controls;
using Maui.DataGrid;
using System.ComponentModel;
using IRMMAUI.ViewModels;
using OxyPlot.Annotations;
using OxyPlot.Legends;

namespace IRMMAUI.Service.impl
{
    public class OxyPlotService : IOxyPlotService
    {

        private List<TableItem> dataModels;

        private double maxYValue;//Y值最大值

        private double maxXValue;//X轴最大值

        private ILineProcessService _lineProcessService;
        public OxyPlotService(ILineProcessService lineProcessService)
        {
            this._lineProcessService = lineProcessService;
        }

        public void GetListView(string title, List<string> list, out PlotModel plotModel, out List<TableItem> items)
        {
            buildDefaultPlotView(title, out plotModel);
            //buildDefaultDataGrid(out dataGrid);

            items = new List<TableItem>();


            var lineSeriesX = new LineSeries
            {
                Color = OxyColors.Green,
                LineJoin = OxyPlot.LineJoin.Bevel,
                Title = "soft",
                StrokeThickness = 3,
                MarkerType = MarkerType.Circle, // 设置标记类型
                MarkerSize = 8,                 // 设置标记大小
                MarkerFill = OxyColors.Green    // 设置标记填充颜色
            };
            var lineSeriesY = new LineSeries
            {
                Color = OxyColors.Red,
                LineJoin = OxyPlot.LineJoin.Bevel,
                StrokeThickness = 3,
                Title = "medium",
                MarkerType = MarkerType.Square, // 设置标记类型
                MarkerSize = 8,                 // 设置标记大小
                MarkerFill = OxyColors.Red    // 设置标记填充颜色
            };
            var lineSeriesZ = new LineSeries
            {
                Color = OxyColors.Blue,
                LineJoin = OxyPlot.LineJoin.Bevel,
                Title = "hard",
                StrokeThickness = 3,
                MarkerType = MarkerType.Triangle, // 设置标记类型
                MarkerSize = 8,                 // 设置标记大小
                MarkerFill = OxyColors.Blue    // 设置标记填充颜色
            };

            if (list != null && list.Count > 0)
            {
                maxYValue = 0.00D;
                maxXValue = 0.00D;
                foreach (var s in list)
                {
                    var strs = _lineProcessService.ProcessLine(s);
                    if (strs.Count > 6)
                    {
                        double xValue, yValue, zValue;
                        int temp = getTemperature(strs[1]);
                        getXYZValue(strs, out xValue, out yValue, out zValue);

                        lineSeriesX.Points.Add(new DataPoint(temp, xValue));
                        lineSeriesY.Points.Add(new DataPoint(temp, yValue));
                        lineSeriesZ.Points.Add(new DataPoint(temp, zValue));
                        items.Add(new TableItem
                        {
                            SampleID = strs[0],
                            Temperature = strs[1],
                            XValue = xValue.ToString(),
                            YValue = yValue.ToString(),
                            ZValue = zValue.ToString(),
                            XOrg = strs[2],
                            YOrg = strs[3],
                            ZOrg = strs[4],
                            C = strs[5],
                        });
                    }

                }

                plotModel.Series.Add(lineSeriesX);
                plotModel.Series.Add(lineSeriesY);
                plotModel.Series.Add(lineSeriesZ);
                buildLineAnnotation(plotModel);

                //dataGrid.BindingContext = new DataViewModel();
                //dataGrid.ItemsSource = dataModels;
            }
        }

        private void buildLineAnnotation(PlotModel plotModel)
        {

            var annotation = new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                X = 5,
                TextPosition = new DataPoint(2, 5),
                TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Left,
                TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom,

                //MinimumX = int.Parse(maxXValue.ToString()) - 100, // X 坐标位置，可以根据需要调整
                //MaximumX = int.Parse(maxXValue.ToString()) - 50,
                //Y = int.Parse(maxXValue.ToString()) - 5,
                Text = "Soft",
                StrokeThickness = 3,
                LineStyle = LineStyle.Solid, // 设置为 None，以消除线的显示
                Color = OxyColors.Gray, // 设置形状的颜色
                Layer = AnnotationLayer.AboveSeries
            };
            plotModel.Annotations.Add(annotation);
        }

        public void GetView(string title, List<string> list, out PlotModel plotModel, out TableView tableView)
        {
            buildDefaultPlotView(title, out plotModel);
            buildDefaultTable(title, out tableView);
            var lineSeriesX = new LineSeries
            {
                Color = OxyColors.Green,
                LineJoin = OxyPlot.LineJoin.Bevel,
                Title = "soft",
                MarkerType = MarkerType.Circle, // 设置标记类型
                MarkerSize = 6,                 // 设置标记大小
                MarkerFill = OxyColors.Green    // 设置标记填充颜色
            };
            var lineSeriesY = new LineSeries
            {
                Color = OxyColors.Red,
                LineJoin = OxyPlot.LineJoin.Bevel,
                Title = "medium",
                MarkerType = MarkerType.Square, // 设置标记类型
                MarkerSize = 6,                 // 设置标记大小
                MarkerFill = OxyColors.Red    // 设置标记填充颜色
            };
            var lineSeriesZ = new LineSeries
            {
                Color = OxyColors.Blue,
                LineJoin = OxyPlot.LineJoin.Bevel,
                Title = "hard",
                MarkerType = MarkerType.Triangle, // 设置标记类型
                MarkerSize = 6,                 // 设置标记大小
                MarkerFill = OxyColors.Blue    // 设置标记填充颜色
            };

            maxYValue = 0.00D;
            maxXValue = 0.00D;
            if (list != null && list.Count > 0)
            {
                foreach (var s in list)
                {
                    var strs = _lineProcessService.ProcessLine(s);
                    if (strs.Count > 6)
                    {
                        double xValue, yValue, zValue;
                        int temp = getTemperature(strs[1]);
                        getXYZValue(strs, out xValue, out yValue, out zValue);

                        lineSeriesX.Points.Add(new DataPoint(temp, xValue));
                        lineSeriesY.Points.Add(new DataPoint(temp, yValue));
                        lineSeriesZ.Points.Add(new DataPoint(temp, zValue));


                        var dataSection = new TableSection();

                        dataSection.Add(new TextCell { Text = strs[0], StyleId = "disclosure" });
                        dataSection.Add(new TextCell { Text = strs[1], StyleId = "disclosure" });
                        dataSection.Add(new TextCell { Text = xValue.ToString(), StyleId = "disclosure" });
                        dataSection.Add(new TextCell { Text = yValue.ToString(), StyleId = "disclosure" });
                        dataSection.Add(new TextCell { Text = zValue.ToString(), StyleId = "disclosure" });
                        dataSection.Add(new TextCell { Text = strs[2], StyleId = "disclosure" });
                        dataSection.Add(new TextCell { Text = strs[3], StyleId = "disclosure" });
                        dataSection.Add(new TextCell { Text = strs[4], StyleId = "disclosure" });
                        dataSection.Add(new TextCell { Text = strs[5], StyleId = "disclosure" });

                        // 将布局添加到ViewCell
                        tableView.Root.Add(dataSection);
                    }

                }
            }
            plotModel.Series.Add(lineSeriesX);
            plotModel.Series.Add(lineSeriesY);
            plotModel.Series.Add(lineSeriesZ);
        }

        private void buildDefaultPlotView(string title, out PlotModel plotModel)
        {
            plotModel = new PlotModel { Title = title, Legends = { }, IsLegendVisible = true };
            var xares = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "IRM(A/m)"
            };
            var yares = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Temperature(℃)",
            };
            plotModel.Axes.Add(xares);
            plotModel.Axes.Add(yares);

            var xlegen = new Legend
            {
                LegendTitleColor = OxyColor.FromUInt32(666666),
                LegendPosition = LegendPosition.RightTop,
                LegendSymbolLength = 80,
                LegendItemSpacing = 40,
                LegendFontSize = 20,
                LegendOrientation = LegendOrientation.Horizontal,
            };
            plotModel.Legends.Add(xlegen);
        }

        private void buildDefaultDataGrid(out DataGrid dataGrid)
        {
            dataGrid = new DataGrid
            {
                BackgroundColor = Colors.White,
                Background = Colors.Gray,
                HeaderBackground = Colors.Gray,
                HeaderBordersVisible = true,
                HeightRequest = 800,
                HeaderHeight = 80,
                RowHeight = 60,
                BorderColor = Colors.Gray,
            };
            dataGrid.Columns.Add(new DataGridColumn() { Title = "SampleID", PropertyName = "SampleID", BindingContext = "SampleID", Width = 100 });
            dataGrid.Columns.Add(new DataGridColumn() { Title = "Temperature", PropertyName = "Temperature", BindingContext = "Temperature", Width = 100 });
            dataGrid.Columns.Add(new DataGridColumn() { Title = "XValue", PropertyName = "XValue", BindingContext = "XValue", Width = 100 });
            dataGrid.Columns.Add(new DataGridColumn() { Title = "YValue", PropertyName = "YValue", BindingContext = "YValue", Width = 100 });
            dataGrid.Columns.Add(new DataGridColumn() { Title = "ZValue", PropertyName = "ZValue", BindingContext = "ZValue", Width = 100 });
            dataGrid.Columns.Add(new DataGridColumn() { Title = "XOrg", PropertyName = "XOrg", BindingContext = "XOrg", Width = 100 });
            dataGrid.Columns.Add(new DataGridColumn() { Title = "YOrg", PropertyName = "YOrg", BindingContext = "YOrg", Width = 100 });
            dataGrid.Columns.Add(new DataGridColumn() { Title = "ZOrg", PropertyName = "ZOrg", BindingContext = "ZOrg", Width = 100 });
            dataGrid.Columns.Add(new DataGridColumn() { Title = "C", PropertyName = "C", BindingContext = "C", Width = 100 });
        }

        private void buildDefaultTable(string title, out TableView tableView)
        {
            tableView = new TableView
            {
                VerticalOptions = LayoutOptions.Fill,
                HeightRequest = 800,
                Intent = TableIntent.Data,
                Root = new TableRoot
                {
                    new TableSection(){
                        new TextCell { Text = "SampleID",  StyleId = "disclosure" },
                        new TextCell { Text = "Temperature",  StyleId = "disclosure" },
                        new TextCell { Text = "XValue",  StyleId = "disclosure" },
                        new TextCell { Text = "YValue",  StyleId = "disclosure" },
                        new TextCell { Text = "ZValue",  StyleId = "disclosure" },
                        new TextCell { Text = "XOrg",  StyleId = "disclosure" },
                        new TextCell { Text = "YOrg",  StyleId = "disclosure" },
                        new TextCell { Text = "ZOrg",  StyleId = "disclosure" },
                        new TextCell { Text = "C",  StyleId = "disclosure" },
                    }
                }
            };
        }

        /**
         * 根据不同的Y最大值，生成图表
         * 
         */
        private void buildDefaultPlotViewWithYValue(string title, out PlotModel plotModel)
        {
            plotModel = new PlotModel { Title = title };
            var xares = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "IRM(A/m)"
            };
            var yares = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Temperature(℃)"
            };
            plotModel.Axes.Add(xares);
            plotModel.Axes.Add(yares);
        }

        private void buildListView(out ListView listView)
        {
            listView = new ListView
            {
                ItemsSource = dataModels,
                RowHeight = 50, // 调整行高
                Header = new StackLayout // 表头
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 5,
                    Children =
                    {
                        new Label { Text = "SampleId" },
                        new Label { Text = "Temperature" },
                        new Label { Text = "XValue" },
                        new Label { Text = "yValue" },
                        new Label { Text = "zValue" },
                        new Label { Text = "xOrg" },
                        new Label { Text = "yOrg" },
                        new Label { Text = "zOrg" },
                        new Label { Text = "C" },
                        // 添加其他表头项...
                    }
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    var viewCell = new ViewCell();
                    var stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 5
                    };

                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("SampleID"), });
                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("Temperature"), });
                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("XValue"), });
                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("YValue"), });
                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("ZValue"), });
                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("XOrg"), });
                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("YOrg"), });
                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("ZOrg"), });
                    stackLayout.Children.Add(new Label { VerticalOptions = LayoutOptions.Center, BindingContext = new Binding("C"), });
                    // 添加其他数据项...

                    viewCell.View = stackLayout;
                    return viewCell;
                }),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
        }


        private int getTemperature(String input)
        {
            int index = input.Length - 1;
            while (index >= 0 && char.IsDigit(input[index]))
            {
                index--;
            }

            if (index == input.Length - 1)
            {
                return 0;
            }
            // 提取数字部分并转换为整数
            string numberPart = input.Substring(index + 1);
            int result;
            if (int.TryParse(numberPart, out result))
            {
                this.maxXValue = this.maxXValue > result ? this.maxXValue : result;
                return result;
            }
            else
            {
                // 如果提取失败，你可以选择抛出异常或者返回默认值
                throw new ArgumentException("字符串尾部没有有效的数字部分");
            }
        }

        private void getXYZValue(List<String> list, out double xValue, out double yValue, out double zValue)
        {
            double x, y, z;
            int s6;
            double.TryParse(list[2], out x);
            double.TryParse(list[3], out y);
            double.TryParse(list[4], out z);
            int.TryParse(list[5], out s6);
            var pow = Math.Pow(10, s6);
            xValue = Math.Round(Math.Abs(x * pow), 4);
            yValue = Math.Round(Math.Abs(y * pow), 4);
            zValue = Math.Round(Math.Abs(z * pow), 4);
            this.maxYValue = maxYValue > xValue ? maxYValue : xValue;
        }

    }
}

