using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace SundayAppProducer
{
    class Program
    {
        private static MunicipalityContext db;
        static void createDB()
        {
            db = new MunicipalityContext();
            
            List<Range> VilniusRange = new List<Range>();

            VilniusRange.Add(new Range { RangeName = "yearly", RangeStart = DateTime.Parse("2016.01.01"), RangeEnd = DateTime.Parse("2016.12.31"), RangeTax = 0.2 });
            VilniusRange.Add(new Range { RangeName = "monthly", RangeStart = DateTime.Parse("2016.05.01"), RangeEnd = DateTime.Parse("2016.05.31"), RangeTax = 0.4 });
            VilniusRange.Add(new Range { RangeName = "daily", RangeStart = DateTime.Parse("2016.12.25"), RangeEnd = DateTime.Parse("2016.12.25"), RangeTax = 0.1 });
            VilniusRange.Add(new Range { RangeName = "daily", RangeStart = DateTime.Parse("2016.01.01"), RangeEnd = DateTime.Parse("2016.01.01"), RangeTax = 0.1 });

            db.Municipality.Add(new Municipality { Name = "Vilnius", Ranges = VilniusRange});

            var count = db.SaveChanges();
        }
        static void Main(string[] args)
        {
            createDB();

            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", "localhost:9092" }
            };

            Console.WriteLine("Please enter the date of Vilnius municipality or exit to quit");

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                string text = null;
                DateTime period = new DateTime();

                while (text != "exit")
                {
                    text = Console.ReadLine();
                    var result = 0.0;
                    if (DateTime.TryParse(text, out period))
                    {
                        foreach (var m in db.Municipality)
                        {
                            if (m.Ranges == null)
                                return; 

                            foreach (var range in m.Ranges)
                            {
                                if (DateTime.Compare(period, range.RangeStart) == 0)
                                {
                                    result = range.RangeTax;
                                }
                                else if (DateTime.Compare(period, range.RangeStart) > 0 && DateTime.Compare(period, range.RangeEnd) < 0)
                                {
                                    result = range.RangeTax;
                                }
                            }
                        }
                    }
                    
                    producer.ProduceAsync("municipality_taxes", null, result.ToString());
                }

                producer.Flush(100);
            }
        }
    }
}
