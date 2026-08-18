using System.Text;
using GrifLib;
using static GrifLib.Common;
using static GrifLib.Dags;

namespace Tests;

public class UnitTestDags
{
    private readonly Grod grod = new("base");
    private List<GrifMessage> result = [];

    private static string Squash(List<GrifMessage> result)
    {
        var sb = new StringBuilder();
        foreach (var item in result)
        {
            if (item.Type == MessageType.Text || item.Type == MessageType.Internal)
            {
                sb.Append(item.Value);
            }
        }
        return sb.ToString();
    }

    [SetUp]
    public void Setup()
    {
        grod.Clear(true);
        result.Clear();
    }

    [Test]
    public void Test_Passing()
    {
        Assert.Pass();
    }

    [Test]
    public void Test_Get()
    {
        var key = "abc";
        var value = "123";
        grod.Set(key, value);
        result = Process(grod, $"{GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Value, Is.EqualTo(value));
    }

    [Test]
    public void Test_Set_Script()
    {
        var key = "abc";
        var answer = $"{COMMENT_TOKEN}\"this is a comment\")";
        var value = "\"" + answer.Replace("\"", "\\\"") + "\"";
        Process(grod, $"{SET_TOKEN}{key},{value})");
        result = Process(grod, $"{GET_TOKEN}{key})");
        Assert.That(Squash(result), Is.EqualTo(answer));
    }

    [Test]
    public void Test_SetArray()
    {
        var key = "abc";
        var value = "123";
        Process(grod, $"{SETARRAY_TOKEN}{key},2,3,{value})");
        result = Process(grod, $"{GETARRAY_TOKEN}{key},2,3)");
        Assert.That(Squash(result), Is.EqualTo(value));
    }

    [Test]
    public void Test_SetArray_Null()
    {
        var key = "abc";
        var value = "";
        Process(grod, $"{SETARRAY_TOKEN}{key},2,3,{value})");
        result = Process(grod, $"{GETARRAY_TOKEN}{key},2,3)");
        Assert.That(Squash(result), Is.EqualTo(value));
    }

    [Test]
    public void Test_SetList_Null()
    {
        var key = "abc";
        var value = "";
        Process(grod, $"{SETLIST_TOKEN}{key},1,{value})");
        result = Process(grod, $"{GETLIST_TOKEN}{key},1)");
        Assert.That(Squash(result), Is.EqualTo(value));
    }

    [Test]
    public void Test_SetList_TabCRLF()
    {
        var key = "abc";
        var value = "abc\t\r\n123";
        Process(grod, $"{SETLIST_TOKEN}{key},1,\"{value}\")");
        result = Process(grod, $"{GETLIST_TOKEN}{key},1)");
        Assert.That(Squash(result), Is.EqualTo(value));
    }

    [Test]
    public void Test_InsertAtList()
    {
        var key = "abc";
        var value = "123";
        Process(grod, $"{ADDLIST_TOKEN}{key},0)");
        Process(grod, $"{ADDLIST_TOKEN}{key},1)");
        Process(grod, $"{ADDLIST_TOKEN}{key},2)");
        Process(grod, $"{ADDLIST_TOKEN}{key},3)");
        Process(grod, $"{INSERTATLIST_TOKEN}{key},1,{value})");
        result = Process(grod, $"{GETLIST_TOKEN}{key},1)");
        Assert.That(Squash(result), Is.EqualTo(value));
        result = Process(grod, $"{GETLIST_TOKEN}{key},4)");
        Assert.That(Squash(result), Is.EqualTo("3"));
    }

    [Test]
    public void Test_Function()
    {
        Process(grod, $"{SET_TOKEN}\"@boo\",\"{WRITE_TOKEN}eek!)\")");
        result = Process(grod, "@boo");
        Assert.That(Squash(result), Is.EqualTo("eek!"));
    }

    [Test]
    public void Test_FunctionParameters()
    {
        Process(grod, $"{SET_TOKEN}\"@boo(x)\",\"{WRITE_TOKEN}$x)\")");
        result = Process(grod, "@boo(eek!)");
        Assert.That(Squash(result), Is.EqualTo("eek!"));
    }

    [Test]
    public void Test_Rnd()
    {
        result = Process(grod, $"{SET_TOKEN}value,{RND_TOKEN}20))");
        result = Process(grod, $"{GET_TOKEN}value)");
        var r1 = long.Parse(Squash(result));
        Assert.That(r1 >= 0 && r1 < 20);
    }

    [Test]
    public void Test_PrettyScript()
    {
        var script = $"{IF_TOKEN} {EQ_TOKEN}{GET_TOKEN}value),0) {THEN_TOKEN} {WRITE_TOKEN}\"zero\") {ELSE_TOKEN} {WRITE_TOKEN}\"not zero\") {ENDIF_TOKEN}";
        var expected = $"{IF_TOKEN} {EQ_TOKEN}{GET_TOKEN}value),0) {THEN_TOKEN}\r\n\t{WRITE_TOKEN}\"zero\")\r\n{ELSE_TOKEN}\r\n\t{WRITE_TOKEN}\"not zero\")\r\n{ENDIF_TOKEN}";
        var actual = PrettyScript(script);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_PrettyScript_Min()
    {
        var script = $"{IF_TOKEN}{EQ_TOKEN}{GET_TOKEN}value),0){THEN_TOKEN}{WRITE_TOKEN}\"zero\"){ELSE_TOKEN}{WRITE_TOKEN}\"not zero\"){ENDIF_TOKEN}";
        var expected = $"{IF_TOKEN} {EQ_TOKEN}{GET_TOKEN}value),0) {THEN_TOKEN}\r\n\t{WRITE_TOKEN}\"zero\")\r\n{ELSE_TOKEN}\r\n\t{WRITE_TOKEN}\"not zero\")\r\n{ENDIF_TOKEN}";
        var actual = PrettyScript(script);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_PrettyScript_Same()
    {
        var script = $"{WRITE_TOKEN}\"hello \\\"wonderful\\\" world.\")";
        var actual = PrettyScript(script);
        Assert.That(actual, Is.EqualTo(script));
    }

    [Test]
    public void Test_IfThenNoStatements()
    {
        var script = $"{IF_TOKEN} {EQ_TOKEN}1,1) {THEN_TOKEN} {ENDIF_TOKEN}";
        result = Process(grod, script);
        Assert.That(Squash(result), Is.EqualTo(""));
    }

    [Test]
    public void Test_IfThenElseNoStatements()
    {
        var script = $"{IF_TOKEN} {EQ_TOKEN}1,2) {THEN_TOKEN} {WRITE_TOKEN}abc) {ELSE_TOKEN} {ENDIF_TOKEN}";
        result = Process(grod, script);
        Assert.That(Squash(result), Is.EqualTo(""));
    }

    [Test]
    public void Test_Write()
    {
        var value1 = "123";
        result = Process(grod, $"{WRITE_TOKEN}{value1})");
        Assert.That(Squash(result), Is.EqualTo(value1));
    }

    [Test]
    public void Test_WriteLine()
    {
        var value1 = "123";
        result = Process(grod, $"{WRITELINE_TOKEN}{value1})");
        // @writeline result ends with two characters, '\' and 'n'.
        // This is the expected behavior. See Test_NL().
        Assert.That(Squash(result), Is.EqualTo(value1 + NL_CHAR));
    }

    [Test]
    public void Test_ValidateScript()
    {
        var script1 = $"{SET_TOKEN}value,123) {WRITE_TOKEN}{GET_TOKEN}value))";
        bool isValid = ValidateScript(script1);
        Assert.That(isValid, Is.True);
        var script2 = $"{SET_TOKEN}value,123 {WRITE_TOKEN}{GET_TOKEN}value))";
        Assert.Throws<ArgumentException>(() => QuickValidate(script2));
        Assert.Throws<ArgumentException>(() => ValidateScript(script2));
    }
}
