using System;

namespace Victoria.Test.Exceptions {
    public class AssertException : Exception {
        
        public AssertException() {}

        protected AssertException(string userMessage)
            : base(userMessage) {
            UserMessage = userMessage;
        }

        protected AssertException(string userMessage, Exception innerException)
            : base(userMessage,innerException) { }

        public string UserMessage { get; set; }
    }
}