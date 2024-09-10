// <copyright file="HelpContextExtensions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Help;
using System.Text;

namespace Lsquared.DotnetLicensesReporter.Customizations;

internal static class HelpContextExtensions
{
    public static void WriteDescription(this HelpContext context, Command command)
    {
        context.Output.WriteLine(command.Description);
        context.Output.WriteLine();
    }

    public static void WriteUsage(this HelpContext context, Command command)
    {
        context.Output.Write("\x1b[1m");
        context.Output.Write("Usage: ");
        context.Output.Write("\x1b[0m");
        context.Output.Write("\x1b[36m");
        context.Output.Write("dotnet");

        var commands = GetParents(command);
        foreach (var parent in commands.Reverse())
        {
            context.Output.Write(" ");
            context.Output.Write(parent.Name);
        }

        context.Output.Write(" ");
        context.Output.Write(command.Name);

        context.Output.Write(" \x1b[1m");
        if (command.Subcommands.Count > 0)
        {
            context.Output.Write("[COMMAND]");

            if (command.Subcommands.Sum(o => o.Options.Count) > 0)
                context.Output.Write(" [COMMAND_OPTIONS]");

            if (command.Arguments.Count > 0)
                context.Output.Write(" [ARGS]");
        }
        else
        {
            if (command.Arguments.Count > 0)
                context.WriteArguments(command.Arguments);

            if (command.Options.Count > 0)
                context.Output.Write("[OPTIONS]");
        }

        context.Output.Write("\x1b[0m ");
        context.Output.WriteLine();
        context.Output.WriteLine();
    }

    public static void WriteArguments(this HelpContext context, Command command)
    {
        if (command.Arguments.Count is 0)
            return;

        context.Output.Write("\x1b[1m");
        context.Output.Write("Arguments:");
        context.Output.WriteLine("\x1b[0m");
        var twoColumnRows = command.Arguments.Where(a => !a.IsHidden).Select(a => context.HelpBuilder.GetTwoColumnRow(a, context)).ToList();
        context.HelpBuilder.WriteColumns(twoColumnRows, context);
        context.Output.WriteLine();
    }

    public static void WriteArguments(this HelpContext context, IEnumerable<Argument> arguments)
    {
        foreach (var argument in arguments)
        {
            WriteArgument(context, argument);
            context.Output.Write(' ');
        }
    }

    public static void WriteArgument(this HelpContext context, Argument argument)
    {
        if (argument.IsHidden) return;
        if (argument.Arity.MinimumNumberOfValues is 0)
            context.Output.Write('[');

        context.Output.Write('<');
        context.Output.Write(argument.Name);
        context.Output.Write('>');

        if (argument.Arity.MaximumNumberOfValues is > 1)
            context.Output.Write("...");

        if (argument.Arity.MinimumNumberOfValues is 0)
            context.Output.Write(']');
    }

    public static void WriteCommands(this HelpContext context, Command command)
    {
        if (command.Subcommands.Count is 0)
            return;

        context.Output.WriteLine();
        context.Output.Write("\x1b[1m");
        context.Output.Write("Commands:");
        context.Output.WriteLine("\x1b[0m");
        var twoColumnRows = command.Subcommands.Where(c => !c.IsHidden).Select(c => context.HelpBuilder.GetTwoColumnRow(c, context)).ToList();
        context.HelpBuilder.WriteColumns(twoColumnRows, context);
        context.Output.WriteLine();
        context.Output.WriteLine("Run 'dotnet licenses [COMMAND] --help' for more information on a command.");
    }

    public static void WriteOptions(this HelpContext context, Command command)
    {
        if (command.Options.Count is 0)
            return;

        context.Output.Write("\x1b[1m");
        context.Output.WriteLine("Options:");
        context.Output.Write("\x1b[0m");
        var twoColumnRows = command.Options.Where(o => !o.IsHidden).Select(o => context.HelpBuilder.GetTwoColumnRow(o, context)).ToList();
        context.HelpBuilder.WriteColumns(twoColumnRows, context);
    }

    public static void WriteLine(this HelpContext context, string? value = null) =>
        context.Output.WriteLine(value);

    public static void WriteLine(this HelpContext context, StringBuilder value) =>
        context.Output.WriteLine(value);

    private static IEnumerable<Symbol> GetParents(Command command)
    {
        var symbol = command.Parents.FirstOrDefault();
        while (symbol is not null)
        {
            yield return symbol;
            symbol = symbol.Parents.FirstOrDefault();
        }
    }
}
