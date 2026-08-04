using static GrifLib.Common;
using static GrifLib.Dags;
using static GrifLib.IFParser;
using static GrifLib.IO;

namespace GrifLib;

/// <summary>
/// Represents the method that handles an input event raised by a component or control.
/// </summary>
public delegate void InputEventHandler(object sender);

/// <summary>
/// Represents the method that will handle an output event raised by the system.
/// </summary>
/// <param name="sender">The source of the event.</param>
public delegate void OutputEventHandler(object sender, GrifMessage e);

/// <summary>
/// Represents the method that will handle an OutChannel event raised by the system.
/// </summary>
/// <param name="sender">The source of the event.</param>
public delegate void OutChannelEventHandler(object sender, GrifMessage e);

public class IFGame
{
    private Grod _baseGrod = new();
    private Grod _overlayGrod = new();
    private string _saveBasePath = "";
    private string? _referenceBasePath;
    private int awaitAnswer = 0;
    private int awaitEnter = 0;

    /// <summary>
    /// Occurs when an input action is performed, such as a key press or mouse event.
    /// </summary>
    public event InputEventHandler? InputEvent;

    /// <summary>
    /// Occurs when output data is available or an output event is raised.
    /// </summary>
    public event OutputEventHandler? OutputEvent;

    /// <summary>
    /// Occurs when an OutChannel message is not handled by the current routines.
    /// </summary>
    public event OutChannelEventHandler? OutChannelEvent;

    /// <summary>
    /// Gets or sets a value indicating whether the game has ended.
    /// </summary>
    public bool GameOver { get; set; } = false;

    /// <summary>
    /// Gets the queue of input messages awaiting processing.
    /// </summary>
    public Queue<GrifMessage> InputMessages { get; } = new();

    /// <summary>
    /// Gets the queue of messages that are pending to be sent or processed by the system.
    /// </summary>
    public Queue<GrifMessage> OutputMessages { get; } = new();

