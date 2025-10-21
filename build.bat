@echo off
pushd "%~dp0"
if not exist bin\ mkdir bin\
csc /nologo /target:winexe /out:bin\lxp.exe /reference:System.Net.Requests.dll,System.Web.Extensions.dll /win32manifest:src\app.manifest src\*.cs