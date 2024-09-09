// <copyright file="ExportTemplateCommand.Handler.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;

using Lsquared.DotnetLicensesReporter.Customizations;
using Lsquared.DotnetLicensesReporter.Templating;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;

namespace Lsquared.DotnetLicensesReporter.Commands;

internal sealed partial class ExportTemplateCommand : Command
{
    internal sealed class CommandHandler : ICommandHandler
    {
        public int Invoke(InvocationContext context) =>
            throw new NotImplementedException();

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var command = (ExportTemplateCommand)context.ParseResult.CommandResult.Command;
            return Handle(
                context.GetHost().Services.GetRequiredService<IConsole>(),
                context.ParseResult.GetValueForArgument(command.TemplateNameArgument),
                context.ParseResult.GetValueForOption(command.FileNameOption),
                context.GetCancellationToken());
        }

        private static async Task<int> Handle(
            IConsole console,
            string? templateName,
            FileInfo? file,
            CancellationToken cancellationToken)
        {
            file?.Directory?.Create();

            var template = Templates.Defaults.FirstOrDefault(o => o.ShortName == templateName);
            if (template is null)
            {
                console.WriteLine("No template named '{templateName}' found!\nRun 'dotnet licenses templates list' to get full list of registered templates.");
                return -1;
            }

            using var inputStream = template.GetStream();
            using var outputStream = file is null ? System.Console.OpenStandardOutput() : file.OpenWrite();
            await inputStream.CopyToAsync(outputStream, 1024, cancellationToken);
            return 0;
        }
    }
}
