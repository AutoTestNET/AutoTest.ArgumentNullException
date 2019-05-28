########################################################################################################################
# shellcheck - lining for bash scrips
FROM nlknguyen/alpine-shellcheck:v0.4.6

COPY ./ ./

# Convert CRLF to CR as it causes shellcheck warnings.
RUN find . -type f -name '*.sh' -exec dos2unix {} \;

# Run shell check on all the shell files.
RUN find . -type f -name '*.sh' | wc -l && find . -type f -name '*.sh' | xargs shellcheck --external-sources

########################################################################################################################
# .NET Core 2.1
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-alpine

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./autotest.argumentnullexception.sln .
COPY ./src/AutoTest.ArgumentNullException/AutoTest.ArgumentNullException.csproj ./src/AutoTest.ArgumentNullException/AutoTest.ArgumentNullException.csproj
COPY ./src/AutoTest.ArgumentNullException.Xunit/AutoTest.ArgumentNullException.Xunit.csproj ./src/AutoTest.ArgumentNullException.Xunit/AutoTest.ArgumentNullException.Xunit.csproj
COPY ./test/AutoTest.ArgumentNullException.Tests/AutoTest.ArgumentNullException.Tests.csproj ./test/AutoTest.ArgumentNullException.Tests/AutoTest.ArgumentNullException.Tests.csproj
COPY ./test/AutoTest.ExampleLibrary/AutoTest.ExampleLibrary.csproj ./test/AutoTest.ExampleLibrary/AutoTest.ExampleLibrary.csproj
COPY ./test/AutoTest.ExampleLibrary.Tests/AutoTest.ExampleLibrary.Tests.csproj ./test/AutoTest.ExampleLibrary.Tests/AutoTest.ExampleLibrary.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN ./coverage.sh netcoreapp2.1 Debug

########################################################################################################################
# .NET Core 2.2
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./autotest.argumentnullexception.sln .
COPY ./src/AutoTest.ArgumentNullException/AutoTest.ArgumentNullException.csproj ./src/AutoTest.ArgumentNullException/AutoTest.ArgumentNullException.csproj
COPY ./src/AutoTest.ArgumentNullException.Xunit/AutoTest.ArgumentNullException.Xunit.csproj ./src/AutoTest.ArgumentNullException.Xunit/AutoTest.ArgumentNullException.Xunit.csproj
COPY ./test/AutoTest.ArgumentNullException.Tests/AutoTest.ArgumentNullException.Tests.csproj ./test/AutoTest.ArgumentNullException.Tests/AutoTest.ArgumentNullException.Tests.csproj
COPY ./test/AutoTest.ExampleLibrary/AutoTest.ExampleLibrary.csproj ./test/AutoTest.ExampleLibrary/AutoTest.ExampleLibrary.csproj
COPY ./test/AutoTest.ExampleLibrary.Tests/AutoTest.ExampleLibrary.Tests.csproj ./test/AutoTest.ExampleLibrary.Tests/AutoTest.ExampleLibrary.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN ./coverage.sh netcoreapp2.1 Debug
