# FlyAway – Airport Booking Management System

FlyAway is a containerized Airport / Flight Booking Management web application built using ASP.NET Core MVC and Entity Framework Core.  
The system supports full booking lifecycle operations (Create, View, Update, Delete), REST-compliant API endpoints, and ticket image generation.

This project was developed as part of a Semester 6 academic software engineering course and demonstrates layered architecture, database persistence, and Docker-based deployment.

---

## 1. System Overview

FlyAway allows users to:

- Register and authenticate accounts
- Browse available flights
- Create flight bookings with passenger information
- View and manage bookings
- Update bookings using PUT and PATCH
- Cancel bookings (DELETE)
- Generate and display downloadable ticket images
- Interact with REST API endpoints
- Run the entire system using Docker (Web + SQL Server)

The application demonstrates proper separation of concerns across UI, API, service, and data layers.

---

## 2. Architecture Overview

The system follows a layered architecture:

### Presentation Layer
- Razor Views (MVC)
- Bootstrap styling
- Custom CSS (wwwroot/css/site.css)
- UI actions invoke MVC controllers and API endpoints

### API Layer
- RESTful controllers under `/api/*`
- Supports HTTP verbs:
  - GET
  - POST
  - PUT
  - PATCH
  - DELETE
  - OPTIONS
- JSON request/response handling

### Business / Application Layer
- Controllers manage validation and request handling
- Services (e.g., TicketImageService, PasswordService)
- DTOs define structured API payloads

### Data Layer
- Entity Framework Core
- AppDbContext
- SQL Server 2022
- Migrations for schema evolution
- Seed data initialization

### File Generation Layer
- Ticket images generated using ImageSharp
- Stored under: Fly-Away/wwwroot/tickets/
- - URL persisted in database as `TicketImageUrl`

---

## 3. Technology Stack

Backend:
- ASP.NET Core 8 (MVC + Web API)
- C#
- Entity Framework Core
- SQL Server 2022

Frontend:
- Razor Views
- Bootstrap
- Custom CSS

Image Generation:
- SixLabors.ImageSharp

Database:
- Microsoft SQL Server
- EF Core Migrations

DevOps / Deployment:
- Docker
- Docker Compose

---

## 4. Project Structure
irport_Management_Group3/
│
├── Fly-Away.sln
├── docker-compose.yml
│
└── Fly-Away/
├── Controllers/
├── DTOs/
├── Data/
├── Models/
├── Services/
├── Views/
├── wwwroot/
│ ├── css/
│ ├── images/
│ ├── fonts/
│ └── tickets/
├── Migrations/
├── Program.cs
├── appsettings.json
└── Dockerfile


---

## 5. Running the Application with Docker (Recommended)

### Prerequisites
- Docker Desktop installed and running
- Ports available:
  - 8080 (Web)
  - 1433 (SQL Server)

### Step 1 – Build and Start Containers

From the repository root:
docker compose up -d --build

### Step 2 – Verify Containers


docker compose ps


Expected services:
- flyaway-web
- flyaway-db

### Step 3 – Access Application

Open in browser:

http://localhost:8080


### Step 4 – Stop Containers

docker compose down


### Reset Database (Fresh State)

docker compose down -v
docker compose up -d --build


---

## 6. Running Without Docker (Optional)

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full SQL Server)

### Update Connection String
Edit `appsettings.json` to point to your SQL instance.

### Apply Migrations
dotnet ef database update
### Run Application
dotnet run

---

## 7. Environment Configuration

Docker environment variables typically include:


ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Server=db,1433;Database=FlyAwayDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
SEED_DATA=true


---

## 8. REST API Endpoints

### Bookings API

Base Route:

/api/bookings


| Method  | Route | Description |
|----------|--------|-------------|
| GET | /api/bookings | Get logged-in user bookings |
| GET | /api/bookings/{id} | Get specific booking |
| POST | /api/bookings | Create booking |
| PUT | /api/bookings/{id} | Replace booking |
| PATCH | /api/bookings/{id} | Partial update |
| DELETE | /api/bookings/{id} | Delete booking |
| OPTIONS | /api/bookings | List supported methods |

---

### Flights API


/api/flights


- GET /api/flights
- GET /api/flights/search

---

### FlightClasses API


/api/flightclasses


- GET /api/flightclasses

---

### Authentication API


/api/auth


- POST /api/auth/register
- POST /api/auth/login
- POST /api/auth/logout
- GET /api/auth/me

---

## 9. Sample JSON Payloads

### Create Booking (POST)

```json
{
  "flight_ID": 1,
  "flightClass_ID": 1,
  "passengerName": "John Smith",
  "email": "john@example.com",
  "phoneNumber": "+1 519 555 1234",
  "passportNumber": "A1234567"
}
Update Booking (PUT)
{
  "flightClass_ID": 2,
  "passengerName": "John Smith",
  "email": "john@example.com",
  "phoneNumber": "+1 519 555 1234",
  "passportNumber": "A1234567"
}
Partial Update (PATCH)
{
  "flightClass_ID": 3
}
```
10. Database Persistence

Bookings are stored in SQL Server.

Ticket image path is stored in TicketImageUrl.

EF Core Migrations manage schema updates.

Data remains persistent across container restarts (unless volume removed).

11. Ticket Image Generation

Upon successful booking:

TicketImageService generates PNG ticket

File saved to:

wwwroot/tickets/ticket_{Ticket_ID}.png

URL stored in database

Displayed in:

My Bookings

Booking Details page

12. Testing Coverage

The system includes documented:

Application-level tests

Functional tests

Integration tests

REST compliance verification

Database persistence validation

All major CRUD flows were verified through containerized execution.

