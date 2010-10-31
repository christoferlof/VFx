using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Victoria.Test.Runner {
    public class TestAssemblyResolver : IAssemblyResolver {
        
        private readonly string _manifest;
        private static readonly XNamespace Xns = "http://schemas.microsoft.com/winfx/2006/xaml";

        public TestAssemblyResolver(string manifest) {
            _manifest = manifest;
        }

        public virtual IEnumerable<string> GetTestAssemblies() {

            var manifest = XElement.Load(new StringReader(_manifest));
            var testAssemblies = from a in manifest.Elements().Elements()
                                 where a.Attribute(Xns + "Name").Value.Contains("Tests")
                                 select a.Attribute(Xns + "Name").Value;

            return testAssemblies.ToList();
        }
    }
}