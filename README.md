# BankAccountManagerApi

Backend API in .Net


## Development

Start MySQL 8.4.3 with a personal password. Create volume to avoid data loss when container is stopped:

> docker run --name mysql -d -p 3306:3306 -e MYSQL_ROOT_PASSWORD=mysql_pws -v mysql:/var/lib/mysql --restart unless-stopped mysql:8.4.3

Create Database and Create User

### Create database 

Connect and execute `MySQL-Creation-Script.sql` script


### Setting Database connection in secrets

Enter the API Project folder

> cd ./src/BankAccountManager.Api/

Init Secrets

> dotnet user-secrets init

Create Database connection entry

> dotnet user-secrets set "ConnectionStrings:MySQL" "Server=localhost;Database=BankAccount;User ID=BankAccountUser;Password=MySQLPassword;"


Similar to appSettings.json
	"ConnectionStrings": {
	  "WebAppDbContext": "Server=localhost;Database=BankAccount;User ID=BankAccountUser;Password=MySQLPassword;"
	}
	

