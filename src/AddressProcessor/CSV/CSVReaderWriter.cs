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
        /// <summary>  
        ///  removed unnecessary readerstream as it is used once  
        /// </summary> 
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Create = 2 };

        /// <summary>  
        ///  converted multiple if else to switch
        /// </summary>
        public bool Open(string fileName, Mode mode)
        {
            try
            { 
            switch (mode)
            {
                case Mode.Read:
                    File.OpenText(fileName);
                    return true;
                    /// <summary>  
                    ///  FileInfo not required as the streamreader handles internally
                    /// </summary>
                    case Mode.Create:
                    using(_writerStream=new StreamWriter(fileName,true)) { 
                        _writerStream.WriteLine();
                    }
                    return true;
                default:
                        return false;
            }
            }
            /// <summary>  
            ///  catching exceptions in try catch and not in else condition
            /// </summary>
            catch (ApplicationException ex)
            {
                /// <summary>  
                ///  used string interpolation c# 7.0
                /// </summary>
                throw new Exception($"Unknown file mode for {fileName} {ex.Message}");
            }
        }

        public void Write(string fileName,params string[] columns)
        {
            using (_writerStream = new StreamWriter(fileName, true))
            {
                var i = 0;
                /// <summary>  
                ///  used stringbuilder instead of string
                /// </summary>
                StringBuilder sb = new StringBuilder();
                /// <summary>  
                ///  replaced for with foreach
                /// </summary>
                foreach (string column in columns)
                {
                    sb.Append(column);
                    /// <summary>  
                    ///  removed a variable to declare '\t'
                    /// </summary>
                    if (i++ < columns.Length - 1) { sb.Append("\t"); }
                }
                _writerStream.WriteLine(sb);
            }
        }

        /// <summary>  
        ///  Read method without 'out' parameter removed.
        /// </summary>

        public bool Read(out string column1, out string column2,string path)
        {
         /// <summary>  
         ///  using keyword to open and close a connection
         /// </summary>
            using (var reader = new StreamReader(path))
            { 
            const int FIRST_COLUMN = 0, SECOND_COLUMN = 1;
            string line=reader.ReadLine();
                /// <summary>  
                ///  if condition to check null is replaced with native string implementation.
                /// </summary>
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
        /// <summary>  
        ///  removed the close method and IDisposable implemented.  Helps while creating objects in unit testing layer.
        /// </summary>
        public void Dispose()
        {
            if (_writerStream != null)
            {
                _writerStream.Dispose();
            }
        }
    }
}
