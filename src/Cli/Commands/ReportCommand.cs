// <copyright file="ReportCommand.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

using Lsquared.DotnetTools.LicensesReporter.Customizations;
using Lsquared.DotnetTools.LicensesReporter.Formatters;

namespace Lsquared.DotnetTools.LicensesReporter.Commands;

internal sealed partial class ReportCommand : Command, ICustomHelpProvider
{
    public Argument<FileSystemInfo> ProjectArgument { get; } = new(
        name: "project",
        description: Strings.ReportCommand.ProjectArgumentDescription)
    {
        Arity = ArgumentArity.ZeroOrOne,
        HelpName = Strings.ReportCommand.ProjectArgumentHelpName,
    };

    public Option<bool> NoRestoreOption { get; } = new(
        aliases: ["--no-restore"],
        description: Strings.ReportCommand.NoRestoreOptionDescription,
        getDefaultValue: () => false)
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<bool> IncludeTransitiveOption { get; } = new(
        aliases: ["--include-transitive"],
        description: Strings.ReportCommand.IncludeTransitiveOptionDescription,
        getDefaultValue: () => false)
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<bool> UniquePackageOption { get; } = new(
        aliases: ["--unique-package", "--unique", "-u"],
        description: Strings.ReportCommand.UniquePackageOptionDescription,
        getDefaultValue: () => false)
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<DirectoryInfo?> OutputDirectoryOption { get; } = new(
        aliases: ["--output", "-o"],
        description: Strings.ReportCommand.OutputDirectoryOptionDescription)
    {
        Arity = ArgumentArity.ZeroOrOne,
        ArgumentHelpName = Strings.ReportCommand.OutputDirectoryOptionHelpName,
    };

    public Option<List<string>> OutputFormatsOption { get; } = new(
        aliases: ["--output-format", "-of"],
        description: Strings.ReportCommand.OutputFormatsOptionDescription,
        parseArgument: (ArgumentResult result) => SplitValues(result.Tokens))
    {
        Arity = ArgumentArity.ZeroOrMore,
        ArgumentHelpName = Strings.ReportCommand.OutputFormatsOptionHelpName,
    };

    public Option<bool> SilentOption { get; } = new(
        aliases: ["--silent"],
        description: Strings.ReportCommand.SilentOptionDescription)
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<FileInfo?> TemplateOption { get; } = new(
        aliases: ["--template", "-t"],
        description: Strings.ReportCommand.TemplateOptionDescription)
    {
        Arity = ArgumentArity.ZeroOrOne,
        ArgumentHelpName = Strings.ReportCommand.TemplateOptionHelpName,
    };

    public Option<bool> OpenFileOption { get; } = new(
        aliases: ["--open"],
        description: Strings.ReportCommand.OpenFileOptionDescription)
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public ReportCommand() : base("report", Strings.ReportCommand.Description)
    {
        TreatUnmatchedTokensAsErrors = true;
        IsHidden = true;

        Add(ProjectArgument.ExistingOnly());
        Add(NoRestoreOption);
        Add(IncludeTransitiveOption);
        Add(UniquePackageOption);
        Add(OutputDirectoryOption!.ExistingOnly());
        var outputFormats = OutputFormats.Get();
        Add(OutputFormatsOption.AddCompletions([.. outputFormats]));
        Add(SilentOption);
        Add(TemplateOption);
        Add(OpenFileOption);

        OutputFormatsOption.SetDefaultValue(OutputFormats.Defaults);
    }

    public IEnumerable<Action<HelpContext>> CustomHelpLayout() =>
        CustomHelpBuilder.DefaultHelp(this);

    private static List<string> SplitValues(IReadOnlyList<Token> tokens) =>
        tokens.SelectMany(o => o.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).ToList();
}
