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
- Map structures for defining and handling fixed numeric arrays
- Built-in searchable help library

## Sections

"GRIF" stands for "Game Runner Interactive Fiction". GRIF is the game runner console application (see below) and is also a file format used to store the interactive fiction game data. GrifLib provides the necessary tools to load, manage, and run these games data files from an application like GRIF.

"GROD", or "Game Resource Overlay Dictionary", is the component of GrifLib for managing game data in memory. These consist of layered dictionary objects containing key/value pairs. Searching for a key starts at the top layer and proceeds through the layers until it is found. Modifications only happen to the top layer, so that top layer can be used to save/restore the current game state.

"DAGS", or "Data Access Game Scripts", is the scripting language used in GrifLib for creating interactive fiction games. DAGS scripts consist of functions which all start with "@". They are stored in a GROD dictionary along with value items. DAGS has functions to directly access the GROD game data, to perform calculations and manipulations, and to output the results. It includes helper routines to validate scripts, format scripts, and colorize script tokens.

"IO" is a class for reading and writing GRIF files to/from disk or stream. The input and output formats are plain text and can either be GRIF format or JSON object format. It can read GRIFSTACK files with lists of GRIF files to open in a specific order.

"IFGame" is a ready-built class for running Interactive Fiction GRIF games. It has an Initialize() routine to prepare the game, an Intro() routine to begin the game, and a GameStep() routine to manage each move. It has event handlers for input and output which can be redirected. It will run background tasks after each command is processed. It will handle OutChannel commands sent to the parent program from the game data, such as commands to save and load files or request input from the user.

"IFParser" can be used to parse the entered commands. It reads the GROD data to determine verbs, nouns, adjectives, and prepositions. It will determine the proper script command to be run from the entered text, which is then passed to DAGS to run.

You can see all this come together in the GRIF console application, found on [Github](https://github.com/BakkerGames/grif).