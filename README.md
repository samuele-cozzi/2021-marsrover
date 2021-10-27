# Mars Rover

Explore Mars by sending remotely controlled vehicles to the surface of the planet. Control it with an API that translates the commands sent from earth to instructions that are understood by the rover.


## Tech Overview

Given the initial starting point (x,y) of a rover and the direction (N,S,E,W) it is facing.

- The rover receives a character array of commands.
- Implement commands that move the rover forward/backward (f,b).
- Implement commands that turn the rover left/right (l,r).
- Implement obstacle detection before each move to a new square. 
- If a given sequence of commands encounters an obstacle, 
the rover moves up to the last possible point, aborts the sequence and reports the obstacle.
- Implement wrapping from one edge of the grid to another. (planets are spheres after all)

## Focused
- CQRS & EF
- Clean Architecture
- DDD
- Real world scenario (Rover + Control room)

## Architecture
![ConceptualArchitecture](https://github.com/samuele-cozzi/2021-MarsRover/blob/main/src/Utilities/MarsRoverArchitectureV2.svg)

## Prerequisites
- dotnet core 3.1
- docker

Note: for linux environment follow this [link](https://docs.docker.com/engine/install/linux-postinstall/) after docker installation

## Develop

### Infrastructure

```docker
docker-compose -f ./src/docker-compose.infrastucture.yml up
```
Using:
- RabbitMQ
- MSSQL

Before Start Run SQL file to create rad models tale

### Run & Test

```docker
rover.sln
```

### Pakages References
- Serilog
- EventFlow

### TODO
- Validation
- Swagger
- Exception Handling
- Queries
- Tests
- DevOps
- Authentication
- Authorization
