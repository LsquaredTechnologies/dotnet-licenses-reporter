// <copyright file="FluidFormatterTests.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Collectors;
using Lsquared.DotnetTools.LicensesReporter.Formatters.Abstracts;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Lsquared.DotnetTools.LicensesReporter.ExtraFormatters;

public abstract class FluidFormatterTests
{
    [Trait("Category", "Snapshot testing")]
    public sealed class Write : FluidFormatterTests
    {
        [Fact]
        internal async Task ShouldReturnsValidOutput()
        {
            // Arrange
            StringReader templateReader = new("""
            {% for package in packages -%}
            - {{ package.PackageId }} v{{ package.PackageVersion }}
            {% endfor -%}

            """);
            FluidFormatter formatter = new ConcreteFluidFormatter(NullLogger<ConcreteFluidFormatter>.Instance, templateReader);
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

    private sealed class ConcreteFluidFormatter(ILogger<ConcreteFluidFormatter> logger, StringReader templateReader)
        : FluidFormatter(logger, "fluid", new FileInfo("dummy"), () => templateReader);
}
