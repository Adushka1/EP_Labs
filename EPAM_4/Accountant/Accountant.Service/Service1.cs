using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acountant.BLL.Models;

namespace Accountant.Service
{
    public partial class Service1 : ServiceBase
    {
        FolderWatcher _folderWatcher;

        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            _folderWatcher = new FolderWatcher(ConfigurationManager.AppSettings["Path"]);
            var loggerThread = new Thread(_folderWatcher.Start);
            loggerThread.Start();
        }

        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            OnStop();
        }

        protected override void OnStop()
        {
            _folderWatcher.Stop();
            Thread.Sleep(1000);
        }
    }
}
