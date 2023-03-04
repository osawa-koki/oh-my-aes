FROM ubuntu:20.04 as base
WORKDIR /app
EXPOSE 80
RUN apt update -y && apt install -y tzdata && apt install -y nginx wget
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb
RUN apt update -y && apt install -y aspnetcore-runtime-7.0

FROM node:18 as web_build
WORKDIR /src
COPY ./web/package.json ./web/yarn.lock ./
RUN yarn install
COPY ./web ./
RUN yarn build

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS api_build
WORKDIR /src
COPY ./src/Program.csproj .
RUN dotnet restore ./Program.csproj
COPY ./src ./
RUN dotnet build ./Program.csproj -c Release -o /src/build

FROM base AS final
COPY ./web/nginx.conf /etc/nginx/nginx.conf
COPY --from=web_build /src/dist /var/www/html
COPY --from=api_build /src/build /app
RUN dotnet Program.dll
CMD service start nginx
