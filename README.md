# Bluehands.Versioning

Bluehands.Versioning provides automatic assembly versioning for .NET projects.

This package is [available on nuget](https://www.nuget.org/packages/Bluehands.Versioning).

## What you need to do

Install the [package from nuget](https://www.nuget.org/packages/Bluehands.Versioning) and create a `Version.txt` file with three version number segments (e.g. `1.0.0`) in the project or solution folder.

Set and update the version number in `Version.txt` as you see fit. Consider using [Semantic Versioning](http://semver.org/).

## What the package does

On each build, the last segment of the version number will be filled in with the Git commit count, i.e. if you have two commits the final version number will be `1.0.0.2` and set in `AssemblyVersion` and `AssemblyFileVersion`.

Additionally, `AssemblyInformationalVersion` will be set to a value containing the branch name, commit hash and date and time of the build.

## Purpose

Each build from a CI server will automatically get its own version number even if you do not increment it manually. This is especially useful during development, when you might not want to increment the version number manually for each internal version yet need to be able to trace an application or library to its exact commit for debugging.

## Additional notes

- A `Version.txt` in the solution folder will be applied to all projects in the solution. A `Version.txt` in a project folder will have precedence.

- To use the generated version as the ClickOnce publish version, set the `UseGeneratedVersionForClickOnce` MSBuild property to `true`. You can do that in one of two ways:
  - Add `<UseGeneratedVersionForClickOnce>true</UseGeneratedVersionForClickOnce>` to a `PropertyGroup` in the project file.
  - Call MSBuild on the command line with an additional `/p:UseGeneratedVersionForClickOnce=True` parameter.

## Known issues

- Does not work with the `dotnet` CLI and thus on platforms other than Windows yet.

- Does not and will never work with `project.json`/`.xproj` projects since they do not use MSBuild.

## Changelog

### Version 1.3.1

- Fixed: building on a build server or after manually deleting the `obj` folder no longer causes an MSB4018 error (The "AssemblyInfo" task failed unexpectedly)

### Version 1.3

- Improved: the generated `AssemblyVersionInfo.cs` is now located in the `obj` folder by default to avoid cluttering up the project folder
- Improved: no `.gitignore` file is automatically created anymore as it is no longer necessary
- Fixed: editing the `AssemblyVersionInfo.cs` file manually no longer leads to inconsistent values in the binary

### Version 1.2

- Added: support for projects using the new MSBuild format
- Improved: no more overwrite prompts when upgrading the package