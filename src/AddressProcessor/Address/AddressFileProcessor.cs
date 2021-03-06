﻿using System;
using AddressProcessing.Address.v1;
using AddressProcessing.CSV;

namespace AddressProcessing.Address
{
    public class AddressFileProcessor
    {
        private readonly IMailShot _mailShot;

        public AddressFileProcessor(IMailShot mailShot)
        {
            if (mailShot == null) throw new ArgumentNullException("mailShot");
            _mailShot = mailShot;
        }

        public void Process(string inputFile)
        {
            var reader = new CSVReaderWriter();
            string column1, column2;

            while(reader.Read(out column1, out column2,inputFile))
            {
                _mailShot.SendMailShot(column1, column2);
                return;
            }
            
        }
    }
}
