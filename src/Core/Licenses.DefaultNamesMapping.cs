// <copyright file="Licenses.DefaultNamesMapping.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Core;

public static partial class Licenses
{
    public static IReadOnlyDictionary<string, string> DefaultNamesMapping { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        [APL_1_0] = "Adaptive Public License 1.0",
        [AGPL_1_0_ONLY] = "Affero General Public License v1.0 only",
        [AGPL_1_0_OR_LATER] = "Affero General Public License v1.0 or later",
        
    };

    /* Apache License 1.0
     * Apache License 1.1
     * Apache License 2.0
     * Beerware License
     * BSD 1-Clause
     * BSD 2-Clause
     * BSD 3-Clause
     * BSD Zero Clause
     */
}
