// <copyright file="SpectreAnsiConsoleWrapper.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.IO;

using Spectre.Console;

namespace Lsquared.DotNetLicensesReporter;

internal sealed class SpectreAnsiConsoleWrapper : IConsole
{
    public IStandardStreamWriter Out { get; }

    public bool IsOutputRedirected { get; }

    public IStandardStreamWriter Error { get; }

    public bool IsErrorRedirected { get; }

    public bool IsInputRedirected { get; }

    internal IAnsiConsole Console { get; } = AnsiConsole.Console;

    public SpectreAnsiConsoleWrapper()
    {
        Out = new Writer(Console);
        Error = new Writer(Console);
    }

    private sealed class Writer(IAnsiConsole console) : IStandardStreamWriter
    {
        public void Write(string? value) =>
            console.Markup(value ?? string.Empty);
    }
}
