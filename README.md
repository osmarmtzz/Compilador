Restore NuGet Packages Before Building
Before building the project for the first time, you need to restore the required NuGet packages to avoid errors such as NETSDK1004. To do this:
Open Visual Studio.
Go to Tools > NuGet Package Manager > Package Manager Console.
In the console at the bottom, run the following command:
dotnet restore
