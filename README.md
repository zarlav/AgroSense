Naziv aplikacije:AgroSense
Koriscene tehnologije:
  -Backend: ASP .Net Core, DataStax Cassandra C# Driver
  -Frontend: React, TypeScript, Vite
  -Baza: Apache Cassandra (preko Docker-a)

Pokretanje baze:
 docker run -d --name cassandra -p 9042:9042 cassandra:3.11
 docker exec -it cassandra cqlsh
 kreiranje instance: create keyspace agrosense with replication = {'class':'SimpleStrategy', 'replication_factor': 1};
 use agrosense;
 Kreirati tabele iz fajla "baza.txt".
Pokretanje backend-a: cd ..AgroSense\AgroSense -> dotnet build -> dotnet run --launch-profile (Backend ce biti dostupan na:https://localhost:7025 i http://localhost:5161) (Preduslov: .Net 8 SDK)
Pokretanje frontend-a: cd ..AgroSense\AgroSenseClient -> npm install  -> npm run dev  (Preduslov: Node.js v18+)
Pokretanje skripte za generisanje merenja: cd ..AgroSense  -> python Generator.py  (Preduslov: Neophodno je da postoji bar jedan kreiran senzor u bazi)

Autori: Lazar Veljkovic, Nadja Dinic, Nikola Djokic
Predmet: Napredne baze podataka
