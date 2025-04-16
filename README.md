# Tesla Car Rental Application

  

A .NET Core Web API service with React frontend for managing Tesla car rentals in Mallorca. Provides functionality for browsing available Tesla vehicles and making reservations across multiple locations.

  

## Functionality

  

- Check availability of Tesla vehicles by model and date range

- Make car reservations with location-based pickup/return

- Dynamic pricing based on car model and duration

  

## Prerequisites

  

- .NET 8.0

- Node.js 20+

- Docker and Docker Compose

- PostgreSQL 16

  

## Project Structure

  

```plaintext

src/

├── Controllers/ # API endpoints

├── Services/ # Business logic

├── Repositories/ # Data access layer

├── Models/ # Domain entities

├── DTOs/ # Data transfer objects

├── Infrastructure/ # Database and configuration

└── Common/ # Shared utilities and error handling

  

client/

└── src/

├── components/ # React components

└── services/ # API integration

```

  

## Design Choices

  

### Location Management

  

Locations are managed using unique codes (`PAP`, `PCC`, `ALC`, `MAN`) representing different rental points across Mallorca, making it easy to track car availability and manage fleet distribution.

  

### Flexible Pricing Model

  

Car pricing is handled through `CarPricingRules`, allowing for model‑specific daily rates and future expansion to support seasonal pricing or special offers.

  

### Reservation Status Tracking

  

Reservations follow a state‑machine pattern (`Planned`, `Confirmed`, `Cancelled`, `Completed`) to manage the rental lifecycle effectively.

  

## Local Development Setup

  

1. Clone the repository and navigate to the project directory

2. Start the development environment:

  

```bash

docker compose up -d

```

This will start:

- PostgreSQL database

- .NET Core Web API

- React frontend

  

## Running the Application

  

### Using Docker (Recommended)

  

-  **Frontend:** http://localhost:3000

-  **API:** http://localhost:5150

-  **Swagger UI:** http://localhost:5150/swagger

  

### Local Development

  

1. Start the database:

  

```bash

docker compose up -d db

```

  

2. Run the API:

  

```bash

dotnet run --project src/CarRental.csproj

```

  

3. Run the frontend:

  

```bash

cd client

npm install

npm run dev

```

  

## Testing

  

The project includes integration tests using xUnit. Run tests via:

  

```bash

# With Docker

docker  compose  run  test

  

# Locally

dotnet  test

```

  

Key test areas include:

  

- Car availability checking

- Reservation creation

- Location management

- Pricing calculations

  

## Configuration

  

### Application Settings

  

-  `appsettings.json` — Default configuration

-  `appsettings.Development.json` — Development environment settings

-  `appsettings.Docker.json` — Docker environment configuration

  

### Environment Variables

  

When running with Docker, you can configure:

  

-  `ASPNETCORE_ENVIRONMENT`

-  `ConnectionStrings__DefaultConnection`

-  `VITE_API_URL`

  

## Initial Data

  

The application comes with seeded data including:

  

- 4 locations across Mallorca (Palma Airport, Palma City Center, Alcudia, Manacor)

- Multiple Tesla models (S, 3, X, Y)

- Sample pricing rules (€100–170 per day)

- Example reservations

  

## Database Schema

  

Main tables:

  

-  **Location**: Stores rental locations across Mallorca

-  **Cars**: Manages the Tesla vehicle fleet

-  **Reservations**: Tracks all car bookings

-  **CarPricingRules**: Defines pricing for each Tesla model

  

## API Endpoints

  

-  **GET**  `/api/cars/availability` — Check car availability

-  **POST**  `/api/reservations` — Create a reservation

-  **GET/POST/PUT/DELETE**  `/api/locations` — Manage locations

-  **GET**  `/api/pricing` — Retrieve pricing information

  

## Error Handling

  

- Validation errors for invalid inputs

- Business logic errors for booking conflicts

- Not‑found errors for invalid resources

- Global exception handling for unexpected errors

  

## Security

  

- Input validation

- Error message sanitization

- CORS configuration

- Rate limiting (planned)

  

## Potential Future Enhancements

  

- User authentication and authorization

- Fleet management capabilities

- Advanced pricing rules (seasonal, promotional)

- Reservation modification and cancellation

- Email notifications
