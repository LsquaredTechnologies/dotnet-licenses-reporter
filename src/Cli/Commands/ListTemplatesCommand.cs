// <copyright file="ListTemplatesCommand.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

using Lsquared.DotNetLicensesReporter.Customizations;

namespace Lsquared.DotNetLicensesReporter.Commands;

internal sealed partial class ListTemplatesCommand : Command, ICustomHelpProvider
{
    public Argument<string?> TemplateNameArgument { get; } = new(
        name: "template-name",
        description: "If specified, only the templates matching the name will be shown.")
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<List<string>> TagsOption { get; } = new(
        aliases: ["--tag", "-t"],
        description: "Filters the templates based on the tag.",
        parseArgument: (ArgumentResult result) => SplitValues(result.Tokens))
    {
        Arity = ArgumentArity.ZeroOrMore,
    };

    public Option<bool> ShowAllColumnsOption { get; } = new(
        aliases: ["--columns-all"],
        description: "Displays all columns in the output.")
    {
        Arity = ArgumentArity.ZeroOrOne,
    };

    public Option<List<string>> ColumnsOption { get; } = new(
        aliases: ["--columns"],
        description: "Specifies the columns to display in the output.",
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
