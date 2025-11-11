using Automation_Experiments;
using FolderWatcher_Pdf;

class Program
{
    static async Task Main()
    {
        FolderWatcher.StartFolderWatcher();

        await CopyWatcher.StartCopyWatcher();

        await IdleWatcher.StartIdleWatcher();
    }

}