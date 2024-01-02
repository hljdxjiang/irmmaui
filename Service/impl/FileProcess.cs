using System;
using IRMMAUI.Common;
using OxyPlot;

namespace IRMMAUI.Service.impl
{
    public class FileProcess : IFileProcess
    {

        public FileProcess(IOxyPlotService oxyPlotService)
        {
        }

        /**
         * 
         * 读取文件，并逐行处理
         */
        public void Process(String filePath, out Dictionary<string, List<string>> _dictionary)
        {
            _dictionary = new Dictionary<string, List<string>>();
            if (!filePath.Equals(String.Empty))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        LineProcess(line, _dictionary);
                    }
                }
            }

        }

        /**
         * 
         * 读取每行数据并按样品分类
         */
        private void LineProcess(String s, Dictionary<string, List<string>> _dictionary)
        {
            List<string> list = null;

            var title = getSampleTitle(s);
            try
            {
                list = _dictionary.First(x => x.Key == title).Value;
            }
            catch (InvalidOperationException)
            {
                list = new List<string>();
            }
            list.Add(s);
            _dictionary[title] = list;
        }


        /**
         * 
         * 获取样品标题
         */
        private string getSampleTitle(String line)
        {
            return line.Split(Constant.LineSpit)[0];
        }
    }

}

