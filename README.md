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
- @return command for exiting scripts immediately
- @while...@do...@endwhile loop structure
- @break and @continue commands for @for and @while loops
- IFGame and IFParser classes for interactive fiction game management
- IFGame provides Input and Output event stacks for external handling
- Improved IFParser with better adjective and preposition support
- @datetime() for getting local or UTC dates and/or times
- Colorized scripts when supported by calling application
- Built-in searchable help library

## Sections

"GRIF" stands for "Game Runner Interactive Fiction". GRIF is a file format used to store interactive fiction game data. GrifLib provides the necessary tools to load, run, and manage these games files.

"GROD", or "Game Resource Overlay Dictionary", is a key component of GrifLib for managing game data. These are layered dictionary objects containing key/value pairs. Searching for a key starts at the top and proceeds through the layers until it is found. Modifications only happen to the top layer, so it can be used to save/restore the current game state.

"DAGS", or "Data Access Game Scripts", is the scripting language used in GrifLib for creating interactive fiction games. DAGS has functions to directly access GROD game data, to perform calculations and manipulations, and to output the results.

"IFGame" is a ready-built class for running GRIF games. It has an Initialize() routine to prepare the game, an Intro() routine to begin the game, and a GameStep() routine to manage each move. It has event handlers for input and output which can be redirected. It will handle OutChannel commands sent to the parent program from the game data, such as commands to save and load files.

"IFParser" can be used to parse the entered commands. It reads the GROD layers to determine verbs, nouns, adjectives, and prepositions. It will determine the proper script command to be run from the entered text, which is then passed to DAGS to run.

You can see all this come together in the Grif console application, found on [Github](https://github.com/BakkerGames/grif).