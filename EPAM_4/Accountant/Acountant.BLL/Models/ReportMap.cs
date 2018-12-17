using System;
using System.Collections.Generic;
using System.Text;
using Accountant.DAL.Entities;
using CsvHelper.Configuration;

namespace Acountant.BLL.Models
{
    public sealed class ReportMap : ClassMap<Report>
    {
        public ReportMap()
        {
            Map(m => m.Id).Ignore();
            Map(m => m.Manager).Ignore();
            Map(m => m.ManagerId).Ignore();
            Map(m => m.Cost);
            Map(m => m.Customer).Name("Customer Name");
            Map(m => m.Date);
            Map(m => m.Manager.Id).Ignore();
            Map(m => m.RowVersion).Ignore();
        }
    }
}
