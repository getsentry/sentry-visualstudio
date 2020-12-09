# Sentry integration with Visual Studio CodeLens

WIP Visual Studio integration for Sentry, supporting both hosted and on-premises.

It currently only supports C#. PR for other languages is welcome.

# Requirements
* .NET Framework 4.8
* Visual Studio 2017 or 2019

# TODO:
- [ ] Figure out why the custom CodeLens UI via `IViewFactory` doesn't work
- [ ] Either cache namespaces or only read the source files up to the namespace definition to reduce unnecessary IO
- [ ] Add screenshots