    /// <summary>
    /// Initializes the game state, configuring save and reference paths and preparing the game data for use.
    /// </summary>
    public void Initialize(Grod grod, string saveBasePath, string? referenceBasePath = null)
    {
        GameOver = false;
        _saveBasePath = saveBasePath;
        _referenceBasePath = referenceBasePath;
        try
        {
            if (!Path.IsPathRooted(_saveBasePath))
            {
                _saveBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), APP_NAME, _saveBasePath);
            }
            if (!Directory.Exists(_saveBasePath))
            {
                Directory.CreateDirectory(_saveBasePath);
            }
        }
        catch (Exception)
        {
            throw new IOException("Failed to initialize game save path.");
        }
        try
        {
            if (!string.IsNullOrEmpty(_referenceBasePath))
            {
                if (!Path.IsPathRooted(_referenceBasePath))
                {
                    _referenceBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_NAME, _referenceBasePath);
                }
                if (!Directory.Exists(_referenceBasePath))
                {
                    throw new IOException("Reference path does not exist.");
                }
            }
        }
        catch (IOException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new IOException("Failed to initialize game resource path.");
        }
        try
        {
            _baseGrod = grod;
            _overlayGrod = new(SAVE_FILENAME, Path.Combine(_saveBasePath, SAVE_FILENAME + SAVE_EXTENSION))
            {
                Parent = _baseGrod
            };
        }
        catch (Exception)
        {
            throw new IOException("Failed to initialize game data.");
        }
    }

    /// <summary>
    /// Runs the script "system.intro" or all background tasks. Signals for the first input.
    /// </summary>
    public void Intro()
    {
        var intro = _overlayGrod.Get(INTRO, true);
        if (!string.IsNullOrEmpty(intro))
        {
            // intro is a script or a message
            var introItems = Process(_overlayGrod, intro);
            foreach (var item in introItems)
            {
                OutputMessages.Enqueue(item);
            }
        }
        else
        {
            // intro is handled by background scripts
            AdvanceGameState();
        }
        while (OutputMessages.Count > 0)
        {
            var outputMessage = OutputMessages.Dequeue();
            if (ProcessOutputMessage(outputMessage))
            {
                return;
            }
        }
        if (GameOver) return;
        if (InputEvent != null)
        {
            InputEvent?.Invoke(this);
        }
    }

    /// <summary>
    /// Runs one game step:
    /// 1) Process input
    /// 2) Advance the game state
    /// 3) Signal ready for another input
    /// </summary>
    public void GameStep()
    {
        bool waitForAnswer = false;
        if (GameOver) return;
        if (InputMessages.Count == 0) return;
        var inputMessage = InputMessages.Dequeue();
        ProcessInputMessage(inputMessage);
        while (!waitForAnswer && OutputMessages.Count > 0)
        {
            var outputMessage = OutputMessages.Dequeue();
            waitForAnswer = ProcessOutputMessage(outputMessage);
        }
        if (GameOver) return;
        if (!waitForAnswer)
        {
            AdvanceGameState();
            while (!waitForAnswer && OutputMessages.Count > 0)
            {
                var outputMessage = OutputMessages.Dequeue();
                waitForAnswer = ProcessOutputMessage(outputMessage);
            }
        }
        if (!waitForAnswer)
        {
            if (GameOver) return;
            if (InputEvent != null)
            {
                InputEvent?.Invoke(this);
            }
        }
    }

    /// <summary>
    /// Generates and returns the current prompt text, processing scripts if present.
    /// </summary>
    public List<GrifMessage> Prompt()
    {
        var prompt = _overlayGrod.Get(PROMPT, true);
        if (IsNull(prompt))
        {
            return [];
        }
        if (IsScript(prompt))
        {
            return Process(_overlayGrod, prompt);
        }
        return [new GrifMessage(MessageType.Text, prompt)];
    }

    /// <summary>
    /// Generates and returns the text or script to be displayed after the prompt.
    /// </summary>
    public List<GrifMessage> AfterPrompt()
    {
        var afterPrompt = _overlayGrod.Get(AFTER_PROMPT, true);
        if (IsNull(afterPrompt))
        {
            return [];
        }
        if (IsScript(afterPrompt))
        {
            return Process(_overlayGrod, afterPrompt);
        }
        return [new GrifMessage(MessageType.Text, afterPrompt)];
    }

    #region Private routines

    /// <summary>
    /// Processes the specified input message and enqueues the resulting items for output.
    /// </summary>
    private void ProcessInputMessage(GrifMessage inputMessage)
    {
        if (awaitEnter > 0)
        {
            awaitEnter--;
            return;
        }
        if (awaitAnswer > 0)
        {
            awaitAnswer--;
            _overlayGrod.Set(INCHANNEL, inputMessage.Value);
            return;
        }
        var results = ParseInput(_overlayGrod, inputMessage.Value);
        foreach (var item in results ?? [])
        {
            OutputMessages.Enqueue(item);
        }
    }

    /// <summary>
    /// Processes an output message and dispatches it to the appropriate handler based on its type.
    /// Returns true if an answer is requested.
    /// </summary>
    public bool ProcessOutputMessage(GrifMessage message)
    {
        switch (message.Type)
        {
            case MessageType.Text:
                OutputEvent?.Invoke(this, message);
                break;
            case MessageType.OutChannel:
                return HandleOutChannel(message);
            case MessageType.Script:
                var outputItems = ProcessItems(_overlayGrod, [message]);
                foreach (var item in outputItems)
                {
                    OutputMessages.Enqueue(item);
                }
                break;
            case MessageType.Debug:
                OutputEvent?.Invoke(this, message);
                OutputEvent?.Invoke(this, new GrifMessage(MessageType.Text, NL_CHAR));
                break;
            case MessageType.Error:
                OutputEvent?.Invoke(this, message);
                break;
            default:
                OutputEvent?.Invoke(this, new GrifMessage(MessageType.Text, $"Unsupported output message type: {message.Type}\\n", message.Value + message.ExtraValue));
                break;
        }
        return false;
    }

    /// <summary>
    /// Advances the game state by processing background scripts and enqueuing resulting output messages.
    /// </summary>
    private void AdvanceGameState()
    {
        var keys = _overlayGrod.Keys(BACKGROUND_PREFIX, true, false);
        foreach (var key in keys)
        {
            var script = $"{SCRIPT_TOKEN}{key})";
            var items = Process(_overlayGrod, script);
            foreach (var item in items)
            {
                OutputMessages.Enqueue(item);
            }
        }
    }

    /// <summary>
    /// Handles an outbound channel message by performing the corresponding action.
    /// Returns true if an answer is requested.
    /// Ignores unsupported OutChannel messages.
    /// </summary>
    private bool HandleOutChannel(GrifMessage message)
    {
        bool exists;

        if (message.Value.Equals(OUTCHANNEL_GAMEOVER, OIC))
        {
            GameOver = true;
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_EXISTS_SAVE, OIC))
        {
            var savefile = Path.Combine(_saveBasePath, SAVE_FILENAME + SAVE_EXTENSION);
            exists = File.Exists(savefile);
            _overlayGrod.Set(INCHANNEL, exists ? "true" : "false");
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_EXISTS_SAVE_NAME, OIC))
        {
            if (message.ExtraValue == null)
            {
                throw new Exception("Save filename not specified.");
            }
            var savefile = Path.Combine(_saveBasePath, message.ExtraValue + SAVE_EXTENSION);
            exists = File.Exists(savefile);
            _overlayGrod.Set(INCHANNEL, exists ? "true" : "false");
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_SAVE, OIC))
        {
            var savefile = Path.Combine(_saveBasePath, SAVE_FILENAME + SAVE_EXTENSION);
            var itemList = _overlayGrod.Items(false, true);
            WriteGrif(savefile, itemList, true);
            _overlayGrod.Changed = false;
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_SAVE_NAME, OIC))
        {
            if (message.ExtraValue == null)
            {
                throw new Exception("Save filename not specified.");
            }
            var savefile = Path.Combine(_saveBasePath, message.ExtraValue + SAVE_EXTENSION);
            var itemList = _overlayGrod.Items(false, true);
            WriteGrif(savefile, itemList, true);
            _overlayGrod.Changed = false;
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_RESTORE, OIC))
        {
            var savefile = Path.Combine(_saveBasePath, SAVE_FILENAME + SAVE_EXTENSION);
            if (!File.Exists(savefile))
            {
                throw new FileNotFoundException(savefile);
            }
            var itemList = ReadGrif(savefile);
            _overlayGrod.Clear(false); // clear only the user data
            _overlayGrod.AddItems(itemList);
            _overlayGrod.Changed = false;
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_RESTORE_NAME, OIC))
        {
            if (message.ExtraValue == null)
            {
                throw new Exception("Save filename not specified.");
            }
            var savefile = Path.Combine(_saveBasePath, message.ExtraValue + SAVE_EXTENSION);
            if (!File.Exists(savefile))
            {
                throw new FileNotFoundException(savefile);
            }
            var itemList = ReadGrif(savefile);
            _overlayGrod.Clear(false); // clear only the user data
            _overlayGrod.AddItems(itemList);
            _overlayGrod.Changed = false;
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_RESTART, OIC))
        {
            _overlayGrod.Clear(false); // clear only the user data
            _overlayGrod.Changed = false;
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_ENTER, OIC))
        {
            awaitEnter++;
            if (InputEvent != null)
            {
                InputEvent?.Invoke(this);
            }
            return true;
        }

        if (message.Value.Equals(OUTCHANNEL_ASK, OIC))
        {
            awaitAnswer++;
            if (InputEvent != null)
            {
                InputEvent?.Invoke(this);
            }
            return true;
        }

        if (message.Value.Equals(OUTCHANNEL_LOAD_LAYER, OIC))
        {
            if (string.IsNullOrEmpty(message.ExtraValue))
            {
                throw new Exception("Grod layer name not specified.");
            }
            var grodName = message.ExtraValue;
            var tempGrod = _overlayGrod.GetGrod(grodName);
            if (tempGrod == null)
            {
                // Insert a new grod layer into the stack to save the data
                tempGrod = new Grod()
                {
                    Name = grodName,
                    Parent = _overlayGrod.Parent
                };
                _overlayGrod.Parent = tempGrod;
            }
            tempGrod.FilePath ??= Path.Combine(_saveBasePath, grodName + DATA_EXTENSION);
            if (!File.Exists(tempGrod.FilePath))
            {
                throw new Exception($"Grod file not found: {tempGrod.FilePath}");
            }
            var existingList = ReadGrif(tempGrod.FilePath);
            tempGrod.Clear(false);
            tempGrod.AddItems(existingList);
            tempGrod.Changed = false;
            return false;
        }

        if (message.Value.Equals(OUTCHANNEL_SAVE_LAYER, OIC))
        {
            if (string.IsNullOrEmpty(message.ExtraValue))
            {
                throw new Exception("Grod name not specified.");
            }
            var grodName = message.ExtraValue;
            var tempGrod = _overlayGrod.GetGrod(grodName);
            if (tempGrod == null)
            {
                throw new Exception("Grod name not found in stack.");
            }
            tempGrod.FilePath ??= Path.Combine(_saveBasePath, grodName + DATA_EXTENSION);
            if (!File.Exists(tempGrod.FilePath))
            {
                throw new Exception($"Grod file not found: {tempGrod.FilePath}");
            }
            var savefile = tempGrod.FilePath;
            var itemList = tempGrod.Items(false, true);
            WriteGrif(savefile, itemList, true);
            tempGrod.Changed = false;
            return false;
        }

        if (IsScript(message.Value))
        {
            var outputItems = ProcessItems(_overlayGrod, [new GrifMessage(MessageType.Script, message.Value)]);
            foreach (var outputItem in outputItems)
            {
                OutputMessages.Enqueue(outputItem);
            }
            return false;
        }

        // Send unhandled OutChannel message to calling program for processing
        OutChannelEvent?.Invoke(this, message);

        return false;
    }

    #endregion
}
