// <copyright file="YamlFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Collectors;

using Microsoft.Extensions.Logging;

using NuGet.Versioning;

using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Lsquared.DotnetTools.LicensesReporter.Formatters;

internal sealed partial class YamlFormatter(ILogger<YamlFormatter> logger) : IFileOutputFormatter
{
    public string Name => "yaml";

    public FileInfo CreateFile(DirectoryInfo directory)
    {
        directory.Create();
        return new(Path.Combine(directory.FullName, "licenses.yaml"));
    }

    public async Task Write(Stream stream, IReadOnlyList<PackageLicense> packages)
    {
        using StreamWriter writer = new(stream, leaveOpen: true);
        Serializer.Serialize(writer, packages);
        await writer.FlushAsync();
    }

    private static readonly ISerializer Serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .WithTypeConverter(new NuGetVersionYamlConverter())
        .WithTypeConverter(new UriYamlConverter())
        .Build();

    private sealed class NuGetVersionYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) =>
            type == typeof(NuGetVersion);

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer) =>
            throw new NotSupportedException();

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer) =>
            serializer(value?.ToString());
    }

    private sealed class UriYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) =>
            type == typeof(Uri);

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer) =>
            throw new NotSupportedException();

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer) =>
            serializer(value?.ToString());
    }
}
