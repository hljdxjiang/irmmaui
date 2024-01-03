using OxyPlot;
using OxyPlot.Maui.Skia;
using IRMMAUI.Service;
using SkiaSharp.Views.Maui.Controls;
using System.Threading;
using CommunityToolkit.Maui.Storage;
using Maui.DataGrid;
using IRMMAUI.ViewModels;
using IRMMAUI.Entity;

namespace IRMMAUI;

public partial class MainPage : ContentPage
{
    string filePath = "";

    Dictionary<string, List<string>> _dictionary;

    bool hasProcess = false;

    private IOxyPlotService _oxyPlotService;
    private IFileProcess _fileProcess;

    public MainPage()
    {
        InitializeComponent();
        this._oxyPlotService = IRMMAUI.MauiProgram.Services.GetRequiredService<IOxyPlotService>();
        this._fileProcess = IRMMAUI.MauiProgram.Services.GetRequiredService<IFileProcess>();
        BindingContext = new DataViewModel();
    }

    private async void OpenFilePickerAsync(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync();
        if (result != null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                filePath = result.FullPath;
                FileUrlPath.Text = filePath;
            });
            hasProcess = false;
        }
    }

    private async void StartProcessAsync(object sender, EventArgs e)
    {
        if (filePath.Equals(String.Empty))
        {
            await DisplayAlert("警告", "请先选择要处理的文件", "确定");
            return;
        }

        try
        {
            _fileProcess.Process(filePath, out _dictionary);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
        }

        try
        {
            RenderNext(0);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
        }



        hasProcess = true;
        await DisplayAlert("提示", "处理完成", "确定");
    }

    private async void download_click(string key, PlotModel plotModel)
    {
        var result = await FolderPicker.PickAsync(FileSystem.Current.AppDataDirectory, new CancellationToken());
        if (result != null)
        {
            var fileName = Path.GetFileName(filePath) + key + ".svg";
            var outPath = result.Folder.Path;

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            // 创建 SVG 渲染器
            var svgExporter = new SvgExporter { Width = 800, Height = 800 };

            // 将 PlotModel 导出为 SVG 字符串
            var svgString = svgExporter.ExportToString(plotModel);

            System.IO.File.WriteAllText(Path.Combine(outPath, fileName), svgString);

            await DisplayAlert("提示", "下载成功，文件路径：" + Path.Combine(FileSystem.Current.AppDataDirectory, Path.GetFileName(outPath)), "确定");
        }

    }

    private void RenderNext(int idx)
    {
        if (idx > _dictionary.Count || idx < 0 || _dictionary.Count == 0)
        {
            DisplayAlert("提示", "没有符合条件的数据生成", "确定");
            return;
        }
        PlotModel plotModel = null;
        //TableView tableView = null;
        //DataGrid dataGrid = null;
        List<TableItem> list;
        var item = _dictionary.ElementAt(idx);
        _oxyPlotService.GetListView(item.Key, item.Value, out plotModel, out list);
        plotView.Model = plotModel;

        //操作按钮组展示
        var nextButtons = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(5)
        };

        if (idx > 0)
        {
            //上一个
            var lastButton = new Button
            {
                Text = "上一个(" + _dictionary.ElementAt(idx - 1).Key + ")",
                Margin = new Thickness(5)
            };
            lastButton.Clicked += (sender, args) => RenderNext(idx - 1);
            nextButtons.Children.Add(lastButton);

        }

        //下载按钮
        Button downButtoon = new Button
        {
            Text = "下载" + item.Key,
            Margin = new Thickness(5)
        };
        //下载按钮绑定方法
        downButtoon.Clicked += (sender, args) => download_click(item.Key, plotModel);

        nextButtons.Add(downButtoon);

        if (idx < _dictionary.Count - 1)
        {
            //下一个
            var nextButton = new Button
            {
                Text = "下一个(" + _dictionary.ElementAt(idx + 1).Key + ")",
                Margin = new Thickness(5)
            };
            nextButton.Clicked += (sender, args) => RenderNext(idx + 1);
            nextButtons.Children.Add(nextButton);

        }

        //更新按钮展示前清空按钮
        btnAndLabel.Children.Clear();

        btnAndLabel.Children.Add(new Label
        {
            Text = (idx + 1).ToString() + "/" + _dictionary.Count,
            VerticalOptions = LayoutOptions.Fill,
            FontSize = 20,
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Colors.Blue
        });
        btnAndLabel.Children.Add(nextButtons);
        DataViewModel dataView = new DataViewModel();
        dataView.Items = list;
        uiGrid.BindingContext = dataView;
        uiGrid.BackgroundColor = Colors.White;
        uiGrid.Background = Colors.Gray;
        uiGrid.HeaderBackground = Colors.Gray;
        uiGrid.HeaderBordersVisible = true;
        uiGrid.HeaderHeight = 80;
        uiGrid.RowHeight = 60;
        uiGrid.BorderColor = Colors.Gray;
        uiGrid.ItemsSource = list;
    }
}


