using CodeSheriff.SCA.Engine;
using CodeSheriff.SCA.Engine.Data;

namespace CodeSheriff.SCA.UnitTests;

[TestClass]
public class TestVersionComparison
{
    [TestMethod]
    public void TestMajor_NoMinor()
    {
        var ver1 = new SoftwareVersion("1.0.0");
        var ver2 = new SoftwareVersion("3.0.0");

        Assert.IsTrue(ver1 < ver2);
        Assert.IsTrue(ver2 > ver1);
        Assert.IsFalse(ver1 > ver2);
        Assert.IsFalse(ver2 < ver1);
    }

    [TestMethod]
    public void TestMajor_WithMinor()
    {
        var ver1 = new SoftwareVersion("1.9.0");
        var ver2 = new SoftwareVersion("3.7.0");

        Assert.IsTrue(ver1 < ver2);
        Assert.IsTrue(ver2 > ver1);
        Assert.IsFalse(ver1 > ver2);
        Assert.IsFalse(ver2 < ver1);
    }

    [TestMethod]
    public void TestMinor_NoRevision()
    {
        var ver1 = new SoftwareVersion("6.2.0");
        var ver2 = new SoftwareVersion("6.12.0");

        Assert.IsTrue(ver1 < ver2);
        Assert.IsTrue(ver2 > ver1);
        Assert.IsFalse(ver1 > ver2);
        Assert.IsFalse(ver2 < ver1);
    }

    [TestMethod]
    public void TestMinor_WithRevision()
    {
        var ver1 = new SoftwareVersion("6.2.5");
        var ver2 = new SoftwareVersion("6.12.3");

        Assert.IsTrue(ver1 < ver2);
        Assert.IsTrue(ver2 > ver1);
        Assert.IsFalse(ver1 > ver2);
        Assert.IsFalse(ver2 < ver1);
    }

    [TestMethod]
    public void TestRevision()
    {
        var ver1 = new SoftwareVersion("6.12.5");
        var ver2 = new SoftwareVersion("6.12.3");

        Assert.IsTrue(ver1 > ver2);
        Assert.IsTrue(ver2 < ver1);
        Assert.IsFalse(ver1 < ver2);
        Assert.IsFalse(ver2 > ver1);
    }

    [TestMethod]
    public void TestNullEqual()
    {
        var ver1 = new SoftwareVersion("6");
        var ver2 = new SoftwareVersion("6.0.0");

        Assert.IsFalse(ver1 > ver2);
        Assert.IsFalse(ver2 < ver1);
        Assert.IsFalse(ver1 < ver2);
        Assert.IsFalse(ver2 > ver1);
    }

    [TestMethod]
    public void TestNullNotEqual()
    {
        var ver1 = new SoftwareVersion("6.1");
        var ver2 = new SoftwareVersion("6.1.1");

        Assert.IsFalse(ver1 > ver2);
        Assert.IsTrue(ver1 < ver2);
        Assert.IsFalse(ver2 < ver1);
        Assert.IsTrue(ver2 > ver1);
    }

    [TestMethod]
    public void TestMajorVersionMax()
    {
        var list = new List<SoftwareVersion>();

        list.Add(new SoftwareVersion("1.0.0"));
        list.Add(new SoftwareVersion("2.0.0"));
        list.Add(new SoftwareVersion("3.0.0"));

        var max = list.Max();

        Assert.AreEqual(3, max.Major);
    }

    [TestMethod]
    public void TestMinorVersionMax()
    {
        var list = new List<SoftwareVersion>();

        list.Add(new SoftwareVersion("5.2.0"));
        list.Add(new SoftwareVersion("5.6.0"));
        list.Add(new SoftwareVersion("5.11.0"));

        var max = list.Max();

        Assert.AreEqual(5, max.Major);
        Assert.AreEqual(11, max.Minor);
    }

    [TestMethod]
    public void TestMixedVersionMax()
    {
        var list = new List<SoftwareVersion>();

        list.Add(new SoftwareVersion("3.11.0"));
        list.Add(new SoftwareVersion("4.1.0"));
        list.Add(new SoftwareVersion("4.2.0"));

        var max = list.Max();

        Assert.AreEqual(4, max.Major);
        Assert.AreEqual(2, max.Minor);
    }
}