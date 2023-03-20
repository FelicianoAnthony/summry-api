namespace StarterApi.ApiModels.Middlewares
{
    public class ExceptionCodeDetails
    {
        public string FileName { get; set; }

        public int LineNumber { get; set; }

        public int ColumnNumber { get; set; }

        public string Method { get; set; }

        public string Class { get; set; }
    }
}
