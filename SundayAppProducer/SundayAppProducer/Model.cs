using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SundayAppProducer
{
    public class MunicipalityContext : DbContext
    {
        public DbSet<Municipality> Municipality { get; set; }
        public DbSet<Range> Ranges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MunicipalityTaxes.db");
        }
    }

    public class Municipality
    {
        public int MunicipalityId { get; set; }
        public string Name { get; set; }

        public ICollection<Range> Ranges { get; set; }
    }

    public class Range
    {
        public int RangeId { get; set; }
        public DateTime RangeStart { get; set; }
        public DateTime RangeEnd { get; set; }
        public string RangeName { get; set; }
        public double RangeTax { get; set; }
    }
}
