using System.Linq;
using Victoria.Test.Runner;

namespace Victoria.Test.Tests.Unit.Runner {
    public class StaticAssemblyResolverTests {
        
        [Fact]
        public void should_return_name_of_assembly_of_provided_type() {
            
            var type = GetType();
            var resolver = new StaticAssemblyResolver(type);

            var assemblies = resolver.GetTestAssemblies();

            Assert.True(assemblies.Where(a => a == type.Assembly.FullName).Any()); 
        }
    }
}