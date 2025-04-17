# Car Rental Application

A .NET Core Web API service with React frontend for managing Tesla car rentals in Mallorca. Provides functionality for checking car availability and making reservations across multiple locations.

## Functionality

- Check availability of Tesla vehicles by model and date range
- Make car reservations with location-based pickup/return
- Dynamic pricing based on car model and duration  

## Prerequisites

- .NET 8.0
- Node.js 20+
- Docker and Docker Compose
- PostgreSQL 16
- React 19
- TailwindCSS 3
- ShadcnUI

## Screenshots
![UI 1](https://github.com/tomgasper/car-rental/blob/main/example/UI_1.png)
![UI 2](https://github.com/tomgasper/car-rental/blob/main/example/UI_2.png)

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
├──  src/
    ├── assets # Static assets
    ├── contexts # React Context/Provider
    ├── hooks # React hooks
    ├── services # API, Error handling services
    └── components/ # React components
```

## Design Choices

### Car Availability Checking

Car availability is determined by querying reservations for the selected vehicle model. This query could easily be optimized to limit the search window to recent reservations (e.g., previous month + buffer) based on maximum rental duration. This approach provides a single source of truth for vehicle availability status.

### Authentication System

Current implementation runs without authentication. System architecture supports future auth integration. Reservations are tracked via unique IDs, enabling guest reservation management if required.

### Pricing Service for Flexible Pricing

Car pricing is handled through `PricingService` service that reads data from `CarPricingRules` table and adds custom pricing logic, allowing for model‑specific daily rates and future expansion to support seasonal pricing or special offers.


### Reservation Status Tracking

Reservations can be in different states (`Planned`, `Confirmed`, `Cancelled`, `Completed`) to allow for extra verification step. In this case after placing a reservation, status would change to `Planned` and then rental company would be able to review the reservation and change it `Confirmed` or `Cancelled`.


## Database Schema

![Database Schema](https://github.com/tomgasper/car-rental/blob/main/example/diagram.png)

## Running the Application

```bash
git clone https://github.com/tomgasper/car-rental.git
cd car-rental
docker compose up -d
```

-  **Frontend:** http://localhost:3000
-  **API:** http://localhost:5150


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

The WebApi project includes unit tests and integration tests using xUnit. Run tests via:

```bash
# With Docker
docker compose run test

# Locally
dotnet test
```

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

## API Endpoints

- **GET** `/v1/api/locations` — Get available rental locations
- **GET** `/v1/api/cars` — Get available car models
- **GET** `/v1/api/cars/availability` — Check car availability  
  **Query Parameters:**  
  - `startDate` (DateTime, required)
  - `endDate` (DateTime, required)
  - `carModel` (string, required) - must be in code format e.g. Tesla Model 3 -> TESLA_MODEL_3

- **POST** `/v1/api/reservations` — Create a reservation  
  **Body (JSON):**  
  - `FirstName` (string, required) 
  - `LastName` (string, required)
  - `Email` (string, required)
  - `PhoneNumber` (string, required)
  - `StartDate` (DateTime, required)
  - `EndDate` (DateTime, required)
  - `CarModel` (string, required)
  - `PickupLocation` (string, required)
  - `ReturnLocation` (string, required)

## Error Handling

- Global exception handling for unexpected errors
- Validation errors for invalid inputs
- Standarized way of returning errors via RFC 7807 Problem Details


## Potential Future Enhancements

- User authentication and authorization
- Fleet management capabilities
- Advanced pricing rules (seasonal, promotional)
- Reservation modification and cancellation
- Email notifications
- Verification via SMS codee