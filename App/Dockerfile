# build environment
FROM node:16.18-alpine AS build
WORKDIR /App
COPY package.json ./
RUN npm install

COPY . ./
RUN npm run build

# => Run container
FROM nginx:stable-alpine

# Nginx config
RUN rm /etc/nginx/conf.d/default.conf
COPY ./nginx.conf /etc/nginx/nginx.conf

# Static build
COPY --from=build /App/build /usr/share/nginx/html

# Default port exposure
EXPOSE 80

# Copy .env file and shell script to container 

WORKDIR /usr/share/nginx/html
COPY ./env.sh .
COPY .env .

# Make our shell script executable

RUN chmod +x env.sh

# Start Nginx server

CMD ["/bin/sh", "-c", "/usr/share/nginx/html/env.sh && nginx -g \"daemon off;\""]
