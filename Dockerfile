# Use the official .NET 6 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /UserManagement/UserAPI

# Copy the project file and restore dependencies
COPY UserAPI.csproj .
RUN dotnet restore

# Copy the entire project and build the application
COPY . .
RUN dotnet build -c Release -o out

# Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/out .

# Expose the port on which the application will run
EXPOSE 8081

# Set the entry point for the container
ENTRYPOINT ["dotnet", "UserManagement.dll"]