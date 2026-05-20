# syntax=docker/dockerfile:1
# Windows Server Core base with the modern .NET SDK. The `dotnet` CLI here can
# build SDK-style csprojs targeting any old TFM (net20, net35, net40, net48...)
# as long as the project supplies its own reference assemblies — which this
# repo does, via Microsoft.NETFramework.ReferenceAssemblies.net20 and the
# files under refasm/.
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022

SHELL ["powershell", "-NoProfile", "-Command"]

WORKDIR C:\\src

# Keep NuGet's package cache on a volume-friendly path so it survives between
# bind-mount runs without polluting the repo.
ENV NUGET_PACKAGES=C:\\nuget-packages

CMD ["powershell.exe", "-NoLogo", "-NoProfile"]
