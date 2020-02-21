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
ENTRYPOINT ["dotnet", "hpaApi.dll"]

# docker build -t 13057686866/hpa-api .
# docker images
# docker run -d -p 8086:80 --name hpa-api 13057686866/hpa-api
# docker tag 13057686866/hpa-api:latest registry.cn-hangzhou.aliyuncs.com/wyt666/hpa-api:0.0.6
# docker login --username=tb5228628_2012 registry.cn-hangzhou.aliyuncs.com
# wyt123456
# docker push registry.cn-hangzhou.aliyuncs.com/wyt666/hpa-api:0.0.6