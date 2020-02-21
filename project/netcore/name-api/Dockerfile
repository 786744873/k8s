FROM mcr.azk8s.cn/dotnet/core/sdk:3.1 AS build
WORKDIR /app
Expose 80

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore

# copy everything else and build app
RUN dotnet publish -c Release -o out

FROM mcr.azk8s.cn/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "name-api.dll"]

# docker build -t 13057686866/name-api .
# docker images
# docker run -d -p 8084:80 --name name-api 13057686866/name-api
# docker login
# docker push 13057686866/name-api:latest