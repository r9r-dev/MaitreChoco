# MaitreChoco
Le bot de Cacao

# Compiler MaitreChoco

## Visual Studio
Installer Visual Studio ou un IDE capable de compiler un projet .net core 2.0

## Tokens

### LUIS Microsoft Language
Créer un compte sur eu.luis.ai
Créer un projet pour le bot
Créer les intentions pour chaque intention du bot
Créer les entités incluses dans chaque intention
Récupérer le token et l'écrire dans le fichier "luis.token" situé dans le dossier bot.MaitreChoco

### Discord
Créer une application dans le centre développeur discord
Générer un token et l'écrire dans le fichier "maitrechoco.token" situé dans le dossier bot.MaitreChoco

### Weather Underground
Créer un compte sur wunderground.com
Aller sur le site d'api wunderground : https://www.wunderground.com/weather/api/
Créer une application et récupérer le key ID dans l'onglet "Key Settings"
Ecrire la clé dans le fichier "wunderground.token" situé dans le dossier bot.MaitreChoco

## Repository Discord.net beta
Ajouter le repository beta de discord.net dans la liste des sources de packages nuget : https://www.myget.org/F/discord-net/api/v3/index.json
Sans cet ajout, le package discord.net ne pourra être restauré. Il est néanmoins possible de revenir aux versions stables (1+) mais aucune garantie n'est possible sur le fonctionnement du bot

## Compilation
exécuter `dotnet build` dans le dossier src

# Lancer MaitreChoco
exécuter `dotnet run` dans le dossier de compilation