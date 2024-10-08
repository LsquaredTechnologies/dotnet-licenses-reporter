// <copyright file="Package.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using NuGet.Versioning;

namespace Lsquared.DotnetTools.LicensesReporter.Collectors;

public sealed record class Package(string Id, NuGetVersion? Version, bool IsAnalyzer);
