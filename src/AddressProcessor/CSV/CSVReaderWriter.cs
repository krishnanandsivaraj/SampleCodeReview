using System;
using System.IO;
using System.Text;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter : ICSVReaderWriter, IDisposable
    {
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Create = 2 };

        public bool Open(string fileName, Mode mode)
        {
            try
            { 
            switch (mode)
            {
                case Mode.Read:
                    File.OpenText(fileName);
                    return true;
                case Mode.Create:
                    using(_writerStream=new StreamWriter(fileName,true)) { 
                        _writerStream.WriteLine();
                    }
                    return true;
                default:
                        return false;
            }
            }
            catch(ApplicationException ex)
            {
                throw new Exception($"Unknown file mode for {fileName} {ex.Message}");
            }
        }

        public void Write(string fileName,params string[] columns)
        {
            using (_writerStream = new StreamWriter(fileName, true))
            {
                var i = 0;
                StringBuilder sb = new StringBuilder();
                foreach (string column in columns)
                {
                    sb.Append(column);
                    if (i++ < columns.Length - 1) { sb.Append("\t"); }
                }
                _writerStream.WriteLine(sb);
            }
        }

        public bool Read(out string column1, out string column2,string path)
        {
            using (var reader = new StreamReader(path))
            { 
            const int FIRST_COLUMN = 0, SECOND_COLUMN = 1;
            string line=reader.ReadLine();
            if (string.IsNullOrEmpty(line)||string.IsNullOrWhiteSpace(line))
            { column1 = column2 = null; return false; }
            else
            {
                string[] columns = line.Split('\t');
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];
                return true;
            }
            };
        }

        public void Dispose()
        {
            if (_writerStream != null)
            {
                _writerStream.Dispose();
            }
        }
    }
}
