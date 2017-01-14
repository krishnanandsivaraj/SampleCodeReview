namespace AddressProcessing.CSV
{
    public interface ICSVReaderWriter
    {
        bool Open(string fileName, CSVReaderWriter.Mode mode);
        bool Read(out string column1, out string column2,string path);
        void Write(string fileName, params string[] columns);
    }
}