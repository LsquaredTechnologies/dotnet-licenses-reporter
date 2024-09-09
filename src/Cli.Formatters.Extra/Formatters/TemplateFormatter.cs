// <copyright file="TemplateFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetLicensesReporter.Formatters.Abstracts;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lsquared.DotnetLicensesReporter.Formatters;

internal sealed partial class TemplateFormatter(ILogger<TemplateFormatter> logger, FileInfo templateFile)
    : DotLiquidFormatter(logger, "template", templateFile, () => new StreamReader(templateFile.OpenRead(), leaveOpen: true))
{
    public sealed class Factory(IServiceProvider services) : IOutputFormatterFactory
    {
        public IOutputFormatter Create(OutputOptions options)
        {
            var templateFile = options.TemplateFile;
            return templateFile is null
                ? NullFormatter.Instance
                : new TemplateFormatter(
                    logger: services.GetRequiredService<ILogger<TemplateFormatter>>(),
                    templateFile: templateFile);
        }
    }
}
