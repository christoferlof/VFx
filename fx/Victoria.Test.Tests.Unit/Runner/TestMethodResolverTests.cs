using System.Linq;
using Victoria.Test.Runner;

namespace Victoria.Test.Tests.Unit.Runner {
    public class TestMethodResolverTests {

        [Fact]
        public void should_load_test_assemblies_provided_by_assembly_resolver() {
            var methodsResolver = new TestMethodResolver(new TestRunnerTests.FakeAssemblyResolver());
            methodsResolver.LoadTestAssemblies();
            Assert.Equal(1,methodsResolver.TestAssemblies.Count());
        }
    }
}