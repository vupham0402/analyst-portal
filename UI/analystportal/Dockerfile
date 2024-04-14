# Build stage
FROM node:17 AS build
WORKDIR /app

# Copy package.json and package-lock.json for dependency installation
COPY package.json package-lock.json* ./

# Install dependencies, consider using --legacy-peer-deps if there are peer dependency conflicts
RUN npm install --force

# Copy the rest of the app's source code
COPY . .

# Build the Angular application, specifying the configuration for production explicitly
RUN npm run build -- --configuration=production

# Serve stage
FROM nginx:alpine
# Copy the built application from the build stage to Nginx's serve directory
COPY --from=build /app/dist/analystportal /usr/share/nginx/html

# Copy custom Nginx configuration file
COPY nginx.conf /etc/nginx/conf.d/default.conf


# Expose port 80 to the host so that the container is accessible on the standard HTTP port
EXPOSE 80

# Command to run Nginx in the foreground to keep the container running
CMD ["nginx", "-g", "daemon off;"]
