using System;
using Victoria.Test.Exceptions;

namespace Victoria.Test.Tests.Unit {
    public class AssertTests {
        
        //test using exceptions - the other tests depend on assert.true so make sure it works without using the fx.. 
        [Fact]
        public void TrueThrows() {
            try {
                Assert.True(false);
            } catch(Exception ex) {
                var actual = (ex is TrueException);
                if(!actual)
                    throw new InvalidProgramException("Assert.True doesn't throw a TrueException when false is passed in as argument");
                return;
            }
            throw new InvalidProgramException("Assert.True doesn't throw any exception when false is passed in as argument");
        }

        [Fact]
        public void TrueDoesntThrow() {
            Assert.True(true);
        }

        [Fact]
        public void EqualThrowsOnIntegers() {
            try {
                Assert.Equal(1,2);
            } catch(Exception ex) {
                Assert.True(ex is EqualException);
                return;
            }
            Assert.True(false);
        }

        [Fact]
        public void EqualDoesntThrowOnIntegers() {
            Assert.Equal(1,1);            
        }

    }
}