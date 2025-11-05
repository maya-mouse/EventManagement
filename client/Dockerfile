FROM node:lts AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci 

COPY . .

RUN npx ng build Client --configuration production


FROM nginx:1.21-alpine AS final


WORKDIR /usr/share/nginx/html

COPY ./nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/dist/Client/browser /usr/share/nginx/html

EXPOSE 80