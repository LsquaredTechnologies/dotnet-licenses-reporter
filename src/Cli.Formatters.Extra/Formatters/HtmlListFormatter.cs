// <copyright file="HtmlListFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Formatters.Abstracts;

using Microsoft.Extensions.Logging;

namespace Lsquared.DotnetTools.LicensesReporter.Formatters;

internal sealed partial class HtmlListFormatter(ILogger<HtmlListFormatter> logger, Func<Stream> getTemplateStream)
    : DotLiquidFormatter(logger, "html-list", new("licenses.html"), () => new StreamReader(getTemplateStream()))
{
}
