@echo off
cd..
cd..
cd Coursewise.Api
dotnet ef database update -p ..\Coursewise.Data --verbose
cd..
cd Scripts/Migrations