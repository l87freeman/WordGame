#!/bin/bash
set -ev

#TAG=$1
#DOCKER_USERNAME=$2
#DOCKER_PASSWORD=$3

# Create publish artifact
#dotnet publish -c Release src

# Build the Docker images
#docker build -t repository/project:$TAG src/bin/Release/netcoreapp1.0/publish/.
#docker tag repository/project:$TAG repository/project:latest

# Login to Docker Hub and upload images
#docker login -u="$DOCKER_USERNAME" -p="$DOCKER_PASSWORD"
#docker push repository/project:$TAG
#docker push repository/project:latest

kubectl delete -f ./kubernetes/dictionary.yaml
kubectl delete -f ./kubernetes/dictionaryService.yaml
kubectl delete -f ./kubernetes/bot.yaml
kubectl delete -f ./kubernetes/botService.yaml
kubectl delete -f ./kubernetes/state.yaml
kubectl delete -f ./kubernetes/stateService.yaml
kubectl delete -f ./kubernetes/game.yaml
kubectl delete -f ./kubernetes/gameService.yaml

docker build -f ./WordGame.Dictionary/Dockerfile -t localhost:5000/word-game-dictionary:latest . --no-cache
docker build -f ./WordGame.Game/Dockerfile -t localhost:5000/word-game-game:latest . --no-cache
docker build -f ./WordGame.GameState/Dockerfile -t localhost:5000/word-game-state:latest . --no-cache
docker build -f ./WordGame.BotService/Dockerfile -t localhost:5000/word-game-bot-service:latest . --no-cache

docker push localhost:5000/word-game-dictionary:latest
docker push localhost:5000/word-game-game:latest
docker push localhost:5000/word-game-state:latest
docker push localhost:5000/word-game-bot-service:latest

kubectl apply -f ./kubernetes/dictionary.yaml
kubectl apply -f ./kubernetes/dictionaryService.yaml
kubectl apply -f ./kubernetes/bot.yaml
kubectl apply -f ./kubernetes/botService.yaml
kubectl apply -f ./kubernetes/state.yaml
kubectl apply -f ./kubernetes/stateService.yaml
kubectl apply -f ./kubernetes/game.yaml
kubectl apply -f ./kubernetes/gameService.yaml
