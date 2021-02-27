# Docker Demo

I took the following process to achieve:
1. Build windows container in Docker for .NET 4.7.1 projects
2. Use Environment properties in app in later in builds, containers and deployments
3. Add Docker support
4. Run with Docker compose commands, inspect containers
5. Deploy to Azure Container Registry

I'm leaving it here for me not to forget the steps.

## Demo - project setup (win container .NET 4.7.1)

1. Empty project, run in IIS Express.
2. Add ENV variable called ASPNET_ENVIRONMENT (similar to ASPNETCORE_ENVIRONMENT) using ConfigurationBuilders.Environment package. Re-run VS upon a change so that the process reads new values.
3. Then add Docker support: Dockerfile and .dockerignore.
4. Inspect Dockerfile.
5. Run in Docker from VS directly.
6. Add Docker Compose, inspect file. 
8. Run from VS.
9. Update code, docker-compose, anything, see how docker updates image and page via volumes.
10. Close VS for manual docker build experiments.

### Manual Docker build
1. Add msbuild for VS 2019 to PATH.
2. Build the docker image: \
 `msbuild DockerDemo/DockerDemo.csproj /t:ContainerBuild /p:Configuration=Release`

Alternatively publish from VS to obj/Docker/publish, then: \
`docker build -f DockerDemo/Dockerfile . -t dockerdemo:latest --build-arg source="obj/Docker/publish"`

Watch out for .dockerignore file, that's why there is \
`--build-arg source="obj/Docker/publish"`!

Build solution doesn't work properly (this is not necessary anyway, just to be complete):
1. it doesn't work from solution dir
2. run from project dir: \
`msbuild /p:SolutionPath=../DockerDemo.sln /p:Configuration=Release ../docker-compose.dcproj`

 If it fails on Newtonsoft.Json version 9.0.0, get the file from nuget and copy to  
C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Sdks\Microsoft.Docker.Sdk\tools or similar.

Run again.

### Play with docker and compose, workflow
Keep VS closed as it does its own magic, I need it manually, because I want to get through the same process in Azure DevOps. So, open it in VS code.
1. make a change in code
2. build the image: \
`msbuild DockerDemo/DockerDemo.csproj /t:ContainerBuild /p:Configuration=Release`
3. run in container via docker compose (experimental syntax) \
`docker compose up dockerdemo -d`
4. see the new image and running container: \
`docker ps` \
`docker image list`
5. Navigate to http://localhost:8088/ and see top left app name and env. 
6. Try to set and run it from hand and see if it's updated on the site. \
`docker compose up dockerdemo -d -e "ASPNET_ENVIRONMENT=PROD"` \
7. Set docker environment property: \
`$env:ASPNET_ENVIRONMENT="LOCAL"` 
8. Update running container: \
`docker compose up dockerdemo -d`

**Optional - see how changes are propagated to image and containers.**

9. Change some code and rebuild image \
`msbuild DockerDemo/DockerDemo.csproj /t:ContainerBuild /p:Configuration=Release`

**docker-compose.yml**
```
 environment:
	ASPNET_ENVIRONMENT: "${ASPNET_ENVIRONMENT}"
```    

### Publish image to Azure Container Registry

*[TODO]*