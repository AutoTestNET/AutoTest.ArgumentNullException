FROM microsoft/aspnetcore-build:2.0.0

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching
COPY ./autotest.argumentnullexception.sln .
COPY ./src/AutoTest.ArgumentNullException/AutoTest.ArgumentNullException.csproj ./src/AutoTest.ArgumentNullException/AutoTest.ArgumentNullException.csproj
COPY ./src/AutoTest.ArgumentNullException.Xunit/AutoTest.ArgumentNullException.Xunit.csproj ./src/AutoTest.ArgumentNullException.Xunit/AutoTest.ArgumentNullException.Xunit.csproj
COPY ./test/AutoTest.ArgumentNullException.Tests/AutoTest.ArgumentNullException.Tests.csproj ./test/AutoTest.ArgumentNullException.Tests/AutoTest.ArgumentNullException.Tests.csproj
COPY ./test/AutoTest.ExampleLibrary/AutoTest.ExampleLibrary.csproj ./test/AutoTest.ExampleLibrary/AutoTest.ExampleLibrary.csproj
COPY ./test/AutoTest.ExampleLibrary.Tests/AutoTest.ExampleLibrary.Tests.csproj ./test/AutoTest.ExampleLibrary.Tests/AutoTest.ExampleLibrary.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

# Build to ensure the tests are their own distinct step
RUN dotnet build -f netcoreapp2.0 -c Debug ./test/AutoTest.ArgumentNullException.Tests/AutoTest.ArgumentNullException.Tests.csproj
RUN dotnet build -f netcoreapp2.0 -c Debug ./test/AutoTest.ExampleLibrary.Tests/AutoTest.ExampleLibrary.Tests.csproj

# Run unit tests
RUN dotnet test --no-restore --no-build -c Debug -f netcoreapp2.0 test/AutoTest.ArgumentNullException.Tests/AutoTest.ArgumentNullException.Tests.csproj
RUN dotnet test --no-restore --no-build -c Debug -f netcoreapp2.0 test/AutoTest.ExampleLibrary.Tests/AutoTest.ExampleLibrary.Tests.csproj
