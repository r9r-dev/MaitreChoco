![version](https://img.shields.io/github/tag/rlcx/MaitreChoco.svg) ![build status](https://ac7da.visualstudio.com/_apis/public/build/definitions/1ffc307b-edc2-4557-b962-93035580e533/3/badge)

# MaitreChoco
Le bot de Cacao

# Compiler MaitreChoco

## Visual Studio
- [x] Installer Visual Studio ou un IDE capable de compiler un projet .net core 2.0

## Tokens

### LUIS Microsoft Language
- [x] Créer un compte sur eu.luis.ai
- [x] Créer un projet pour le bot
- [x] Créer les intentions pour chaque intention du bot
- [x] Créer les entités incluses dans chaque intention
- [x] Récupérer le token et l'écrire dans le fichier "luis.token" situé dans le dossier bot.MaitreChoco

### Discord
- [x] Créer une application dans le centre développeur discord
- [x] Générer un token et l'écrire dans le fichier "maitrechoco.token" situé dans le dossier bot.MaitreChoco

### Weather Underground
- [x] Créer un compte sur wunderground.com
- [x] Aller sur le site d'api wunderground : https://www.wunderground.com/weather/api/
- [x] Créer une application et récupérer le key ID dans l'onglet "Key Settings"
- [x] Ecrire la clé dans le fichier "wunderground.token" situé dans le dossier bot.MaitreChoco

## Repository Discord.net beta
- [x] Ajouter le repository beta de discord.net dans la liste des sources de packages nuget : https://www.myget.org/F/discord-net/api/v3/index.json

Sans cet ajout, le package discord.net ne pourra être restauré. Il est néanmoins possible de revenir aux versions stables (1+) mais aucune garantie n'est possible sur le fonctionnement du bot

## Compilation
- [x] exécuter `dotnet build` dans le dossier src

# Lancer MaitreChoco
- [x] exécuter `dotnet run` dans le dossier de compilation