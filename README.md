# ✈️ Fly-Away — Flight Booking System (ASP.NET Core MVC)

Fly-Away is a full-stack **ASP.NET Core MVC flight booking application** that allows users to browse flights, book tickets, manage bookings, and generate downloadable ticket images.  
This project demonstrates **MVC architecture, authentication, session handling, database integration, and UI rendering**.

---

## 📌 Project Overview

Fly-Away simulates a simplified airline booking platform where users can:

- Register and log in
- Browse available flights
- View flight details
- Select travel class (Economy / Business / First)
- Book a flight
- Automatically generate a ticket PNG
- View, update, or cancel bookings

The project focuses on **backend correctness, MVC flow, and functional UI**.

---

## 🧱 Technology Stack

| Layer | Technology |
|-----|-----------|
| Backend | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Database | SQL Server (LocalDB) |
| Authentication | Session-based login |
| Image Generation | ImageSharp |
| Frontend | Razor Views + CSS |
| Deployment | Docker |
| Version Control | Git + GitHub |

---

## 🏗️ Architecture

This project follows the **Model–View–Controller (MVC)** pattern:

- **Models** — Database entities (Flight, Ticket, Account, Seat, Gate, etc.)
- **Views** — Razor `.cshtml` UI pages
- **Controllers** — Request handling and business logic
- **Services** — Password hashing and ticket image generation
- **wwwroot** — Static assets (CSS, ticket images)

---

## 🔐 Authentication Flow

- Users can **register** and **log in**
- Passwords are **hashed** before storage
- Login state is maintained using **ASP.NET Sessions**
- Protected pages redirect unauthenticated users to Login

---

## ✈️ Booking Flow

1. User logs in
2. User views **Available Flights**
3. Selects a flight → opens **Flight Details**
4. Chooses travel class
5. Booking is created automatically
6. Seat and gate are assigned
7. Ticket PNG is generated and stored
8. Booking appears under **My Bookings**

---

## 🎟️ Ticket Image Generation

Each booking generates a **PNG ticket** containing:

- Ticket ID
- Barcode
- Airline
- Route
- Departure & arrival
- Class, seat, and gate info

Generated tickets are stored under:
wwwroot/tickets/ticket_{TicketID}.png


They are viewable directly from the UI.

---

## 🧭 Key Pages & Routes

| Page | Route |
|----|------|
| Home / Flights | `/` |
| Flight Details | `/FlightsPage/Details/{id}` |
| Login | `/AuthPage/Login` |
| Register | `/AuthPage/Register` |
| My Bookings | `/BookingsPage/MyBookings` |
| Booking Details | `/BookingsPage/Details/{id}` |

---

## 🧪 Seed Data

The database is pre-populated with:

- Airlines
- Airports
- Flights
- Seats
- Gates
- Flight classes

This allows the application to be tested immediately after startup.

---

## 🐳 Docker Support

The project includes Docker configuration for containerized execution.

### Build & Run

```bash
docker build -t fly-away .
docker run -p 5000:8080 fly-away

Or using Docker Compose:
docker-compose up --build

📂 Project Structure
Fly-Away/
│
├── Controllers/
├── Models/
├── Views/
├── Services/
├── Data/
├── DTOs/
├── ViewModels/
├── wwwroot/
│   ├── css/
│   ├── tickets/
│
├── Program.cs
├── Dockerfile
└── README.md

screenshots of the App:

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/e90c6c29-1671-496d-b7c3-b244496f127e" />

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/7aecf063-5257-4bc6-9dde-b81d51476e77" />

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/f6b22451-892d-4e49-9ebd-e66fede65469" />

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/bda6b2be-a1dd-4c97-a860-3eda247737fc" />

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/cd6bba27-d6df-4f79-b89b-b24c9a68ed61" />
