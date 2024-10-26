FROM node:20

EXPOSE 3000

WORKDIR /app

COPY package*.json /app

RUN yarn

COPY . .

CMD [ "yarn", "dev" ]