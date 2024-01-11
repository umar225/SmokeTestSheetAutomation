#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Coursewise.Api/*", "./" ]
COPY ["Coursewise.Api/*.csproj", "Coursewise.Api/"]
COPY ["Coursewise.Data/Coursewise.Data.csproj", "Coursewise.Data/"]
COPY ["Coursewise.Domain/Coursewise.Domain.csproj", "Coursewise.Domain/"]
COPY ["Coursewise.Common/Coursewise.Common.csproj", "Coursewise.Common/"]
COPY ["Coursewise.Logging/Coursewise.Logging.csproj", "Coursewise.Logging/"]
COPY ["Coursewise.Security.Compact/Coursewise.Security.Compact.csproj", "Coursewise.Security.Compact/"]
COPY ["Coursewise.AWS/Coursewise.AWS.csproj", "Coursewise.AWS/"]
COPY ["Coursewise.Typeform/Coursewise.Typeform.csproj", "Coursewise.Typeform/"]
COPY ["Coursewise.Module/Coursewise.Module.csproj", "Coursewise.Module/"]


RUN dotnet restore "Coursewise.Api/Coursewise.Api.csproj"
COPY . .
WORKDIR "/src/Coursewise.Api"
RUN dotnet build "Coursewise.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Coursewise.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coursewise.Api.dll"] 

