// <copyright file="FluidFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.Globalization;
using System.Text.Encodings.Web;

using Fluid;

using Lsquared.DotNetLicensesReporter.Collectors;

using Microsoft.Extensions.Logging;

namespace Lsquared.DotNetLicensesReporter.Formatters.Abstracts;

internal abstract partial class FluidFormatter(
    ILogger<FluidFormatter> logger,
    string name,
    FileInfo templateFile,
    Func<TextReader> templateReaderFactory) : IFileOutputFormatter
{
    public string Name { get; } = name;

    public FileInfo CreateFile(DirectoryInfo directory)
    {
        directory.Create();
        var extension = templateFile.Extension;
        if (extension is ".tpl" or ".tmpl") extension = Path.GetExtension(Path.GetFileNameWithoutExtension(templateFile.Name))!;
        return new(Path.Combine(directory.FullName, Path.ChangeExtension("licenses.???", extension)));
    }

    public async Task Write(Stream stream, IReadOnlyList<PackageLicense> packages)
    {
        using StreamWriter writer = new(stream);
        await Write(writer, packages);
    }

    internal async Task Write(TextWriter writer, IReadOnlyList<PackageLicense> packages)
    {
        using var templateReader = templateReaderFactory();
        var templateContents = await templateReader.ReadToEndAsync();
        if (!Parser.TryParse(templateContents, out var compiledTemplate, out var error))
        {
            LogTemplateParseError(logger, error, templateFile.Name);
            return;
        }

        TemplateOptions options = new() { CultureInfo = CultureInfo.CurrentCulture, MemberAccessStrategy = Strategy, };
        TemplateContext context = new(new { packages }, options, allowModelMembers: true);
        await compiledTemplate.RenderAsync(writer, HtmlEncoder.Default, context);
        await writer.FlushAsync();
    }

    [LoggerMessage(LogLevel.Error, "An error occurs when compiling the template '{TemplateFileName}':\n {Error}")]
    private static partial void LogTemplateParseError(ILogger logger, string error, string templateFileName);

    private static readonly MemberAccessStrategy Strategy = new UnsafeMemberAccessStrategy()
    {
        IgnoreCasing = true,
    };
    private static readonly FluidParser Parser = new(new FluidParserOptions() { AllowFunctions = true, });
}
