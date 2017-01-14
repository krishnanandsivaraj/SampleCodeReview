using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressProcessing;
using AddressProcessing.CSV;
using AddressProcessing.Address;
using AddressProcessing.Address.v1;

namespace AddressProcessingClient
{
    class Program
    {
        internal class FakeMailShotService : IMailShot
        {
            internal int Counter { get; private set; }

            public void SendMailShot(string name, string address)
            {
                Counter++;
            }
        }
        static void Main(string[] args)
        {
            string column1 = null, column2;
            using (var reader = new CSVReaderWriter())
            {

                //if (reader.Open(@"c:\test\someothertext2.csv", CSVReaderWriter.Mode.Create))
                //{
                    reader.Write(@"c:\test\someothertext2.csv", "column1", "column2");
                    reader.Read(out column1, out column2, @"c:\test\someothertext2.csv");
                //}
            };
        }
    }
}
