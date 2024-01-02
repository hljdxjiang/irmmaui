using System;
using System.Threading.Tasks;

namespace IRMMAUI.Platforms.Windows
{
    public class FolderPicker : IFolderPicker
    {
        public async Task<string> PickFolder()
        {
            try
            {
                FolderPicker folderPicker = new FolderPicker
                {
                    SuggestedStartLocation = PickerLocationId.Desktop
                };

                StorageFolder folder = await folderPicker.PickSingleFolderAsync();

                if (folder != null)
                {
                    // 处理选定的目录
                    return folder.Path;
                }
                else
                {
                    // 用户取消选择
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                return ex.Message;
            }
        }
    }
}

