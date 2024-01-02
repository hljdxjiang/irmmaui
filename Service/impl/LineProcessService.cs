using System;
using System.Text.RegularExpressions;
using IRMMAUI.Common;

namespace IRMMAUI.Service.impl
{
    public class LineProcessService : ILineProcessService
    {

        public LineProcessService()
        {
        }

        public List<String> ProcessLine(String line)
        {
            String[] ret = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); ;
            List<String> list = new List<string>(ret);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (Regex.IsMatch(list[i], Constant.Regex_) && Regex.IsMatch(list[i], Constant.RegexNum))
                    {
                        var str = Regex.Matches(list[i], Constant.RegexDouble);
                        if (str != null && str.Count > 0)
                        {
                            list[i] = str[0].Value;
                            if (str.Count > 1)
                            {
                                list.Insert(i + 1, str[1].Value);
                            }
                            if (str.Count > 2)
                            {
                                list.Insert(i + 2, str[2].Value);
                            }
                        }

                    }
                }

            }
            return list;
        }
    }
}

