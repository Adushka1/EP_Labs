using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Accountant.DAL.Entities;
using CsvHelper;

namespace Acountant.BLL.Models
{
    public class Separator
    {
        public static IEnumerable<Report> DivideFile(string fileAdress)
        {
            IEnumerable<Report> reports;
            using (var sr = new StreamReader(fileAdress))
            {
                var csv = new CsvReader(sr);
                reports = csv.GetRecords<Report>();
            }

            return reports;
        }
    }
}
