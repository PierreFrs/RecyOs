FROM node:18-alpine3.20 AS build
LABEL authors="Benjamin ROLLIN"

RUN npm install -g @angular/cli

# Set the working directory
WORKDIR /usr/local/app

# Add the source code to app
COPY ./ /usr/local/app/

# Install all the dependencies
RUN npm install

# Generate the build of the application
ARG NODE_ENV="production"
RUN if [ "$NODE_ENV" = "production" ] ;  then ng build --aot --output-hashing=all ; else ng build -c stage --aot --output-hashing=all ; fi

# Étape finale
FROM nginx:stable-alpine3.20

# Copier les fichiers de build Angular générés à partir de l'étape de construction
COPY --from=build /usr/local/app/dist/fuse/ /usr/share/nginx/html

# Exposer le port 443 pour HTTPS
EXPOSE 443

# Commande par défaut
CMD ["nginx", "-g", "daemon off;"]


