# PS5GameManagementSystem

# PS5 Game Management System

A distributed application built using .NET microservices for managing a PlayStation 5 game catalogue and user game libraries.

The system consists of two independent microservices that communicate with each other using HTTP REST APIs. Each service has its own database and can run independently.

---

## Architecture

```text
                         ┌──────────────────────┐
                         │    SIConsoleClient   │
                         │     Console App      │
                         └──────────┬───────────┘
                                    │
                                    │ HTTP
                                    ▼
                    ┌─────────────────────────────┐
                    │  SIGameCatalogueService    │
                    │       .NET Web API          │
                    │                             │
                    │  Game Catalogue Management  │
                    └──────────────┬──────────────┘
                                   │
                                   ▼
                           ┌───────────────┐
                           │  Game Database │
                           │   SQL Server  │
                           │ / PostgreSQL   │
                           └───────────────┘


                         ┌──────────────────────┐
                         │    SIConsoleClient   │
                         └──────────┬───────────┘
                                    │
                                    │ HTTP
                                    ▼
                    ┌─────────────────────────────┐
                    │      SILibraryService       │
                    │        .NET Web API         │
                    │                             │
                    │   User Game Library         │
                    └──────────────┬──────────────┘
                                   │
                                   │ HTTP
                                   ▼
                    ┌─────────────────────────────┐
                    │  SIGameCatalogueService    │
                    │                             │
                    │       Game Information      │
                    └─────────────────────────────┘
                                   │
                                   ▼
                           ┌───────────────┐
                           │  Library DB   │
                           │   SQL Server  │
                           └───────────────┘
