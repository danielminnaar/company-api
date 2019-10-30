### README for Company project
##### -- by Daniel Minnaar (daniel.minnaar@gmail.com)

An overview of the solution:
## Software
* API written in C#, using dotnet core 3.0 Web API
* (Crude) REST client written in HTML and JS with JQuery
* Testing framework using xUnit
* Database server is Postgres
* Authentication using signed JWT

## Infrastructure
* Hosted on AWS EC2 Ubuntu 18.04
* Containerized API and DB with Docker
* NGINX for reverse proxy to Client and API

## How to use:
The API and REST client runtime is externally accessible:
* Client: http://ec2-52-34-75-216.us-west-2.compute.amazonaws.com/api-client/index.html
* API: http://ec2-52-34-75-216.us-west-2.compute.amazonaws.com/company
* Refer to the `company-api.yaml` OpenAPI definition for API reference (use http://editor.swagger.com to view)

The client provides basic functionality to interact with the company api, although additional endpoints are available on the API to call directly. All endpoints use the /company/ base URI.

You need to authenticate before interacting with most endpoints; this is achieved by making a POST to /authenticate using the following credentials as a JSON payload:
{
   "Username": "companyadmin",
   "Password": "password"
}

The REST client does all this for you, by clicking on the `Authenticate` button.

Use the /test endpoint to confirm a successful route to the controller that doesn't require authentication.

## How to build:
The company api source can be compiled locally using `dotnet run`, but this requires an environment variable for the connection string to be created on the local machine, and a running instance of the Postgres server. I strongly recommend using the docker-compose which will build the source and run all components for you.

* I've written a simple script to tear down and rebuild the containers, which can be executed with `sudo ./reset.sh`
