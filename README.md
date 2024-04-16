# Right Place

Final project on the subject of **"Cloud-oriented web applications"**.

This is a rest api service for the real estate portal.
The project implements Clean Architecture, CQRS, MediatR, Repository pattern, custom JWT authorization, file storage in minIO object storage.


## 👷 Frameworks, Libraries and Technologies

- [.NET 7](https://github.com/dotnet/core)
- [ASP.NET Core 7](https://github.com/dotnet/aspnetcore)
- [Entity Framework Core 7](https://github.com/dotnet/efcore)
- [MediatR](https://github.com/jbogard/MediatR)
- [Mapster](https://github.com/MapsterMapper/Mapster)
- [IdentityModel](https://github.com/IdentityModel)
- [PostgreSQL](https://github.com/postgres)
- [MinIO](https://github.com/minio/minio-dotnet)
- [Bogus](https://github.com/bchavez/Bogus)
- [Asp.Versioning](https://github.com/dotnet/aspnet-api-versioning)
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Docker](https://github.com/docker)


## 🐳 List of containers

- **app** - container for all application layers


- **database** - postgresql database container


- **pgadmin** - graphical access to databases


- **minio** - store static files from users


## 🚜 How to run the server

<i>The first time the containers are launched, random data generation will be performed to check the functionality
(Bogus package).</i>

1. Build and start Docker images based on the configuration defined in the docker-compose.yml

        make up   # docker-compose up --build

2. Stop and remove containers

        make down  # docker-compose down



## 🔐 Local access

| container     | port | login       | password     | GUI                                      |
|---------------|------|-------------|--------------|------------------------------------------|
| minIO storage | 9001 | dev_user    | dev_password | http://localhost:9001/login              |
| database      | 5432 | dev_user    | dev_pwd      | -                                        |
| pgadmin       | 8080 | dev@dev.com | dev_pwd      | http://localhost:9000/login              |
| app           | 5000 | -           | -            | http://localhost:5000/swagger/index.html |     


## 🖨️ Swagger documentation

1. Swagger UI
    
        http://localhost:5000/swagger/index.html

2. Swagger static file
        
        ![swagger static file](https://github.com/gitEugeneL/RightPlace/main/swagger.json)

## 🔧 Implementation features

### Authentication

*Authentication is implemented using a JWT access token and refresh token.*

*AccessToken is used to authorize users, the refresh token is used to update a pair of tokens.*

*RefreshToken is recorded in the database and allows each user to have 5 active devices at the same time.*

#### Login
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/auth/login</b></code><code>(allows you to login, issues accessToken and sets refreshToken in cookies)</code>
</summary>

##### Body
> | name     | type       | data type    |                                                           
> |----------|------------|--------------|
> | email    | required   | string       |
> | password | required   | string       |

##### Responses
> | http code | content-type       | response                                               |
> |-----------|--------------------|--------------------------------------------------------|
> | `200`     | `application/json` | `{"type: "Bearer", "token": "eyJhbGciOi..........."}`  |
> | `403`     | `application/json` | `string`                                               |

##### Set Cookies
> | name         | example                                                              |                                                      
> |--------------|----------------------------------------------------------------------|
> | refreshToken | refreshToken=Wna@3da...; Expires=...; Secure; HttpOnly; Domain=...;` |
</details>

#### Refresh
<details>
<summary>
    <code>POST</code> <code><b>/api/v1/auth/refresh</b></code><code>(allows to refresh access and refresh tokens)</code>
</summary>

##### Required Cookies
> | name         | example                  |                                                      
> |--------------|--------------------------|
> | refreshToken | refreshToken=Wna@3da...; |

##### Responses
> | http code | content-type       | response                                              |
> |-----------|--------------------|-------------------------------------------------------|
> | `200`     | `application/json` | `{"type: "Bearer", "token": "eyJhbGciOi..........."}` |
> | `403`     | `application/json` | `string`                                              |

##### Set Cookies
> | name         | example                                                              |                                                      
> |--------------|----------------------------------------------------------------------|
> | refreshToken | refreshToken=Wna@3da...; Expires=...; Secure; HttpOnly; Domain=...;` |
</details>

#### Logout

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/auth/logout</b></code><code>(allows to logout and deactivates refresh token)</code>
</summary>

##### Body
> 1. Valid access JWT Bearer token in the header

##### Responses
> | http code | content-type                                    | response                        |
> |-----------|-------------------------------------------------|---------------------------------|
> | `200`     | `application/json` `and remove HttpOnly Cookie` | `No body returned for response` |
> | `401`     | `application/json`                              | `string`                        |
> | `403`     | `application/json`                              | `string`                        |
</details>

--------------------------------------------------------

### User

*Functionality that allows to manage and interact with users*

#### Register

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/users</b></code><code>(allows you to register)</code>
</summary>

##### Body
> | name     | type       | data type    |                                                           
> |----------|------------|--------------|
> | email    | required   | string       |
> | password | required   | string       |

##### Responses

> | http code | content-type       | response                                                                                                                                                         |
> |-----------|--------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json` | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "dateOfBirth": "2024-04-16"}` |
> | `409`     | `application/json` | `string`                                                                                                                                                         |
</details>

#### Get all users (*jwt required*)

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/users</b></code><code>(allows you to return the list of users)</code>
</summary>

##### Parameters
> | name       | type         | data type   |                                                           
> |------------|--------------|-------------|
> | PageNumber | not required | int32       |
> | PageSize   | not required | int32       |

##### Responses 

> | http code | content-type        | response                                                                                                                                                                                                                                 |
> |-----------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"items": [ {"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "dateOfBirth": "2024-04-16"} ], "pageNumber": 0, "totalPages": 0, "totalItemsCount": 0 }` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                                                 |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                                                 |
</details>

#### Get one user (*jwt required*)

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/users/{ userId:uuid }</b></code><code>(allows you to return user profile)</code>
</summary>

##### Responses

> | http code | content-type          | response                                                                                                                                                         |
> |-----------|-----------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`    | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "dateOfBirth": "2024-04-16"}` |
> | `401`     | `application/json`    | `string`                                                                                                                                                         |
> | `403`     | `application/json`    | `string`                                                                                                                                                         |
> | `404`     | `application/json`    | `string`                                                                                                                                                         |
</details>

#### Update your account (*jwt required*)

<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/users</b></code><code>(allows to update personal profile)</code>
</summary>

##### Body
> | name        | type         | data type    |                                                           
> |-------------|--------------|--------------|
> | firstName   | not required | string       |
> | lastName    | not required | string       |
> | phone       | not required | string       |
> | dateOfBirth | not required | string       |

##### Responses

> | http code | content-type        | response                                                                                                                                                         |
> |-----------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "dateOfBirth": "2024-04-16"}` |
> | `401`     | `application/json`  | `string`                                                                                                                                                         |
> | `403`     | `application/json`  | `string`                                                                                                                                                         |
> | `404`     | `application/json`  | `string`                                                                                                                                                         |
</details>

#### Delete your account (*jwt required*)

<details>
<summary>
    <code>DELETE</code> <code><b>/api/v1/users</b></code><code>(allows to delete personal profile)</code>
</summary>

##### Responses

> | http code | content-type          | response     |
> |-----------|-----------------------|--------------|
> | `204`     | `application/json`    | `No Content` |
> | `401`     | `application/json`    | `string`     |
> | `403`     | `application/json`    | `string`     |
> | `404`     | `application/json`    | `string`     |
</details>

--------------------------------------------------------

### Advert 

*Functionality that allows to manage and interact with adverts*

#### Create new advert (*jwt required*)

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/advertisements</b></code><code>(allows you to create an advert)</code>
</summary>

##### Body
> | name        | type       | data type |                                                           
> |-------------|------------|-----------|
> | categoryId  | required   | uuid      |
> | typeId      | required   | uuid      |
> | title       | required   | string    |
> | description | required   | string    |
> | price       | required   | int       |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                                                              |
> |-----------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json` | `{ "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "description": "string", "price": 0, "images": [ "string" ], "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "typeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "informationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "created": "2024-04-16T16:35:50.556Z", "updated": "2024-04-16T16:35:50.556Z"}` |
> | `401`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                                                              |
> | `403`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                                                              |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                                                              |
> | `409`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                                                              |
</details>

#### Update you advert (*jwt required*)

<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/advertisements</b></code><code>(allows you to create an advert)</code>
</summary>

##### Body
> | name        | type       | data type |                                                           
> |-------------|------------|-----------|
> | advertId    | required   | uuid      |
> | description | required   | string    |
> | price       | required   | int       |

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                                                              |
> |-----------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{ "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "description": "string", "price": 0, "images": [ "string" ], "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "typeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "informationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "created": "2024-04-16T16:35:50.556Z", "updated": "2024-04-16T16:35:50.556Z"}` |
> | `401`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                                                              |
> | `403`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                                                              |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                                                              |
</details>

#### Get all adverts

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/advertisements</b></code><code>(allows you to get all adverts)</code>
</summary>

##### Parameters
> | name         | type         | data type |                                                           
> |--------------|--------------|-----------|
> | CategoryId   | not required | uuid      |
> | TypeId       | not required | uuid      |
> | OwnerId      | not required | uuid      |
> | SortOrderAsc | not required | boolean   |
> | City         | not required | string    |
> | SortBy       | not required | string    |
> | PageNumber   | not required | int32     |
> | PageSize     | not required | int32     |

##### Responses
> | http code | content-type          | response                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
> |-----------|-----------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`    | `{"items": [ { "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "description": "string", "price": 0, "images": [ "string" ], "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "typeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "informationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "created": "2024-04-16T16:43:32.440Z", "updated": "2024-04-16T16:43:32.440Z"} ], "pageNumber": 0, "totalPages": 0, "totalItemsCount": 0 }`  |
</details>

#### Get one advert

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/advertisements{ advertId:uiid }</b></code><code>(allows you to get one advert)</code>
</summary>

##### Responses
> | http code | content-type       | response                                                                                                                                                                                                                                                                                                                                                                                                                              |
> |-----------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json` | `{ "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "title": "string", "description": "string", "price": 0, "images": [ "string" ], "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "typeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "addressId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "informationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "created": "2024-04-16T16:35:50.556Z", "updated": "2024-04-16T16:35:50.556Z"}` |
> | `404`     | `application/json` | `string`                                                                                                                                                                                                                                                                                                                                                                                                                              |
</details>


#### Delete your advert (*jwt required*)

<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/advertisements/{ advertisementId:uuid }</b></code><code>(allows you to delete your advert)</code>
</summary>

##### Responses
> | http code | content-type          | response     |
> |-----------|-----------------------|--------------|
> | `204`     | `application/json`    | `No Content` |
> | `401`     | `application/json`    | `string`     |
> | `403`     | `application/json`    | `string`     |
> | `404`     | `application/json`    | `string`     |
</details>

--------------------------------------------------------

### Address

*Each User can add an address to their adverts*

#### Add address data for advert (*jwt required*)

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/addresses</b></code><code>(allows you to add address data for advert)</code>
</summary>

##### Body
> | name        | type       | data type |                                                           
> |-------------|------------|-----------|
> | advertId    | required   | uuid      |
> | city        | required   | string    |
> | street      | required   | string    |
> | province    | required   | string    |
> | house       | required   | string    |
> | gpsPosition | required   | string    |

##### Responses
> | http code | content-type        | response                                                                                                                                                                                                     |
> |-----------|---------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`  | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "advertId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "city": "string", "street": "string", "province": "string", "house": "string", "gpsPosition": "string"}` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                     |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                     |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                     |
> | `409`     | `application/json`  | `string`                                                                                                                                                                                                     |
</details>


#### Update address data for advert (*jwt required*)

<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/addresses</b></code><code>(allows you to update address data for advert)</code>
</summary>

##### Body
> | name        | type      | data type |                                                           
> |-------------|-----------|-----------|
> | advertId    | required  | uuid      |
> | city        | required  | string    |
> | street      | required  | string    |
> | province    | required  | string    |
> | house       | required  | string    |
> | gpsPosition | required  | string    |


> ##### Responses
> | http code | content-type        | response                                                                                                                                                                                                     |
> |-----------|---------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`  | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "advertId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "city": "string", "street": "string", "province": "string", "house": "string", "gpsPosition": "string"}` |
> | `401`     | `application/json`  | `string`                                                                                                                                                                                                     |
> | `403`     | `application/json`  | `string`                                                                                                                                                                                                     |
> | `404`     | `application/json`  | `string`                                                                                                                                                                                                     |
</details>

#### Get address data

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/address/{ addressId:uuid }</b></code><code>(allows you to return address data)</code>
</summary>

##### Responses
> | http code | content-type          | response                                                                                                                                                                                                     |
> |-----------|-----------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`    | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "advertId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "city": "string", "street": "string", "province": "string", "house": "string", "gpsPosition": "string"}` |
> | `404`     | `application/json`    | `string`                                                                                                                                                                                                     |
</details>

--------------------------------------------------------

### Information

*Each user can add detailed information to their advert*

#### Add information for advert (*jwt required*)

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/information</b></code><code>(allows you to add information for advert)</code>
</summary>

##### Body
> | name                   | type      | data type |                                                           
> |------------------------|-----------|-----------|
> | advertId               | required  | uuid      |
> | roomCount              | required  |  int      |
> | area                   | required  | int       |
> | floor                  | required  | int       |
> | elevator               | required  | boolean   |
> | balcony                | required  | boolean   |
> | yearOfConstruction     | required  | string    |
> | energyEfficiencyRating | required  | string    |

##### Responses
> | http code | content-type          | response                                                                                                                                                                                                                                    |
> |-----------|-----------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `201`     | `application/json`    | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "advertId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "roomCount": 0, "area": 0, "yearOfConstruction": 0, "elevator": true, "balcony": true, "floor": 0, "energyEfficiencyRating": "string"}` |
> | `401`     | `application/json`    | `string`                                                                                                                                                                                                                                    |
> | `403`     | `application/json`    | `string`                                                                                                                                                                                                                                    |
> | `404`     | `application/json`    | `string`                                                                                                                                                                                                                                    |
> | `409`     | `application/json`    | `string`                                                                                                                                                                                                                                    |
</details>

#### Update your advert information (*jwt required*)

<details>
<summary>
    <code>PUT</code> <code><b>/api/v1/information</b></code><code>(allows to update information for advert)</code>
</summary>

##### Body
> | name                   | type         | data type |                                                           
> |------------------------|--------------|-----------|
> | informationId          | required     | uuid      |
> | roomCount              | not required | int       |
> | floor                  | not required | int       |
> | yearOfConstruction     | not required | string    |
> | energyEfficiencyRating | not required | string    |
> | elevator               | not required | boolean   |
> | balcony                | not required | boolean   |

##### Responses
> | http code | content-type        | response                                                                                                                                                         |
> |-----------|---------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`  | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "dateOfBirth": "2024-04-16"}` |
> | `401`     | `application/json`  | `string`                                                                                                                                                         |
> | `403`     | `application/json`  | `string`                                                                                                                                                         |
> | `404`     | `application/json`  | `string`                                                                                                                                                         |
</details>

#### Get information

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/information/{ informationId:uuid }</b></code><code>(allows you to return information for advert)</code>
</summary>

##### Responses
> | http code | content-type          | response                                                                                                                                                          |
> |-----------|-----------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`    | `{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "email": "string", "firstName": "string", "lastName": "string", "phone": "string", "dateOfBirth": "2024-04-16"}`  |
> | `404`     | `application/json`    | `string`                                                                                                                                                          |
</details>

--------------------------------------------------------

### Category

*Each User can get a list of available categories (house, apartment, etc.)*

#### Get all categories

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/categories</b></code><code>(allows you to return all categories)</code>
</summary>

##### Responses
> | http code | content-type          | response                                                                                          |
> |-----------|-----------------------|---------------------------------------------------------------------------------------------------|
> | `200`     | `application/json`    | `[ { "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string", "description": "string" } ]` |
</details>

--------------------------------------------------------

### Type

*Each user can get a list of available types (buy, rent and so on)*

#### Get all advert types

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/types</b></code><code>(allows you to return all types)</code>
</summary>

##### Responses
> | http code | content-type          | response                                                                 |
> |-----------|-----------------------|--------------------------------------------------------------------------|
> | `200`     | `application/json`    | `[ { "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "string" } ]` |
</details>

--------------------------------------------------------

### Images

*Each user creating an advert can upload up to 5 images for each advert.
This user can also update and delete them*

#### Upload images (*jwt required*)

<details>
<summary>
    <code>POST</code> <code><b>/api/v1/files/upload-image/{ advertId:uuid }</b></code><code>(allows you to upload images to you adverts)</code>
</summary>

##### Body
> | name | type         | data type |                                                           
> |------|--------------|-----------|
> | file | required     | image/png |

##### Responses
> | http code | content-type          | response   |
> |-----------|-----------------------|------------|
> | `200`     | `application/json`    | `"string"` |
> | `401`     | `application/json`    | `"string"` |
> | `403`     | `application/json`    | `"string"` |
> | `404`     | `application/json`    | `"string"` |
> | `409`     | `application/json`    | `"string"` |
</details>

#### Download images

<details>
<summary>
    <code>GET</code> <code><b>/api/v1/files/download-image/{ advertId:uuid }/{ imageName:string }</b></code><code>(allows you to download images to you adverts)</code>
</summary>

##### Responses
> | http code | content-type          | response       |
> |-----------|-----------------------|----------------|
> | `200`     | `multipart/form-data` | `"immage/png"` |
> | `404`     | `application/json`    | `"string"`     |
</details>

#### Delete your images (*jwt required*)

<details>
<summary>
    <code>DELETE</code> <code><b>/api/v1/files/delete-image/{ advertId:uuid }/{ imageName:uuid }</b></code><code>(allows you to delete your images)</code>
</summary>

##### Responses
> | http code | content-type          | response     |
> |-----------|-----------------------|--------------|
> | `204`     | `application/json`    | `No Content` |
> | `401`     | `application/json`    | `string`     |
> | `403`     | `application/json`    | `string`     |
> | `404`     | `application/json`    | `string`     |
</details>

--------------------------


## 💾 Database diagram

![Database diagram](https://github.com/gitEugeneL/RightPlace/blob/main/database-diagram.png)
