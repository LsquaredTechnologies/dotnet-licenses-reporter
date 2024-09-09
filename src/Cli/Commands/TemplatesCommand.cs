// <copyright file="TemplatesCommand.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Help;

using Lsquared.DotNetLicensesReporter.Customizations;

namespace Lsquared.DotNetLicensesReporter.Commands;

internal sealed class TemplatesCommand : Command, ICustomHelpProvider
{
    public ExportTemplateCommand ExportTemplateCommand { get; } = [];

    public ListTemplatesCommand ListTemplatesCommand { get; } = [];

    public TemplatesCommand() : base("templates", Strings.TemplatesCommand.Description)
    {
        TreatUnmatchedTokensAsErrors = true;
        Add(ExportTemplateCommand);
        Add(ListTemplatesCommand);
    }

    public IEnumerable<Action<HelpContext>> CustomHelpLayout() =>
        CustomHelpBuilder.DefaultHelp(this);
}
