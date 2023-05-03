using ProjectMapSg;

public class Program
{
    public static async Task Main()
    {
        var targetProjectPath = Path.Combine(ProjectDirectory.Path, "..\\TestProject\\ProjectMapSg.TestProject.csproj");
        //var targetProjectPath = Path.Combine(ProjectDirectory.Path, "..\\..\\StaticSharp\\StaticSharpDemo\\StaticSharpDemo.csproj");
        var outputPath = Path.Combine(Path.GetDirectoryName(targetProjectPath), 
            $".generated/{typeof(ProjectMapSg.ProjectMapSg).FullName}");
        await RoslynSourceGeneratorLauncher.RoslynSourceGeneratorLauncher.Launch(
            new ProjectMapSg.ProjectMapSg(), targetProjectPath, outputPath);
    }
}