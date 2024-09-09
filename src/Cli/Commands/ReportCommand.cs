// <copyright file="ReportCommand.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Parsing;

using Lsquared.DotNetLicensesReporter.Formatters;

namespace Lsquared.DotNetLicensesReporter.Commands;

internal sealed partial class ReportCommand : Command
{
    public Argument<FileSystemInfo> ProjectArgument { get; } = new(
        name: "project",
        description: "The project or solution to report licenses from.",
        getDefaultValue: () => new DirectoryInfo(Environment.CurrentDirectory))
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<DirectoryInfo?> OutputDirectoryOption { get; } = new(
        aliases: ["--output", "-o"],
        description: "The output directory write files to.")
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<List<string>> OutputFormatsOption { get; } = new(
        aliases: ["--output-format", "-f"],
        description: "The output formats to use to display package licenses.",
        parseArgument: (ArgumentResult result) => SplitValues(result.Tokens))
    {
        Arity = ArgumentArity.ZeroOrMore,
    };

    public Option<bool> SilentOption { get; } = new(
        aliases: ["--silent"],
        description: ".")
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<FileInfo?> TemplateOption { get; } = new(
        aliases: ["--template", "-t"],
        description: "A liquid template file to use to display package licenses when output-formats contain 'template'.")
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<bool> OpenFileOption { get; } = new(
        aliases: ["--open"],
        description: ".")
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public ReportCommand() : base("report", Strings.ReportCommand.Description)
    {
        TreatUnmatchedTokensAsErrors = true;
        IsHidden = true;

        Add(ProjectArgument.ExistingOnly());
        Add(OutputDirectoryOption!.ExistingOnly());
        var outputFormats = OutputFormats.Get();
        Add(OutputFormatsOption.AddCompletions([.. outputFormats]));
        Add(SilentOption);
        Add(TemplateOption);
        Add(OpenFileOption);

        OutputFormatsOption.SetDefaultValue(OutputFormats.Defaults);
    }

    private static List<string> SplitValues(IReadOnlyList<Token> tokens) =>
        tokens.SelectMany(o => o.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).ToList();
}
