# Right Place

**5th semester.**

Final project on the subject of **Cloud-oriented web applications**.

This is a rest api service for the real estate portal.

The project implements a clean architecture and separation into logical layers 
(Domain -> Application -> Infrastructure -> Api), CQRS pattern, Repository pattern, JWT authorization with refresh token, storing static files in minIO (S3-like storage).

<i>The main technologies used: Docker, PostgreSQL, MinIO file storage, .NET 7, Entity Framework, ASP.NET.</i>


## List of containers

* **minio** - store static files from users.
    

* **pgadmin** - graphical access to databases.
    

* **database** - postgresql database container.


* **app** - container for all application layers.

## Local access

| container     | port | login       | password     | GUI                                      |
|---------------|------|-------------|--------------|------------------------------------------|
| minIO storage | 9001 | dev_user    | dev_password | http://localhost:9001/login              |
| database      | 5432 | dev_user    | dev_pwd      | -                                        |
| pgadmin       | 8080 | dev@dev.com | dev_pwd      | http://localhost:9000/login              |
| app           | 5000 | -           | -            | http://localhost:5000/swagger/index.html |     


## How to run the server

<i>The first time the containers are launched, random data generation will be performed to check the functionality
(Bogus package).</i>


1. Build and start Docker images based on the configuration defined in the docker-compose.yml.

        make up     // docker-compose up --build

2. Stop and remove containers.

        make down   // docker-compose down


## API documentation

   1. Swagger documentation

           http://localhost:5000/swagger/index.html


## Database diagram

![Database diagram](https://github.com/gitEugeneL/RightPlace/blob/main/database-diagram.png)



## Implementation features

* **Authentication with refresh token:**

   + Authentication is implemented using a JWT access token and refresh token. 
The access token is used to authorize the user, the refresh token is used to update a pair of tokens. 
The refresh token is recorded in the database and allows each user to have 5 active devices at the same time.
     - **Actions:** Login, Refresh, Logout


* **File Storage:**
  + Each user who creates an ad can upload up to 5 images for each ad. 
  He can also update and delete them. 
  Other users receive images when viewing an ad.
      - **Actions:** Upload, Download, Delete

* **User:**
  + Functionality that allows to manage and interact with users.
    - **Actions:** Create, Get, Get all with pagination, Update, Delete 


* **Category:**
   + The user can get a list of available categories (House, Apartment and so on)
      - **Actions:** Get all


* **Type:**
   + The user can get a list of available types (buy, rent and so on)
      - **Actions:** Get all


* **Advert:**
   + Functionality that allows to manage and interact with adverts

     - **Actions:** Create, Update, Delete, Get, Get all with pagination and numerous filters (address, gps, price, information, area, type, category and so on)
     - <i>Related entities will be deleted during deletion: Information, Address, Images, Images in MinIO storage</i>


* **Address:**

  + The user can add an address to their ad
    - **Actions:** Create, Get, Update.


* **Information:**

  + The user can add detailed information about the property to their ad
    - **Actions:** Create, Get, Update.
