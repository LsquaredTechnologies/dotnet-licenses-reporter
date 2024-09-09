// <copyright file="IFileOutputFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotNetLicensesReporter.Formatters;

public interface IFileOutputFormatter : IOutputFormatter
{
    FileInfo CreateFile(DirectoryInfo directory);
}
