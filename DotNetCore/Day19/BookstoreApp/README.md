# 📖 PageTurner — ASP.NET Core Online Bookstore

A full-featured ASP.NET Core 8.0 application implementing all 5 user stories from the Wipro NGA .NET Cohort assignment.

---

## 🚀 Quick Start

```bash
# Requires .NET 8 SDK
dotnet restore
dotnet run
# Visit: https://localhost:5001
```

**Demo Accounts:**
| Role | Username | Password |
|------|----------|----------|
| Admin | `admin` | `Admin@123` |
| Customer | `john_doe` | `John@123` |

---

## 🏗️ Project Architecture

```
BookstoreApp/
├── Attributes/           # Custom validation attributes
│   └── ValidationAttributes.cs
│       ├── ValidIsbnAttribute     — ISBN-10/13 format + checksum
│       └── ValidBookPriceAttribute — Range & decimal-place check
│
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── BooksController.cs        — Public catalog (attribute-routed /catalog/*)
│   ├── AdminBooksController.cs   — Admin CRUD (/admin/books/*)
│   ├── CartController.cs         — Session-based cart (/cart/*)
│   ├── OrdersController.cs       — Checkout & order history
│   └── AuthController.cs         — Login/Register/Logout
│
├── Data/
│   └── BookstoreDbContext.cs      — EF Core InMemory with seeded data
│
├── Filters/              # Custom MVC Filters
│   └── Filters.cs
│       ├── GlobalExceptionFilter    — Catches all unhandled exceptions
│       ├── RequestLoggingFilter     — Logs every request/response
│       ├── AdminOnlyFilter          — Role-based access (Admin)
│       └── SessionValidationFilter  — Validates active session
│
├── Models/
│   ├── Book.cs                    — Book entity with validation attributes
│   └── UserOrderCart.cs           — AppUser, Order, OrderItem, CartItem
│
├── Repositories/         # Repository Pattern
│   └── Repositories.cs
│       ├── IBookRepository / BookRepository
│       └── IOrderRepository / OrderRepository
│
├── Services/
│   └── CartService.cs             — Session-backed cart operations
│
├── Views/                # Razor Views (MVC)
│   ├── Home/           Index, Error
│   ├── Books/          Index (catalog), Details
│   ├── AdminBooks/     Index, Create, Edit
│   ├── Cart/           Index
│   ├── Orders/         Checkout, Confirmation, Index, AdminAll
│   ├── Auth/           Login, Register
│   └── Shared/         _Layout.cshtml
│
└── Program.cs            — DI, middleware, routing configuration
```

---

## 📋 User Story Implementation

### User Story 1: Book Management System
- **Razor Pages** (extendable via `/Pages/` folder) for add/edit/delete
- **MVC Controller** `BooksController` → `/catalog` — list & detail views
- **MVC Controller** `AdminBooksController` → `/admin/books` — admin CRUD
- **Custom Attributes:**
  - `ValidIsbnAttribute` — validates ISBN-10 (with checksum) and ISBN-13
  - `ValidBookPriceAttribute` — range check + 2 decimal place enforcement

### User Story 2: User Authentication & Authorization
- Cookie authentication (`AddAuthentication().AddCookie(...)`)
- Role-based claims: `Admin` / `Customer`
- `AdminOnlyFilter` (IAuthorizationFilter) restricts admin controllers
- Session stores `UserId`, `Username`, `UserRole` alongside cookie claims
- Login/Register via `AuthController` with anti-forgery tokens

### User Story 3: Shopping Cart & Order Processing
- `CartService` — serializes cart to JSON in `ISession`
- Cart persists across requests (30-min idle timeout)
- `OrdersController` — Checkout → PlaceOrder → Confirmation flow
- Stock deduction on order placement
- Orders saved to EF Core with full item details

### User Story 4: Custom Validations & Filters
| Filter | Type | Scope |
|--------|------|-------|
| `GlobalExceptionFilter` | IExceptionFilter | Global (all MVC) |
| `RequestLoggingFilter` | IActionFilter | Global (all MVC) |
| `AdminOnlyFilter` | IAuthorizationFilter | Per-controller |
| `SessionValidationFilter` | IActionFilter | Per-controller |

### User Story 5: Advanced Routing & Best Practices
- **Attribute routing**: `/catalog`, `/catalog/details/{id}`, `/admin/books`, `/login`, etc.
- **Convention routing**: `admin/{controller}/{action}/{id?}` pattern
- **Repository Pattern**: `IBookRepository`, `IOrderRepository` — data access isolated from controllers
- **Dependency Injection**: All services, repositories, filters registered via `builder.Services`
- **Separation of Concerns**: Models → Repositories → Services → Controllers → Views
- **Anti-forgery tokens** on all POST forms
- **HTTPS redirection** + HSTS in production

---

## 🔒 Security Features
- Cookie auth with 2-hour sliding expiration
- HTTP-only, secure session cookies
- Anti-forgery token validation on all POST endpoints
- Role-based access control via claims + filters
- Password hashing (SHA-256 + salt; upgrade to BCrypt for production)
- Input validation on all model properties

---

## 🧩 Design Patterns Used
| Pattern | Where |
|---------|-------|
| Repository Pattern | `BookRepository`, `OrderRepository` |
| Dependency Injection | All services via constructor injection |
| Filter Pipeline | Cross-cutting concerns (logging, auth, exceptions) |
| Service Layer | `CartService` for cart business logic |
| MVC | Controllers + Views for all features |

---

## ⚠️ Production Checklist
- [ ] Replace `UseInMemoryDatabase` with SQL Server / PostgreSQL
- [ ] Replace SHA-256 password hash with BCrypt (`BCrypt.Net-Next`)
- [ ] Add `[DataProtection]` for anti-forgery in load-balanced environments
- [ ] Enable HTTPS only (`RequireHttpsMetadata = true`)
- [ ] Add unit tests for custom attributes and filters
- [ ] Implement refresh tokens / remember-me

---

## 📦 NuGet Packages
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.Cookies" Version="2.2.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```
