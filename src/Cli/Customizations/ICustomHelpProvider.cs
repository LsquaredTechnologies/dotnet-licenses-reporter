// <copyright file="ICustomHelpProvider.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine.Help;

namespace Lsquared.DotnetTools.LicensesReporter.Customizations;

internal interface ICustomHelpProvider
{
    IEnumerable<Action<HelpContext>> CustomHelpLayout();
}
