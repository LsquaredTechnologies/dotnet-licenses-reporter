// <copyright file="JsonFormatterTests.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetLicensesReporter.Collectors;

using Microsoft.Extensions.Logging.Abstractions;

namespace Lsquared.DotnetLicensesReporter.ExtraFormatters;

public abstract class JsonFormatterTests
{
    [Trait("Category", "Snapshot testing")]
    public sealed class Write : FluidFormatterTests
    {
        [Fact]
        internal async Task ShouldReturnsValidOutput()
        {
            // Arrange
            StringReader template = new("""
            {% for package in packages -%}
            - {{ package.PackageId }} v{{ package.PackageVersion }}
            {% endfor -%}

            """);
            JsonFormatter formatter = new(NullLogger<JsonFormatter>.Instance, true);
            using MemoryStream stream = new();
            PackageLicense licenseInformation = new(
                "Lsquared.DotnetLicensesReporter.Cli",
                new("1.0.0"),
                new("https://nuget.org/Lsquared.DotnetLicensesReporter.Cli"),
                "MIT",
                new("https://mit-license.org/"),
                "Copyright",
                []);

            // Act
            await formatter.Write(stream, [licenseInformation]);

            // Assert
            await Verify(stream, extension: "txt");
        }
    }
}
