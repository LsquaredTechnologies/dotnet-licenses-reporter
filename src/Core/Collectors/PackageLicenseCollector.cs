// <copyright file="PackageLicenseCollector.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Core;

using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Lsquared.DotnetTools.LicensesReporter.Collectors;

public static class PackageLicenseCollector
{
    public static IEnumerable<PackageLicense> Collect(IEnumerable<Package> packages, CollectionOptions options)
    {
        _ = options;
        return CollectCore(packages).OrderBy(o => o.PackageId);
    }

    private static IEnumerable<PackageLicense> CollectCore(IEnumerable<Package> packages)
    {
        var settings = Settings.LoadDefaultSettings(null);
        var globalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(settings);
        foreach (var package in packages)
        {
            var cachedPackage = GlobalPackagesFolderUtility.GetPackage(
                new PackageIdentity(package.Id, package.Version),
                globalPackagesFolder);
            yield return GetLicense(cachedPackage);
        }
    }

    private static PackageLicense GetLicense(DownloadResourceResult package)
    {
        using var nuspecStream = package.PackageReader.GetNuspec();
        var manifest = Manifest.ReadFrom(nuspecStream, false);
        var metadata = manifest.Metadata;
        UpdateProjectUrl(metadata);
        UpdateLicenseUrl(metadata, package);
        var license = Licenses.GetCustomName(Licenses.Map(metadata.LicenseUrl));
        return PackageLicense.Create(metadata, license);
    }

    private static void UpdateLicenseUrl(ManifestMetadata metadata, DownloadResourceResult package)
    {
        if (metadata.LicenseMetadata?.Type is LicenseType.File)
        {
            // Read LICENSE file contents and check for license standard texts...
            using var stream = package.PackageReader.GetStream(metadata.LicenseMetadata.License);
            using StreamReader reader = new(stream, leaveOpen: true);
            var contents = reader.ReadToEnd();
            var licenseUrl = TryDetectLicenseFromContents(metadata, contents);
            if (licenseUrl is not null)
                metadata.SetLicenseUrl(licenseUrl);
        }
    }

    private static string? TryDetectLicenseFromContents(ManifestMetadata metadata, string contents)
    {
        // please order licenses from most used to least used...

        if (contents.Contains("MIT License", StringComparison.OrdinalIgnoreCase))
            return Licenses.DefaultUrlsMapping.FirstOrDefault(o => o.Value == Licenses.MIT).Key;

        if (contents.Contains("3-clause BSD", StringComparison.OrdinalIgnoreCase))
            return Licenses.DefaultUrlsMapping.FirstOrDefault(o => o.Value == Licenses.BSD_3).Key;

        if (contents.Contains("2-clause BSD", StringComparison.OrdinalIgnoreCase))
            return Licenses.DefaultUrlsMapping.FirstOrDefault(o => o.Value == Licenses.BSD_2).Key;

        if (contents.Contains("1-clause BSD", StringComparison.OrdinalIgnoreCase))
            return Licenses.DefaultUrlsMapping.FirstOrDefault(o => o.Value == Licenses.BSD_1).Key;

        if (contents.Contains("Apache License, Version 2.0", StringComparison.OrdinalIgnoreCase))
            return Licenses.DefaultUrlsMapping.FirstOrDefault(o => o.Value == Licenses.APACHE_2_0).Key;

        if (contents.Contains("Apache Software License, Version 1.1", StringComparison.OrdinalIgnoreCase))
            return Licenses.DefaultUrlsMapping.FirstOrDefault(o => o.Value == Licenses.APACHE_1_1).Key;

        if (contents.Contains("Apache Software License 1", StringComparison.OrdinalIgnoreCase))
            return Licenses.DefaultUrlsMapping.FirstOrDefault(o => o.Value == Licenses.APACHE_1_0).Key;

        if (contents.Contains("Zero clause BSD", StringComparison.OrdinalIgnoreCase) ||
            contents.Contains("0BSD", StringComparison.OrdinalIgnoreCase))
        {
            return Licenses.DefaultUrlsMapping.FirstOrDefault(o => o.Value == Licenses.BSD_0).Key;
        }

        return null;
    }

    private static void UpdateProjectUrl(ManifestMetadata metadata)
    {
        var projectUrl =
                metadata.ProjectUrl ??
                (metadata.Repository is null ? null : new Uri(metadata.Repository.Url)) ??
                ExtractFrom(metadata.Readme) ??
                ExtractFrom(metadata.ReleaseNotes) ??
                ExtractFrom(metadata.LicenseUrl);
        if (projectUrl is not null)
            metadata.SetProjectUrl(projectUrl.ToString());
    }

    private static Uri? ExtractFrom(string? url) => url switch
    {
        null => null,
        _ => ExtractFrom(new UriBuilder(url).Uri),
    };

    private static Uri? ExtractFrom(Uri? uri)
    {
        if (uri is null) return null;
        var url = uri.GetLeftPart(UriPartial.Path);
        var index = url.IndexOf("/blob"); // GitHub, TODO add more providers...
        return new(index >= 0 ? url[..index] : url);
    }
}
