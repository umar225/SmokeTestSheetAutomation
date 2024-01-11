@echo off
cd..
cd..
cd Coursewise.Api
dotnet ef migrations remove -p ..\Coursewise.Data -v
cd..
cd Scripts/Migrations