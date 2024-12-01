# Use the official .NET SDK 8.0 image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy all the files from the current directory to the container
COPY . .

# Restore dependencies for the project
RUN dotnet restore ogybot.Bot/ogybot.Bot.csproj

# Build the project in Release mode
RUN dotnet build ogybot.Bot/ogybot.Bot.csproj -c Release -o /app/build

# Publish the project into a folder optimized for deployment
RUN dotnet publish ogybot.Bot/ogybot.Bot.csproj -c Release -o /app/publish --no-restore

# Use the runtime-only .NET 8.0 image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the runtime container
WORKDIR /app

# Copy the build artifacts from the build stage
COPY --from=build /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "ogybot.Bot.dll"]