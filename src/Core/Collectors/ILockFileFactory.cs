// <copyright file="ILockFileFactory.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using NuGet.ProjectModel;

namespace Lsquared.DotnetLicensesReporter.Collectors;

public interface ILockFileFactory
{
    LockFile GetFromFile(string path);
}
