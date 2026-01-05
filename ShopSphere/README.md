# Senior .NET Developer

## Overview
This project demonstrates a microservices-based e-commerce system using .NET 8, Kafka, and Docker.

## Tech Stack
- .NET 8
- ASP.NET Core Web API
- EF Core InMemory
- Kafka (Confluent.Kafka)
- Docker & Docker Compose

## Services
- UserService
- OrderService

## How to Run
```bash
docker-compose up --build
```

User Service: http://localhost:5001/swagger  
Order Service: http://localhost:5002/swagger

## Architecture
- Event-driven communication using Kafka
- At-least-once delivery semantics
- No HTTP calls directly between services

## AI Usage
ChatGPT was used to 
- Scaffold Kafka integration patterns
- Docker configuration
- Validate architectural decisions.
