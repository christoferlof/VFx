using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

namespace Victoria.Test.Console {
    public class Packager {

        private const string XapPath = "Content\\ClientBin\\TestRunner.xap";
        private const string PackagePath = "Package\\";
        private static readonly XNamespace Tns = "http://schemas.microsoft.com/client/2007/deployment";
        private static readonly XNamespace Xns = "http://schemas.microsoft.com/winfx/2006/xaml";
        private static readonly XNamespace Vns = "urn:Victoria.Test";
        private static readonly XElement DeploymentParts = new XElement(Tns + "Deployment.Parts");

        public static void CreatePackage() {
            InitializeManifest();

            using (var package = Package.Open(GetPath(XapPath), FileMode.Create)) {
                foreach (var assembly in FindParts()) {
                    CreateAssemblyPart(package, assembly);
                    AddAssemblyToManifest(assembly);
                }
                CreateManifest(package);
            }
        }

        private static void InitializeManifest() {
            new XElement(Tns + "Deployment",
                new XAttribute(XNamespace.Xmlns + "x", Xns.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "v", Vns.NamespaceName),
                new XAttribute("EntryPointAssembly", "Victoria.Test.Runner"),
                new XAttribute("EntryPointType", "Victoria.Test.Runner.App"),
                new XAttribute("RuntimeVersion", "4.0.50826.0"),
                DeploymentParts
            );
        }

        private static void CreateManifest(Package package) {

            using (var writer = XmlWriter.Create(GetPath(PackagePath + "\\" + "AppManifest.xaml"),
                new XmlWriterSettings { OmitXmlDeclaration = true})) {
                DeploymentParts.Parent.Save(writer);
            }
            CreateManifestPart(package);
        }

        private static void AddAssemblyToManifest(string assembly) {

            var fullName = Assembly.ReflectionOnlyLoadFrom(assembly).FullName;

            var assemblyPartElement = new XElement(Tns + "AssemblyPart");
            var xname = new XAttribute(Xns + "Name", fullName);            
            var source = new XAttribute("Source", Path.GetFileName(assembly));
            assemblyPartElement.Add(xname);
            assemblyPartElement.Add(source);
            DeploymentParts.Add(assemblyPartElement);
        }

        private static IEnumerable<string> FindParts() {
            return Directory.EnumerateFiles(GetPath(PackagePath), "*.dll");
        }

        private static void CreateManifestPart(Package package) {
            CreatePart(package, "AppManifest.xaml", "text/xaml");
        }

        private static void CreateAssemblyPart(Package package, string assembly) {
            var assemblyName = Path.GetFileName(assembly);
            CreatePart(package, assemblyName, "binary/octet-stream");
        }

        private static void CreatePart(Package package, string file, string contentType) {
            var part = package.CreatePart(
                new Uri("/" + file, UriKind.Relative), contentType);

            using (var fileStream = new FileStream(
                GetPath(PackagePath + file), FileMode.Open, FileAccess.Read)) {
                CopyStream(fileStream, part.GetStream());
            }
        }

        private static void CopyStream(Stream source, Stream target) {
            const int bufSize = 0x1000;
            var buf = new byte[bufSize];
            var bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0) { 
                target.Write(buf, 0, bytesRead);
            }
        }

        private static string GetPath(string relativePath) {
            var root = Path.GetDirectoryName(typeof(Packager).Assembly.Location);
            return Path.Combine(root,relativePath);
        }
    }
}