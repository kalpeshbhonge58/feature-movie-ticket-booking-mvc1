# CineBooker Enterprise

A production-quality Movie Ticket Booking System built with ASP.NET Core MVC.

## Features

- **Authentication & Authorization**: Secure user registration and login with ASP.NET Core Identity. Role-based access for Admin and User roles.
- **Movie Management (Admin)**: Add, edit, and delete movies with support for poster URLs, genres, and descriptions.
- **Theater & Screen Management (Admin)**: Create theaters and multiple screens per theater, including seat capacity.
- **Show Management (Admin)**: Schedule shows by assigning movies to screens at specific times.
- **Interactive Seat Selection**: Visual seat grid with real-time status (Available, Selected, Booked).
- **Booking System**: Transaction-safe booking process to prevent double booking.
- **Mock Payment**: Simulated payment gateway to complete bookings.
- **User Dashboard**: View personal booking history and ticket details.
- **Admin Dashboard**: Overview of revenue, total movies, and recent bookings.

## Architecture

The project follows a layered architecture to ensure separation of concerns:
- **Presentation Layer**: ASP.NET Core MVC Controllers and Razor Views.
- **Application Layer**: Business logic encapsulated in Services (`MovieService`, `BookingService`, etc.).
- **Data Layer**: Entity Framework Core with SQLite, `ApplicationDbContext`, and `DbInitializer`.
- **Domain Layer**: Entity models representing the core business objects.

## Tech Stack

- **Backend**: ASP.NET Core 8 MVC, C#
- **Database**: SQLite with Entity Framework Core (Code First)
- **Identity**: ASP.NET Core Identity
- **Frontend**: Razor Views, Bootstrap 5, JavaScript (jQuery/AJAX)
- **Interactivity**: AJAX-based seat selection and booking submission.

## Setup Instructions

1.  **Prerequisites**: Ensure you have the .NET 8 SDK installed.
2.  **Restore Dependencies**:
    ```bash
    dotnet restore
    ```
3.  **Database Setup**: The database will be automatically created and seeded on the first run. To manually apply migrations:
    ```bash
    dotnet ef database update
    ```
4.  **Run the Application**:
    ```bash
    dotnet run
    ```

## Admin Credentials

- **Email**: admin@cinebooker.com
- **Password**: Admin@123

## Database Details

- **Database Engine**: SQLite
- **Filename**: `cinebooker.db` (created in the root directory)
- **Concurrency**: Transactions are used in `BookingService` to ensure seat availability is validated before finalizing bookings.
