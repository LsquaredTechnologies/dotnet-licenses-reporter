// <copyright file="CustomHelpBuilder.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Help;

namespace Lsquared.DotnetLicensesReporter.Customizations;

/// <summary>
/// Represents a custom help builder.
/// </summary>
internal sealed class CustomHelpBuilder : HelpBuilder
{
    public static IEnumerable<Action<HelpContext>> DefaultHelp(Command command) => [
        (HelpContext context) => context.WriteUsage(command),
        (HelpContext context) => context.WriteDescription(command),
        (HelpContext context) => context.WriteArguments(command),
        (HelpContext context) => context.WriteOptions(command),
        (HelpContext context) => context.WriteCommands(command),
    ];

    /// <summary>
    /// The singleton lazy instance.
    /// </summary>
    public static readonly Lazy<HelpBuilder> Instance = new(() =>
    {
        int windowWidth;
        try
        {
            windowWidth = System.Console.WindowWidth;
        }
        catch
        {
            windowWidth = int.MaxValue;
        }

        return new CustomHelpBuilder(windowWidth);
    });

    private CustomHelpBuilder(int maxWidth = int.MaxValue)
        : base(LocalizationResources.Instance, maxWidth)
    {
    }

    /// <inheritdoc/>
    public override void Write(HelpContext context)
    {
        var command = context.Command;
        if (command is ICustomHelpProvider helpProvider)
        {
            var blocks = helpProvider.CustomHelpLayout();
            foreach (var block in blocks)
                block(context);
        }
        else
        {
            base.Write(context);
        }
    }
}
