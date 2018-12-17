using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using Accountant.DAL.Entities;
using Accountant.DAL.Interfaces;
using Accountant.DAL.Repositories;
using CsvHelper;
using IWriter = Acountant.BLL.Interfaces.IWriter;

namespace Acountant.BLL.Models
{
    public class Writer : IWriter
    {
        public void WriteToDatabase(RenamedEventArgs e)
        {
            using (var unit = new UnitOfWork("DBConnection"))
            {
                try
                {
                    unit.ReportRepository.Insert(GetReports(e));
                    unit.Save();
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
                {
                    Console.WriteLine(dbUpdateConcurrencyException);
                    throw;
                }
            }
        }

        public void WriteHeader(FileSystemEventArgs e)
        {
            using (var sr = new StreamWriter(e.FullPath))
            using (var csvWriter = new CsvWriter(sr))
            {
                csvWriter.Configuration.RegisterClassMap(typeof(ReportMap));
                csvWriter.WriteHeader<Report>();
            }
        }

        private Manager GetManager(string fileName)
        {
            return new Manager
            {
                Name = fileName.Split('_')[0]
            };
        }

        private IEnumerable<Report> GetReports(RenamedEventArgs e)
        {
            var manager = GetManager(e.OldName);
            using (var sr = new StreamReader(e.OldFullPath))
            using (var csvReader = new CsvReader(sr))
            {
                csvReader.Configuration.RegisterClassMap(typeof(ReportMap));
                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<Report>();
                    System.Console.WriteLine(record.Customer);
                    record.Manager = manager;
                    record.ManagerId = manager.Id;
                    yield return record;
                }
            }
        }
    }
}