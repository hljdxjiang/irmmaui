using System;
using OxyPlot;

namespace IRMMAUI.Service
{
    public interface IOxyPlotService
    {
        void GetView(string title, List<string> list, out PlotModel plotModel, out TableView tableView);
    }
}

