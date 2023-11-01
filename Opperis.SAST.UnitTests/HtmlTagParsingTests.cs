using Opperis.SAST.Engine.HtmlTagParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SAST.UnitTests
{
    [TestClass]
    public class HtmlTagParsingTests
    {
        [TestMethod]
        public void TestNumberOfScripts()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.Layout.txt")))
            {
                content = reader.ReadToEnd();
            }

            Assert.AreEqual(3, CSHtmlScriptTagParser.GetScriptTags(content).Count);
        }

        [TestMethod]
        public void TestScriptContents()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.Layout.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);
            Assert.AreEqual("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/jquery/1.6.1/jquery.min.js\">", scripts[0].Text);
            Assert.AreEqual("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.1.0/js/bootstrap.bundle.min.js\">", scripts[1].Text);
            Assert.AreEqual("<script src=\"~/js/site.js\" asp-append-version=\"true\">", scripts[2].Text);
        }

        [TestMethod]
        public void TestIsExternal()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.Layout.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);
            Assert.IsTrue(scripts[0].IsExternal);
            Assert.IsTrue(scripts[1].IsExternal);
            Assert.IsFalse(scripts[2].IsExternal);
        }

        [TestMethod]
        public void TestContainsBody()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.InternalExternal.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);

            Assert.AreEqual(2, scripts.Count);

            Assert.IsFalse(scripts[0].ContainsBody);
            Assert.IsTrue(scripts[1].ContainsBody);
        }

        [TestMethod]
        public void TestInternalExternal()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.InternalExternal.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);

            Assert.AreEqual(2, scripts.Count);

            Assert.IsFalse(scripts[0].IsExternal);
            Assert.IsFalse(scripts[1].IsExternal);
        }

        [TestMethod]
        public void TestInternalExternal_OpperisBlog()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.OpperisBlog.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);

            Assert.AreEqual(1, scripts.Count);

            Assert.IsFalse(scripts[0].IsExternal);
        }

        [TestMethod]
        public void TestHasIntegrity()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.ValidationPartial.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);

            Assert.AreEqual(4, scripts.Count);

            Assert.IsNull(scripts[0].Integrity);
            Assert.IsNull(scripts[1].Integrity);

            Assert.AreEqual("sha256-F6h55Qw6sweK+t7SiOJX+2bpSAa3b/fnlrVCJvmEj1A=", scripts[2].Integrity);
            Assert.AreEqual("sha256-9GycpJnliUjJDVDqP0UEu/bsm9U+3dnQUH8+3W10vkY=", scripts[3].Integrity);
        }

        [TestMethod]
        public void TestLineNumbers_Layout()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.Layout.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);

            Assert.AreEqual(59, scripts[0].LineNumberStart);
            Assert.AreEqual(60, scripts[1].LineNumberStart);
            Assert.AreEqual(61, scripts[2].LineNumberStart);
        }

        [TestMethod]
        public void TestLineNumbers_ValidationPartial()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.ValidationPartial.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);

            Assert.AreEqual(2, scripts[0].LineNumberStart);
            Assert.AreEqual(3, scripts[1].LineNumberStart);
            Assert.AreEqual(6, scripts[2].LineNumberStart);
            Assert.AreEqual(12, scripts[3].LineNumberStart);
        }

        [TestMethod]
        public void TestNonce()
        {
            string content = "";

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.HtmlTestFiles.InternalExternal.txt")))
            {
                content = reader.ReadToEnd();
            }

            var scripts = CSHtmlScriptTagParser.GetScriptTags(content);

            Assert.AreEqual(2, scripts.Count);

            Assert.IsNull(scripts[0].Nonce);
            Assert.AreEqual("011121314151", scripts[1].Nonce);
        }
    }
}
