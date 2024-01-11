# Use an official Cypress base image
FROM cypress/base:20.9.0

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

CMD ["npx", "cypress", "run"]
