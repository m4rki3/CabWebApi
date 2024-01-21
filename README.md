# CabWebApi
---
This is backend cab ordering application on __ASP.NET Web API__ is developed as pet-project based on _Onion Architecture_.

_CabWebApi_ controllers provides _CRUD_ operations and simple business logic related with order making.

__Swagger__ is used for testing and interaction with controllers->actions.

## Content
---
- <a href="#installing">Installing</a>
	- <a href="#docker">Docker</a>
	- <a href="#without-docker">Without Docker</a>
- <a href="#developing">Developing</a>
	- <a href="#technologies">Technologies</a>
	- <a href="#dependencies">Dependencies</a>
- <a href="#testing">Testing</a>
- <a href="#database">Database</a>
- <a href="#about">About</a>
	- <a href="#team">Team</a>

<h2 id="installing">Installing</h2>

-----
<h3 id="docker">Docker</h3>

1. Clone _CabWebApi_ repository to your local machine:
	```
	git clone https://github.com/m4rki3/CabWebApi.git
	```

2. On the next step you need to open __Cmd/PowerShell__ and move to directory where _CabWebApi.sln_ file is located.

	This commands may be useful to move across directories in __Cmd/PS__:
	```
	cd ..
	cd 'absolute-or-relative-path-with-qoutes'
	```
3. Then you should up your docker compose, described in _CabWebApi/docker-compose.yml_, using this command:
	```
	docker compose up -d
	```
4. Go to [page](https://localhost/swagger/index.html) on your web browser, where you can see __Swagger__ html page.

__Note__

_I decided not to use volumes._

_That's why database do not save data and you may not get models you have registered after restarting._


<h3 id="without-docker">Without Docker</h3>

Project requires __NET 6.0__ version or higher.

Database context in API requires __Microsoft SQL Server 16.0.1000.6__ version or higher.

You may use __LocalDb__, __Express__ or __Development__ edition, which has to be installed on your OS.

1. Clone _CabWebApi_ repository to your local machine:
	```
	git clone https://github.com/m4rki3/CabWebApi.git
	```


2. Put your database connection string into application configuration in _CabWebApi/CabWebApi/appsettings.json_.
	```json
	"ConnectionStrings": {
	  "DefaultConnection": "YourConnectionString"
	},
	```
	For example:
	```json
	"ConnectionStrings": {
	  "DefaultConnection": "Data Source=DESKTOP-9TT9RV2,1433; Initial Catalog=Cab; Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False; User Id=sa; Password=cabpassword123;"
	},
	```

3. Check project dependencies on <a href="#dependencies">this heading</a> and if it will be needed to install use __NuGet__ or __PM__.

__Note__

_I used `Database.EnsureCreated();` in CabWebApi/CabWebApi.Infrastructure.Data/CabContext.cs, which provides creating of Cab database._

_If you want to use migrations you have to comment 21st line in this file._


<h2 id="developing">Developing</h2>

---

<h3 id="technologies">Technologies</h3>

- __.NET 6.0__
- __Microsoft SQL Server 16.0.1000.6__
- __ASP.NET Core__
- __Entity Framework Core__

On _Linux_ or _macOS_ it is recommended to use __Express__ or __Development__ editions of MS SQL Server instead of __LocalDb__.

<h3 id="dependencies">Dependencies</h3>

__NuGet__ packages included below must be installed for each project file.

- CabWebApi.csproj
	- Microsoft.EntityFrameworkCore.Design >= 7.0.14
	- Microsoft.EntityFrameworkCore.SqlServer >= 7.0.10
	- Microsoft.VisualStudio.Azure.Containers.Tools.Targets >= 1.19.4
	- Swashbuckle.AspNetCore.Swagger >= 6.5.0
	- Swashbuckle.AspNetCore.SwaggerGen >= 6.5.0
	- Swashbuckle.AspNetCore.SwaggerUI >= 6.5.0

- CabWebApi.Infrastructure.Data.csproj
	- Microsoft.EntityFrameworkCore >= 7.0.14
	- Microsoft.EntityFrameworkCore.Design >= 7.0.14
	- Microsoft.EntityFrameworkCore.Relational >= 7.0.14
	- Microsoft.EntityFrameworkCore.SqlServer >= 7.0.10
	- Microsoft.EntityFrameworkCore.Tools >= 7.0.14

- CabWebApi.Infrastructure.Business.csproj
	- Microsoft.AspNetCore.Authentication.Cookies >= 2.2.0

- CabWebApi.Domain.Interfaces.csproj
	- Microsoft.EntityFrameworkCore >= 7.0.12


<h2 id="testing">Testing</h2>

---

I decided to test my project using simple __Swagger__.

There are list of controllers and actions in API testing [page](https://localhost/swagger/index.html) on web application running.

You could put required data into actions and get _JSON_ response with status code.


<h2 id="database">Database</h2>

---

_Cab database_ consist of five tables related with models using __Entity Framework Core__: Cars, Drivers, Locations, Orders, Users.

Order table use _one-to-one_ relations with Users, Locations and Cars, which have _many-to-one_ relation with Driver table.

Location model adding was problematic because I could not know specialities of client sise.


<h2 id="about">About</h2>

---

<h3 id="team">Team</h3>

- Ilya Makovich - .NET Backend Developer

>One day I had an idea to create my own backend project.
I was inspired by Cab mobile applications, which are in use among a lot of people nowadays.