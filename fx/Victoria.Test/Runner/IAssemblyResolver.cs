using System.Collections.Generic;

namespace Victoria.Test.Runner {
    public interface IAssemblyResolver {
        IEnumerable<string> GetTestAssemblies();
    }
}