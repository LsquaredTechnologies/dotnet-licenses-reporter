// <copyright file="DotLiquidFormatterTests.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Collectors;
using Lsquared.DotnetTools.LicensesReporter.Formatters.Abstracts;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Lsquared.DotnetTools.LicensesReporter.ExtraFormatters;

public abstract class DotLiquidFormatterTests
{
    [Trait("Category", "Snapshot testing")]
    public sealed class Write : DotLiquidFormatterTests
    {
        [Fact]
        internal async Task ShouldReturnsValidOutput()
        {
            // Arrange
            StringReader templateReader = new("""
            {% for package in packages -%}
            - {{ package.package_id }} v{{ package.package_version }}
            {% endfor -%}

            """);
            DotLiquidFormatter formatter = new ConcreteDotLiquidFormatter(NullLogger<ConcreteDotLiquidFormatter>.Instance, templateReader);
            using StringWriter writer = new();
            PackageLicense licenseInformation = new(
                "Lsquared.DotnetTools.LicensesReporter.Cli",
                new("1.0.0"),
                new("https://nuget.org/Lsquared.DotnetTools.LicensesReporter.Cli"),
                "MIT",
                new("https://mit-license.org/"),
                "Copyright",
                []);

            // Act
            await formatter.Write(writer, [licenseInformation]);

            // Assert
            await Verify(writer.GetStringBuilder().ToString(), extension: "txt");
        }
    }

    private sealed class ConcreteDotLiquidFormatter(ILogger<ConcreteDotLiquidFormatter> logger, StringReader templateReader)
        : DotLiquidFormatter(logger, "dotliquid", new FileInfo("dummy"), () => templateReader);

}
