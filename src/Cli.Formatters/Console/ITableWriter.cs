// <copyright file="ITableWriter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Console;

internal interface ITableWriter
{
    Task Print<T>(
        TextWriter writer,
        IEnumerable<string> headers,
        IEnumerable<T> rows,
        Func<T, IEnumerable<object?>> formatter,
        Func<string, int?>? columnMaxWidth = null);
}
