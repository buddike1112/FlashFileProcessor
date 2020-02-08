FlashFileProcessor
==================

This is a simple POC to check the performance provided by .NET Core 3.0, towards small to large files.

Technologies used : 
-------------------

Topshelf for service deployment and debugging purposes, Microsoft Dependency Injection framework to inject dependencies, -
Microsoft Hosting abstractions to host the service as a hosted service. Visual Studio 2019 Community edition for solution development.

Basic Functionality:
--------------------

1. Read options from appsettings.json using IOption pattern 2.
2. Check whether specific file type available in a configured folder.
3. If available check each and every field with given rule.
4. Seperate successfully validated items and failed items.
5. Write successful items into seperate file in different destination folder.
6. Write failed items into seperate file in different rejection folder. 
7.Provide one or more reasons for the failure of each record, reasons were configurable

Version History:
----------------

Version 1.0 - Initial code base.
Version 1.1 - Restructure the project, added documentation using GhostDoc and updated the README file.
            - Integrated Microsoft Logging framework to the solution.
