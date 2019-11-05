FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS backendBuilder
RUN apk --update --no-cache --quiet add git openssh
WORKDIR /project
RUN git clone https://github.com/rosenbjerg/FormValidator.git
WORKDIR /project/FormValidator/FormValidator/FormValidatorTests
RUN dotnet restore
ENTRYPOINT dotnet test
