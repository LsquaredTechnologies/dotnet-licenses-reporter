// <copyright file="RootCommand.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine.Help;

using Lsquared.DotnetLicensesReporter.Customizations;

namespace Lsquared.DotnetLicensesReporter.Commands;

internal sealed class RootCommand : System.CommandLine.RootCommand, ICustomHelpProvider
{
    public ReportCommand ReportCommand { get; } = [];

    public TemplatesCommand TemplatesCommand { get; } = [];

    public RootCommand()
    {
        TreatUnmatchedTokensAsErrors = true;
        Name = "licenses";
        Add(ReportCommand);
        Add(TemplatesCommand);
    }

    public IEnumerable<Action<HelpContext>> CustomHelpLayout() => [
        (HelpContext context) => {
            context.Output.WriteLine("\x1b[1mUsage: \x1b[0m");
            context.Output.WriteLine("\x1b[36mdotnet licenses\x1b[1m [<PROJECT | SOLUTION>] [OPTIONS]\x1b[0m");
            context.Output.WriteLine("\x1b[36mdotnet licenses\x1b[1m [COMMAND] [COMMAND_OPTIONS]\x1b[0m");
            context.Output.WriteLine();
        },
        (HelpContext context) => context.WriteDescription(ReportCommand),
        (HelpContext context) => context.WriteArguments(ReportCommand),
        (HelpContext context) => context.WriteOptions(ReportCommand),
        (HelpContext context) => {
            context.Output.WriteLine("  --version                       Show version information");
            context.Output.WriteLine("  -?, -h, --help                  Show help and usage information");
        },
        (HelpContext context) => context.WriteCommands(ReportCommand),
        (HelpContext context) => context.WriteCommands(this),
    ];
}
