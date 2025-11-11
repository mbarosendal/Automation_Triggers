using Automation_Experiments;
using FolderWatcher_Pdf;

class Program
{
    static async Task Main()
    {
        while (true)
        {
            Console.WriteLine("Select an option to trigger:");
            Console.WriteLine("1. Start Folder Watcher");
            Console.WriteLine("2. Start Copy Watcher");
            Console.WriteLine("3. Start Idle Watcher");
            Console.WriteLine("4. Exit");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    FolderWatcher.StartFolderWatcher();
                    Console.WriteLine("Folder Watcher started.");
                    break;

                case "2":
                    await CopyWatcher.StartCopyWatcher();
                    Console.WriteLine("Copy Watcher started.");
                    break;

                case "3":
                    await IdleWatcher.StartIdleWatcher();
                    Console.WriteLine("Idle Watcher started.");
                    break;

                case "4":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
