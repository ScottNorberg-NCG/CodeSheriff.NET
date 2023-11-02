using Opperis.SAST.Engine.CompiledCSHtmlParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.UnitTests
{
    [TestClass]
    public class SyntaxTreeTests
    {
        [TestMethod]
        public void TestPullControllerAndMethod_NoArea()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.Other.SyntaxTree.txt")))
            {
                content = reader.ReadToEnd();
            }

            var info = SyntaxTreeParser.Parse(content);
            Assert.IsNull(info.Area);
            Assert.AreEqual("AuthOnly", info.Controller);
            Assert.AreEqual("StoredXSS", info.Method);
        }
    }
}
