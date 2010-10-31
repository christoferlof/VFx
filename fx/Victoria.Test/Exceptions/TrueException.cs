namespace Victoria.Test.Exceptions {
    public class TrueException : AssertException {
        public TrueException() : base("Assert.True failed") { }
    }
}