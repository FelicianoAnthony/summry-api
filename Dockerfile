# Use the .NET SDK 6.0 image as the build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

# Set the working directory to /summry_api
WORKDIR /summry_api

# Copy the current directory into the container
COPY . .