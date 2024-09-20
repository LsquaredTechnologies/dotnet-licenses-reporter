// <copyright file="HtmlTableFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Formatters.Abstracts;

using Microsoft.Extensions.Logging;

namespace Lsquared.DotnetTools.LicensesReporter.Formatters;

internal sealed partial class HtmlTableFormatter(ILogger<HtmlListFormatter> logger, Func<Stream> getTemplateStream)
    : DotLiquidFormatter(logger, "html-table", new("licenses.html"), () => new StreamReader(getTemplateStream()))
{
}
