using GrifLib;
using static GrifLib.Dags;

namespace Tests;

public class DagsTokenTests
{
    #region Setup
    private readonly Grod grod = new("base");
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
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Negative()
    {
        var value = "-23";
        var expectedValue = "23";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Zero()
    {
        var value = "0";
        var expectedValue = "0";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
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
    public void Test_AddList_OneItem()
    {
        var key = "key";
        var value = "1";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value})");
        Assert.That(result, Has.Count.EqualTo(0));
        Assert.That(grod.Get(key, false), Is.EqualTo(value));
    }

    [Test]
    public void Test_AddList_TwoItems()
    {
        var key = "key";
        var value1 = "1";
        var expectedValue1 = "1";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1})");
        Assert.That(result, Has.Count.EqualTo(0));
        Assert.That(grod.Get(key, false), Is.EqualTo(expectedValue1));
        var value2 = "2";
        var expectedValue2 = $"{value1},{value2}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value2})");
        Assert.That(result, Has.Count.EqualTo(0));
        Assert.That(grod.Get(key, false), Is.EqualTo(expectedValue2));
    }

    [Test]
    public void Test_AddList_Empty()
    {
        var key = "key";
        var value = "";
        string? expectedValue = "";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value})");
        Assert.That(result, Has.Count.EqualTo(0));
        Assert.That(grod.Get(key, false), Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AddList_ValueEmptyValue()
    {
        var key = "key";
        var value1 = "value1";
        var value2 = "";
        var value3 = "value3";
        string? expectedValue = $"{value1},,{value3}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1})");
        var grod1 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value2})");
        var grod2 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value3})");
        var grod3 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        Assert.That(grod3, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AddList_ValueNullValue()
    {
        var key = "key";
        var value1 = "value1";
        string? value2 = null;
        var value3 = "value3";
        string? expectedValue = $"{value1},,{value3}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1})");
        var grod1 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value2})");
        var grod2 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value3})");
        var grod3 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        Assert.That(grod3, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AddList_NullNullNull()
    {
        var key = "key";
        string? value1 = null;
        string? value2 = null;
        string? value3 = null;
        string? expectedValue = $",,";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1})");
        var grod1 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value2})");
        var grod2 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value3})");
        var grod3 = grod.Get(key, false);
        Assert.That(result, Has.Count.EqualTo(0));
        Assert.That(grod3, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @addto
    #endregion

    #region @add
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
    #endregion

    #region @div
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
    #endregion

    #region @mod
    #endregion

    #region @msg
    #endregion

    #region @multo
    #endregion

    #region @mul
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
    #endregion

    #region @sub
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
