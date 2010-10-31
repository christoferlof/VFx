namespace Victoria.Test.Exceptions {
    public class EqualException : AssertException {

        public EqualException(object expected, object actual) 
            : base(string.Format("Assert.Equal failed. Expected: '{0}', Actual: '{1}'",expected,actual)) {
        }
    }
}