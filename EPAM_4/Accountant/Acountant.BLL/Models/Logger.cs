using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Accountant.DAL.Interfaces;
using Accountant.DAL.Repositories;

namespace Acountant.BLL.Models
{
    public class Logger
    {
        FileSystemWatcher watcher;
        private IUnitOfWork _unit;
        object obj = new object();
        bool enabled = true;

        public Logger()
        {
            watcher = new FileSystemWatcher("C:\\Users\\Adushka\\EP_Labs\\EPAM_4\\Folder");
            watcher.Deleted += OnDeleted;
            watcher.Created += OnCreated;
            watcher.Changed += OnChanged;
            watcher.Renamed += OnRenamed;
            _unit = new UnitOfWork();
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "Renamed in " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Changed";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
            foreach (var element in Separator.DivideFile(filePath))
            {
                _unit.ReportRepository.Insert(element);
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "Created";
            string filePath = e.FullPath;
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
                    writer.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} файл {filePath} был {fileEvent}");
                    writer.Flush();
                }
            }
        }
    }
}
