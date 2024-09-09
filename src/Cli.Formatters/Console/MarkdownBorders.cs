// <copyright file="MarkdownBorders.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Console;

internal sealed class MarkdownBorders : TableBorders
{
    public override void WriteColumnSeparator(TextWriter writer) =>
        writer.Write(" | ");

    public override void WriteEndColumn(TextWriter writer) =>
        writer.Write(" |");

    public override void WriteHeaderSeparator(TextWriter writer, int[] lengths)
    {
        writer.Write("| ");
        for (var i = 0; i < lengths.Length; ++i)
        {
            if (i > 0)
                writer.Write(" | ");

            for (var j = 0; j < lengths[i]; ++j)
                writer.Write('-');
        }

        writer.Write(" |");
    }

    public override void WriteStartColumn(TextWriter writer) =>
        writer.Write("| ");
}
