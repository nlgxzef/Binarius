![](Binary/Resources/binarius.png)

# Fork Note
Binarius is a fork that's created to improve Binary with QoL improvements and new features.  
The main library used by this application called Nikki also got forked and renamed to Darius.

# AI Notice
This tool partially utilizes GitHub Copilot and Claude.  
They are used mainly as advanced autocomplete and code-assistance tools to assist with tests, debugging, format research and documentation.  
All AI generated content is reviewed, tested and approved by developers before actually getting implemented.

# Binarius
Binarius is a tool for editing Black Box Need for Speed games binary files, powered by [Darius](https://github.com/nlgxzef/Darius "Darius") and [Endscript](https://github.com/nlgxzef/Endscript "Endscript").

Supported games: Need for Speed Underground, Underground 2, Most Wanted, Carbon, ProStreet and Undercover.

## Features
- UI to edit collection data of databases built using Nikki & Endscript libraries.
- Ability to use various commands, create and restore backups, import and export collections.
- Endscript support: special .end files that are imported to edit databases.
- Easy to use; simple API
- Built-in tools, such as:
  - NFS-Hasher
  - NFS-Raider
  - Swatch Picker
  - Texture Editor
  - String Editor
  - CarPart Editor
  - Career Editor
- User configuration settings that can be changed and applied any time

## Documentation
The [Endscript documentation](https://github.com/nlgxzef/Endscript/blob/master/DOCUMENTATION.md) lists every
endscript command with its format, examples and the version that added it. It also covers how Binarius reads
`version` lines, strings and INI files.

## Building Binarius

Binarius is built from several repositories. They are not linked as Git submodules, so clone each one yourself,
side by side in the same folder.

### Requirements
- Windows
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- Visual Studio 2022 (optional; you can also build from the command line)

### 1. Clone the repositories

Clone all of them into the same parent folder, keeping their default folder names:

```
git clone https://github.com/nlgxzef/Binarius.git
git clone https://github.com/nlgxzef/Darius.git
git clone https://github.com/nlgxzef/Endscript.git
git clone https://github.com/nlgxzef/CoreExtensions.git
git clone https://github.com/nlgxzef/ILWrapper.git
```

The folder should then look like this:

```
<your folder>
├── Binarius
├── CoreExtensions
├── Darius
├── Endscript
└── ILWrapper
```

The projects find each other by these relative paths, so don't rename or move the folders.

### 2. Build

Open `Binarius\Binary.sln` in Visual Studio and build the **Release** configuration, or run this from the
`Binarius` folder:

```
dotnet build Binary.sln -c Release
```

The program is built to `Binarius\Binary\bin\Release\net6.0-windows\`.

Binarius is a 32-bit (x86) .NET 6 application. Keep it that way: the texture compressor
(`LZCompressLib.dll`, in the Darius repository) is a 32-bit native library.

### Updating

Pull every repository, not just Binarius, because a Binarius change often needs matching changes in the others:

```
git -C Binarius pull
git -C Darius pull
git -C Endscript pull
git -C CoreExtensions pull
git -C ILWrapper pull
```

### Building only a library

Darius (`Nikki.sln`) and Endscript (`Endscript.sln`) can be built on their own too. They need the same folder layout:
Darius needs CoreExtensions next to it, and Endscript needs Darius and CoreExtensions.
