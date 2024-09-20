// <copyright file="TableBorders.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetTools.LicensesReporter.Console;

internal abstract class TableBorders
{
    public static readonly TableBorders None = new NoBorders();

    public static readonly TableBorders Simple = new SimpleBorders();

    public static readonly TableBorders Markdown = new MarkdownBorders();

    public virtual void WriteLowerBorder(TextWriter writer, int[] lengths)
    {
        // noop
    }

    public virtual void WriteColumnSeparator(TextWriter writer) =>
        writer.Write("  ");

    public virtual void WriteEndColumn(TextWriter writer)
    {
        // noop
    }

    public virtual void WriteHeaderSeparator(TextWriter writer, int[] lengths)
    {
        for (var i = 0; i < lengths.Length; ++i)
        {
            for (var j = 0; j < lengths[i]; ++j)
                writer.Write('─');
            writer.Write("  ");
        }
    }

    public virtual void WriteStartColumn(TextWriter writer)
    {
        // noop
    }

    public virtual void WriteUpperBorder(TextWriter writer, int[] lengths)
    {
        // noop
    }
}
