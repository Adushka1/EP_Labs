using System;
using System.Collections.Generic;
using System.IO;
using Accountant.DAL.Entities;
using Acountant.BLL.Models;
using CsvHelper;
using Xunit;

namespace Accountant.Tests
{
    public class CsvHelperTests
    {
        [Fact]
        public void SimpleReadAndWriteTest()
        {
            var manager = new Manager
            {
                Id = 2,
                Name = "ilya",
            };

            var report = new Report
            {
                Cost = 12331,
                Date = DateTime.Now,
                Manager = manager,
                ManagerId = manager.Id
            };

            using (var sr = new StreamWriter("Test.csv"))
            using (var csvWriter = new CsvWriter(sr))
            {
                csvWriter.Configuration.RegisterClassMap(typeof(ReportMap));
                csvWriter.WriteHeader<Report>();
                for (var i = 0; i < 5; i++)
                {
                    csvWriter.NextRecord();
                    csvWriter.WriteRecord(new Report
                    {
                        Cost = i * 9,
                        Date = DateTime.Now,
                        Manager = manager,
                        ManagerId = manager.Id,
                        Customer = "Vasya"
                    });
                }
            }

            using (var sr = new StreamReader("Test.csv"))
            using (var csvReader = new CsvReader(sr))
            {
                csvReader.Configuration.RegisterClassMap(typeof(ReportMap));
                csvReader.Read();
                var f0 = csvReader.GetRecord<Report>();
                Assert.True(f0.Id == 0);
                Assert.True(f0.Date.ToString("d") == report.Date.ToString("d"));
            }
        }
    }
}
