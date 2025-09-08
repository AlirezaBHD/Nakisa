<img width="1280" height="720" alt="Untitled Design (2)" src="https://github.com/user-attachments/assets/e9db4eb6-4f9a-4767-a96c-7f1abcd358d1" />

**Nakisa** is a Telegram bot that builds structured music communities around playlists.  
Users can join categorized playlists, share songs, promote their channels, and connect with people who share the same music taste.  
  
---

## 🚀 Features

- 🔹 User registration via Telegram
- 🔹 Join categorized playlists
- 🔹 Submit songs and mention your Telegram ID or channel
- 🔹 React to songs shared by others
- 🔹 Community-driven channel promotion
- 🔹 Discover and connect with people who share your music taste

---

## 🛠 Tech Stack

- **.NET 9** (ASP.NET Core, Clean Architecture)
- **PostgreSQL** as the database
- **Entity Framework Core** for ORM and migrations
- **Telegram Bot API**
- Clean Architecture with the following layers:
  - `API`
  - `Application`
  - `Domain`
  - `Infrastructure`
  - `Persistence`
  - `SharedKernel`
  - `Tests`

---

## ⚡ Getting Started

### 1️⃣ Clone the repository

```bash
git clone https://github.com/username/nakisa.git
cd nakisa
```

### 2️⃣ Configure environment variables
Create a `.env` file (or use `appsettings.json`) with:
```
ConnectionStrings__DefaultConnection="Host=host;Port=5432;Database=nakisa;Username=postgres;Password=postgres"  
TelegramBot__Token=Your Bot Token  
TelegramClient__ApiHash=Your Telegram Api Hash  
TelegramClient__ApiId=Your Telegram Api Id  
TelegramClient__PhoneNumber=+111111111111  
```
### 3️⃣ Run the database

Using Docker:

`docker-compose up -d`

### 4️⃣ Apply migrations

`dotnet ef database update --project Nakisa.Persistence --startup-project Nakisa.API`

### 5️⃣ Run the project

`dotnet run --project Nakisa.API`

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!  
Feel free to check the [issues page](https://github.com/AlirezaBHD/Nakisa/issues).

1. Fork it
  
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
  
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
  
4. Push to the branch (`git push origin feature/amazing-feature`)
  
5. Open a Pull Request
  

---

## 📜 License

This project is licensed under the MIT License.
