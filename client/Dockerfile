FROM node:alpine AS build
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .


EXPOSE 5000
CMD [ "npm", "run", "devdep"]