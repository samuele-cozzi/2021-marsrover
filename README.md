# Mars Rover


## Prerequisites
- dotnet core 3.1
- docker

Note: for linux environment follow this [link](https://docs.docker.com/engine/install/linux-postinstall/) after docker installation

## Develop

### Infrastructure

```docker
docker-compose -f ./src/docker-compose.infrastucture.yml up
```
### Run & Test

```docker
docker-compose build
docker-compose up
```
