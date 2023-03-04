FROM ubuntu:20.04
WORKDIR /app
RUN apt update -y && apt install -y tzdata && apt install -y nginx wget
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb
RUN apt update -y && apt install -y dotnet-sdk-7.0
