// <copyright file="Template.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetTools.LicensesReporter.Templating;

internal sealed record class Template(string TemplateName, string ShortName, string Author, HashSet<string> Tags, Func<Stream> GetStream);
