// <copyright file="Licenses.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace Lsquared.DotnetTools.LicensesReporter.Core;

public static partial class Licenses
{
    public static string? Map(Uri licenseUrl)
    {
        if (DefaultUrlsMapping.TryGetValue(licenseUrl.ToString(), out var license))
            return license;

        if (CustomUrlsMapping.TryGetValue(licenseUrl.ToString(), out license))
            return license;

        var path = licenseUrl.GetLeftPart(UriPartial.Path);
        var extension = Path.GetExtension(path);
        return extension is ".htm" or ".html" or ".txt" ? Path.GetFileNameWithoutExtension(path) : Path.GetFileName(path);
    }

    public static string? Map(string? licenseUrl)
    {
        if (string.IsNullOrWhiteSpace(licenseUrl))
            return null;

        if (DefaultUrlsMapping.TryGetValue(licenseUrl.ToString(), out var license))
            return license;

        if (CustomUrlsMapping.TryGetValue(licenseUrl.ToString(), out license))
            return license;

        UriBuilder b = new(licenseUrl);
        return Map(b.Uri);
    }

    [return: NotNullIfNotNull(nameof(license))]
    public static string? GetCustomName(string? license)
    {
        if (string.IsNullOrWhiteSpace(license))
            return null;

        if (DefaultNamesMapping.TryGetValue(license, out var result))
            return result;

        if (CustomNamesMapping.TryGetValue(license, out result))
            return result;

        return license;
    }

    private static readonly Dictionary<string, string> CustomNamesMapping = new(StringComparer.OrdinalIgnoreCase)
    {
        ["LICENSE-1.0"] = APACHE_1_0,
        ["LICENSE-1.1"] = APACHE_1_1,
        ["LICENSE-2.0"] = APACHE_2_0,
    };
    private static readonly Dictionary<string, string> CustomUrlsMapping = new(StringComparer.OrdinalIgnoreCase);
}
