// <copyright file="PackageCollector.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.Text.RegularExpressions;

using Microsoft.Build.Evaluation;

using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.ProjectModel;

namespace Lsquared.DotnetTools.LicensesReporter.Collectors;

public sealed class PackageCollector(ILockFileFactory lockFileFactory)
{
    public IEnumerable<Package> Collect(IEnumerable<FileInfo> projectFiles, CollectionOptions options)
    {
        var packages = CollectCore(projectFiles, options);
        if (options.Unique)
            packages = packages.DistinctBy(o => $"{o.Id}/{o.Version}");

        return packages;
    }

    private IEnumerable<Package> CollectCore(IEnumerable<FileInfo> projectFiles, CollectionOptions options)
    {
        ProjectCollection buildEngine = new();
        foreach (var projectFile in projectFiles)
        {
            var project = buildEngine.LoadProject(projectFile.FullName);
            var projectAssetsFile = new FileInfo(project.GetProjectAssetsFile() ?? string.Empty);
            if (!projectAssetsFile.Exists)
            {
                if (project.IsNetSdkProject())
                    throw new InvalidOperationException($"{projectAssetsFile.Name} not found. Please run 'dotnet restore'");

                throw new NotSupportedException("Old .Net Framework projet formats are not supported");
            }

            var assetsFile = lockFileFactory.GetFromFile(projectAssetsFile.FullName);
            var targetFramework = project.GetTargetFramework();
            if (targetFramework == NuGetFramework.UnsupportedFramework)
                continue;

            var runtimeIdentifier = project.GetRuntimeIdentifier();
            var target = assetsFile.Targets
                .Single(o => o.TargetFramework == targetFramework && (o.RuntimeIdentifier ?? string.Empty) == runtimeIdentifier);

            var libraryPackages = target.Libraries.Where(o => o.IsPackage());
            if (!options.IncludeTransitive)
            {
                var tfm = GetTargetFrameworkInformation(assetsFile, target);
                var directlyReferencedPackages = project.GetPackageReferencesFromProjectForFramework(tfm.FrameworkName, assetsFile);
                libraryPackages = libraryPackages.Where(o => IsDirectlyReferenced(o, directlyReferencedPackages));
            }

            if (options.ExcludedPackages.Count is > 0)
                libraryPackages = libraryPackages.Where(o => options.ExcludedPackages.TrueForAll(pattern => Regex.IsMatch(o.Name!, pattern)));

            foreach (var libraryPackage in libraryPackages)
            {
                // TODO Check if library pacakge is an analyzer! How?
                var isAnalyzer = false;
                yield return new Package(libraryPackage.Name!, libraryPackage.Version, isAnalyzer);
            }
        }
    }

    private static bool IsDirectlyReferenced(LockFileTargetLibrary library, IEnumerable<PackageReference> directlyReferencedPackages) =>
        directlyReferencedPackages.Any(o =>
            string.Equals(library.Name, o.PackageIdentity.Id, StringComparison.OrdinalIgnoreCase) &&
            (o.PackageIdentity.Version is null || library.Version!.Equals(o.PackageIdentity.Version)));

    private static TargetFrameworkInformation GetTargetFrameworkInformation(LockFile assetsFile, LockFileTarget target)
    {
        try
        {
            return assetsFile.PackageSpec.TargetFrameworks.First(
                o => o.FrameworkName.Equals(target.TargetFramework));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to identify the target framework information for {target}!",
                ex);
        }
    }
}
