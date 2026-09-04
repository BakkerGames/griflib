using GrifLib;
using static GrifLib.Common;
using static GrifLib.Dags;

namespace Tests;

public class TestDags
{
    #region Setup ###DONE###

    private readonly Grod grod = new("test");
    private List<GrifMessage> result = [];

    [SetUp]
    public void Setup()
    {
        grod.Clear(true);
        result.Clear();
    }

    private static List<GrifMessage> ProcessTest(Grod grod, string script)
    {
        Assert.DoesNotThrow(() => QuickValidate(script));
        return Process(grod, script);
    }

    #endregion

    [Test]
    public void TestProcessTest()
    {
        Grod grod = new("testGrod");
        string script = $"{WRITE_TOKEN}abc)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "abc") }));
    }

    [Test]
    public void TestTwoCommands()
    {
        Grod grod = new("testGrod");
        string script = $"{WRITE_TOKEN}abc){WRITE_TOKEN}def)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "abc"), new(MessageType.Text, "def") }));
    }

    [Test]
    public void TestWriteError()
    {
        var value = "abc";
        // missing end paren
        var script = $"{WRITE_TOKEN}{value}";
        try
        {
            var result = ProcessTest(grod, script);
            Assert.Fail("FAIL - Did not throw an error");
        }
        catch (Exception)
        {
            Assert.Pass("PASS - Did throw an error");
        }
    }

    [Test]
    public void TestGetAndSet()
    {
        Grod grod = new("testGrod");
        grod.Set("key1", "value1");
        string script = $"{GET_TOKEN}key1)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Internal, "value1") }));
    }

