using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using WindowsDesktop.Properties;
using WindowsDesktop.Utils;

namespace WindowsDesktop.Interop;

internal class ComInterfaceAssemblyBuilder
{
    private const string _assemblyName = "VirtualDesktopCore.{0}.generated.dll";
    private const string _placeholderOsBuild = "{OS_BUILD}";
    private const string _placeholderAssemblyVersion = "{ASSEMBLY_VERSION}";
    private const string _placeholderInterfaceId = "00000000-0000-0000-0000-000000000000";

    // Now using assembly version even though regenerating our DLL won't strictly be needed for every new version this is the safest option
    // Otherwise people will surely forget to increment a specific version here
    private static readonly Version? _requireVersion = Assembly.GetExecutingAssembly().GetName().Version;
    private static readonly Regex _assemblyRegex = new(@"VirtualDesktopCore\.10\.0\.(?<build>\d+\.\d+)(\.\w*|)\.dll");
    private static readonly Regex _buildNumberRegex = new(@"\.Build(?<build>\d+\.\d+)\.");
    private static readonly Version osBuild = OS.Build;
    
    private static ComInterfaceAssembly? _assembly;

    private readonly VirtualDesktopCompilerConfiguration _configuration;

    public ComInterfaceAssemblyBuilder(VirtualDesktopCompilerConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public ComInterfaceAssembly GetAssembly()
        => _assembly ??= new ComInterfaceAssembly(this.LoadExistingAssembly() ?? this.CreateAssembly());

    private Assembly? LoadExistingAssembly()
    {
        if (this._configuration.CompiledAssemblySaveDirectory.Exists)
        {
            foreach (var file in this._configuration.CompiledAssemblySaveDirectory.GetFiles())
            {
                if (Version.TryParse(OS.VersionPrefix + _assemblyRegex.Match(file.Name).Groups["build"].ToString(), out var build)
                    && build == osBuild)
                {
                    try
                    {
                        var name = AssemblyName.GetAssemblyName(file.FullName);
                        if (name.Version >= _requireVersion)
                        {
                            Debug.WriteLine($"Assembly found: {file.FullName}");
#if !DEBUG
                            return Assembly.LoadFile(file.FullName);
#else
                            Debug.WriteLine($"Debug force assembly creation");
#endif
                        }
                        else 
                        {
                            Debug.WriteLine($"Outdated assembly: {name.Version} < {_requireVersion}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to load assembly: ");
                        Debug.WriteLine(ex);

                        File.Delete(file.FullName);
                    }
                }
            }
        }

        return null;
    }

    private Assembly CreateAssembly()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var compileTargets = new List<string>();
        {
            var assemblyInfo = executingAssembly.GetManifestResourceNames().Single(x => x.Contains("AssemblyInfo.cs"));
            var stream = executingAssembly.GetManifestResourceStream(assemblyInfo);
            if (stream != null)
            {
                using var reader = new StreamReader(stream, Encoding.UTF8);
                var sourceCode = reader
                    .ReadToEnd()
                    .Replace(_placeholderOsBuild, osBuild.ToString())
                    .Replace(_placeholderAssemblyVersion, _requireVersion.ToString(3));
                compileTargets.Add(sourceCode);
            }
        }

        var interfaceNames = executingAssembly
            .GetTypes()
            .Select(x => x.GetComInterfaceNameIfWrapper())
            .Where(x => string.IsNullOrEmpty(x) == false)
            .Cast<string>()
            .ToArray();
        var iids = IID.GetIIDs(interfaceNames);

        // e.g.
        //   IVirtualDesktop
        //           ├── 10240, VirtualDesktopCore.Interop.Build10240..interfaces.IVirtualDesktop.cs
        //           └── 22000, VirtualDesktopCore.Interop.Build22000..interfaces.IVirtualDesktop.cs
        //   IVirtualDesktopPinnedApps
        //           └── 10240, VirtualDesktopCore.Interop.Build10240..interfaces.IVirtualDesktopPinnedApps.cs
        var interfaceSourceFiles = new Dictionary<string, SortedList<Version, string>>();

        // This is where we decide which interface variant goes into our generated DLL assembly
        foreach (var name in executingAssembly.GetManifestResourceNames())
        {
            var interfaceName = Path.GetFileNameWithoutExtension(name).Split('.').LastOrDefault();
            if (interfaceName != null
                && interfaceNames.Contains(interfaceName)
                && Version.TryParse(OS.VersionPrefix + _buildNumberRegex.Match(name.Replace('_','.')).Groups["build"].ToString(), out var build))
            {
                if (interfaceSourceFiles.TryGetValue(interfaceName, out var sourceFiles) == false)
                {
                    sourceFiles = new SortedList<Version, string>();
                    interfaceSourceFiles.Add(interfaceName, sourceFiles);
                }

                sourceFiles.Add(build, name);
            }
        }

        foreach (var (interfaceName, sourceFiles) in interfaceSourceFiles)
        {
            var resourceName = sourceFiles.Aggregate("", (current, kvp) =>
            {
                var (build, resourceName) = kvp;
                return build <= osBuild ? resourceName : current;
            });

            var stream = executingAssembly.GetManifestResourceStream(resourceName);
            if (stream == null) continue;

            using var reader = new StreamReader(stream, Encoding.UTF8);
            var sourceCode = reader.ReadToEnd().Replace(_placeholderInterfaceId, iids[interfaceName].ToString());
            compileTargets.Add(sourceCode);
        }

        return this.Compile(compileTargets.ToArray());
    }

    private Assembly Compile(IEnumerable<string> sources)
    {
        try
        {
            var name = string.Format(_assemblyName, osBuild);
            var syntaxTrees = sources.Select(x => SyntaxFactory.ParseSyntaxTree(x));
            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Concat(new[] { Assembly.GetExecutingAssembly(), })
                .Where(x => x.IsDynamic == false)
                .Select(x => MetadataReference.CreateFromFile(x.Location));
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(name)
                .WithOptions(options)
                .WithReferences(references)
                .AddSyntaxTrees(syntaxTrees);

            string? errorMessage;

            if (this._configuration.SaveCompiledAssembly)
            {
                var dir = this._configuration.CompiledAssemblySaveDirectory;
                if (dir.Exists == false) dir.Create();

                var path = Path.Combine(dir.FullName, name);
                var result = compilation.Emit(path);
                if (result.Success) return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);

                File.Delete(path);
                errorMessage = string.Join(Environment.NewLine, result.Diagnostics.Select(x => $"  {x.GetMessage()}"));
            }
            else
            {
                using var stream = new MemoryStream();
                var result = compilation.Emit(stream);
                if (result.Success)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    return AssemblyLoadContext.Default.LoadFromStream(stream);
                }

                errorMessage = string.Join(Environment.NewLine, result.Diagnostics.Select(x => $"  {x.GetMessage()}"));
            }

            throw new Exception("Failed to compile COM interfaces assembly." + Environment.NewLine + errorMessage);
        }
        finally
        {
            GC.Collect();
        }
    }
}
