# stage 1

FROM node:16-alpine AS controlroom-ui-build
WORKDIR /app
COPY ./applications/controlroom-ui .
#WORKDIR "/src/applications/controlroom-ui"
RUN npm ci && npm run build

# stage 2

FROM nginx:alpine
COPY --from=controlroom-ui-build /app/dist/controlroom-ui /usr/share/nginx/html
EXPOSE 80