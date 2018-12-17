using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Accountant.DAL.Interfaces;
using Accountant.DAL.Repositories;
using Acountant.BLL.Interfaces;

namespace Acountant.BLL.Models
{
    public class FolderWatcher : IFolderWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private object obj = new object();
        private bool _enabled = true;
        private IUnitOfWork _unit;
        private IWriter _writer;
        public FolderWatcher(string monitoredFolder)
        {
            _watcher = new FileSystemWatcher(monitoredFolder) { Filter = "*_*.csv" };
            _watcher.Deleted += OnDeleted;
            _watcher.Created += OnCreated;
            _watcher.Changed += OnChanged;
            _watcher.Renamed += OnRenamed;
            _writer = new Writer();
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
            while (_enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _enabled = false;
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "Renamed in " + e.FullPath;
            string filePath = e.OldFullPath;

            if (e.Name.IndexOf(".tmp", StringComparison.Ordinal) == -1) return;
            Thread.Sleep(1000);
            Task.Factory.StartNew(() => _writer.WriteToDatabase(e));

            RecordEntry(fileEvent, filePath);

        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {

            string fileEvent = "Changed";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Created";
            string filePath = e.FullPath;

            _writer.WriteHeader(e);
            RecordEntry(fileEvent, filePath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Deleted";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void RecordEntry(string fileEvent, string filePath)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter("D:\\templog.txt", true))
                {
                    writer.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} file {filePath} was {fileEvent}");
                    writer.Flush();
                }
            }
        }
    }
}
