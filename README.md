# Bluehands.Versioning

Bluehands.Versioning provides automatic assembly versioning for .NET projects.

## What you need to do

After installing the package, create a `Version.txt` file with three version number segments (e.g. `1.0.0`) in the project or solution folder.

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

- It is recommended to not commit the `AssemblyVersionInfo.cs` file since it is generated automatically. Usually a `.gitignore` file including it should be created automatically.

## Known issues

- Does not work with the `dotnet` CLI and thus on platforms other than Windows yet.

- Does not and will never work with `project.json`/`.xproj` projects since they do not use MSBuild.

- Projects using the new MSBuild format introduced in Visual Studio 2017, i.e. most newer .NET Core and .NET Standard projects, use a different mechanism for setting the `AssemblyVersion` data. If you opt-out of the default behavior by setting `GenerateAssemblyInfo` to `false`, you'll need to do a few things:
  1. Create a `Properties` subfolder under the project root.
  2. If necessary, remove any duplicate attributes in your own `AssemblyInfo.cs` file (as indicated in the build error message).
  3. Add `AssemblyVersionInfo.cs` to your `.gitignore`.

## Changelog