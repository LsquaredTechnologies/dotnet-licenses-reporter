// <copyright file="LockFileTargetLibraryExtensions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using NuGet.ProjectModel;

namespace Lsquared.DotnetTools.LicensesReporter;

public static class LockFileTargetLibraryExtensions
{
    public static bool IsPackage(this LockFileTargetLibrary library) =>
        string.Equals(library.Type, "package", StringComparison.OrdinalIgnoreCase);
}
