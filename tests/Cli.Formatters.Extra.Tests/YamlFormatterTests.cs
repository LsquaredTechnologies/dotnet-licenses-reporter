// <copyright file="YamlFormatterTests.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Collectors;
using Lsquared.DotnetTools.LicensesReporter.Formatters;

using Microsoft.Extensions.Logging.Abstractions;

namespace Lsquared.DotnetTools.LicensesReporter.ExtraFormatters;

public abstract class YamlFormatterTests
{
    [Trait("Category", "Snapshot testing")]
    public sealed class Write : YamlFormatterTests
    {
        [Fact]
        internal async Task ShouldReturnsValidOutput()
        {
            // Arrange
            YamlFormatter formatter = new(NullLogger<YamlFormatter>.Instance);
            using MemoryStream stream = new();
            PackageLicense licenseInformation = new(
                "Lsquared.DotnetTools.LicensesReporter.Cli",
                new("1.0.0"),
                new("https://nuget.org/Lsquared.DotnetTools.LicensesReporter.Cli"),
                "MIT",
                new("https://mit-license.org/"),
                "Copyright",
                []);

            // Act
            await formatter.Write(stream, [licenseInformation]);

            // Assert
            stream.Seek(0, SeekOrigin.Begin);
            await Verify(stream, extension: "txt");
        }
    }
}
