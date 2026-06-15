using Fallout.Common;
using Fallout.Common.Git;
using Fallout.Common.IO;
using Fallout.Solutions;

partial class Build : FalloutBuild
{
    readonly AbsolutePath OutputDirectory = RootDirectory / "output";
    readonly AbsolutePath SourceDirectory = RootDirectory / "source";

    readonly string[] CompiledAssemblies = { "Renamer.dll"};

    [GitRepository]
    [Required]
    readonly GitRepository GitRepository;

    [Solution(GenerateProjects = true)]
    Solution Solution;

    public static int Main() => Execute<Build>(x => x.Clean);

}
