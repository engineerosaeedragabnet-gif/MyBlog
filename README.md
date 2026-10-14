# ✍️ MyBlog

A full-featured blog application built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**. Users can publish posts with images, organize them with categories and tags, and discuss them through comments.

## ✨ Features

- **Posts CRUD**: create, read, update, and delete posts
- **Image Upload**: with file type (jpg, jpeg, png, webp) and size (max 2 MB) validation, unique file names via `Guid`, and automatic cleanup of old images
- **Categories**: one-to-many relationship with posts, with delete protection when a category has posts
- **Tags**: many-to-many relationship with posts, unique tag names, and filtering by tag
- **Comments**: add and delete comments on each post, with server-side validation
- **Search**: search posts by title or content
- **Filtering and Sorting**: filter by category or tag, sort by newest, oldest, or title
- **Pagination**: preserves search, filter, and sort state across pages
- **Home Page**: latest posts and categories with post counts
- **Custom 404 Page**
- **Security**: Anti-Forgery Tokens on all POST actions, ViewModels to prevent over-posting, and validation on both client and server

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|------------|
| Framework | ASP.NET Core MVC (.NET) |
| ORM | Entity Framework Core (Code First) |
| Database | SQL Server |
| Frontend | Razor Views, Bootstrap 5, jQuery Validation |
| Tools | Visual Studio, Git, GitHub |

---

## 🗂️ Project Structure

```
MyBlog/
├── Controllers/    # Home, Posts, Categories, Tags, Comments
├── Models/         # Post, Category, Tag, Comment
├── ViewModels/     # PostFormViewModel, PostListViewModel, ...
├── Data/           # AppDbContext
├── Views/          # Razor views
├── Migrations/     # EF Core migrations
└── wwwroot/        # Static files and uploaded images
```

---

## 🧩 Database Design

- **Category** 1 ──── * **Post**
- **Post** 1 ──── * **Comment**
- **Post** * ──── * **Tag** (via the `PostTag` join table)

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (the version used by the project)
- SQL Server or SQL Server Express
- Visual Studio 2022 or later (or VS Code)

### Installation

1. **Clone the repository**
```bash
   git clone https://github.com/YOUR_USERNAME/MyBlog.git
   cd MyBlog
```

2. **Update the connection string** in `appsettings.json`
```json
   "ConnectionStrings": {
     "dbContext": "Server=localhost\\SQLEXPRESS;Database=MyBlogManagement;Trusted_Connection=True;TrustServerCertificate=True;"
   }
```

3. **Apply the migrations**

   Package Manager Console:
```
   Update-Database
```
   or CLI:
```bash
   dotnet ef database update
```

4. **Run the project**
```bash
   dotnet run
```

5. Open the URL shown in the terminal (for example `https://localhost:5001`).

> The first time you run the app, create at least one **Category** before adding posts.

---

## 🧠 What I Learned

- Building CRUD applications with the MVC pattern
- Modeling one-to-many and many-to-many relationships in EF Core
- Safe file uploads and validation
- Using ViewModels and protecting against over-posting
- Building queries with `IQueryable` (search, filter, sort, pagination)
- Delete behaviors and data integrity (`DeleteBehavior.Restrict`)
- The Post/Redirect/Get pattern and `TempData`

---

## 🔮 Future Improvements

- Authentication and authorization with ASP.NET Identity (Admin / User roles)
- Rich text editor for post content
- Refactor to the Repository pattern and Clean Architecture
- REST API with JWT authentication
- Unit tests with xUnit

---

## 👤 Author
Saeed Ragab

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
