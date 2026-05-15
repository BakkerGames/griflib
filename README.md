# GrifLib

GrifLib is a C# library designed to be used for running games and handling game data.

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
- Many new built-in script functions
- @while ... @do ... @endwhile loop structure
- @return command allows exiting scripts immediately
- Optional IFGame and IFParser classes for interactive fiction game management
- IFGame provides Input and Output event stacks for external handling
- Improved IFParser with better adjective and preposition support
- @datetime() for getting local or UTC dates and/or times
- Colorized scripts when supported by calling application
- Built-in searchable help library (in progress)

## Names

"GRIF" stands for "Game Runner Interactive Fiction". GRIF files are used to store interactive fiction game data. GrifLib provides the necessary tools to run and manage these games.

"GROD" stands for "Game Resource Overlay Dictionary", which is a key component of GrifLib for managing game data. These are layered dictionary objects containing key/value pairs. Searching for a key starts at the top and proceeds through the layers until it is found. Modifications only happen at the top level, so it can be used to save/restore the current game state.

"DAGS" stands for "Data Access Game Scripts", which is the scripting language used in GrifLib for creating interactive fiction games. DAGS has functions to directly access GROD game data, to perform calculations and manipulations, and to output the results.