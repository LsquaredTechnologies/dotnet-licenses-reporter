// <copyright file="ReportCommand.Handler.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Diagnostics;

using Lsquared.DotnetTools.LicensesReporter.Collectors;
using Lsquared.DotnetTools.LicensesReporter.Formatters;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;

namespace Lsquared.DotnetTools.LicensesReporter.Commands;

internal sealed partial class ReportCommand : Command
{
    internal sealed class CommandHandler : ICommandHandler
    {
        public int Invoke(InvocationContext context) =>
            throw new NotImplementedException();

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var command = (ReportCommand)context.ParseResult.CommandResult.Command;
            var currentDir = new DirectoryInfo(Environment.CurrentDirectory);
            return Handle(
                context.GetHost().Services.GetRequiredService<IConsole>(),
                context.ParseResult.GetValueForArgument(command.ProjectArgument) ?? currentDir,
                false,
                true,
                true,
                context.ParseResult.GetValueForOption(command.OutputDirectoryOption) ?? currentDir,
                context.ParseResult.GetValueForOption(command.OutputFormatsOption) ?? [.. OutputFormats.Defaults],
                context.ParseResult.GetValueForOption(command.SilentOption),
                context.ParseResult.GetValueForOption(command.TemplateOption),
                context.ParseResult.GetValueForOption(command.OpenFileOption),
                context.GetHost().Services.GetRequiredService<PackageCollector>(),
                context.GetHost().Services.GetRequiredService<IOutputFormatterSelector>());
        }

        private static async Task<int> Handle(
            IConsole console,
            FileSystemInfo projectFile,
            bool noRestore,
            bool includeTransitive,
            bool unique,
            DirectoryInfo? outputDirectory,
            List<string> outputFormats,
            bool silent,
            FileInfo? templateFile,
            bool openFile,
            PackageCollector packageCollector,
            IOutputFormatterSelector formatterSelector)
        {
            if (projectFile is DirectoryInfo dir)
            {
                (var error, var file) = SearchForProject(dir);
                if (error is not null)
                {
                    console.Error.WriteLine(error);
                    return -1;
                }

                projectFile = file!;
            }

            CollectionOptions collectOptions = new()
            {
                ProjectFile = (FileInfo)projectFile,
                ShouldRestore = !noRestore,
                IncludeTransitive = includeTransitive,
                Unique = unique,
                ExcludedProjects = { @"^(?!.*Test)" },
                ExcludedPackages = { @"^(?!System\.)", @"^(?!Microsoft.Extensions\.)" },
            };

            OutputOptions outputOptions = new()
            {
                Directory = outputDirectory,
                Formats = outputFormats,
                Silent = silent,
                TemplateFile = templateFile,
                OpenFile = openFile,
            };

            var ansi = console.AsAnsiConsole();
            var packages = ansi
                   .Status()
                   .Spinner(Spinner.Known.Default)
                   .SpinnerStyle(Style.Parse("bold"))
                   .Start(
                        "Project analysis...",
                        (StatusContext context) =>
                        {
                            var s = Stopwatch.StartNew();

                            ansi.WriteLine("Collecting projects...");
                            var projects = ProjectCollector.Collect(collectOptions).ToList();
                            ansi.Cursor.SetPosition(0, 0);
                            ansi.Cursor.MoveUp();
                            ansi.WriteLine($"\x1b[32m{projects.Count}\x1b[0m projects collected.");

                            ansi.WriteLine("Collecting packages...");
                            var dependencies = packageCollector.Collect(projects, collectOptions).ToList();
                            ansi.Cursor.MoveUp();
                            ansi.WriteLine($"\x1b[32m{dependencies.Count}\x1b[0m {(collectOptions.Unique ? "unique " : string.Empty)}packages collected.");

                            ansi.WriteLine("Collecting licenses...");
                            var packages = PackageLicenseCollector.Collect(dependencies, collectOptions).ToList();
                            ansi.Cursor.MoveUp();
                            ansi.WriteLine("Licenses collected.");

                            ansi.WriteLine($"Collection runs for {s.Elapsed.TotalSeconds:0.000}s");
                            return packages;
                        });

            var formatters = formatterSelector.SelectFormatters(outputOptions).ToList();
            await formatters.Render(packages, outputOptions);
            return 0;
        }

        private static (string? Error, FileInfo? File) SearchForProject(DirectoryInfo dir)
        {
            var candidateFiles = Patterns.SelectMany(dir.EnumerateFiles).ToList();
            return candidateFiles.Count switch
            {
                1 => (null, candidateFiles[0]),
                0 => ("No project nor solution found!", null),
                _ => ("Multiple projects or solutions found! Please use '--project' to specify the project or solution to process.", null),
            };
        }

        private static readonly string[] Patterns = ["*.sln", "*.*proj"];
    }
}
