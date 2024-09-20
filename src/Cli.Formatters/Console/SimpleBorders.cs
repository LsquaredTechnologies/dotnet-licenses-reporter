// <copyright file="SimpleBorders.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetTools.LicensesReporter.Console;

internal sealed class SimpleBorders : TableBorders
{
    public override void WriteLowerBorder(TextWriter writer, int[] lengths)
    {
        writer.Write('└');
        for (var length = 0; length < lengths.Length - 1; ++length)
        {
            for (var len = 0; len < length; ++len)
                writer.Write('─');
            writer.Write('┴');
        }

        writer.Write('┘');
    }

    public override void WriteColumnSeparator(TextWriter writer) =>
        writer.Write(" │ ");

    public override void WriteEndColumn(TextWriter writer) =>
        writer.Write(" │");

    public override void WriteStartColumn(TextWriter writer) =>
        writer.Write("│ ");

    public override void WriteUpperBorder(TextWriter writer, int[] lengths)
    {
        writer.Write('┌');
        for (var length = 0; length < lengths.Length - 1; ++length)
        {
            for (var len = 0; len < length; ++len)
                writer.Write('─');
            writer.Write('┬');
        }

        writer.Write('┐');
    }
}
