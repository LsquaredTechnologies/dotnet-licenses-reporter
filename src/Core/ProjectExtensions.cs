// <copyright file="ProjectExtensions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Microsoft.Build.Evaluation;

using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;

namespace Lsquared.DotnetLicensesReporter;

public static class ProjectExtensions
{
    public static string? GetProjectAssetsFile(this Project project) =>
        project.GetPropertyValue("ProjectAssetsFile");

    public static string? GetRuntimeIdentifier(this Project project) =>
        project.GetPropertyValue("RuntimeIdentifier");

    public static NuGetFramework GetTargetFramework(this Project project) =>
        NuGetFramework.Parse(project.GetPropertyValue("TargetFramework"), DefaultFrameworkNameProvider.Instance);

    public static bool IsNetSdkProject(this Project project) =>
        string.Equals(project.GetPropertyValue("UsingMicrosoftNETSdk"), bool.TrueString, StringComparison.OrdinalIgnoreCase);

    public static IEnumerable<PackageReference> GetPackageReferencesFromProjectForFramework(this Project project, NuGetFramework framework, LockFile assetsFile)
    {
        foreach (var item in project.GetItems("PackageReference"))
        {
            var packageName = item.EvaluatedInclude;
            var packageVersion = item.GetMetadataValue("Version");
            var packageEvaluatedVersion = assetsFile.Libraries.FirstOrDefault(o => string.Equals(o.Name, packageName, StringComparison.OrdinalIgnoreCase))?.Version;
            yield return new PackageReference(
                new PackageIdentity(item.EvaluatedInclude, packageEvaluatedVersion),
                framework);
        }
    }
}
