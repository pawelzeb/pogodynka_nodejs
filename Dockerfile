FROM debian:bullseye-slim

#Ustawiamy strefę czasową
ENV TZ=Europe/Warsaw

RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

#Instalacja wymaganych pakietów
RUN apt-get update && \
    apt-get install -y \
    curl \
    gnupg \
    ca-certificates \
    tzdata
    # ln -fs /usr/share/zoneinfo/$TZ /etc/localtime && \
    # dpkg-reconfigure --frontend noninteractive tzdata

#Instalacja Node.js & npm (np. wersja LTS 20.x)
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs && \
    npm install -g npm@latest


#Port Nodejs (4000) -> (Zmapowany zostanie do 4000)
EXPOSE 4000

# 🗂️ Katalog roboczy
WORKDIR /pogodynka_nodejs

# 📥 Kopiujemy pliki do kontenera
COPY . .
# COPY start.sh /start.sh
# RUN chmod +x /start.sh
RUN npm install

# 🧠 Użytkownik CMD sam definiuje — poniżej tylko placeholder
# CMD ["node", "pogodynka.js"]
CMD ["npm", "start"]
# CMD ["/start.sh"]