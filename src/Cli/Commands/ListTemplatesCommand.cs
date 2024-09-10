// <copyright file="ListTemplatesCommand.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

using Lsquared.DotnetLicensesReporter.Customizations;

namespace Lsquared.DotnetLicensesReporter.Commands;

internal sealed partial class ListTemplatesCommand : Command, ICustomHelpProvider
{
    public Argument<string?> TemplateNameArgument { get; } = new(
        name: "template-name",
        description: Strings.ListTemplatesCommand.TemplateArgumentDescription)
    {
        Arity = ArgumentArity.ZeroOrOne,
        Name = Strings.ListTemplatesCommand.TemplateArgumentHelpName,

    };

    public Option<List<string>> TagsOption { get; } = new(
        aliases: ["--tag", "-t"],
        description: Strings.ListTemplatesCommand.TagsOptionDescription,
        parseArgument: (ArgumentResult result) => SplitValues(result.Tokens))
    {
        Arity = ArgumentArity.ZeroOrMore,
    };

    public Option<bool> ShowAllColumnsOption { get; } = new(
        aliases: ["--columns-all"],
        description: Strings.ListTemplatesCommand.ShowAllColumnsOptionDescription)
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<List<string>> ColumnsOption { get; } = new(
        aliases: ["--columns"],
        description: Strings.ListTemplatesCommand.ColumnsOptionDescription,
        parseArgument: (ArgumentResult result) => SplitValues(result.Tokens))
    {
        AllowMultipleArgumentsPerToken = true,
        Arity = ArgumentArity.ZeroOrMore,
    };

    public ListTemplatesCommand() : base("list", Strings.ListTemplatesCommand.Description)
    {
        TreatUnmatchedTokensAsErrors = true;

        Add(TemplateNameArgument);
        Add(TagsOption);
        Add(ShowAllColumnsOption);
        Add(ColumnsOption.AddCompletions("author", "tags"));

        AddValidator((result) =>
        {
            var hasColumns = result.FindResultFor(ColumnsOption) is not null;
            var hasColumnsAll = result.FindResultFor(ShowAllColumnsOption) is not null;
            if (hasColumns && hasColumnsAll)
                result.ErrorMessage = "Options '--columns-all' and '--two' are mutually exclusive.";
        });
    }

    public IEnumerable<Action<HelpContext>> CustomHelpLayout() =>
        CustomHelpBuilder.DefaultHelp(this);

    private static List<string> SplitValues(IReadOnlyList<Token> tokens) =>
        tokens.SelectMany(o => o.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).ToList();
}
