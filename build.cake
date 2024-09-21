
var solutionPath = "./RichillCapital.Binance.sln";
var projectPath = "./RichillCapital.Binance.csproj";

var configuration = Argument("configuration", "Release");
var artifactsDirectory = "./artifacts";

Task("Restore")
    .Does(() =>
    {
        DotNetRestore(solutionPath);
    });

Task("Build")
    .Does(() =>
    {
        DotNetBuild(solutionPath, new DotNetBuildSettings
        {
            Configuration = configuration,
            NoRestore = true,
        });
    });

Task("UnitTests")
    .Does(() =>
    {
        var testProjects = GetFiles("./Tests/**/*.UnitTests.csproj");

        foreach (var project in testProjects)
        {
            DotNetTest(project.FullPath, new DotNetTestSettings
            {
                Configuration = configuration,
                NoBuild = true,
                NoRestore = true,
            });
        }
    });

Task("Pack")
    .Does(() =>
    {
        DotNetPack(projectPath, new DotNetPackSettings
        {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true,
            OutputDirectory = artifactsDirectory,
        });
    });

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTests")
    .IsDependentOn("Pack");

var target = Argument("target", "Default");

RunTarget(target);
