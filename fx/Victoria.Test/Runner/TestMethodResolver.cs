using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Victoria.Test.Runner {
    public class TestMethodResolver {

        private readonly IAssemblyResolver _testAssemblyResolver;

        public TestMethodResolver(IAssemblyResolver testAssemblyResolver) {
            _testAssemblyResolver = testAssemblyResolver;
        }

        public IList<Assembly> TestAssemblies { get; private set; }

        public void LoadTestAssemblies() {
            TestAssemblies = new List<Assembly>();
            foreach (var assembly in _testAssemblyResolver.GetTestAssemblies()) {
                TestAssemblies.Add(Assembly.Load(assembly));
            }
        }

        public IEnumerable<MemberInfo> GetTestMethods(string testPath) {

            var methods = new List<MemberInfo>();

            if (testPath.IsMethod()) {
                methods.Add(GetTestMethod(testPath));
            } else if (testPath.IsRootNamespace()) {
                methods.AddRange(GetTestMethodsInRootNamespace(testPath));
            } else if (testPath.IsBlank()) {
                methods.AddRange(GetAllTestMethods());
            }

            return methods;
        }

        public virtual MemberInfo GetTestMethod(string testPath) {

            var typeSegments = testPath.Split('.');
            var methodName = typeSegments.Last();
            var declaringType = typeSegments[typeSegments.Count() - 2];
            var declaringAssembly = testPath.Substring(0, testPath.IndexOf("Unit") + 4);
            var testAssembly = TestAssemblies.Where(a => a.FullName.Contains(declaringAssembly)).Single();

            var testClass = testAssembly
                .GetExportedTypes()
                .Where(t => t.Name == declaringType)
                .Single();

            return GetTestMethodsInClass(testClass)
                .Where(s => s.Name == methodName)
                .Single();
        }

        public IEnumerable<MemberInfo> GetAllTestMethods() {

            var methods = new List<MemberInfo>();

            foreach (var assembly in TestAssemblies) {
                var testClasses = assembly.GetExportedTypes().Where(
                    t => t.Name.EndsWith("Tests") || t.Name.EndsWith("spec"));
                LoadTestMethodsFromClasses(testClasses, methods);
            }

            return methods;
        }

        public IEnumerable<MemberInfo> GetTestMethodsInRootNamespace(string testPath) {

            var testAssembly = TestAssemblies.Where(a => a.FullName.Contains(testPath)).Single();
            var testClasses = testAssembly.GetExportedTypes().Where(t => t.IsClass);

            var methods = new List<MemberInfo>();
            LoadTestMethodsFromClasses(testClasses, methods);
            return methods;
        }

        private static void LoadTestMethodsFromClasses(IEnumerable<Type> testClasses, List<MemberInfo> methods) {
            foreach (var testClass in testClasses) {
                methods.AddRange(GetTestMethodsInClass(testClass));
            }
        }

        private static IEnumerable<MemberInfo> GetTestMethodsInClass(Type testClass) {
            return testClass
                .FindMembers(MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, null, null)
                .Where(s => s.IsMarkedWith<FactAttribute>());
        }
    }
}