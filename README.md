# minimdb
Mini IMDB. The movies and TV shows catalogue.

 Quick and dirty demo project featuring the following topics:

* ASP.Net Core backend
  * API built with DI
  * Different configurations for different environments
  * Connected to PostgreSQL database with EF core
  * CRUD operations, pagination, search
  * Authentication and authorization using JWT tokens
  * Data validation
  * In-memory caching example
  * Basic internationalization example with 3 languages
  * Exception handling strategy: it's better to avoid using exceptions or convert them into app specific error objects
  * API documentation using Swagger
* Angular 8 frontend
  * Backend API usage
  * Authentication and Authorization quick and dirty idea
  * Input data validation
  * CRUD operations, pagination, search
  * Basic internationalization example with 3 languages
  * Different configurations for testing purposes
* Docker containerization

## How to build and run

### Running in Docker
```
docker-compose build
docker-compose up -d
```
To stop:
`docker-compose down`

The application will be available at http://localhost:10080

To build project image run in solution directory:

`docker build -f "MiniMdb.Backend\Dockerfile" -t minimdb.backend .`

### Running locally:

Prerequisites:
* NPM 6+ https://nodejs.org/en/download/
* .NET Core 3.1 https://dotnet.microsoft.com/download/dotnet-core/3.1
* (Visual Studio 2019 or VS Code)

The app requires Postgres database at runtime.
The easiest way to have it is run it in docker like this:
`docker run --name minimdb_postgres_dev -v minimdbdata:/var/lib/postgresql/data -p 5432:5432 postgres:11 -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=minimdb-dev`

To start the app locally run the following commands inside MiniMdb.Backend directory:
```
set ASPNETCORE_ENVIRONMENT Development
dotnet restore
dotnet build
dotnet run
```

To run frontend separately use the following commands inside MiniMdb.Backend/ClientApp directory:

```
npm install
npm run ng serve
```

The app will be available by the url: http://localhost:5000

## API Documentation

Autogenerated API docs: `http://localhost:5000/swagger`

## Testing

To run tests run the following command in solution directory:
```
dotnet test MiniMdb.Testing
```

## Authentication and authorization

There are two hardcoded users:
1. Admin role, `admin@example.com`, `m3g@pA$$W0rDDD`
2. User role, `anyone@example.com`, `w3@kpa$$w0rd`

Admin can do any operation in the app including: creating new titles, editing, removing.
User can't edit or create anything. The only difference from anonymous access is the ability to
see Movie/Series details on a separate page (just a demonstration of authorization policies).

# Development Concepts
### Internationalization

Frontend follows Angular localization techniques: https://angular.io/guide/i18n

Backend follows ASP.NET Core localization techniques: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-3.1

To localize strings from database I would get request culture like that in controller:

`var requestCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>();`

After I know the locale I would add this field to calls to backend services.
In database I would make additional tables with corresponding localizations `MediaTitlesTranslations` with columns `id`, `locale`, `[translateable columns]`.
Then I can join `MediaTitles` with it to select corresponding translation.
Or maybe there is some convenient library for SQL localizations, haven't explored this topic.

### Caching

Current implementation of backend uses InMemoryCache to cache single entities in API request
to get a MediaTitle (`GET /api/mediatitles/{id}`). Cached value will invalidated if corresponding item is deleted or updated. 
Later I could use distributed Redis cache, more complex caching strategies incorporating.

### Exception Handling

I prefer not to use exceptions in my apps because it's expensive and unnecessary in most cases.
To address this I would use custom tuple-like structure (like `Result<TValue,TError>`) 
for methods returning values with error object and value object inside.
Exceptions must be catched ASAP and wrapped in result with error.
Every result from function call must be checked for error instead of writing try-catch clauses.
There could be of course some complicated cases with async exception handling.

### Data Integrity Strategies

1. DB replication, backup and recovery 
2. Careful DB scheme modelling (normalization); Foreign keys with cascade constraints
3. Automatic audit of changes in DB (moving removed records to another table (or marking as removed) instead of deleting; even sourcing or more simple approach to record who/when/what changed in database)