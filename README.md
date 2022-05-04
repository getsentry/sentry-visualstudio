<p align="center">
  <a href="https://sentry.io/?utm_source=github&utm_medium=logo" target="_blank">
    <picture>
      <source srcset="https://sentry-brand.storage.googleapis.com/sentry-logo-white.png" media="(prefers-color-scheme: dark)" />
      <source srcset="https://sentry-brand.storage.googleapis.com/sentry-logo-black.png" media="(prefers-color-scheme: light), (prefers-color-scheme: no-preference)" />
      <img src="https://sentry-brand.storage.googleapis.com/sentry-logo-black.png" alt="Sentry" width="280">
    </picture>
  </a>
</p>

Sentry Integration with Visual Studio
===========

[![build](https://github.com/getsentry/sentry-visualstudio/workflows/build/badge.svg?branch=master)](https://github.com/getsentry/sentry-visualstudio/actions?query=branch%3Amaster)

WIP Visual Studio CodeLens integration for Sentry, supporting both hosted and on-premises.

It currently only supports C#. PRs for other languages are welcome.

# Requirements
* .NET Framework 4.8
* Visual Studio 2017 or 2019

# TODO:
- [ ] Figure out why the custom CodeLens UI via `IViewFactory` doesn't work
- [ ] Either cache namespaces or only read the source files up to the namespace definition to reduce unnecessary IO

![CodeLens](.github/code-lens-ex-count.png)
![CodeLens exception ount](.github/code-lens.png)
![Window](.github/window.png)
