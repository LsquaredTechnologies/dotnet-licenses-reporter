// <copyright file="IOutputFormatterSelector.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetTools.LicensesReporter.Formatters;

public interface IOutputFormatterSelector
{
    IEnumerable<IOutputFormatter> SelectFormatters(OutputOptions options);
}
