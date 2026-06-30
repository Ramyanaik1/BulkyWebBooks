# BulkyWebBooks - Book Store Management System

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4)
![C#](https://img.shields.io/badge/C%23-239120)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927)
![License](https://img.shields.io/badge/license-MIT-green)

A full-featured online book store management system built with ASP.NET Core MVC, supporting product catalog management, role-based access, shopping cart, and order processing.

![BulkyWebBooks Home Page](Screenshots/home.png)

## 🚀 Features

### 📚 Product & Category Management
- **Product CRUD** - Create, update, and delete books with title, ISBN, author, and description
- **Tiered Pricing** - Configure different price brackets (1-50, 50+, 100+ units) for bulk orders
- **Category Management** - Organize books into categories for easy browsing
- **Image Upload** - Attach cover images to each product

### 🔐 Authentication & Roles
- **User Registration & Login** - Secure account creation with ASP.NET Identity
- **Role-Based Access Control** - Separate roles for Admin, Employee, Customer Admin, and Company
- **Company Accounts** - Special pricing and workflows for registered company users

### 🛒 Shopping & Orders
- **Shopping Cart** - Add, update, and remove items before checkout
- **Order Processing** - Track orders from placement through fulfillment
- **Order History** - View past orders and current order status

### 📊 Admin Dashboard
- **Product Table View** - Manage all listed books with quick Edit/Details/Delete actions
- **User Management** - Admins can manage registered users and their roles
- **Content Management** - Centralized control panel for categories and products

## 🛠️ Technologies Used

### Backend

| Technology | Purpose |
|---|---|
| ASP.NET Core MVC | Web application framework |
| Entity Framework Core | ORM for database operations |
| SQL Server | Relational database |
| ASP.NET Identity | Authentication & authorization |
| C# | Core programming language |

### Frontend

| Technology | Purpose |
|---|---|
| Razor Views | Server-rendered UI |
| Bootstrap | Responsive UI framework |
| JavaScript | Client-side interactivity |
| HTML5/CSS3 | Markup and styling |

## 📁 Project Structure
BulkyWebBooks/

├── BulkyWebBooks/

│   ├── Controllers/          # MVC Controllers (Product, Category, Cart, Order, Account)

│   ├── Models/                # Entity models (Product, Category, ApplicationUser, Order)

│   ├── Views/                 # Razor views for each controller

│   ├── Data/                  # DbContext and database configuration

│   ├── wwwroot/                # Static files (CSS, JS, images, uploaded product images)

│   ├── appsettings.json        # Configuration settings

│   └── Program.cs              # Application entry point

│

├── Screenshots/                # Application screenshots

├── .gitignore

└── README.md

## 🔧 Installation & Setup

### Prerequisites
- .NET 6.0 SDK or higher
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 (recommended) or VS Code

### Step 1: Clone the Repository
```bash
git clone https://github.com/Ramyanaik1/BulkyWebBooks.git
cd BulkyWebBooks
```

### Step 2: Configure the Database Connection
Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BulkyWebBooksDb;Trusted_Connection=True;"
}
```

### Step 3: Apply Migrations
```bash
dotnet ef database update
```

### Step 4: Run the Application
```bash
dotnet run
```

The application will be available at:
- **Local:** `https://localhost:7200` (or the port shown in your terminal)

## 📱 How to Use

### 1. Register an Account
- Navigate to the **Register** page
- Enter your name, email, password, role, and contact details
- Submit to create your account

### 2. Login
- Use your registered credentials to log in
- You'll be redirected based on your assigned role

### 3. Browse & Shop
- Explore the book collection from the **Home** page
- View book details and add items to your **Cart**
- Proceed to checkout to place an order

### 4. Manage Products (Admin/Employee)
- Navigate to **Content Management → Products**
- Click **Add New Product** to create a new listing
- Use **Edit**, **Details**, or **Delete** to manage existing products

### 5. Manage Categories (Admin)
- Navigate to **Category** management
- Add, edit, or remove book categories used across the catalog

## 🖥️ Application Screenshots

### Home Page
Browse the full book collection with pricing and quick view details.
![Home](Screenshots/home.png)

### Product Management
Update existing product details including pricing tiers and cover image.
![Products](Screenshots/products.png)

### Create Product
Add new books to the catalog with title, ISBN, author, and category.
![Categories](Screenshots/categories.png)

### Register Page
Secure account creation with role selection.
![Login](Screenshots/login.png)

### Admin Product Table
Manage all listed products with quick actions for edit, details, and delete.
![Cart](Screenshots/cart.png)

## 🔒 Security Features

- **Password Hashing** - User passwords are hashed via ASP.NET Identity, never stored in plain text
- **Role-Based Authorization** - Controller actions restricted by `[Authorize(Roles = "...")]` attributes
- **Input Validation** - Server-side model validation on all forms
- **Anti-Forgery Tokens** - CSRF protection on all POST requests

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👨‍💻 Author

**Ramya Umesh Naik**
- GitHub: [@Ramyanaik1](https://github.com/Ramyanaik1)

## 🙏 Acknowledgments

- ASP.NET Core documentation and community
- Bootstrap for the responsive UI framework
- Entity Framework Core for simplified data access

---
Made with ❤️ for book lovers everywhere
