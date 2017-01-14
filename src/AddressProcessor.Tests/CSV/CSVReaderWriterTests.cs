using NUnit.Framework;
using AddressProcessing;
using AddressProcessing.CSV;
using System;
using System.IO;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private const string testCsvFileForWrite = @"c:\test\someothertext2.csv";
        private const string testCsvFileForRead = @"test_data\csvreader.csv";
        public ICSVReaderWriter iCsvReaderWriter = null;

        [SetUp]
        public void CSVReaderWriterTestsSetUp()
        {
            this.iCsvReaderWriter = new CSVReaderWriter();
            Directory.CreateDirectory(@"c:\test\");
        }
        [Test]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void Checks_Wrong_FileName_With_Read_Mode()
        {
           bool result= iCsvReaderWriter.Open(@"c:\test\somesampletext.csv",CSVReaderWriter.Mode.Read);
            Assert.IsFalse(result);
        }

        [Test]
        public void Creates_File_With_Write_Mode()
        {
            using (var reader = new CSVReaderWriter())
            {
                bool result = reader.Open(@"c:\test\someothertext.csv", CSVReaderWriter.Mode.Create);
                Assert.IsTrue(result);
            };
        }

        [Test]
        public void Reads_File_With_FilNames()
        {
            string column1=null, column2;
            using ( var reader=new CSVReaderWriter()) {
                bool status = reader.Open(@"test_data\csvreader.csv", CSVReaderWriter.Mode.Read);
                if (status) { reader.Read(out column1, out column2, testCsvFileForRead); }
                Assert.AreEqual("Shelby Macias", column1);
            } ;
        }

        [Test]
        public void Write_Files()
        {
            string column1 = null,column2;
            using (var reader = new CSVReaderWriter())
            {

                    reader.Write(@"c:\test\someothertext2.csv","column1", "column2");
                    reader.Read(out column1,out column2, testCsvFileForWrite);
                    Assert.AreEqual("column1", column1);
            };
        }

        [TearDown]
        public void CSVReaderWriterTestsTearDown()
        {
            string[] filePaths = Directory.GetFiles(@"c:\test\");
            foreach (string filePath in filePaths)
                File.Delete(filePath);
        }
    }
}
