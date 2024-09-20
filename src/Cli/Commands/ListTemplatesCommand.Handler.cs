// <copyright file="ListTemplatesCommand.Handler.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.CommandLine;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;

using Lsquared.DotnetTools.LicensesReporter.Templating;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Lsquared.DotnetTools.LicensesReporter.Commands;

internal sealed partial class ListTemplatesCommand : Command
{
    internal sealed class CommandHandler : ICommandHandler
    {
        public int Invoke(InvocationContext context) =>
            throw new NotImplementedException();

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var command = (ListTemplatesCommand)context.ParseResult.CommandResult.Command;
            return Handle(
                context.GetHost().Services.GetRequiredService<IConsole>(),
                context.ParseResult.GetValueForArgument(command.TemplateNameArgument),
                context.ParseResult.GetValueForOption(command.TagsOption) ?? [],
                context.ParseResult.GetValueForOption(command.ShowAllColumnsOption),
                context.ParseResult.GetValueForOption(command.ColumnsOption) ?? []);
        }

        private static Task<int> Handle(
            IConsole console,
            string? templateName,
            List<string> tags,
            bool showAllColumns,
            List<string> columns)
        {
            ICollection<(string Header, Func<Template, IRenderable> Row, bool IsOptional)> visibleColumns = showAllColumns
                ? Columns.Values
                : Enumerable.Union<KeyValuePair<string, (string Header, Func<Template, IRenderable> Row, bool IsOptional)>>(
                    Columns.Where(o => !o.Value.IsOptional),
                    Columns.Where(o => columns.Contains(o.Key))).Select(o => o.Value).ToList();

            var table = new Table().NoBorder();

            foreach ((var header, _, _) in visibleColumns)
                _ = table.AddColumn(header, (o) => o.PadRight(2).NoWrap());

            IRenderable[] separatorLine = visibleColumns.Select(o => new Text(new string('—', o.Header.Length * 3)).Crop()).ToArray();
            _ = table.AddRow(separatorLine);

            IEnumerable<Template> templates = Templates.Defaults;
            if (!string.IsNullOrWhiteSpace(templateName))
                templates = templates.Where(o => o.ShortName.Contains(templateName));

            if (tags.Count > 0)
                templates = templates.Where(o => o.Tags.Intersect(tags).Any());

            foreach (var template in templates)
            {
                var values = visibleColumns.Select(o => o.Row(template)).ToArray();
                _ = table.AddRow(values);
            }

            console.AsAnsiConsole().Write(table);
            return Task.FromResult(0);
        }
    }

    private static readonly Dictionary<string, (string Header, Func<Template, IRenderable> Row, bool IsOptional)> Columns = new()
    {
        ["templateName"] = ("Template name", (Template o) => new Text(o.TemplateName), false),
        ["shortName"] = ("Short name", (Template o) => new Text(o.ShortName), false),
        ["author"] = ("Author", (Template o) => new Text(o.Author), true),
        ["tags"] = ("Tags", (Template o) => new Text(string.Join(',', o.Tags)), true),
    };
}
