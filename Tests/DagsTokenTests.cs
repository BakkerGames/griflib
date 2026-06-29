using GrifLib;
using Newtonsoft.Json.Linq;
using static GrifLib.Common;
using static GrifLib.Dags;

/*
    [Test]
    public void Test_()
    {
        var key = "key";
        var value = "1";
        var expectedValue = "1";
        result = Process(grod, $"... {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"... {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }
*/

namespace Tests;

public class DagsTokenTests
{
    #region Setup
    private readonly Grod grod = new("test");
    private List<GrifMessage> result = [];

    [SetUp]
    public void Setup()
    {
        grod.Clear(true);
        result.Clear();
    }
    #endregion

    #region @abs

    [Test]
    public void Test_ABS_Positive()
    {
        var value = "23";
        var expectedValue = "23";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Negative()
    {
        var value = "-23";
        var expectedValue = "23";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Zero()
    {
        var value = "0";
        var expectedValue = "0";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_NoParams()
    {
        result = Process(grod, $"{ABS_TOKEN})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_ABS_Empty()
    {
        var value = "\"\"";
        var expectedValue = "0";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Null()
    {
        string? value = null;
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_ABS_Invalid()
    {
        var value = "abc";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @addlist

    [Test]
    public void Test_ADDLIST_OneItem()
    {
        var key = "key";
        var value = "1";
        var expectedValue = "1";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_TwoItems()
    {
        var key = "key";
        var value1 = "1";
        var expectedValue1 = "1";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        var value2 = "2";
        var expectedValue2 = $"{value1},{value2}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    [Test]
    public void Test_ADDLIST_Empty()
    {
        var key = "key";
        var value = "";
        var expectedValue = "";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_ValueEmptyValue()
    {
        var key = "key";
        var value1 = "value1";
        var value2 = "";
        var value3 = "value3";
        var expectedValue = $"{value1},,{value3}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_ValueNullValue()
    {
        var key = "key";
        var value1 = "value1";
        string? value2 = null;
        var value3 = "value3";
        var expectedValue = $"{value1},,{value3}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_NullNullNull()
    {
        var key = "key";
        string? value1 = null;
        string? value2 = null;
        string? value3 = null;
        var expectedValue = $",,";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @addto

    [Test]
    public void Test_ADDTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "9";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_ADDTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_ADDTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @add

    [Test]
    public void Test_ADD_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "9";
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_ADD_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{ADD_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_ADD_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @and
    #endregion

    #region @bitwiseand
    #endregion

    #region @bitwiseor
    #endregion

    #region @bitwisexor
    #endregion

    #region @cleararray
    #endregion

    #region @clearbit
    #endregion

    #region @clearlist
    #endregion

    #region @comment
    #endregion

    #region @concat
    #endregion

    #region @containslist
    #endregion

    #region @contains
    #endregion

    #region @datetime
    #endregion

    #region @debug
    #endregion

    #region @divto

    [Test]
    public void Test_DIVTO_Value()
    {
        var key = "key";
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Remainder()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_DIVTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_DIVTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_DIVTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_DIVTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @div

    [Test]
    public void Test_DIV_Value()
    {
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "5";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Remainder()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Blank()
    {
        var value1 = "5";
        var value2 = "";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_DIV_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_DIV_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        result = Process(grod, $"{DIV_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_DIV_NotExists()
    {
        var notexists = "notexists";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{DIV_TOKEN}{GET_TOKEN}{notexists}),{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_DIV_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @do
    #endregion

    #region @elseif
    #endregion

    #region @else
    #endregion

    #region @endforeachkey
    #endregion

    #region @endforeachlist
    #endregion

    #region @endfor
    #endregion

    #region @endif
    #endregion

    #region @endwhile
    #endregion

    #region @eq
    #endregion

    #region @exec
    #endregion

    #region @exists
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
    #endregion

    #region @golabel
    #endregion

    #region @gt
    #endregion

    #region @if
    #endregion

    #region @inlist
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

    #region @label
    #endregion

    #region @len
    #endregion

    #region @le
    #endregion

    #region @listlength
    #endregion

    #region @lower
    #endregion

    #region @lt
    #endregion

    #region @max
    #endregion

    #region @min
    #endregion

    #region @modto

    [Test]
    public void Test_MODTO_Value()
    {
        var key = "key";
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "0";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Remainder()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_ByZero()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "0";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MODTO_Negative1()
    {
        // -20 % 30 = -20
        var key = "key";
        var value1 = "-20";
        var value2 = "30";
        var expectedValue = "-20";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
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
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
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
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
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
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MODTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MODTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MODTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MODTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @mod

    [Test]
    public void Test_MOD_Value()
    {
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "0";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Remainder()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_ByZero()
    {
        var value1 = "5";
        var value2 = "0";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MOD_Negative1()
    {
        // -20 % 30 = -20
        var value1 = "-20";
        var value2 = "30";
        var expectedValue = "-20";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative2()
    {
        // -20 % 6 = -2
        var value1 = "-20";
        var value2 = "6";
        var expectedValue = "-2";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative3()
    {
        // 20 % -6 = 2
        var value1 = "20";
        var value2 = "-6";
        var expectedValue = "2";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative4()
    {
        // -20 % -6 = -2
        var value1 = "-20";
        var value2 = "-6";
        var expectedValue = "-2";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Blank1()
    {
        // 0 % 5 = 0
        var value1 = "";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Blank2()
    {
        var value1 = "5";
        var value2 = "";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MOD_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MOD_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        result = Process(grod, $"{MOD_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MOD_NotExists()
    {
        // 0 % 5 = 0
        var notexists = "notexists";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MOD_TOKEN}{GET_TOKEN}{notexists}),{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MOD_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @msg
    #endregion

    #region @multo

    [Test]
    public void Test_MULTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "20";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "0";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MULTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MULTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @mul

    [Test]
    public void Test_MUL_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "20";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_ValueNegative()
    {
        var value1 = "5";
        var value2 = "-4";
        var expectedValue = "-20";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "0";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_FromBlank()
    {
        var value1 = "";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MUL_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MUL_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_MUL_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @negto
    #endregion

    #region @neg
    #endregion

    #region @ne
    #endregion

    #region @nl
    #endregion

    #region @not
    #endregion

    #region @null
    #endregion

    #region @ongolabel
    #endregion

    #region @or
    #endregion

    #region @rand
    #endregion

    #region @removeatlist
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

    #region @subto

    [Test]
    public void Test_SUBTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_SUBTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "-5";
        result = Process(grod, $"{SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_SUBTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @sub

    [Test]
    public void Test_SUB_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_ValueNegative()
    {
        var value1 = "5";
        var value2 = "14";
        var expectedValue = "-9";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_FromBlank()
    {
        var value1 = "";
        var value2 = "5";
        var expectedValue = "-5";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_SUB_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{SUB_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.Not.EqualTo(MessageType.Error));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    [Test]
    public void Test_SUB_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Error));
    }

    #endregion

    #region @swap
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
    #endregion

    #region @upper
    #endregion

    #region @while
    #endregion

    #region @writeline
    #endregion

    #region @write
    #endregion
}
