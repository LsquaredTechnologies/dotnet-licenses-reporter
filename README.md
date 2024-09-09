# .NET Licenses Reporter


## Table of Contents

1. [👋 About](#about)
2. [🔥 Getting Started](#getting-started)
   - [💻 Installing](#installing)
   - [🚀 Usage](#usage)

4. [🤝 Contributing](#contributing)

6. [✍ Author](#author)
7. [📝 License](#license)

[![GitHub Release][github-release-badge-url]][github-release-url]
[![Feature Requests][github-feature-request-badge-url]][github-feature-request-url]
[![Bugs][github-issues-badge-url]][github-issues-url]
[![Contributors][github-contributors-badge-url]][github-contributors-url]
[![NuGet Version][nuget-version-badge-url]][nuget-version-url]
{# TODO add Chocolatey #}
{# TODO add WinGet #}


## Table of Contents

1. [👋 About](#about)
2. [🔥 Getting Started](#getting-started)
   - [💻 Installing](#installing)
   - [🚀 Usage](#usage)

4. [🤝 Contributing](#contributing)

6. [✍ Author](#author)
7. [📝 License](#license)


## 👋 About

> .NET tool to report licenses in a project or solution.


## 💡 Description

<!-- Add a longer description here with images and more... -->


## 🔥 Getting Started

Make sure you have [`dotnet`](https://dotnet.microsoft.com/) installed.


[Lsquared.DotnetLicensesReporter.Cli](https://nuget.org/Lsquared.DotnetLicensesReporter.Cli)
tool can be installed using `dotnet` CLI.


For advanced users and developers, it can be installed and tested directly from source code (see below for more information).

### 💻 Installing


#### From `dotnet` CLI:

```shell
dotnet tool install --global Lsquared.DotnetLicensesReporter.Cli
```

or inside a specific directory:

```shell
dotnet new tool-manifest
dotnet tool install Lsquared.DotnetLicensesReporter.Cli
```

If tool is already installed and need upgrade:

```shell
dotnet tool update Lsquared.DotnetLicensesReporter.Cli
```

To remove the tool:

```shell
dotnet tool uninstall Lsquared.DotnetLicensesReporter.Cli
```



#### From code source (for development purpose):

1. Clone this repository with `git clone git@github.com:LsquaredTechnologies/dotnet-licenses-reporter.git`
2. Compile the application with `dotnet build -c Release -o ./artifacts`
3. Executable should be located inside `artifacts` repository




### 🚀 Usage

```shell
Usage: dotnet licenses [command] [command-options]



Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  templates

Run 'dotnet licenses [command] --help' for more information on a command.
```







## 🤝 Contributing

Contributions, fixes and feature requests are welcome! 




For major changes, please open an issue first to discuss what you would like to change.

### 🤩 Quick start


We support both VSCode and Visual Studio for all .Net developments and VSCode for all other files.




#### Run tests

```shell
cd [Project]/App/
dotnet test
```

```shell
cd [Project]/Lib/
dotnet test
```






## 🎗️ Sponsorships

Follow or ⭐️ this project if it helped you!




## 🏆 Credits

<!-- TODO add open collective -->


## ✍ Author

👤 LionelL2 








## 📝 License


Copyright © 2024 LionelL2






[github-release-badge-url]: https://flat.badgen.net/github/release//
[github-release-url]: https://github.com///releases
[github-feature-request-badge-url]: https://img.shields.io/github/issues///feature-request.svg
[github-feature-request-url]: https://github.com///issues?q=is%3Aopen+is%3Aissue+label%3Afeature-request+sort%3Areactions-%2B1-desc
[github-issues-badge-url]: https://img.shields.io/github/issues///bug.svg
[github-issues-url]: https://github.com///issues?utf8=✓&q=is%3Aissue+is%3Aopen+label%3Abug
[github-contributors-badge-url]: https://img.shields.io/github/contributors//.svg?style=flat-square
[github-contributors-url]: https://github.com///graphs/contributors
[nuget-version-badge-url]: https://img.shields.io/nuget/v/Lsquared.DotnetLicensesReporter.Cli?style=flat-square
[nuget-version-url]: https://www.nuget.org/packages/Lsquared.DotnetLicensesReporter.Cli



_This README was generated with ❤️ by [dotnet-readme-generator](https://github.com/LsquaredTechnologies/dotnet-readme-generator)_
