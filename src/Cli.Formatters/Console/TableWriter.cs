// <copyright file="TableWriter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotNetLicensesReporter.Console;

internal sealed class TableWriter : ITableWriter
{
    public static TableWriterBuilder Builder => new();

    public TableBorders Borders { get; set; } = TableBorders.Simple;

    public Task Print<T>(
        TextWriter writer,
        IEnumerable<string> headers,
        IEnumerable<T> rows,
        Func<T, IEnumerable<object?>> formatter,
        Func<string, int?>? columnMaxWidth = null)
    {
        var computedHeaderRow = headers.ToArray();
        var computedRows = rows.Select(o => formatter(o).Select(o => o?.ToString()).ToArray()).ToList();
        var lengths = ComputeLengths(computedHeaderRow, computedRows);

        Borders.WriteUpperBorder(writer, lengths);
        WriteRow(computedHeaderRow);

        Borders.WriteHeaderSeparator(writer, lengths);
        writer.WriteLine();

        foreach (var row in computedRows)
            WriteRow(row);

        Borders.WriteLowerBorder(writer, lengths);
        return Task.CompletedTask;

        int[] ComputeLengths(string?[] headerRow, List<string?[]> rows)
        {
            var lengths = new int[headerRow.Length];
            for (var i = 0; i < headerRow.Length; ++i)
            {
                lengths[i] = int.Max(lengths[i], headerRow[i]?.Length ?? 0);
            }

            foreach (var row in rows)
            {
                for (var i = 0; i < row.Length; ++i)
                {
                    lengths[i] = int.Max(lengths[i], row[i]?.Length ?? 0);
                }
            }

            for (var i = 0; i < headerRow.Length; ++i)
            {
                var max = columnMaxWidth?.Invoke(headerRow[i]!);
                if (max is { } maxLength)
                    lengths[i] = int.Min(lengths[i], maxLength);
            }

            return [.. lengths];
        }

        void WriteRow(string?[] row)
        {
            Borders.WriteStartColumn(writer);
            WriteRowCell(row[0] ?? string.Empty, lengths[0]);
            for (var i = 1; i < computedHeaderRow.Length; ++i)
            {
                Borders.WriteColumnSeparator(writer);
                WriteRowCell(row[i] ?? string.Empty, lengths[i]);
            }

            Borders.WriteEndColumn(writer);
            writer.WriteLine();
        }

        void WriteRowCell(string text, int length)
        {
            if (text.Length > length) text = string.Concat(text[..(length - 1)], "…");
            writer.Write(text);
            WritePadding(writer, length, text);
        }
    }

    private static void WritePadding(TextWriter writer, int length, string text) =>
        writer.Write(new string(' ', length - text.Length));
}
