> More info: https://hub.docker.com/_/mongo/

Get MongoDB docker container up and running by running `docker run`:

> docker run --name f1mongo -d mongo

That's pretty much it. The rest can be done by the application. Make sure to get the IP Address so that the application can connect:

> docker inspect f1mongo | grep IPAddress
