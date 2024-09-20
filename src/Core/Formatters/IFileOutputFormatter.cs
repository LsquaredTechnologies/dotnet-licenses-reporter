// <copyright file="IFileOutputFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetTools.LicensesReporter.Formatters;

public interface IFileOutputFormatter : IOutputFormatter
{
    FileInfo CreateFile(DirectoryInfo directory);
}
