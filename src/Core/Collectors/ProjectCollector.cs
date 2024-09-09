// <copyright file="ProjectCollector.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.Text.RegularExpressions;

using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;

namespace Lsquared.DotnetLicensesReporter.Collectors;

public static class ProjectCollector
{
    public static IEnumerable<FileInfo> Collect(CollectionOptions options)
    {
        var projectFile = options.ProjectFile;
        if (projectFile.Extension is not ".sln")
        {
            // Only one project to collect...
            if (options.ShouldRestore) Restore(projectFile.FullName);
            return [projectFile];
        }

        // One solution with potentially multiple projects to collect
        var solution = SolutionFile.Parse(projectFile.FullName);
        if (options.ShouldRestore) Restore(projectFile.FullName);

        return from project in solution.ProjectsInOrder
               where project.ProjectType is SolutionProjectType.KnownToBeMSBuildFormat
               where options.ExcludedProjects.Count is 0 || options.ExcludedProjects.TrueForAll(o => Regex.IsMatch(project.ProjectName, o))
               select new FileInfo(project.AbsolutePath);
    }

    private static void Restore(string path)
    {
        Dictionary<string, string> globalProperties = new()
        {
            ["Configuration"] = "Debug",
            ["Platform"] = "Any CPU",
            ////["OutputPath"] = "bin/Debug",
            ["EnableNuGetPackageRestore"] = "true",
        };

        // TODO Log restore information
        ProjectCollection buildEngine = new();
        BuildParameters buildParams = new(buildEngine);
        BuildRequestData build = new(path, globalProperties, null, ["Restore"], null);
        var buildResult = BuildManager.DefaultBuildManager.Build(buildParams, build);
        // TODO Log restore result
        _ = buildResult;
    }
}
