// <copyright file="Program.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Text;

using Lsquared.DotNetLicensesReporter;
using Lsquared.DotNetLicensesReporter.Collectors;
using Lsquared.DotNetLicensesReporter.Commands;
using Lsquared.DotNetLicensesReporter.Console;
using Lsquared.DotNetLicensesReporter.Customizations;
using Lsquared.DotNetLicensesReporter.Formatters;
using Lsquared.DotNetLicensesReporter.Templating;

using Microsoft.Build.Locator;
using Microsoft.Extensions.DependencyInjection;

MSBuildLocator.RegisterDefaults();
Console.InputEncoding = Console.OutputEncoding = new UTF8Encoding();

var root = new Lsquared.DotNetLicensesReporter.Commands.RootCommand();
var parser = new CommandLineBuilder(root)
    .UseDefaults()
    .UseHelpBuilder((_) => CustomHelpBuilder.Instance.Value)
    ////.UseHelpBuilder((_) => CustomHelpBuilder.Instance.Value)
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
        root.ReportCommand.OutputFormatsOption.AddCompletions(OutputFormats.Get().ToArray());
        root.ReportCommand.OutputFormatsOption.SetDefaultValue(OutputFormats.Defaults);
        await next(context);
    })
    .Build();

return await parser.Parse(args).InvokeAsync();
