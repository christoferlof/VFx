using System.Collections.Generic;
using System.Linq;
using Victoria.Test.Runner;

namespace Victoria.Test.Tests.Unit.Runner {
    public class TestAssemblyResolverTests {


        private const string Manifest = @"<Deployment xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" EntryPointAssembly=""Victoria.Test.Runner"" EntryPointType=""Victoria.Test.Runner.App"" RuntimeVersion=""4.0.50826.0"" xmlns=""http://schemas.microsoft.com/client/2007/deployment""><Deployment.Parts><AssemblyPart x:Name=""Caliburn.Micro"" Source=""Caliburn.Micro.dll"" /><AssemblyPart x:Name=""Driverslog"" Source=""Driverslog.dll"" /><AssemblyPart x:Name=""Driverslog.Tests.Unit"" Source=""Driverslog.Tests.Unit.dll"" /><AssemblyPart x:Name=""Victoria.Test"" Source=""Victoria.Test.dll"" /><AssemblyPart x:Name=""Victoria.Test.Runner"" Source=""Victoria.Test.Runner.dll"" /><AssemblyPart x:Name=""Victoria.Test.Tests.Unit"" Source=""Victoria.Test.Tests.Unit.dll"" /></Deployment.Parts></Deployment>";
        private IEnumerable<string> _assemblies;

        public TestAssemblyResolverTests() {
            var resolver = new TestAssemblyResolver(Manifest);
            _assemblies = resolver.GetTestAssemblies();
        }

        [Fact]
        public void should_find_driverslog_assembly() {
            Assert.True(_assemblies.Any(x => x.Contains("Driverslog.Tests.Unit")));
        }

        [Fact]
        public void should_find_fx_assembly() {
            Assert.True(_assemblies.Any(x => x.Contains("Victoria.Test.Tests.Unit")));
        }
    }
}