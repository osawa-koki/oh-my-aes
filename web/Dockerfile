FROM nginx:1.23 as base
WORKDIR /app

FROM node:18 as build
WORKDIR /src
COPY package.json yarn.lock ./
RUN yarn install
COPY . .
RUN yarn build

FROM base as final
COPY nginx.dev.conf /etc/nginx/nginx.conf
COPY --from=build /src/dist /var/www/html
