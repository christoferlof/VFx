using System;
using System.Collections.Generic;

namespace Victoria.Test.Runner {
    public class StaticAssemblyResolver : IAssemblyResolver {
        private readonly Type _entryType;

        public StaticAssemblyResolver(Type entryType) {
            _entryType = entryType;
        }

        public IEnumerable<string> GetTestAssemblies() {
            return new []{_entryType.Assembly.FullName};
        }
    }
}