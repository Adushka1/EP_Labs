using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acountant.BLL.Models;

namespace Accountant.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "Service1");
            if (ctl != null && ctl.Status != ServiceControllerStatus.Stopped)
            {
                System.Console.WriteLine("Service is still running");
                return;
            }

            var folderWatcher = new FolderWatcher(ConfigurationManager.AppSettings["Path"]);
            var loggerThread = new Thread(folderWatcher.Start);
            loggerThread.Start();
        }
    }
}
