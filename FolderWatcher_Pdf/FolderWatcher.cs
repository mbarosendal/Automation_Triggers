using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Experiments
{
    internal static class FolderWatcher
    {
        internal static void StartFolderWatcher()
        {
            Console.Clear();

            string path = @"C:\Users\Public\Temp";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var watcher = new FileSystemWatcher(path)
            {
                Filter = "*.pdf",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            watcher.Created += OnPdfAdded;
            watcher.Deleted += OnPdfRemoved;
            watcher.Renamed += OnPdfRenamed;

            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Monitoring for PDF-files: {path}");
            Console.WriteLine("Press Enter to exit...\n");
            Console.ReadLine();
            Console.Clear();
        }

        static void OnPdfAdded(object sender, FileSystemEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(500);

                using var pdfDoc = new PdfDocument(new PdfReader(e.FullPath));
                var info = pdfDoc.GetDocumentInfo();

                Console.WriteLine($"[ADDED] {e.Name}");
                Console.WriteLine($"  Pages: {pdfDoc.GetNumberOfPages()}");
                Console.WriteLine($"  Title: {info.GetTitle() ?? "N/A"}");
                Console.WriteLine($"  Author: {info.GetAuthor() ?? "N/A"}");
                Console.WriteLine($"  Size: {new FileInfo(e.FullPath).Length / 1024} KB");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {e.Name}: {ex.Message}\n");
            }
        }

        static void OnPdfRemoved(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"[REMOVED] {e.Name}\n");
        }

        static void OnPdfRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"[RENAMED] {e.OldName} → {e.Name}\n");
        }

    }
}
