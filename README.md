# BankAccountManagerApi

Backend API in .Net


## Development

Start MySQL 8.4.3 with a personal password. Create volume to avoid data loss when container is stopped:

> docker run --name mysql -d -p 3306:3306 -e MYSQL_ROOT_PASSWORD=MySQLPassword -v mysql:/var/lib/mysql --restart unless-stopped mysql:8.4.3

Create Database and Create User

### Create database 

Connect and execute `MySQL-Creation-Script.sql` script


# appSettings

Enter the API Project folder

> cd ./src/BankAccountManager.Api/

Init Secrets

```
dotnet user-secrets init
```

### Setting Database connection in secrets


Create Database connection entry

```
dotnet user-secrets set "ConnectionStrings:MySQL" "Server=localhost;Database=BankAccount;User ID=BankAccountUser;Password=MySQLPassword;"
```

```
Similar to appSettings.json
	"ConnectionStrings": {
	  "WebAppDbContext": "Server=localhost;Database=BankAccount;User ID=BankAccountUser;Password=MySQLPassword;"
	}
```
	
### Cors
Alowed origins multiple orgins

```
dotnet user-secrets set "Cors:AllowedOrigins:0" "http://localhost:3000"
dotnet user-secrets set "Cors:AllowedOrigins:1" "http://localhost:3001"
```

```
Similar to appSettings.json
	"Cors": {
	  "AllowedOrigins": ["http://localhost:3000", "http://localhost:3001"]
	}
```

# Docker

## Build

```
docker build -f ./src/BankAccountManager.Api/Dockerfile ./ -t bank-account-manager/api:v20250228.1
```

## Run

> Get local IP from Docker to connect with MySQL. Ex.: ```192.168.1.193```
```
docker run -it -p 8080:8080 -e "ConnectionStrings:MySQL=Server=192.168.1.193;Database=BankAccount;User ID=BankAccountUser;Password=MySQLPassword;" bank-account-manager/api:v20250228.1
```
