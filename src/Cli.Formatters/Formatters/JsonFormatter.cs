// <copyright file="JsonFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.Text.Json;
using System.Text.Json.Serialization;

using Lsquared.DotnetLicensesReporter.Collectors;
using Lsquared.DotnetLicensesReporter.Formatters;

using Microsoft.Extensions.Logging;

using NuGet.Versioning;

namespace Lsquared.DotnetLicensesReporter;

internal sealed partial class JsonFormatter(ILogger<JsonFormatter> logger, bool prettyPrint) : IFileOutputFormatter
{
    public string Name => prettyPrint ? "json-pretty" : "json";

    public FileInfo CreateFile(DirectoryInfo directory)
    {
        directory.Create();
        return new(Path.Combine(directory.FullName, "licenses.json"));
    }

    public async Task Write(Stream stream, IReadOnlyList<PackageLicense> packages)
    {
        await JsonSerializer.SerializeAsync(stream, packages, _options);
        await stream.FlushAsync();
    }

    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { NugetVersionJsonConverter.Instance },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = prettyPrint,
    };

    private sealed class NugetVersionJsonConverter : JsonConverter<NuGetVersion>
    {
        public static readonly NugetVersionJsonConverter Instance = new();

        public override NuGetVersion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, NuGetVersion value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }
}
