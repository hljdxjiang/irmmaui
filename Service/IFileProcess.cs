using System;
namespace IRMMAUI.Service
{
    public interface IFileProcess
    {
        void Process(String filePath, out Dictionary<string, List<string>> _dictionary);
    }
}

