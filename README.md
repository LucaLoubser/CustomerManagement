### Prerequisites
- .NET 10 SDK
- SQL Server at `localhost`, Windows auth
- Node 18.x with Angular CLI 15
- dotnet tool install --global dotnet-ef

#### Install correct angular version
1. cd angular
2. nvm install 18.20.4
3. nvm use 18.20.4
4. node -v
5. npm install -g @angular/cli@15.2.11

### Running Backend
#### Adding User Secrets
1. add secrets.export.json to the project root
2. cd src\CustomerManagement.Api
3. Get-Content ..\\..\\secrets.export.json -Raw | dotnet user-secrets set

#### Verify if secrets are set
1. dotnet user-secrets list
2. MediatRLicenseKey and AutoMapperLicenseKey should be listed

#### Running DB Migrations
1. cd ..\\
2. $env:ASPNETCORE_ENVIRONMENT = "Development"
3. dotnet build
4. dotnet ef database update --project CustomerManagement.Infrastructure --startup-project CustomerManagement.Api

#### Running the API
(from src directory)
1. cd CustomerManagement.Api
2. dotnet dev-certs https --trust
3. dotnet run --launch-profile https

api swagger available at https://localhost:7120/swagger/index.html

### Running Frontend
1. cd angular
2. npm install
3. npm start

frontend available at: http://localhost:4200

### Running Tests
1. cd tests/CustomerManagement.Application.Tests
dotnet test