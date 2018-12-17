using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Acountant.BLL.Interfaces
{
   public interface IFolderWatcher
   {
       void Start();
       void Stop();
   }
}
