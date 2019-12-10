FlashFileProcessor
This is a simple POC to check the performance provided by .NET Core 3.0.

Technologies used : Topshelf for service deployment and debugging purposes, Microsoft Dependency Injection framework to inject dependencies, Microsoft Hosting abstractions to host the service as a hosted service. Visual Studio 2019 Community edition

Version 1.0 (master) - This version will do below activities

1.Read options from appsettings.json using IOption pattern 2.Check whether specific file type available in a configured folder 3.If available check each and every field with given rule 4.Seperate successfully validated items and failed items 5.Write successful items into seperate file in different destination folder 6.Write failed items into seperate file in different rejection folder 7.Provide one or more reasons for the failure of each record, reasons were configurable

Future improvements: 
1.Integrate dummy Web API's and clients to handle some imaginary calls so validation process depends on Rules plus API call results 2.Create technical documentation using open source designing tools.

Version 1.1 - Restructure the project, added documentation using GhostDoc and updated the README file
