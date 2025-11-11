using System;
using System.Threading;
using System.Threading.Tasks;
using TextCopy;

namespace Automation_Experiments
{
    internal static class CopyWatcher
    {
        private static string lastClipboard = "";

        internal static async Task StartCopyWatcher()
        {
            Console.Clear();
            lastClipboard = await ClipboardService.GetTextAsync() ?? "";

            Console.WriteLine("Monitoring clipboard... (Press Enter to exit)\n");
            var cts = new CancellationTokenSource();
            var monitorTask = MonitorClipboard(cts.Token);

            Console.ReadLine();
            Console.Clear();
            cts.Cancel();

            try { await monitorTask; } catch { }
        }

        static async Task MonitorClipboard(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    string? current = await ClipboardService.GetTextAsync();
                    if (!string.IsNullOrWhiteSpace(current) && current != lastClipboard)
                    {
                        lastClipboard = current;
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [COPIED]:");
                        Console.WriteLine($"  Length: {current.Length} chars");
                        Console.WriteLine($"  Preview: {GetPreview(current)}");
                        Console.WriteLine();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[ERROR] {e.Message}\n");
                }
                await Task.Delay(200, ct);
            }
        }

        static string GetPreview(string text)
        {
            text = text.Replace("\r", "").Replace("\n", " ");
            return text.Length > 100 ? text.Substring(0, 100) + "..." : text;
        }
    }
}
