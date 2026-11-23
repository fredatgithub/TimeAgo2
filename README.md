# TimeAgo2

TimeAgo2 est une application .NET MAUI permettant de suivre le temps écoulé depuis les dernières occurrences d'activités.

## Fonctionnalités

- Créer des activités ou événements.
- Sélectionner une activité pour consulter la liste de ses occurrences.
- Ajouter rapidement une nouvelle occurrence depuis la liste principale avec le bouton `+`.
- Ajouter une occurrence à une date, une heure et avec une note facultative.
- Afficher automatiquement le temps écoulé depuis la dernière occurrence.
- Supprimer une occurrence ou une activité complète.
- Choisir entre le français, l'anglais, l'espagnol ou l'allemand.
- Consulter les informations de l'application depuis la page À propos.
- Conserver les données localement dans un fichier `activities.json`.

## Plateformes

Le projet cible :

- Android ;
- Windows ;
- les plateformes supplémentaires prévues par le modèle MAUI ne sont pas toutes activées dans la configuration actuelle.

## Prérequis

- .NET 9 SDK ;
- les workloads et outils nécessaires à .NET MAUI ;
- Visual Studio 2022 ou Visual Studio Code avec les extensions MAUI adaptées.

## Compilation

Depuis la racine du dépôt :

```powershell
dotnet restore
dotnet build .\TimeAgo2\TimeAgo2.csproj
```

Pour compiler uniquement la cible Windows :

```powershell
dotnet build .\TimeAgo2\TimeAgo2.csproj -f net9.0-windows10.0.19041.0
```
