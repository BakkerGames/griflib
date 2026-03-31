# GrifLib

## Overview

GrifLib is the engine library for GRIF, a game runner for interactive fiction games. It is designed to be a simple but extensible engine that can handle various styles of interactive fiction.

GrifLib can be integrated into other applications to provide game support such as scripting, in-memory data storage, and save/restore functionality.

## Features

- Supports GRIF and JSON game formats
- DAGS scripting language for game logic
- In-memory data storage for static and modified game data
- Automatic save and load functionality with a top-level overlay
- Can return interleaved results including media information
- Can handle system events such as sleep
- Can stack multiple GRIF files for modular game design
- Outchannel support can be customized for different output methods
- Handles 64-bit integers for larger data values
- Scripts can have local variables for internal processing
- @while ... @do ... @endwhile loop structure
- @return command allows exiting scripts immediately
- Many new built-in script functions
- Optional IFGame and IFParser classes for interactive fiction game management
- IFGame provides Input and Output event stacks for external handling
- Improved IFParser with better adjective and preposition support