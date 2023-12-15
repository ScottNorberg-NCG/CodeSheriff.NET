using Opperis.SAST.Engine.Trufflehog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opperis.SCA.UnitTests;

[TestClass]
public class TestTruffleHogLoad
{
    [TestMethod]
    public void TestWithLineNumber()
    {
        string content = "";

        using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.JsonTestFiles.trufflehog_linenumber.json")))
        {
            content = reader.ReadToEnd();
        }

        var asObject = System.Text.Json.JsonSerializer.Deserialize<Result>(content);

        Assert.IsNotNull(asObject);
        Assert.AreEqual("SQLServer", asObject.DetectorName);
        Assert.AreEqual(20, asObject.SourceMetadata.Data.Filesystem.line);
    }

    [TestMethod]
    public void TestWithoutLineNumber()
    {
        string content = "";

        using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Opperis.SAST.UnitTests.JsonTestFiles.trufflehog_nolinenumber.json")))
        {
            content = reader.ReadToEnd();
        }

        var asObject = System.Text.Json.JsonSerializer.Deserialize<Result>(content);

        Assert.IsNotNull(asObject);
        Assert.AreEqual("PrivateKey", asObject.DetectorName);
        Assert.IsNull(asObject.SourceMetadata.Data.Filesystem.line);
    }
}
