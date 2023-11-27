FROM node:18.12.1-alpine
# FROM node:20.9.0-alpine
RUN apk upgrade libssl3 libcrypto3
WORKDIR /app

COPY ["package.json", "package-lock.json*", "./"]

RUN npm install

COPY . .

CMD [ "node", "index.js" ]

EXPOSE 3000

# HEALTHCHECK CMD curl --fail http://localhost:3000 || exit 1
# USER node