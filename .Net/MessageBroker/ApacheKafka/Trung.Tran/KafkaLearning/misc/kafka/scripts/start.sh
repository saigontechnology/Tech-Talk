cp ./config-override/server.properties ./config/server.properties;
cp ./config-override/zookeeper.properties ./config/zookeeper.properties;

bin/zookeeper-server-start.sh ./config/zookeeper.properties &
sleep 7s; bin/kafka-server-start.sh ./config/server.properties;