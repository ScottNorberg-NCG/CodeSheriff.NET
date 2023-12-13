using Opperis.SCA.Engine;
using Opperis.SCA.Engine.Data;

namespace Opperis.SCA.UnitTests;

[TestClass]
public class TestVersionMatch
{
    [TestMethod]
    public void TestMajorVersionBetween()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1.0.0", true);
        cpe.SetMaxVersion("3.0.0", false);

        var toTest = new SoftwareVersion("2.1.19");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorMaxVersionEqual_NotInclude()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1.0.0", true);
        cpe.SetMaxVersion("3.0.0", false);

        var toTest = new SoftwareVersion("3.0.0");

        Assert.IsFalse(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorMaxVersionEqual_Include()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1.0.0", false);
        cpe.SetMaxVersion("3.0.0", true);

        var toTest = new SoftwareVersion("3.0.0");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorMinVersionEqual_Include()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1.0.0", true);
        cpe.SetMaxVersion("3.0.0", false);

        var toTest = new SoftwareVersion("1.0.0");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorMinVersionEqual_NotInclude()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1.0.0", false);
        cpe.SetMaxVersion("3.0.0", true);

        var toTest = new SoftwareVersion("1.0.0");

        Assert.IsFalse(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMinorVersionBetween()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("2.1.0", true);
        cpe.SetMaxVersion("2.5.0", false);

        var toTest = new SoftwareVersion("2.2.0");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMinorMaxVersionEqual_NotInclude()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("4.0.0", true);
        cpe.SetMaxVersion("4.2.0", false);

        var toTest = new SoftwareVersion("4.2.0");

        Assert.IsFalse(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMinorMaxVersionEqual_Include()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("0.8.0", false);
        cpe.SetMaxVersion("0.9.0", true);

        var toTest = new SoftwareVersion("0.9.0");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMinorMinVersionEqual_Include()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("92.6.0", true);
        cpe.SetMaxVersion("92.9.0", false);

        var toTest = new SoftwareVersion("92.6.0");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMinorMinVersionEqual_NotInclude()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("9.16.0", false);
        cpe.SetMaxVersion("9.20.0", true);

        var toTest = new SoftwareVersion("9.16.0");

        Assert.IsFalse(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestRevisionVersionBetween()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("2.0.2", true);
        cpe.SetMaxVersion("2.0.4", false);

        var toTest = new SoftwareVersion("2.0.3");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestRevisionMaxVersionEqual_NotInclude()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("4.2.55", true);
        cpe.SetMaxVersion("4.2.102", false);

        var toTest = new SoftwareVersion("4.2.102");

        Assert.IsFalse(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestRevisionMaxVersionEqual_Include()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("4.2.55", false);
        cpe.SetMaxVersion("4.2.102", true);

        var toTest = new SoftwareVersion("4.2.102");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestRevisionMinVersionEqual_Include()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("92.6.1", true);
        cpe.SetMaxVersion("92.6.6", false);

        var toTest = new SoftwareVersion("92.6.1");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestRevisionMinVersionEqual_NotInclude()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("9.20.0", false);
        cpe.SetMaxVersion("9.20.5", true);

        var toTest = new SoftwareVersion("9.20.0");

        Assert.IsFalse(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorVersionBetween_WithNulls()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1", true);
        cpe.SetMaxVersion("3", false);

        var toTest = new SoftwareVersion("2.1.19");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorMaxVersionEqual_NotInclude_WithNulls()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1.0.0", true);
        cpe.SetMaxVersion("3.0.0", false);

        var toTest = new SoftwareVersion("3");

        Assert.IsFalse(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorMaxVersionEqual_Include_WithNulls()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1.0.0", false);
        cpe.SetMaxVersion("3.0.0", true);

        var toTest = new SoftwareVersion("3");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorMinVersionEqual_Include_WithNulls()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1", true);
        cpe.SetMaxVersion("3", false);

        var toTest = new SoftwareVersion("1");

        Assert.IsTrue(cpe.IsMatch(toTest));
    }

    [TestMethod]
    public void TestMajorMinVersionEqual_NotInclude_WithNulls()
    {
        var cpe = new ParsedCpe();

        cpe.SetMinVersion("1", false);
        cpe.SetMaxVersion("3", true);

        var toTest = new SoftwareVersion("1.0.0");

        Assert.IsFalse(cpe.IsMatch(toTest));
    }
}