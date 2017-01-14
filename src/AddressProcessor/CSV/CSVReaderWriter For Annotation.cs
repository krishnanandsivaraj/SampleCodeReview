using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /* Review comments
        *) On opening/writing a stream from/to an external file, we have to use 'using' keyword. In this way, we can avoid closing our file readers through code.
        *) Use of 'switch' instead of multiple if else
        *) Exception should be logged with try catch.  Application exceptions can be created for this scenario.  In this scenario the exception is thrown but connection remains open. In finally close the connection or use try catch
        *) Use string interpolation (c# 7.0) "Unknown file mode for {0}",filename -> $"Unknown file mode for {filename}"
        *) In method Read, for can be converted to foreach. Jon Skeet in stackoverflow says foreach is now faster than for loops and takes less code. http://tinyurl.com/jp4tpje
        *) WriteLine and ReadLine are functions but referenced once.  Maybe okay for any future use cases.
        *) Read method have common code that can be a function called inside both Read methods. Violates DRY.
        *) The first Read method (without out parameter assigns values for first and second column). Column1 and Column2 assignments are not needed there. Voilates YAGNI.
        *) close method is not needed if we use using. We can save some lines of code.
        *) No unit tests? :)
        *) Too much vaiables declaration.  If we use the same value again and again, we can make a variable but not for one value.
    */

    public class CSVReaderWriterForAnnotation
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _writerStream = fileInfo.CreateText();
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        public bool Read(string column1, string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            } 
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
