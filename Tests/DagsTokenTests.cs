using GrifLib;
using static GrifLib.Common;
using static GrifLib.Dags;

/*
    [Test]
    public void Test_()
    {
        var key = "key";
        var value = "1";
        var expectedValue = "1";
        result = ProcessTest(grod, $"... {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"... {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }
*/

namespace Tests;

public class DagsTokenTests
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

    private static List<GrifMessage> ProcessTest(Grod grod, string? script)
    {
        Assert.DoesNotThrow(() => QuickValidate(script));
        return Process(grod, script);
    }

    #endregion

    #region @abs ###DONE###

    [Test]
    public void Test_ABS_Positive()
    {
        var value = "23";
        var expectedValue = "23";
        result = ProcessTest(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Negative()
    {
        var value = "-23";
        var expectedValue = "23";
        result = ProcessTest(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Zero()
    {
        var value = "0";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_NoParams()
    {
        result = ProcessTest(grod, $"{ABS_TOKEN})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ABS_Empty()
    {
        var value = "\"\"";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Null()
    {
        string? value = null;
        result = ProcessTest(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ABS_Invalid()
    {
        var value = "abc";
        result = ProcessTest(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @addlist ###DONE###

    [Test]
    public void Test_ADDLIST_OneItem()
    {
        var key = "key";
        var value = "1";
        var expectedValue = "1";
        result = ProcessTest(grod, $"{ADDLIST_TOKEN}{key},{value}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_TwoItems()
    {
        var key = "key";
        var value1 = "1";
        var expectedValue1 = "1";
        result = ProcessTest(grod, $"{ADDLIST_TOKEN}{key},{value1}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        var value2 = "2";
        var expectedValue2 = $"{value1},{value2}";
        result = ProcessTest(grod, $"{ADDLIST_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    [Test]
    public void Test_ADDLIST_Empty()
    {
        var key = "key";
        var value = "";
        var expectedValue = $"{NULL}";
        result = ProcessTest(grod, $"{ADDLIST_TOKEN}{key},{value}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_ValueEmptyValue()
    {
        var key = "key";
        var value1 = "value1";
        var value2 = "";
        var value3 = "value3";
        var expectedValue = $"{value1},{NULL},{value3}";
        result = ProcessTest(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_ValueNullValue()
    {
        var key = "key";
        var value1 = "value1";
        string? value2 = null;
        var value3 = "value3";
        var expectedValue = $"{value1},{NULL},{value3}";
        result = ProcessTest(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_NullNullNull()
    {
        var key = "key";
        string? value1 = null;
        string? value2 = null;
        string? value3 = null;
        var expectedValue = $"{NULL},{NULL},{NULL}";
        result = ProcessTest(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @addto ###DONE###

    [Test]
    public void Test_ADDTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "9";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ADDTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ADDTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @add ###DONE###

    [Test]
    public void Test_ADD_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "9";
        result = ProcessTest(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ADD_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{ADD_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ADD_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @and ###DONE###

    [Test]
    public void Test_AND_TrueAndTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {TRUE} {AND_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AND_TrueAndFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {TRUE} {AND_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AND_FalseAndTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {FALSE} {AND_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AND_FalseAndFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {FALSE} {AND_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @bitwiseand ###DONE###

    [Test]
    public void Test_BITWISEAND()
    {
        var expectedValue1 = "2";
        result = ProcessTest(grod, $"{BITWISEAND_TOKEN}7,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "0";
        result = ProcessTest(grod, $"{BITWISEAND_TOKEN}8,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    #endregion

    #region @bitwiseor ###DONE###

    [Test]
    public void Test_BITWISEOR()
    {
        var expectedValue1 = "7";
        result = ProcessTest(grod, $"{BITWISEOR_TOKEN}7,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "10";
        result = ProcessTest(grod, $"{BITWISEOR_TOKEN}8,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    #endregion

    #region @bitwisexor ###DONE###

    [Test]
    public void Test_BITWISEXOR()
    {
        var expectedValue1 = "5";
        result = ProcessTest(grod, $"{BITWISEXOR_TOKEN}7,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "15";
        result = ProcessTest(grod, $"{BITWISEXOR_TOKEN}8,7)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    #endregion

    #region @cleararray ###DONE###

    [Test]
    public void Test_CLEARARRAY()
    {
        var key = "abc";
        var value = "123";
        var expectedValue = "";
        result = ProcessTest(grod, $"{SETARRAY_TOKEN}{key},2,3,{value}) {CLEARARRAY_TOKEN}{key}) {GETARRAY_TOKEN}{key},2,3)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @clearbit ###DONE###

    [Test]
    public void Test_CLEARBIT()
    {
        var expectedValue1 = "3";
        result = ProcessTest(grod, $"{CLEARBIT_TOKEN}7,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "6";
        result = ProcessTest(grod, $"{CLEARBIT_TOKEN}7,0)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));

        var expectedValue3 = "0";
        result = ProcessTest(grod, $"{CLEARBIT_TOKEN}1073741824,30)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue3));
    }

    #endregion

    #region @clearlist ###DONE###

    [Test]
    public void Test_CLEARLIST()
    {
        var key = "abc";
        var value1 = "123";
        var value2 = "456";
        var expectedValue = "";
        result = ProcessTest(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {CLEARLIST_TOKEN}{key}) {GETLIST_TOKEN}{key},0)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @comment ###DONE###

    [Test]
    public void Test_COMMENT()
    {
        var script = $"{COMMENT_TOKEN}\"this is a comment\")";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void Test_COMMENT_TwoComments()
    {
        string script = $"{COMMENT_TOKEN}\"This is a comment\n\") {COMMENT_TOKEN}\"Another comment\")";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    #endregion

    #region @concat ###DONE###

    [Test]
    public void Test_CONCAT()
    {
        var expectedValue = "abcdef123";
        result = ProcessTest(grod, $"{CONCAT_TOKEN}abc, def, 123)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @contains ###DONE###

    [Test]
    public void Test_CONTAINS_True()
    {
        var value = "111abc222";
        var search = "abc";
        var expectedValue = TRUE;
        var script = $@"{CONTAINS_TOKEN}{value},{search})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_CONTAINS_False()
    {
        var value = "111abc222";
        var search = "xyz";
        var expectedValue = FALSE;
        var script = $@"{CONTAINS_TOKEN}{value},{search})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @datetime ###DONE###

    [Test]
    public void Test_DATETIME()
    {
        var script = @"@write(@datetime(""MM-dd-yyyy HH:mm:ss""))";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Text));
        Assert.That(result[0].Value, Has.Length.EqualTo(19)); // value will vary, length fixed
        Assert.That(result[0].ExtraValue, Has.Length.EqualTo(6)); // value will vary, length fixed
    }

    [Test]
    public void Test_DATETIME_UTC()
    {
        var script = @"@write(@datetime(""MM-dd-yyyy HH:mm:ss"",utc))";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Text));
        Assert.That(result[0].Value, Has.Length.EqualTo(19)); // value will vary, length fixed
        Assert.That(result[0].ExtraValue, Is.EqualTo("UTC"));
    }

    [Test]
    public void Test_DATETIME_Roundtrip()
    {
        var script = @"@write(@datetime(""O""))";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Text));
        Assert.That(result[0].Value, Has.Length.EqualTo(33)); // value will vary, length fixed
        Assert.That(result[0].ExtraValue, Has.Length.EqualTo(6)); // value will vary, length fixed
    }

    [Test]
    public void Test_DATETIME_RoundtripUTC()
    {
        var script = @"@write(@datetime(""O"",utc))";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Text));
        Assert.That(result[0].Value, Has.Length.EqualTo(28)); // value will vary, length fixed
        Assert.That(result[0].Value[^1], Is.EqualTo('Z'));
        Assert.That(result[0].ExtraValue, Is.EqualTo("UTC"));
    }

    #endregion

    #region @debug ###DONE###

    [Test]
    public void Test_DEBUG()
    {
        var expectedValue1 = "### this is a debug comment";
        ProcessTest(grod, $"{SET_TOKEN}{DEBUG_FLAG},true)");
        result = ProcessTest(grod, $"{DEBUG_TOKEN}\"### this is a debug comment\")");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "579";
        ProcessTest(grod, $"{SET_TOKEN}{DEBUG_FLAG},true)");
        result = ProcessTest(grod, $"{DEBUG_TOKEN}{ADD_TOKEN}123,456))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));

        ProcessTest(grod, $"{SET_TOKEN}{DEBUG_FLAG},false)");
        result = ProcessTest(grod, $"{DEBUG_TOKEN}\"### this is a comment\")");
        Assert.That(result, Has.Count.EqualTo(0));
    }

    #endregion

    #region @divto ###DONE###

    [Test]
    public void Test_DIVTO_Value()
    {
        var key = "key";
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Remainder()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIVTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIVTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIVTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIVTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @div ###DONE###

    [Test]
    public void Test_DIV_Value()
    {
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Remainder()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = ProcessTest(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Blank()
    {
        var value1 = "5";
        var value2 = "";
        result = ProcessTest(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIV_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIV_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        result = ProcessTest(grod, $"{DIV_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIV_NotExists()
    {
        var notexists = "notexists";
        var value2 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{DIV_TOKEN}{GET_TOKEN}{notexists}),{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIV_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @do ###DONE###
    // part of @while
    #endregion

    #region @elseif ###DONE###
    // part of @if
    #endregion

    #region @else ###DONE###
    // part of @if
    #endregion

    #region @endforeachkey ###DONE###
    // part of @foreachkey
    #endregion

    #region @endforeachlist ###DONE###
    // part of @foreachlist
    #endregion

    #region @endfor ###DONE###
    // part of @for
    #endregion

    #region @endif ###DONE###
    // part of @if
    #endregion

    #region @endwhile ###DONE###
    // part of @while
    #endregion

    #region @eq

    [Test]
    public void Test_EQ_NumberEqual()
    {
        var value1 = "5";
        var value2 = "005";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{EQ_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_EQ_NumberGreater()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{EQ_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_EQ_NumberLess()
    {
        var value1 = "3";
        var value2 = "4";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{EQ_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_EQ_AlphaEqual()
    {
        var value1 = "abc";
        var value2 = "ABC";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{EQ_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_EQ_AlphaGreater()
    {
        var value1 = "def";
        var value2 = "abc";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{EQ_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_EQ_AlphaLess()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{EQ_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_EQ_NullNull()
    {
        var value1 = NULL;
        var value2 = NULL;
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{EQ_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_EQ_NullEmpty()
    {
        var value1 = NULL;
        var value2 = "";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{EQ_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @exec
    #endregion

    #region @false
    #endregion

    #region @flipbit
    #endregion

    #region @foreachkey
    #endregion

    #region @foreachlist
    #endregion

    #region @format
    #endregion

    #region @for
    #endregion

    #region @frombinary
    #endregion

    #region @fromhex
    #endregion

    #region @getarray
    #endregion

    #region @getbit
    #endregion

    #region @getchar

    [Test]
    public void Test_GETCHAR()
    {
        var script = @"
            @set(text,Hello)
            @write(@getchar(@get(text),2))
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, "l")
        };
        result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(answer));
    }

    [Test]
    public void Test_GETCHAR_OutOfBounds()
    {
        var script = @"
            @set(text,Hi)
            @write(@getchar(@get(text),5))
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, " ")
        };
        result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(answer));
    }

    [Test]
    public void Test_GETCHAR_Invalid()
    {
        var script = @"
            @set(text,Hello)
            @write(@getchar(@get(text),abc))
            ";
        var answer = "Invalid number: abc";
        result = ProcessTest(grod, script);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
            Assert.That(result[0].Value, Does.Contain(answer));
        }
    }

    [Test]
    public void Test_GETCHAR_NegativeIndex()
    {
        var script = @"
            @set(text,Hello)
            @write(@getchar(@get(text),-1))
            ";
        var answer = "Index out of range";
        result = ProcessTest(grod, script);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
            Assert.That(result[0].Value, Does.Contain(answer));
        }
    }

    [Test]
    public void Test_GETCHAR_ZeroIndex()
    {
        var script = @"
            @set(text,Hello)
            @write(@getchar(@get(text),0))
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, "H")
        };
        result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(answer));
    }

    [Test]
    public void Test_GETCHAR_EmptyString()
    {
        var script = @"
            @set(text,"""")
            @write(@getchar(@get(text),1))
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, " ")
        };
        result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(answer));
    }

    [Test]
    public void Test_GETCHAR_Unicode()
    {
        var testValue = "😊🌟🚀";
        var script = @$"
            @set(text,{testValue})
            @write(@getchar(@get(text),1))
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, testValue[1].ToString())
        };
        result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(answer));
    }

    [Test]
    public void Test_GETCHAR_Null()
    {
        var script = @"
            @set(text,null)
            @write(@getchar(@get(text),1))
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, " ")
        };
        result = ProcessTest(grod, script);
        Assert.That(result, Is.EqualTo(answer));
    }

    #endregion

    #region @getinchannel
    #endregion

    #region @getlist
    #endregion

    #region @getvalue
    #endregion

    #region @get
    #endregion

    #region @ge

    [Test]
    public void Test_GE_NumberEqual()
    {
        var value1 = "5";
        var value2 = "005";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GE_NumberGreater()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GE_NumberLess()
    {
        var value1 = "3";
        var value2 = "4";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GE_AlphaEqual()
    {
        var value1 = "abc";
        var value2 = "ABC";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GE_AlphaGreater()
    {
        var value1 = "def";
        var value2 = "abc";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GE_AlphaLess()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GE_NullNull()
    {
        var value1 = NULL;
        var value2 = NULL;
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GE_NullEmpty()
    {
        var value1 = NULL;
        var value2 = "";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GE_EmptySpace()
    {
        var value1 = "";
        var value2 = "\" \""; //need quotes
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @golabel
    #endregion

    #region @gt

    [Test]
    public void Test_GT_NumberEqual()
    {
        var value1 = "5";
        var value2 = "005";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GT_NumberGreater()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GT_NumberLess()
    {
        var value1 = "3";
        var value2 = "4";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GT_AlphaEqual()
    {
        var value1 = "abc";
        var value2 = "ABC";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GT_AlphaGreater()
    {
        var value1 = "def";
        var value2 = "abc";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GT_AlphaLess()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GT_NullNull()
    {
        var value1 = NULL;
        var value2 = NULL;
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GT_NullEmpty()
    {
        var value1 = NULL;
        var value2 = "";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_GT_EmptySpace()
    {
        var value1 = "";
        var value2 = "\" \""; //need quotes
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{GT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @if
    #endregion

    #region @inlist
    // See @listcontains
    #endregion

    #region @insertatlist
    #endregion

    #region @isbool
    #endregion

    #region @isfalse
    #endregion

    #region @isnull
    #endregion

    #region @isnumber
    #endregion

    #region @isscript
    #endregion

    #region @istrue
    #endregion

    #region @keyexists
    #endregion

    #region @label
    #endregion

    #region @len
    #endregion

    #region @le

    [Test]
    public void Test_LE_NumberEqual()
    {
        var value1 = "5";
        var value2 = "005";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{LE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LE_NumberGreater()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{LE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LE_NumberLess()
    {
        var value1 = "3";
        var value2 = "4";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{LE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LE_AlphaEqual()
    {
        var value1 = "abc";
        var value2 = "ABC";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{LE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LE_AlphaGreater()
    {
        var value1 = "def";
        var value2 = "abc";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{LE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LE_AlphaLess()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{LE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LE_NullNull()
    {
        var value1 = NULL;
        var value2 = NULL;
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{LE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LE_NullEmpty()
    {
        var value1 = NULL;
        var value2 = "";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{LE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @listcontains ###DONE###

    [Test]
    public void Test_LISTCONTAINS_True()
    {
        var key = "key";
        var value = "111,abc,222";
        var search = "abc";
        var expectedValue = TRUE;
        var script = $@"{LISTCONTAINS_TOKEN}{key},{search})";
        grod.Set(key, value);
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LISTCONTAINS_False()
    {
        var key = "key";
        var value = "111,abc,222";
        var search = "xyz";
        var expectedValue = FALSE;
        grod.Set(key, value);
        var script = $@"{LISTCONTAINS_TOKEN}{key},{search})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @listcontainsall ###DONE###

    [Test]
    public void Test_LISTCONTAINSALL_True()
    {
        var key1 = "key1";
        var key2 = "key2";
        var value1 = "111,abc,222";
        var value2 = "abc,111";
        var expectedValue = TRUE;
        grod.Set(key1, value1);
        grod.Set(key2, value2);
        var script = $@"{LISTCONTAINSALL_TOKEN}{key1},{key2})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LISTCONTAINSALL_False()
    {
        var key1 = "key1";
        var key2 = "key2";
        var value1 = "111,abc,222";
        var value2 = "abc,000";
        var expectedValue = FALSE;
        grod.Set(key1, value1);
        grod.Set(key2, value2);
        var script = $@"{LISTCONTAINSALL_TOKEN}{key1},{key2})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @listlength
    #endregion

    #region @lower
    #endregion

    #region @lt

    [Test]
    public void Test_LT_NumberEqual()
    {
        var value1 = "5";
        var value2 = "005";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{LT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LT_NumberGreater()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{LT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LT_NumberLess()
    {
        var value1 = "3";
        var value2 = "4";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{LT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LT_AlphaEqual()
    {
        var value1 = "abc";
        var value2 = "ABC";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{LT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LT_AlphaGreater()
    {
        var value1 = "def";
        var value2 = "abc";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{LT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LT_AlphaLess()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{LT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LT_NullNull()
    {
        var value1 = NULL;
        var value2 = NULL;
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{LT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_LT_NullEmpty()
    {
        var value1 = NULL;
        var value2 = "";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{LT_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @max
    #endregion

    #region @min
    #endregion

    #region @modto ###DONE###

    [Test]
    public void Test_MODTO_Value()
    {
        var key = "key";
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Remainder()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_ByZero()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "0";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_Negative1()
    {
        // -20 % 30 = -20
        var key = "key";
        var value1 = "-20";
        var value2 = "30";
        var expectedValue = "-20";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Negative2()
    {
        // -20 % 6 = -2
        var key = "key";
        var value1 = "-20";
        var value2 = "6";
        var expectedValue = "-2";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Negative3()
    {
        // 20 % -6 = 2
        var key = "key";
        var value1 = "20";
        var value2 = "-6";
        var expectedValue = "2";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Negative4()
    {
        // -20 % -6 = -2
        var key = "key";
        var value1 = "-20";
        var value2 = "-6";
        var expectedValue = "-2";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @mod ###DONE###

    [Test]
    public void Test_MOD_Value()
    {
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Remainder()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_ByZero()
    {
        var value1 = "5";
        var value2 = "0";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_Negative1()
    {
        // -20 % 30 = -20
        var value1 = "-20";
        var value2 = "30";
        var expectedValue = "-20";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative2()
    {
        // -20 % 6 = -2
        var value1 = "-20";
        var value2 = "6";
        var expectedValue = "-2";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative3()
    {
        // 20 % -6 = 2
        var value1 = "20";
        var value2 = "-6";
        var expectedValue = "2";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative4()
    {
        // -20 % -6 = -2
        var value1 = "-20";
        var value2 = "-6";
        var expectedValue = "-2";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Blank1()
    {
        // 0 % 5 = 0
        var value1 = "";
        var value2 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Blank2()
    {
        var value1 = "5";
        var value2 = "";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_NotExists()
    {
        // 0 % 5 = 0
        var notexists = "notexists";
        var value2 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{MOD_TOKEN}{GET_TOKEN}{notexists}),{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @msg ###DONE###

    [Test]
    public void Test_MSG()
    {
        var key = "key";
        var value = "Hello, World!";
        var expectedValue = "Hello, World!";
        grod.Set(key, value);
        string script = $"{MSG_TOKEN}{key})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
        Assert.That(result[1].Value, Is.EqualTo(NL_CHAR));
    }

    #endregion

    #region @multo ###DONE###

    [Test]
    public void Test_MULTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "20";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MULTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MULTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @mul ###DONE###

    [Test]
    public void Test_MUL_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "20";
        result = ProcessTest(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_ValueNegative()
    {
        var value1 = "5";
        var value2 = "-4";
        var expectedValue = "-20";
        result = ProcessTest(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_FromBlank()
    {
        var value1 = "";
        var value2 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MUL_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "0";
        result = ProcessTest(grod, $"{MUL_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MUL_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @negto ###DONE###

    [Test]
    public void Test_NEGTO()
    {
        var key = "key";
        var value = "5";
        var expectedValue = "-5";
        grod.Set(key, value);
        string script = $"{NEGTO_TOKEN}{key}) {GET_TOKEN}{key})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEGTO_Negative()
    {
        var key = "key";
        var value = "-5";
        var expectedValue = "5";
        grod.Set(key, value);
        string script = $"{NEGTO_TOKEN}{key}) {GET_TOKEN}{key})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEGTO_Zero()
    {
        var key = "key";
        var value = "0";
        var expectedValue = "0";
        grod.Set(key, value);
        string script = $"{NEGTO_TOKEN}{key}) {GET_TOKEN}{key})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEGTO_Error()
    {
        var key = "key";
        var value = "abc";
        grod.Set(key, value);
        string script = $"{NEGTO_TOKEN}{key}) {GET_TOKEN}{key})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @neg ###DONE###

    [Test]
    public void Test_NEG()
    {
        var value = "5";
        var expectedValue = "-5";
        string script = $"{NEG_TOKEN}{value})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEG_Negative()
    {
        var value = "-5";
        var expectedValue = "5";
        string script = $"{NEG_TOKEN}{value})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEG_Zero()
    {
        var value = "0";
        var expectedValue = "0";
        string script = $"{NEG_TOKEN}{value})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEG_Error()
    {
        var value = "abc";
        string script = $"{NEG_TOKEN}{value})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @ne

    [Test]
    public void Test_NE_NumberEqual()
    {
        var value1 = "5";
        var value2 = "005";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{NE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NE_NumberGreater()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{NE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NE_NumberLess()
    {
        var value1 = "3";
        var value2 = "4";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{NE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NE_AlphaEqual()
    {
        var value1 = "abc";
        var value2 = "ABC";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{NE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NE_AlphaGreater()
    {
        var value1 = "def";
        var value2 = "abc";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{NE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NE_AlphaLess()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue = TRUE;
        result = ProcessTest(grod, $"{NE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NE_NullNull()
    {
        var value1 = NULL;
        var value2 = NULL;
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{NE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NE_NullEmpty()
    {
        var value1 = NULL;
        var value2 = "";
        var expectedValue = FALSE;
        result = ProcessTest(grod, $"{NE_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @nl
    #endregion

    #region @not

    [Test]
    public void Test_NOT_AND_TrueAndNotTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {TRUE} {AND_TOKEN} {NOT_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NOT_AND_TrueAndNotFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {TRUE} {AND_TOKEN} {NOT_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NOT_OR_FalseOrNotFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {FALSE} {OR_TOKEN} {NOT_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NOT_OR_FalseOrNotTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {FALSE} {OR_TOKEN} {NOT_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @null
    #endregion

    #region @ongolabel
    #endregion

    #region @or

    [Test]
    public void Test_OR_FalseOrFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {FALSE} {OR_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_OR_TrueOrFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {TRUE} {OR_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_OR_FalseOrTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {FALSE} {OR_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_OR_TrueOrTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {TRUE} {OR_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @rand
    #endregion

    #region @removeatlist ###DONE###

    [Test]
    public void Test_REMOVEATLIST()
    {
        var key = "key";
        var value = "123";
        var expectedValue = "123";
        ProcessTest(grod, $"{SETLIST_TOKEN}{key},3,{value})");
        ProcessTest(grod, $"{REMOVEATLIST_TOKEN}{key},0)");
        result = ProcessTest(grod, $"{GETLIST_TOKEN}{key},2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @replace
    #endregion

    #region @return
    #endregion

    #region @rnd
    #endregion

    #region @script
    #endregion

    #region @setarray
    #endregion

    #region @setbit
    #endregion

    #region @setchar
    #endregion

    #region @setextra
    #endregion

    #region @setlist
    #endregion

    #region @setoutchannel
    #endregion

    #region @set
    #endregion

    #region @substring
    #endregion

    #region @subto ###DONE###

    [Test]
    public void Test_SUBTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_SUBTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "-5";
        result = ProcessTest(grod, $"{SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_SUBTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @sub ###DONE###

    [Test]
    public void Test_SUB_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = ProcessTest(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_ValueNegative()
    {
        var value1 = "5";
        var value2 = "14";
        var expectedValue = "-9";
        result = ProcessTest(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_FromBlank()
    {
        var value1 = "";
        var value2 = "5";
        var expectedValue = "-5";
        result = ProcessTest(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = ProcessTest(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_SUB_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = ProcessTest(grod, $"{SUB_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = ProcessTest(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_SUB_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = ProcessTest(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @swap

    [Test]
    public void Test_Swap()
    {
        var key1 = "key1";
        var key2 = "key2";
        var value1 = "value1";
        var value2 = "value2";
        var expectedValue = "value2,value1";
        var script = @$"{SET_TOKEN}{key1},{value1}) {SET_TOKEN}{key2},{value2}) {SWAP_TOKEN}{key1},{key2}) {CONCAT_TOKEN}{GET_TOKEN}{key1}),"","",{GET_TOKEN}{key2}))";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @then
    #endregion

    #region @tobinary
    #endregion

    #region @tohex
    #endregion

    #region @trim
    #endregion

    #region @true

    [Test]
    public void Test_True()
    {
        var value1 = "1";
        var expectedValue1 = TRUE;
        var script1 = $"{ISTRUE_TOKEN}{value1})";
        result = ProcessTest(grod, script1);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var value2 = "0";
        var expectedValue2 = FALSE;
        var script2 = $"{ISTRUE_TOKEN}{value2})";
        result = ProcessTest(grod, script2);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));

        var value3 = "abc";
        var expectedValue3 = FALSE;
        var script3 = $"{ISTRUE_TOKEN}{value3})";
        result = ProcessTest(grod, script3);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue3));

        var value4 = NULL;
        var expectedValue4 = FALSE;
        var script4 = $"{ISTRUE_TOKEN}{value4})";
        result = ProcessTest(grod, script3);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue4));
    }

    #endregion

    #region @upper
    #endregion

    #region @while ###DONE###

    [Test]
    public void Test_WHILE()
    {
        var script = @"
            @set(_a,0)
            @while @lt(@get(_a),3) @do
                @addto(_a,1)
                @write(@get(_a))
            @endwhile
            @write(""xyz"")
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, "1"),
            new(MessageType.Text, "2"),
            new(MessageType.Text, "3"),
            new(MessageType.Text, "xyz")
        };
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(4));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result, Is.EqualTo(answer));
    }

    [Test]
    public void Test_WHILE_Return()
    {
        var script = @"
            @set(_a,0)
            @while @lt(@get(_a),3) @do
                @addto(_a,1)
                @write(@get(_a))
                @if @eq(@get(_a),2) @then
                    @return
                @endif
            @endwhile
            @write(""xyz"")
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, "1"),
            new(MessageType.Text, "2")
        };
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result, Is.EqualTo(answer));
    }

    #endregion

    #region @writeline ###DONE###

    [Test]
    public void Test_WRITELINE()
    {
        var value1 = "abc";
        var expectedValue1 = "abc";
        string script = $"{WRITELINE_TOKEN}{value1})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        Assert.That(result[1].Value, Is.EqualTo(NL_CHAR));
    }

    [Test]
    public void Test_WRITELINE_Concatenate()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue1 = "abc";
        var expectedValue2 = "def";
        string script = $"{WRITELINE_TOKEN}{value1},{value2})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        Assert.That(result[1].Value, Is.EqualTo(expectedValue2));
        Assert.That(result[2].Value, Is.EqualTo(NL_CHAR));
    }

    [Test]
    public void Test_WRITELINE_FromGet()
    {
        var key = "key";
        var value = "abc";
        var expectedValue = "abc";
        grod.Set(key, value);
        string script = $"{WRITELINE_TOKEN}{GET_TOKEN}{key}))";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
        Assert.That(result[1].Value, Is.EqualTo(NL_CHAR));
    }

    #endregion

    #region @write ###DONE###

    [Test]
    public void Test_WRITE()
    {
        var value1 = "abc";
        var expectedValue1 = "abc";
        string script = $"{WRITE_TOKEN}{value1})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
    }

    [Test]
    public void Test_WRITE_Concatenate()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue1 = "abc";
        var expectedValue2 = "def";
        string script = $"{WRITE_TOKEN}{value1},{value2})";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        Assert.That(result[1].Value, Is.EqualTo(expectedValue2));
    }

    [Test]
    public void Test_WRITE_FromGet()
    {
        var key = "key";
        var value = "abc";
        var expectedValue = "abc";
        grod.Set(key, value);
        string script = $"{WRITE_TOKEN}{GET_TOKEN}{key}))";
        result = ProcessTest(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion
}
