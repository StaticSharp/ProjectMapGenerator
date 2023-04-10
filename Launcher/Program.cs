using StaticSharpProjectMapGenerator;

public class Program
{
    public static async Task Main()
    {
        var targetProjectPath = Path.Combine(ProjectDirectory.Path, "..\\TestProject\\StaticSharpProjectMapGenerator.TestProject.csproj");
        //var targetProjectPath = Path.Combine(ProjectDirectory.Path, "..\\..\\StaticSharp\\StaticSharp\\StaticSharp.csproj");
        var outputPath = Path.Combine(Path.GetDirectoryName(targetProjectPath), 
            $".generated/{typeof(StaticSharpProjectMapGenerator.StaticSharpProjectMapGenerator).FullName}");
        await RoslynSourceGeneratorLauncher.RoslynSourceGeneratorLauncher.Launch(
            new StaticSharpProjectMapGenerator.StaticSharpProjectMapGenerator(), targetProjectPath, outputPath);
    }
}