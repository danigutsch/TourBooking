# Root Configuration & Version Management Files

This repository is a showcase for modern .NET solution configuration and version management. Below is an overview of the key files and folders at the root of the repository that control build, tooling, and code style. Understanding these files is essential for working effectively in any .NET project.

---

## Root-Level Files & Folders

- **global.json**
  - Pins the .NET SDK version for all contributors and CI builds, ensuring consistent tooling. This file can also specify roll-forward behavior and allow prerelease SDKs. Learn more: https://learn.microsoft.com/en-us/dotnet/core/tools/global-json

- **Directory.Build.props**
  - Applies MSBuild properties and settings to all projects in the repo. This is the place to set the default TargetFramework, language version, code analysis, and other solution-wide build options. It helps enforce consistency and reduces duplication across project files. Learn more: https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory

- **Directory.Packages.props**
  - Centralizes NuGet package version management for all projects. By defining package versions here, you avoid version drift and simplify upgrades. This repo also enables transitive package version pinning, which ensures that all transitive dependencies use the exact versions specified, improving build reproducibility and security. Learn more: https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management

- **.editorconfig**
  - Defines code style, formatting, and analyzer rules for all editors and IDEs. This ensures a consistent coding style across the team, regardless of individual developer settings. .editorconfig can also suppress or elevate specific code analysis warnings. Learn more: https://learn.microsoft.com/en-us/visualstudio/ide/editorconfig-code-style-settings-reference

- **.github/**
  - GitHub-specific configuration, such as workflows (CI/CD), issue templates, and pull request templates.

- **.config/**
  - General configuration files for tools and services used by the repository.

- **.git/**
  - Git version control metadata. Do not edit manually.

- **.vs/**
  - Visual Studio local solution settings and caches. Not required for source control.

- **.vscode/**
  - Visual Studio Code workspace settings and recommended extensions.

- **.idea**
  - JetBrains Rider/IntelliJ IDE project settings.

- **README.md**, **LICENSE**, **CODING_GUIDELINES.md**
  - Documentation and legal files. README.md is the main entry point for understanding the repo, while CODING_GUIDELINES.md details code standards.

---

For more on .NET solution configuration best practices, see the official Microsoft documentation linked above.
