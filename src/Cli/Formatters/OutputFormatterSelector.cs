// <copyright file="OutputFormatterSelector.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Formatters;

internal sealed class OutputFormatterSelector(
    IEnumerable<IOutputFormatter> registeredFormatters,
    IEnumerable<IOutputFormatterFactory> registeredFormatterFactories) : IOutputFormatterSelector
{
    public IEnumerable<IOutputFormatter> SelectFormatters(OutputOptions options)
    {
        var formattersByName =
            Enumerable.Union(
                registeredFormatters,
                registeredFormatterFactories.Select(o => o.Create(options))).ToDictionary(o => o.Name);

        return options.Formats.Select(o => formattersByName[o]);
    }
}
