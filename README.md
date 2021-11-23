[![CodeQL](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/codeql-analysis.yml)
[![Unit Test](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/dotnet-unittest.yml/badge.svg)](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/dotnet-unittest.yml)
[![codecov](https://codecov.io/gh/samuele-cozzi/2021-marsrover/branch/main/graph/badge.svg?token=ARTZVUUV8G)](https://codecov.io/gh/samuele-cozzi/2021-marsrover)
[![Maintainability](https://api.codeclimate.com/v1/badges/fac11f4252ca3d792cda/maintainability)](https://codeclimate.com/github/samuele-cozzi/2021-marsrover/maintainability)
[![Test Coverage](https://api.codeclimate.com/v1/badges/fac11f4252ca3d792cda/test_coverage)](https://codeclimate.com/github/samuele-cozzi/2021-marsrover/test_coverage)

[![Docker Rover](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/docker-push-rover.yml/badge.svg)](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/docker-push-rover.yml)
[![Docker Control Room API](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/docker-push-controlroomapi.yml/badge.svg)](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/docker-push-controlroomapi.yml)
[![Docker Controlroom UI](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/docker-push-controlroomui.yml/badge.svg)](https://github.com/samuele-cozzi/2021-marsrover/actions/workflows/docker-push-controlroomui.yml)

# Mars Rover

Explore Mars by sending remotely controlled vehicles to the surface of the planet. Control it with an API that translates the commands sent from earth to instructions that are understood by the rover.


## Overview

Given the initial starting point (x,y) of a rover and the direction (N,S,E,W) it is facing.

- The rover receives a character array of commands.
- Implement commands that move the rover forward/backward (f,b).
- Implement commands that turn the rover left/right (l,r).
- Implement obstacle detection before each move to a new square. 
- If a given sequence of commands encounters an obstacle, 
the rover moves up to the last possible point, aborts the sequence and reports the obstacle.
- Implement wrapping from one edge of the grid to another. (planets are spheres after all)

## Ideas
Real world scenario (Rover + Control room)

### Concetpual Architecture
![ConceptualArchitecture](https://github.com/samuele-cozzi/2021-MarsRover/blob/main/Utilities/MarsRoverArchitecture.svg)

### Explored Concepts
- CQRS & EF
- Clean Architecture & DDD
- Queue Integration


## Tech Stack
- Infrastructure
  - MSSQL ([Microsoft SQL Server](https://www.microsoft.com/it-it/sql-server/sql-server-downloads))
  - RabbitMQ ([RabbitMq](https://www.rabbitmq.com/))
  - seq ([aeq](https://datalust.co/seq))
  - Event Store ([GES](https://eventstore.com/))
  - Docker Compose ([Docker](https://www.docker.com/))
  
- Develop
  
  - .NET Core 5.0 
  - EventFlow
  - Angular 12

### CQRS & EF
Image of sequence diagram

### Clean  Architecture & DDD in .NET   
Image of clean architecture
Many tanks to...

### Eventflow Concepts
- Aggregates
- Command bus and commands
- Event store
- In-memory, Entiy Framework read model.
- Integration
- Asynchronous subscriber

### Tools
- [mind map](https://gitmind.com/app/doc/1e898538b34c43ba53532e5440b584ad)
- [trello](https://trello.com/b/RbRn6Qcc/marsrover-2021)
- [app.diagrams.net](https://app.diagrams.net/#DMarsRoverArchitecture.svg)
- [web sequence diagrams](https://www.websequencediagrams.com/)

## Tech Details

### Prerequisites
- docker
- node 
- dotnet core 5
- angular (npm install -g @angular/cli)

Note: for linux environment follow this [link](https://docs.docker.com/engine/install/linux-postinstall/) after docker installation

### Run Infrastructure

```docker
docker-compose -f ./src/docker-compose.infrastucture.yml up
```
Using:
- Docker [Visual Studio Code](https://code.visualstudio.com/download)
- RabbitMQ [management](http://localhost:15672/)
- MSSQL [Azure Data Studio](https://docs.microsoft.com/en-us/sql/connect/ad/sql-server-connect-ad-sql-server-azure)
- seq [management](http://localhost:5340)

### Applications

- Control Room UI [app](http://localhost:5010/)
- Control Room API [swagger](http://localhost:5000/swagger)
- Control Room API [hangfire](http://localhost:5000/hangfire)

#### Control Room UI

#### Control Room API

#### Rover

### Build & Run

```docker
docker-compose -f ./src/docker-compose.infrastucture.yml -f ./src/docker-compose.yml -f ./src/docker-compose.override.yml build
docker-compose -f ./src/docker-compose.infrastucture.yml -f ./src/docker-compose.yml -f ./src/docker-compose.override.yml up
```


### Debug in Linux

```docker
code ./src/applications/rover/rover
debug
code ./src/applications/controlroom.api/controlroom.api
debug
code ./src/applications/controlroom.ui
ng serve --open
```

### Debug in Windows

```docker
./src/rover.sln
```

### Pakages References
- Serilog
- EventFlow
- Hangfire
- Angular
- Angular Material
- Angular Flex





### Deploy


## Many Thanks To
- [Angular Tutorial](https://angular.io/tutorial)
- [Event Flow Docs](https://docs.geteventflow.net/GettingStarted.html)
- [Event Flow Github](https://github.com/eventflow/EventFlow)


## TODO
- Tests
- DevOps
- Authentication
- Authorization
