using System;
using Victoria.Test.Exceptions;

namespace Victoria.Test {
    public class Assert {
        
        public static void Equal<T>(T expected, T actual) {
            if (!actual.Equals(expected)) throw new EqualException(expected,actual);
        }
        
        public static void True(bool condition) {
            if(!condition) throw new TrueException();
        }
    }
}