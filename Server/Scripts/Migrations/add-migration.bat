@echo off
cd..
cd..
cd Coursewise.Api
dotnet ef migrations add %1 -p ..\Coursewise.Data -v
cd..
cd Scripts/Migrations