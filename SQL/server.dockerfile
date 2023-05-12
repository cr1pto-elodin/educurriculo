FROM mcr.microsoft.com/mssql/server:2019-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Educ4cao!

COPY ./init.sql /docker-entrypoint-initdb.d/