// <copyright file="PackageLicenseCollector.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotNetLicensesReporter.Core;

using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Lsquared.DotNetLicensesReporter.Collectors;

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

    private static PackageLicense GetLicense(DownloadResourceResult pacakge)
    {
        using var nuspecStream = pacakge.PackageReader.GetNuspec();
        var manifest = Manifest.ReadFrom(nuspecStream, false);
        var metadata = manifest.Metadata;
        UpdateProjectUrl(metadata);
        UpdateLicenseUrl(metadata);
        var license = Licenses.GetCustomName(Licenses.Map(metadata.LicenseUrl));
        return PackageLicense.Create(metadata, license);
    }

    private static void UpdateLicenseUrl(ManifestMetadata metadata)
    {
        if (metadata.LicenseMetadata?.Type is LicenseType.File)
        {
            if (TryGetLicenseUrl(metadata, out var licenseUrl))
                metadata.SetLicenseUrl(licenseUrl);
        }
    }

    private static bool TryGetLicenseUrl(ManifestMetadata metadata, out string? licenseUrl)
    {
        UriBuilder b = new(metadata.ProjectUrl);
        // GitHub, TODO add more providers...
        b.Host = b.Host.Replace("github.com", "raw.githubusercontent.com");
        b.Path += $"/{metadata.Repository?.Branch ?? metadata.Repository?.Commit ?? "main"}/{metadata.LicenseMetadata.LicenseUrl}";
        licenseUrl = b.Uri.ToString();
        return true;
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
