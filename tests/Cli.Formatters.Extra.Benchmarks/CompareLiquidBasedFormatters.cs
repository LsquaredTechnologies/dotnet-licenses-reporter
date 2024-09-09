// <copyright file="CompareLiquidBasedFormatters.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using BenchmarkDotNet.Attributes;

using Lsquared.DotNetLicensesReporter.Collectors;
using Lsquared.DotNetLicensesReporter.Formatters.Abstracts;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Lsquared.DotNetLicensesReporter.ExtraFormatters;

public class CompareLiquidBasedFormatters
{
    public string Template { get; } = """
            {% for package in packages -%}
            - {{ package.Id }} v{{ package.Version }}
            {% endfor -%}
            """;

    public List<PackageLicense> Packages { get; } = [
        new("Id1", new("1.0.0"), new("https://nuget.org/Id1"), "MIT", new("https://mit-license.org/"), "Copyright", ["Lsquared"]),
        new("Id2", new("1.0.0"), new("https://nuget.org/Id2"), "MIT", new("https://mit-license.org/"), "Copyright", ["Lsquared"]),
        new("Id3", new("1.0.0"), new("https://nuget.org/Id3"), "MIT", new("https://mit-license.org/"), "Copyright", ["Lsquared"]),
        new("Id4", new("1.0.0"), new("https://nuget.org/Id4"), "MIT", new("https://mit-license.org/"), "Copyright", ["Lsquared"]),
        new("Id5", new("1.0.0"), new("https://nuget.org/Id5"), "MIT", new("https://mit-license.org/"), "Copyright", ["Lsquared"]),
    ];

    public CompareLiquidBasedFormatters()
    {
        _fluidFormatter = new ConcreteFluidFormatter(NullLogger<ConcreteFluidFormatter>.Instance, new StringReader(Template));
        _liquidFormatter = new ConcreteDotLiquidFormatter(NullLogger<ConcreteDotLiquidFormatter>.Instance, new StringReader(Template));
    }

    [Benchmark]
    public async Task Fluid() =>
        await _fluidFormatter.Write(new StringWriter(), Packages);

    [Benchmark]
    public async Task DotLiquid() =>
        await _liquidFormatter.Write(new StringWriter(), Packages);

    private readonly FluidFormatter _fluidFormatter;
    private readonly DotLiquidFormatter _liquidFormatter;

    private sealed class ConcreteFluidFormatter(ILogger<ConcreteFluidFormatter> logger, StringReader templateReader)
        : FluidFormatter(logger, "fluid", new FileInfo("dummy"), () => templateReader);
    private sealed class ConcreteDotLiquidFormatter(ILogger<ConcreteDotLiquidFormatter> logger, StringReader templateReader)
        : DotLiquidFormatter(logger, "dotliquid", new FileInfo("dummy"), () => templateReader);
}
