using System;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace IRMMAUI.Service.impl
{
    public class OxyPlotService : IOxyPlotService
    {
        private ILineProcessService _lineProcessService;
        public OxyPlotService(ILineProcessService lineProcessService)
        {
            this._lineProcessService = lineProcessService;
        }

        public void GetView(string title, List<string> list, out PlotModel plotModel, out TableView tableView)
        {
            buildDefaultPlotView(title, out plotModel);
            buildDefaultTable(title, out tableView);
            var lineSeriesX = new LineSeries
            {
                Color = OxyColors.Green,
                LineJoin = OxyPlot.LineJoin.Bevel,
                Title = "X",
                MarkerType = MarkerType.Circle, // 设置标记类型
                MarkerSize = 2,                 // 设置标记大小
                MarkerFill = OxyColors.Green    // 设置标记填充颜色
            };
            var lineSeriesY = new LineSeries
            {
                Color = OxyColors.Red,
                LineJoin = OxyPlot.LineJoin.Bevel,
                Title = "Y",
                MarkerType = MarkerType.Square, // 设置标记类型
                MarkerSize = 2,                 // 设置标记大小
                MarkerFill = OxyColors.Red    // 设置标记填充颜色
            };
            var lineSeriesZ = new LineSeries
            {
                Color = OxyColors.Blue,
                LineJoin = OxyPlot.LineJoin.Bevel,
                Title = "Z",
                MarkerType = MarkerType.Triangle, // 设置标记类型
                MarkerSize = 2,                 // 设置标记大小
                MarkerFill = OxyColors.Blue    // 设置标记填充颜色
            };

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


                        var dataSection = new TableSection("Data");

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

        private void buildDefaultTable(string title, out TableView tableView)
        {
            tableView = new TableView
            {
                WidthRequest = 800,
                Intent = TableIntent.Form,
                Root = new TableRoot
                {
                    new TableSection("header"){
                        new TextCell { Text = "SampleId",  StyleId = "disclosure" },
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
            xValue = Math.Abs(x * pow);
            yValue = Math.Abs(y + pow);
            zValue = Math.Abs(z * pow);
        }
    }
}

