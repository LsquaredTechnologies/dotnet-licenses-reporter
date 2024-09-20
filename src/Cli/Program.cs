// <copyright file="Program.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Text;

using Lsquared.DotnetTools.LicensesReporter;
using Lsquared.DotnetTools.LicensesReporter.Collectors;
using Lsquared.DotnetTools.LicensesReporter.Commands;
using Lsquared.DotnetTools.LicensesReporter.Console;
using Lsquared.DotnetTools.LicensesReporter.Customizations;
using Lsquared.DotnetTools.LicensesReporter.Formatters;
using Lsquared.DotnetTools.LicensesReporter.Templating;

using Microsoft.Build.Locator;
using Microsoft.Extensions.DependencyInjection;

MSBuildLocator.RegisterDefaults();
Console.InputEncoding = Console.OutputEncoding = new UTF8Encoding();

var root = new RootCommand();
var parser = new CommandLineBuilder(root)
    .UseDefaults()
    .UseHelpBuilder((_) => CustomHelpBuilder.Instance.Value)
    .UseHost((host) => host
        .ConfigureServices((_, services) => services
            .AddAnsiConsole()
            .AddScoped<PackageCollector>()
            .AddScoped<ILockFileFactory, LockFileFactory>()
            .AddScoped<IOutputFormatterSelector, OutputFormatterSelector>()
            .AddScoped<IOutputFormatter, ConsoleTableFormatter>()
            .AddScoped<IOutputFormatter>((sp) => ActivatorUtilities.CreateInstance<JsonFormatter>(sp, true))
            .AddScoped<IOutputFormatter>((sp) => ActivatorUtilities.CreateInstance<JsonFormatter>(sp, false))
            .AddScoped<IOutputFormatter>((sp) => ActivatorUtilities.CreateInstance<HtmlListFormatter>(sp, Templates.HtmlList.GetStream))
            .AddScoped<IOutputFormatter>((sp) => ActivatorUtilities.CreateInstance<HtmlTableFormatter>(sp, Templates.HtmlTable.GetStream))
            .AddScoped<IOutputFormatter, MarkdownListFormatter>()
            .AddScoped<IOutputFormatter, MarkdownTableFormatter>()
            .AddScoped<IOutputFormatter, YamlFormatter>()
            .AddScoped<IOutputFormatterFactory, TemplateFormatter.Factory>()
            .AddScoped<ITableWriter, TableWriter>())
        .UseCommandHandler<ReportCommand, ReportCommand.CommandHandler>()
        .UseCommandHandler<ExportTemplateCommand, ExportTemplateCommand.CommandHandler>()
        .UseCommandHandler<ListTemplatesCommand, ListTemplatesCommand.CommandHandler>())
    .AddMiddleware(async (context, next) =>
    {
        _ = root.ReportCommand.OutputFormatsOption.AddCompletions([.. OutputFormats.Get()]);
        root.ReportCommand.OutputFormatsOption.SetDefaultValue(OutputFormats.Defaults);
        await next(context);
    })
    .Build();

return await parser.Parse(args).InvokeAsync();
