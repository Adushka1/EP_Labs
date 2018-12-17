﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Acountant.BLL.Interfaces
{
    public interface IWriter
    {
        void WriteToDatabase(RenamedEventArgs e);
        void WriteHeader(FileSystemEventArgs e);

    }
}
