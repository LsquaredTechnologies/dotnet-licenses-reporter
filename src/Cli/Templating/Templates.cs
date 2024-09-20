// <copyright file="Templates.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetTools.LicensesReporter.Templating;

internal static class Templates
{
    public static readonly Template HtmlList = new(
        "HTML List",
        "html-list",
        "Lsquared",
        ["html", "list"],
        () => typeof(Templates).Assembly!.GetManifestResourceStream("Lsquared.DotnetTools.LicensesReporter.templates.html-list.html.tmpl")!);

    public static readonly Template HtmlTable = new(
        "HTML Table",
        "html-table",
        "Lsquared",
        ["html", "table"],
        () => typeof(Templates).Assembly!.GetManifestResourceStream("Lsquared.DotnetTools.LicensesReporter.templates.html-table.html.tmpl")!);

    public static List<Template> Defaults { get; } = [HtmlList, HtmlTable];
}
