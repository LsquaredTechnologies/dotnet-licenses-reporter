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

    public IEnumerable<Action<HelpContext>> CustomHelpLayout() =>
        CustomHelpBuilder.DefaultHelp(this);
}
