// <copyright file="DotLiquidFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.Globalization;

using DotLiquid;

using Lsquared.DotnetTools.LicensesReporter.Collectors;

using Microsoft.Extensions.Logging;

using NuGet.Versioning;

namespace Lsquared.DotnetTools.LicensesReporter.Formatters.Abstracts;

internal abstract partial class DotLiquidFormatter(
    ILogger<DotLiquidFormatter> logger,
    string name,
    FileInfo templateFile,
    Func<TextReader> templateReaderFactory) : IFileOutputFormatter
{
    public string Name { get; } = name;

    static DotLiquidFormatter()
    {
        Template.RegisterSafeType(typeof(PackageLicense), ["*"]);
        Template.RegisterSafeType(typeof(Uri), o => o.ToString());
        Template.RegisterSafeType(typeof(NuGetVersion), o => o.ToString());
    }

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

    public async Task Write(TextWriter writer, IReadOnlyList<PackageLicense> packages)
    {
        try
        {
            using var templateReader = templateReaderFactory();
            var templateContents = await templateReader.ReadToEndAsync();
            var template = Template.Parse(templateContents);
            template.Render(writer, new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new { packages }),
            });
            await writer.FlushAsync();
        }
        catch (Exception ex)
        {
            LogTemplateParseError(logger, ex.Message, templateFile.Name, ex);
        }
    }

    [LoggerMessage(LogLevel.Error, "An error occurs when compiling the template '{TemplateFileName}':\n {Error}")]
    private static partial void LogTemplateParseError(ILogger logger, string error, string templateFileName, Exception exception);

}
