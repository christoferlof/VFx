namespace Victoria.Test.Tests.Unit {
    public class ContextSpecificationTests {
        
        [Fact]
        public void should_establish_context() {
            Assert.True(new Target().ContextInvoked);
        }

        [Fact]
        public void should_because() {
            Assert.True(new Target().BecauseInvoked);
        }

        public class Target : ContextSpecification {
            
            public bool ContextInvoked;
            public override void Context() {
                ContextInvoked = true;
            }

            public bool BecauseInvoked;
            public override void Because() {
                BecauseInvoked = true;
            }
        }
    }
}