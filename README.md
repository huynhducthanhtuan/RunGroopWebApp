## RunGroup

##### Practice ASP.NET Core Web MVC (.NET 6.0) Project

<details><summary><b>Installation Guide</b></summary>

#### 1. Clone repository

```bash
git clone https://github.com/huynhducthanhtuan/practice-nestjs-typescript.git
```

#### 2. Import SQL Server database from `RunGroups.bak` file

#### 3. Update SQL Server database connection string

`appsettings.json`

```bash
"ConnectionStrings": {
  "DefaultConnection": "Data Source=THANHTUAN;Initial Catalog=RunGroups;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
}
```

##### Change `THANHTUAN` with your computer name

#### 4. Register [Cloudinary](https://cloudinary.com/) account and update Cloudinary config

`appsettings.json`

```bash
"CloudinarySetting": {
  "CloudName": "",
  "ApiKey": "",
  "ApiSecret": ""
}
```

##### Update above config with your Cloudinary config

#### 5. Register [IPInfo](https://ipinfo.io/) account and update IPInfo token in API URL

`Controllers/HomeController.cs`

```bash
string url = "https://ipinfo.io?token=IPInfo-Token";
```

##### Change `IPInfo-Token` with your IPInfo token

#### 6. Run project

</details>

<details><summary><b>Exception</b></summary>

##### If Step 2 fails, you can do it manually like this

##### Create SQL Server database named `RunGroups`

##### Update SQL Server database connection string (Step 3)

##### Add Migration (Open Package Manager Console)

```bash
Add-Migration Initialize
```

##### Update Database (Open Package Manager Console)

```bash
Update-Database
```

##### Seed Data (Open Terminal)

```bash
dotnet run seeddata
```

</details>

### Reference Resources

[ASP.NET Core MVC Course](https://www.youtube.com/playlist?list=PL82C6-O4XrHde_urqhKJHH-HTUfTK6siO/)

[RunGroup Repository](https://github.com/teddysmithdev/RunGroop/)

[ChatGPT](https://chat.openai.com/)
