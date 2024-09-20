// <copyright file="LockFileFactory.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using NuGet.ProjectModel;

namespace Lsquared.DotnetTools.LicensesReporter.Collectors;

internal sealed class LockFileFactory : ILockFileFactory
{
    public LockFile GetFromFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Project assets file was not found for project {path}.\nPlease execute 'dotnet restore' before executing.");

        return Format.Read(path);
    }

    private static readonly LockFileFormat Format = new();
}
