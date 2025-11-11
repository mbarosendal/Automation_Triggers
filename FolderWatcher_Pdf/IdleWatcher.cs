using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FolderWatcher_Pdf
{
    internal class IdleWatcher
    {
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        private static bool isIdle = false;
        private static int idleThresholdSeconds = 5;


        public static async Task StartIdleWatcher()
        {
            Console.Clear();

            Console.WriteLine($"Monitoring user activity... (idle threshold: {idleThresholdSeconds}s)");

            var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            await MonitorIdleTime(cts.Token);
        }

        static async Task MonitorIdleTime(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                int idleSeconds = GetIdleTimeSeconds();
                bool nowIdle = idleSeconds >= idleThresholdSeconds;

                if (nowIdle && !isIdle)
                {
                    isIdle = true;
                    Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] USER IDLE - Starting background tasks...");
                    Console.WriteLine($"  Idle for: {idleSeconds} seconds");

                    _ = Task.Run(() => SimulateHeavyTask("Video encoding"));
                    _ = Task.Run(() => SimulateHeavyTask("Database backup"));
                    _ = Task.Run(() => SimulateHeavyTask("File synchronization"));
                }
                else if (!nowIdle && isIdle)
                {
                    isIdle = false;
                    Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] USER ACTIVE - Pausing background tasks...");
                }
                else if (!isIdle)
                {
                    Console.Write($"\r  Active - Idle in: {idleThresholdSeconds - idleSeconds}s  ");
                }

                await Task.Delay(1000, ct);
            }

            Console.WriteLine("\n\nMonitoring stopped.");
        }

        static int GetIdleTimeSeconds()
        {
            var lastInput = new LASTINPUTINFO();
            lastInput.cbSize = (uint)Marshal.SizeOf(lastInput);

            if (!GetLastInputInfo(ref lastInput))
                return 0;

            uint idleTime = (uint)Environment.TickCount - lastInput.dwTime;
            return (int)(idleTime / 1000);
        }

        static async Task SimulateHeavyTask(string taskName)
        {
            Console.WriteLine($"    ▶ {taskName} started");
            await Task.Delay(5000);
            Console.WriteLine($"    ✓ {taskName} completed");
        }
    }
}
