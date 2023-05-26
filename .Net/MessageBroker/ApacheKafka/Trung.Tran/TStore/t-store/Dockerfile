# Stage 1: Compile and Build angular codebase

# Use official node image as the base image
FROM node:latest as build

# Set the working directory
WORKDIR /usr/local/app

# Add the source code to app
COPY ./t-store/ /usr/local/app/

# Install all the dependencies
RUN yarn install

# Generate the build of the application
RUN yarn build


# Stage 2: Serve app with nginx server

# Use official nginx image as the base image
FROM nginx:latest

# Copy the build output to replace the default nginx contents.
COPY --from=build /usr/local/app/dist/t-store /usr/share/nginx/html

# Expose port 80, 443
EXPOSE 80
EXPOSE 443