    [Test]
    public void TestIfCondition()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"Condition met\") {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Condition met") }));
    }

    [Test]
    public void TestIfNotCondition()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {NOT_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"Condition met\") {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Condition met") }));
    }

    [Test]
    public void TestIfWithElseCondition()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"Condition met\") {ELSE_TOKEN} {WRITE_TOKEN}\"Condition not met\") {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Condition not met") }));
    }

    [Test]
    public void TestIfWithElseIfCondition()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"Condition met\") {ELSEIF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"Second condition met\") {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Second condition met") }));
    }

    [Test]
    public void TestIfNestedAnswer1()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {TRUE} {THEN_TOKEN} {IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}Answer1) {ELSE_TOKEN} {WRITE_TOKEN}Answer2) {ENDIF_TOKEN} {ELSEIF_TOKEN} {FALSE} {THEN_TOKEN} {IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}Answer3) {ELSE_TOKEN} {WRITE_TOKEN}Answer4) {ENDIF_TOKEN} {ELSE_TOKEN} {WRITE_TOKEN}Answer5) {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Answer1") }));
    }

    [Test]
    public void TestIfNestedAnswer2()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {TRUE} {THEN_TOKEN} {IF_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}Answer1) {ELSE_TOKEN} {WRITE_TOKEN}Answer2) {ENDIF_TOKEN} {ELSEIF_TOKEN} {FALSE} {THEN_TOKEN} {IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}Answer3) {ELSE_TOKEN} {WRITE_TOKEN}Answer4) {ENDIF_TOKEN} {ELSE_TOKEN} {WRITE_TOKEN}Answer5) {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Answer2") }));
    }

    [Test]
    public void TestIfNestedAnswer3()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {FALSE} {THEN_TOKEN} {IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}Answer1) {ELSE_TOKEN} {WRITE_TOKEN}Answer2) {ENDIF_TOKEN} {ELSEIF_TOKEN} {TRUE} {THEN_TOKEN} {IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}Answer3) {ELSE_TOKEN} {WRITE_TOKEN}Answer4) {ENDIF_TOKEN} {ELSE_TOKEN} {WRITE_TOKEN}Answer5) {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Answer3") }));
    }

    [Test]
    public void TestIfNestedAnswer4()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {FALSE} {THEN_TOKEN} {IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}Answer1) {ELSE_TOKEN} {WRITE_TOKEN}Answer2) {ENDIF_TOKEN} {ELSEIF_TOKEN} {TRUE} {THEN_TOKEN} {IF_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}Answer3) {ELSE_TOKEN} {WRITE_TOKEN}Answer4) {ENDIF_TOKEN} {ELSE_TOKEN} {WRITE_TOKEN}Answer5) {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Answer4") }));
    }

    [Test]
    public void TestIfNestedAnswer5()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {FALSE} {THEN_TOKEN} {IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}Answer1) {ELSE_TOKEN} {WRITE_TOKEN}Answer2) {ENDIF_TOKEN} {ELSEIF_TOKEN} {FALSE} {THEN_TOKEN} {IF_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}Answer3) {ELSE_TOKEN} {WRITE_TOKEN}Answer4) {ENDIF_TOKEN} {ELSE_TOKEN} {WRITE_TOKEN}Answer5) {ENDIF_TOKEN}";
        grod.Set("key1", "value1");
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Answer5") }));
    }

    [Test]
    public void TestIfNE()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {NE_TOKEN}1,2) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfNENull()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {NE_TOKEN}null,2) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfNEString()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {NE_TOKEN}abc,xyz) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfGT()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {GT_TOKEN}2,1) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfGTNull()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {GT_TOKEN}2,null) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfGTString()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {GT_TOKEN}xyz,abc) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfGE()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {GE_TOKEN}1,1) {AND_TOKEN} {GE_TOKEN}2,1) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfGENull()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {GE_TOKEN}null,null) {AND_TOKEN} {GE_TOKEN}2,null) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfGEString()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {GE_TOKEN}abc,abc) {AND_TOKEN} {GE_TOKEN}xyz,abc) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfLT()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {LT_TOKEN}1,2) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfLTNull()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {LT_TOKEN}null,2) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfLTString()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {LT_TOKEN}abc,xyz) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfLE()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {LE_TOKEN}1,1) {AND_TOKEN} {LE_TOKEN}1,2) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfLENull()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {LE_TOKEN}null,null) {AND_TOKEN} {LE_TOKEN}null,2) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestIfLEString()
    {
        Grod grod = new("testGrod");
        string script = $"{IF_TOKEN} {LE_TOKEN}abc,abc) {AND_TOKEN} {LE_TOKEN}abc,xyz) {THEN_TOKEN} {WRITE_TOKEN}answer) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "answer") }));
    }

    [Test]
    public void TestParameterWithFunction()
    {
        Grod grod = new("testGrod");
        grod.Set("key1", "value1");
        string script = $"{WRITE_TOKEN}{GET_TOKEN}key1))";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "value1") }));
    }

    [Test]
    public void TestParameterWithNestedFunction()
    {
        Grod grod = new("testGrod");
        grod.Set("key1", "value1");
        grod.Set("key2", "key1");
        string script = $"{WRITE_TOKEN}{GET_TOKEN}{GET_TOKEN}key2)))";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "value1") }));
    }

    [Test]
    public void TestUnknownToken()
    {
        Grod grod = new("testGrod");
        string script = "@unknown";
        var result = ProcessTest(grod, script);
        var expected = "@unknown: User-defined function is null";
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
            Assert.That(result[0].Value, Does.Contain(expected));
        }
    }

    [Test]
    public void Test_UserDefinedScript_NoParams()
    {
        var key = "@myScript";
        var value = "Hello from user-defined script!";
        var script = $"{WRITE_TOKEN}\"{value}\")";
        var expectedValue = "Hello from user-defined script!";
        grod.Set(key, script);
        var result = ProcessTest(grod, key);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_UserDefinedScript_Params()
    {
        var key = "@myScript(x,y,z)";
        var value = $"{WRITE_TOKEN}$x,$y,$z)";
        grod.Set(key, value);
        var xValue = "100";
        var yValue = "abc";
        var zValue = "mc2";
        var script = $"@myScript({xValue},{yValue},{zValue})";
        var result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(xValue));
        Assert.That(result[1].Value, Is.EqualTo(yValue));
        Assert.That(result[2].Value, Is.EqualTo(zValue));
    }

    [Test]
    public void Test_UserDefinedScript_OptionalParams()
    {
        var key = "@myScript(x,y,z)";
        var value = $"{WRITE_TOKEN}$x,$y,$z)";
        grod.Set(key, value);
        var xValue = "100";
        var yValue = "abc";
        var script = $"@myScript({xValue},{yValue})";
        var result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(xValue));
        Assert.That(result[1].Value, Is.EqualTo(yValue));
        Assert.That(result[2].Value, Is.Empty);
    }

    [Test]
    public void Test_UserDefinedScript_ModifiedParams()
    {
        var key = "@myScript(x,y,z)";
        var repValue = "555";
        var value = $"{SET_TOKEN}_y,{repValue}) {WRITE_TOKEN}$x,$y,$z)";
        grod.Set(key, value);
        var xValue = "100";
        var yValue = "abc";
        var zValue = "mc2";
        var script = $"@myScript({xValue},{yValue},{zValue})";
        var result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(xValue));
        Assert.That(result[1].Value, Is.EqualTo(repValue));
        Assert.That(result[2].Value, Is.EqualTo(zValue));
    }

    [Test]
    public void TestDivByZero()
    {
        Grod grod = new("testGrod");
        string script = $"{DIV_TOKEN}6,0)";
        var result = ProcessTest(grod, script);
        var expected = "Division by zero is not allowed.";
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
            Assert.That(result[0].Value, Does.Contain(expected));
        }
    }

    [Test]
    public void TestInvalidCommand()
    {
        Grod grod = new("testGrod");
        string script = "@invalidcommand()";
        var result = ProcessTest(grod, script);
        var expected = "@invalidcommand(): User-defined function not found";
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
            Assert.That(result[0].Value, Does.Contain(expected));
        }
    }

    [Test]
    public void TestEmptyScript()
    {
        Grod grod = new("testGrod");
        string script = "";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage>()));
    }

    [Test]
    public void TestWhitespaceScript()
    {
        Grod grod = new("testGrod");
        string script = "   ";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage>() { new(MessageType.Text, script) }));
    }

    [Test]
    public void TestScriptWithMixedContent()
    {
        Grod grod = new("testGrod");
        string script = $"{COMMENT_TOKEN}\"This is a comment\n\") {WRITE_TOKEN}Hello) {COMMENT_TOKEN}\"Another comment\") {WRITE_TOKEN}World)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Hello"), new(MessageType.Text, "World") }));
    }

    [Test]
    public void TestGetValue()
    {
        Grod grod = new("testGrod");
        grod.Set("key1", "value1");
        grod.Set("key2", $"{GET_TOKEN}key1)");
        string script = $"{GETVALUE_TOKEN}key2)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Internal, "value1") }));
    }

    [Test]
    public void TestGetValueNonExistentKey()
    {
        Grod grod = new("testGrod");
        string script = $"{GETVALUE_TOKEN}nonexistent)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Internal, "") }));
    }

    [Test]
    public void TestGetValueWithNestedFunction()
    {
        Grod grod = new("testGrod");
        grod.Set("key1", "value1");
        grod.Set("key2", $"{GET_TOKEN}key1)");
        grod.Set("key3", $"{GET_TOKEN}key2)");
        string script = $"{GETVALUE_TOKEN}key3)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Internal, "value1") }));
    }

    [Test]
    public void TestComplexScript()
    {
        Grod grod = new("testGrod");
        grod.Set("a", "10");
        grod.Set("b", "20");
        string script = $"{IF_TOKEN} {GT_TOKEN}{GET_TOKEN}a),5) {AND_TOKEN} {LT_TOKEN}{GET_TOKEN}b),30) {THEN_TOKEN} {WRITE_TOKEN}{ADD_TOKEN}{GET_TOKEN}a),{GET_TOKEN}b))) {ELSE_TOKEN} {WRITE_TOKEN}Out of range) {ENDIF_TOKEN}";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "30") }));
    }

    [Test]
    public void TestCommentCommand()
    {
        Grod grod = new("testGrod");
        string script = $"{COMMENT_TOKEN}\"This is a comment\"){WRITE_TOKEN}Hello)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Hello") }));
    }

    [Test]
    public void TestCommentCommandWithNewline()
    {
        Grod grod = new("testGrod");
        string script = $"{COMMENT_TOKEN}\"This is a comment\nwith a newline\"){WRITE_TOKEN}Hello)";
        var result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(new List<GrifMessage> { new(MessageType.Text, "Hello") }));
    }

    [Test]
    public void TestHelp()
    {
        var grod = Dags.Help();
        Assert.That(grod, Is.Not.Null);
        Assert.That(grod.Items(true, true), Is.Not.Empty);
    }

    [Test]
    public void TestHelpSearchTerm()
    {
        var grod = Dags.Help("value");
        Assert.That(grod, Is.Not.Null);
        Assert.That(grod.Items(true, true), Is.Not.Empty);
    }

    [Test]
    public void TestHelpSearchNotFound()
    {
        var grod = Dags.Help("ZZZZZZZZ");
        Assert.That(grod?.Count(true) ?? 0, Is.Zero);
    }
}
