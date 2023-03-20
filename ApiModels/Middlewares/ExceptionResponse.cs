namespace StarterApi.ApiModels.Middlewares
{
    public class ExceptionResponse
    {
        public ExceptionResponse()
        {
            ExceptionHandled = false;
        }

        public string ExceptionType { get; set; }

        public string Error { get; set; }

        public List<string> InnerException { get; set; }

        public bool ExceptionHandled { get; set; }

        public string Class { get; set; }

        public string Method { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
