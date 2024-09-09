// <copyright file="PackageLicense.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using NuGet.Packaging;
using NuGet.Versioning;

namespace Lsquared.DotnetLicensesReporter.Collectors;

public sealed record class PackageLicense(
    string PackageId,
    NuGetVersion PackageVersion,
    Uri? ProjectUrl,
    string? License,
    Uri? LicenseUrl,
    string? Copyright,
    List<string> Authors)
{
    public static PackageLicense Create(ManifestMetadata metadata, string? license) =>
        new(
            metadata.Id,
            metadata.Version,
            metadata.ProjectUrl,
            license ?? "Unknown",
            metadata.LicenseUrl,
            metadata.Copyright,
            metadata.Authors.Select(o => o.Trim()).ToList());
}
