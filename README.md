# Event Management PoC (Clean Architecture + CQRS + Standalone Angular)


## Technology Stack

| Part       | Technologies |
|--------------|------------|
| **Frontend**  | Angular (Standalone Components), TypeScript, Tailwind CSS |
| **Backend**   | ASP.NET Core 8 Web API, C#, REST, JWT Auth |
| **Database**  | PostgreSQL + EF Core |
| **Deployment**| Docker & Docker Compose |


## Launch

```bash
   git clone https://github.com/maya-mouse/EventManagement.git
   cd EventManagement
   docker compose up --build -d

```

## .env example

```bash
POSTGRES_USER=user
POSTGRES_PASSWORD=password_from_env_file
POSTGRES_DB=eventdb
DB_PORT=5432

```

JWT_SECRET_KEY="PoCS3cReTK3y_f0r_EveNtWav3_Pr0oF_oF_c0ncEpT_S3cur1ty_512b1t_v3r1fy"

## Access to services

| Service   | URL                         |
| --------- | --------------------------- |
| **Frontend UI** | [http://localhost:4200](http://localhost:4200)                 |
| **Swagger API** | [http://localhost:5000/swagger](http://localhost:5000/swagger) |

## Test Accounts

| Email                 | Username | Password  |
| --------------------- | -------- | --------- |
| `alice.org@event.com` | Alice    | `pass123` | 
| `bob4321@event.com`  | Bob      | `123pass` |

## Key Features Implemented

**- Auth** : Registration / Login (JWT, BCrypt)
**- Events** :
 - Creating, editing, deleting
 - POST /events, PATCH /events/{id}, DELETE /events/{id} Join / Leave
 - Events calendar: Month and Week view for linked events
