// <copyright file="AnsiConsoleServiceCollectionExtensions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;

using Lsquared.DotnetTools.LicensesReporter;

namespace Microsoft.Extensions.DependencyInjection;

internal static class AnsiConsoleServiceCollectionExtensions
{
    public static IServiceCollection AddAnsiConsole(this IServiceCollection services) => services
        .AddSingleton<SpectreAnsiConsoleWrapper>()
        .AddSingleton<IConsole>((sp) => sp.GetRequiredService<SpectreAnsiConsoleWrapper>())
        .AddSingleton((sp) => sp.GetRequiredService<SpectreAnsiConsoleWrapper>().Console);
}
