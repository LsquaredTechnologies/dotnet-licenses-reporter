// <copyright file="TableWriterBuilder.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotNetLicensesReporter.Console;

internal sealed class TableWriterBuilder
{
    public TableWriterBuilder WithBorders(TableBorders borders)
    {
        _initializers.Add((o) => o.Borders = borders);
        return this;
    }

    public TableWriter Build()
    {
        TableWriter tableWriter = new();
        foreach (var initializer in _initializers)
            initializer(tableWriter);
        return tableWriter;
    }

    private readonly List<Action<TableWriter>> _initializers = [];
}
