using CodeSheriff.SAST.Engine.DataCleaning;

namespace CodeSheriff.SAST.UnitTests
{
    [TestClass]
    public class StringRedactorTests
    {
        [TestMethod]
        public void TestNoArray()
        {
            var original = "var something = 1;";
            var redacted = StringRedactor.RedactByteArray(original);
            Assert.AreEqual(original, redacted);
        }

        [TestMethod]
        public void TestBasicArray()
        {
            var original = "{2, 12, 22}";
            var redacted = StringRedactor.RedactByteArray(original);
            Assert.AreEqual("{REDACTED}", redacted);
        }

        [TestMethod]
        public void TestWithArray()
        {
            var original = "var ENCRYPTION_KEY = new byte[] {9, 18, 27};";
            var redacted = StringRedactor.RedactByteArray(original);
            Assert.AreEqual("var ENCRYPTION_KEY = new byte[] {REDACTED};", redacted);
        }
    }
}