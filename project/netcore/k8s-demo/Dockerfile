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
ENTRYPOINT ["dotnet", "k8s-demo.dll"]

# docker build -t 13057686866/k8s-demo .
# docker images
# docker run -d -p 8085:80 --name k8s-demo 13057686866/k8s-demo
# docker login
# docker push 13057686866/k8s-demo:latest