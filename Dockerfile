# Use the official .NET 6 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["./UserManagement/UserManagement.csproj", "UserManagement/"]
RUN dotnet restore "./UserManagement/MyApi.csproj"

# Copy the entire project and build the application
COPY . .
WORKDIR "/src/UserManagement"
RUN dotnet build "UserManagement.csproj" -c Release -o /app/build

# Create a runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .

# Expose the port on which the application will run
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "UserManagement.dll"]