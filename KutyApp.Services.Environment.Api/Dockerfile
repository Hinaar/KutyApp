FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["KutyApp.Services.Environment.Api/KutyApp.Services.Environment.Api.csproj", "KutyApp.Services.Environment.Api/"]
COPY ["KutyApp.Services.Environment.Bll/KutyApp.Services.Environment.Bll.csproj", "KutyApp.Services.Environment.Bll/"]
RUN dotnet restore "KutyApp.Services.Environment.Api/KutyApp.Services.Environment.Api.csproj"
COPY . .
WORKDIR "/src/KutyApp.Services.Environment.Api"
RUN dotnet build "KutyApp.Services.Environment.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KutyApp.Services.Environment.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KutyApp.Services.Environment.Api.dll"]