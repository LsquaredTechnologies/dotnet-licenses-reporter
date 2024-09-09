// <copyright file="IOutputFormatterFactory.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Formatters;

public interface IOutputFormatterFactory
{
    IOutputFormatter Create(OutputOptions options);
}
