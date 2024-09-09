// <copyright file="IOutputFormatterFactory.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotNetLicensesReporter.Formatters;

public interface IOutputFormatterFactory
{
    IOutputFormatter Create(OutputOptions options);
}
