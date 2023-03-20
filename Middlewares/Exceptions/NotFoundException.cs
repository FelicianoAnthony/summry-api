using System.Runtime.Serialization;

namespace StarterApi.Middlewares.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception, ISerializable
    {

        public string DefaultExceptionMessage => "The ID at a specific API endpoint was not found";

        public override string Message { get; }


        public NotFoundException() : base() { Message = DefaultExceptionMessage; }

        public NotFoundException(string message) : base(message) { Message = message ?? DefaultExceptionMessage; }

        public NotFoundException(string message, Exception inner) : base(message, inner) { Message = message ?? DefaultExceptionMessage; }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
