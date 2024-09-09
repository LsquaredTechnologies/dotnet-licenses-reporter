// <copyright file="Module.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.Runtime.CompilerServices;

using Lsquared.DotNetLicensesReporter.Formatters;

namespace Lsquared.DotNetLicensesReporter;

internal static class Module
{
    [ModuleInitializer]
    public static void Initialize()
    {
        OutputFormats.Defaults.Add("table");
        OutputFormats.Add("table");
        OutputFormats.Add("json");
        OutputFormats.Add("markdown-list");
        OutputFormats.Add("markdown-table");
    }
}
