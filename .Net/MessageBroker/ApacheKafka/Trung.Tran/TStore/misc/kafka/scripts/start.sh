cp ./config-override/server.properties ./config/server.properties;
cp ./config-override/zookeeper.properties ./config/zookeeper.properties;

mkdir /zookeeper-data
echo "$BROKER_ID" > /zookeeper-data/myid

sed -i -e "s/{BROKER_ID}/$BROKER_ID/g" \
	-e "s/{BROKER_RACK}/$BROKER_RACK/g" \
	-e "s/:{LOCAL_PORT}/:$LOCAL_PORT/g" \
	-e "s/:{DOCKER_PORT}/:$DOCKER_PORT/g" \
	./config/server.properties;

bin/zookeeper-server-start.sh ./config/zookeeper.properties &
$CREATE_ACLS = "true" && scripts/create-default-acls.sh &
sleep 7s; bin/kafka-server-start.sh ./config/server.properties;