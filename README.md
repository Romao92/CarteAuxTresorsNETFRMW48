# 🗺️ Carte aux Trésors (.NET Framework 4.8)

Ce projet est une implémentation du jeu de simulation "Carte aux Trésors" selon l'exercice de CarbonIT, développée en **C# avec .NET Framework 4.8**, en respectant les principes **SOLID** et avec une architecture orientée services et stratégie.

---

## 🚀 Fonctionnalités

- Lecture d’un fichier d’entrée texte décrivant la carte, les trésors, les montagnes et les aventuriers.
- Simulation **tour par tour**, avec gestion des mouvements, orientation et ramassage de trésors.
- Blocage correct des mouvements interdits (montagnes, cases occupées, bords de carte).
- Journalisation des actions dans la console **et dans un fichier log** (`logs/`).
- Génération d’un **fichier de sortie** avec état final de la carte.
- **Visualisation tour par tour** de la carte.
- Suite de **tests unitaires xUnit** complète.

---

## 🧪 Structure du projet

CarteAuxTresorsNETFramework48/ --> Projet console
CarteAuxTresorsNETFramework48.Tests/ --> Projet de tests xUnit
InputFile/ --> Fichiers d’entrée (.txt)
OutputFile/ --> Fichiers de sortie générés
logs/ --> Fichier log généré par NLog

---

## 📝 Exemple d'entrée (`InputFile/input.txt`)

C - 3 - 4
M - 1 - 0
M - 2 - 1
T - 0 - 3 - 2
T - 2 - 3 - 1
A - Lara - 1 - 1 - S - AADADAGGA

## 📝 Exemple en sortie (`ResultOutput_input-date`)
C - 3 - 4
M - 1 - 0
M - 2 - 1
T - 2 - 3 - 0
A - Lara - 0 - 3 - O - 2


⚙️ Utilisation
✔️ Étapes :
Placer un ou plusieurs fichiers .txt dans le dossier InputFile/

Lancer le programme (CarteAuxTresorsNETFramework48.exe)

Les résultats sont générés dans OutputFile/

Le journal d’exécution complet est disponible dans logs/

🧪 Lancer les tests
Ouvrir la solution dans Visual Studio 2022 puis :

Menu Test > Exécuter tous les tests

Ou Ctrl + R, A

Les tests sont dans le projet CarteAuxTresorsNETFramework48.Tests.

📦 Technologies
.NET Framework 4.8
C# 10
xUnit
NLog

🔐 Licence
Ce projet est publié à but pédagogique.

🙌 Auteur
Développé par Duval Romain pour l'exercice CarbonIT.
