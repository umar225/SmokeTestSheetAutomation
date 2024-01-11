@echo off
cd..
cd..
cd Coursewise.Api
dotnet ef database update %1 -p ..\Coursewise.Data -v
cd..
cd Scripts/Migrations