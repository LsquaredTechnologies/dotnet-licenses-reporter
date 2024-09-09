// <copyright file="ExportTemplateCommand.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Help;

using Lsquared.DotNetLicensesReporter.Customizations;
using Lsquared.DotNetLicensesReporter.Templating;

using Spectre.Console;

namespace Lsquared.DotNetLicensesReporter.Commands;

internal sealed partial class ExportTemplateCommand : Command, ICustomHelpProvider
{
    public Argument<string?> TemplateNameArgument { get; } = new(
        name: "template-name",
        description: "The name of the template to export.")
    {
        Arity = ArgumentArity.ExactlyOne,
    };

    public Option<FileInfo> FileNameOption { get; } = new(
        ["--filename", "-f"],
        description: Strings.ExportTemplateCommand.FileNameOptionDescription)
    {
        Arity = ArgumentArity.ZeroOrOne,
        ArgumentHelpName = "filename",
    };

    public ExportTemplateCommand() : base("export", Strings.ExportTemplateCommand.Description)
    {
        TreatUnmatchedTokensAsErrors = true;

        Add(TemplateNameArgument.FromAmong(Templates.Defaults.Select(o => o.ShortName).ToArray()));
        Add(FileNameOption);
    }

    public IEnumerable<Action<HelpContext>> CustomHelpLayout() =>
        CustomHelpBuilder.DefaultHelp(this);
}
