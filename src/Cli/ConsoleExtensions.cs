// <copyright file="ConsoleExtensions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;

using Spectre.Console;

namespace Lsquared.DotnetTools.LicensesReporter;

internal static class ConsoleExtensions
{
    public static IAnsiConsole AsAnsiConsole(this IConsole console) =>
        ((SpectreAnsiConsoleWrapper)console).Console;
}
