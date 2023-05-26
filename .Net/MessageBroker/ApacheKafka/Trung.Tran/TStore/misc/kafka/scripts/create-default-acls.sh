#!/bin/bash
sleep 14s;

if [ -e /_initflag ]
then
    echo "Already ran!";
else	
	bin/kafka-acls.sh --bootstrap-server kafka-1:29093,kafka-2:29095,kafka-3:29097 --command-config ./config-override/adminclient.properties \
	  --add --transactional-id "*" \
	  --allow-principal User:transproducer \
	  --operation WRITE --operation DESCRIBE;
	touch /_initflag;
fi