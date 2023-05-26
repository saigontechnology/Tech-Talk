FROM openjdk:8-jdk
WORKDIR /
EXPOSE 9092

RUN wget -O kafka-bin.tgz https://downloads.apache.org/kafka/3.2.3/kafka_2.13-3.2.3.tgz &&\
	tar -xzf kafka-bin.tgz &&\
	mv kafka_2.13-3.2.3 kafka &&\
	rm kafka-bin.tgz

WORKDIR /kafka

CMD ./scripts/start.sh

