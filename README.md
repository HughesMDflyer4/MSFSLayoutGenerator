# MSFS Layout Generator

A drag-and-drop application for updating layout.json files in Microsoft Flight Simulator (2020) packages.

## Q&A
### Q. What is a layout.json?
A. The Virtual File System (VFS) in Microsoft Flight Simulator relies on a layout.json file in the root of each package to determine which files it is allowed to load, and to perform a basic file integrity check.

### Q. Who is this application for?
A. This tool will benefit anyone who creates packages by hand rather than using the Project Editor in Developer Mode. Livery artists in particular may find this useful, as adding each texture to layout.json manually can be tedious.

## Usage
1. Download the [latest release](https://github.com/HughesMDflyer4/MSFSLayoutGenerator/releases/latest) from this repository and place the executable in a convenient location on your PC.
2. Drag any file named "layout.json" onto the exe.
3. Launch Microsoft Flight Simulator and check out your changes.

### Notes
* The application will only write to files named "layout.json" to avoid accidentally modifying unrelated files.
* The application will update any number of layout.json files when providing the full path of each file as an argument. This can be useful when updating content across multiple packages. An example is below:
```
MSFSLayoutGenerator.exe "G:\MSFS\Community\example-package-1\layout.json" "G:\MSFS\Community\example-package-2\layout.json"
```

## Requirements
* Windows 10 (x64)
* .NET Framework 4.7.2
