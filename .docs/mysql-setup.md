First of all, run the docker container which will have your MySQL server:

> docker run --name f1mysql -e MYSQL_ROOT_PASSWORD=1234567890 -d mysql:5.6.26

Add F1 MySQL data to `/src/sql-cripts`:

> http://superuser.com/questions/19318/how-can-i-give-write-access-of-a-folder-to-all-users-in-linux

```
sudo mkdir /src
sudo chmod 757 -R /src
mkdir /src/sql-scripts
mv /home/tugberk/Documents/f1/* /src/sql-scripts/
```

Next step is to set up a client container so that you can connect to this MySQL instance. Run the following command to do that:

> docker run -it --link f1mysql:mysql -v /src/sql-scripts:/opt/sql-scripts --rm mysql sh -c 'exec mysql -h"$MYSQL_PORT_3306_TCP_ADDR" -P"$MYSQL_PORT_3306_TCP_PORT" -uroot -p"$MYSQL_ENV_MYSQL_ROOT_PASSWORD"'

A few things to notice here:

 - We are linking the `mysql` container to our `f1mysql` container which we have created earlier. Read more on linking containers [here](https://docs.docker.com/userguide/dockerlinks/).
 - We are mounting the `/src/sql-cripts` path from host to `/opt/sql-scripts` for the client container so that it can get access to some SQL scripts we want it to run.
 - We are able to reach out some of the information about the linked container thanks to [the way environment variables work with docker linking](https://docs.docker.com/userguide/dockerlinks/#environment-variables).

Now, we should have a interactive command line for mysql client. Create the database `f1` now and source data SQL script.

```
CREATE DATABASE f1;
USE f1;
source /opt/sql-scripts/f1db.sql
```

Now you have an awesome database in your hand :) A few queries to see the awesomeness:

```
mysql > SHOW TABLES;
+----------------------+
| Tables_in_f1         |
+----------------------+
| circuits             |
| constructorResults   |
| constructorStandings |
| constructors         |
| driverStandings      |
| drivers              |
| lapTimes             |
| pitStops             |
| qualifying           |
| races                |
| results              |
| seasons              |
| status               |
+----------------------+
13 rows in set (0.00 sec)
```

```
mysql> SELECT COUNT(*) FROM lapTimes;
+----------+
| COUNT(*) |
+----------+
|   106469 |
+----------+
1 row in set (0.00 sec)
```

## Connecting to MySQL instance from your application

First, grab the IP Address of your MySQL container instance.

> docker inspect f1mysql | grep IPAddress

Once you have the IP address, you can use the below connection string for that:

```
Data Source=172.17.0.12;port=3306;Initial Catalog=f1;User Id=root;password=1234567890
```

## Connecting to MySQL Server from your application inside another container

> TODO: write this up...
