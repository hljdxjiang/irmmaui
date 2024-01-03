using System;
using IRMMAUI.Entity;
using Maui.DataGrid;
using OxyPlot;

namespace IRMMAUI.Service
{
    public interface IOxyPlotService
    {
        void GetView(string title, List<string> list, out PlotModel plotModel, out TableView tableView);

        void GetListView(string title, List<string> list, out PlotModel plotModel, out List<TableItem> items);
    }
}

