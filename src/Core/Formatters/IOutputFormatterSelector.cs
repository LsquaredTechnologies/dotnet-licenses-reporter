// <copyright file="IOutputFormatterSelector.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Formatters;

public interface IOutputFormatterSelector
{
    IEnumerable<IOutputFormatter> SelectFormatters(OutputOptions options);
}
