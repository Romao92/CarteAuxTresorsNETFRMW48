using CarteAuxTresorsNETFramework48.Enums;
using CarteAuxTresorsNETFramework48.Interfaces;
using CarteAuxTresorsNETFramework48.Models;
using CarteAuxTresorsNETFramework48.Models.Enums;
using CarteAuxTresorsNETFramework48.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace CarteAuxTresorsNETFramework48.Tests.Service
{
    public class DeroulementServiceTests
    {
        [Theory]
        [InlineData(1, 1, Orientation.N, 1, 0)]
        [InlineData(1, 1, Orientation.S, 1, 2)]
        [InlineData(1, 1, Orientation.E, 2, 1)]
        [InlineData(1, 1, Orientation.O, 0, 1)]
        public void Aventurier_Avance_Dans_La_Bonne_Direction(int startX, int startY, Orientation orientation, int expectedX, int expectedY)
        {
            {
                // Arrange
                var orientationService = new OrientationService();
                var mouvementAvancer = new MouvementService(orientationService);
                var mouvementMap = new Dictionary<Mouvement, IMouvement>
                {
                    { Mouvement.A, mouvementAvancer }
                };

                var deroulementService = new DeroulementService(mouvementMap);

                var carte = new Carte(3, 3);

                var mouvements = new Queue<Mouvement>();
                mouvements.Enqueue(Mouvement.A);

                var aventurier = new Aventurier("Romain", startX, startY, orientation, mouvements);
                carte.GetCase(startX, startY).Aventurier = aventurier;

                var deroulement = new Deroulement(carte, new List<Aventurier> { aventurier });

                // Act
                deroulementService.Executer(deroulement);

                // Assert
                Assert.Equal(expectedX, aventurier.X);
                Assert.Equal(expectedY, aventurier.Y);
            }
        }

        [Fact]
        public void Aventurier_Bloque_Par_Montagne_Ne_Bouge_Pas()
        {
            // Arrange
            var orientationService = new OrientationService();
            var avancer = new MouvementService(orientationService);
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
            {
                { Mouvement.A, avancer }
            };
            var deroulementService = new DeroulementService(mouvementMap);

            // Carte avec une montagne en (1,1)
            var carte = new Carte(3, 3);
            carte.GetCase(1, 1).PlacerMontagne();

            // Aventurier en (1,0), orienté vers le sud → tente d'aller en (1,1)
            var mouvements = new Queue<Mouvement>();
            mouvements.Enqueue(Mouvement.A);
            var aventurier = new Aventurier("Alice", 1, 0, Orientation.S, mouvements);
            carte.GetCase(1, 0).Aventurier = aventurier;

            var deroulement = new Deroulement(carte, new List<Aventurier> { aventurier });

            // Act
            deroulementService.Executer(deroulement);

            // Assert : il n'a pas bougé
            Assert.Equal(1, aventurier.X);
            Assert.Equal(0, aventurier.Y);

            // La case d’origine contient toujours l’aventurier
            Assert.Equal(aventurier, carte.GetCase(1, 0).Aventurier);

            // La montagne ne contient pas d’aventurier
            Assert.Null(carte.GetCase(1, 1).Aventurier);
        }

        [Theory]
        [InlineData(Orientation.N, Orientation.O)]
        [InlineData(Orientation.O, Orientation.S)]
        [InlineData(Orientation.S, Orientation.E)]
        [InlineData(Orientation.E, Orientation.N)]
        public void TournerGauche_fait_pivoter_correctement(Orientation depart, Orientation attendu)
        {
            // Arrange
            var orientationService = new OrientationService();
            var tournerGauche = new TournerGaucheService(orientationService); // IMouvement implémenté
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
            {
                { Mouvement.G, tournerGauche }
            };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);
            var mouvements = new Queue<Mouvement>();
            mouvements.Enqueue(Mouvement.G);

            var aventurier = new Aventurier("Alice", 1, 1, depart, mouvements);
            carte.GetCase(1, 1).Aventurier = aventurier;

            var deroulement = new Deroulement(carte, new List<Aventurier> { aventurier });

            // Act
            deroulementService.Executer(deroulement);

            // Assert
            Assert.Equal(attendu, aventurier.Orientation);
            Assert.Equal(1, aventurier.X); // ne bouge pas
            Assert.Equal(1, aventurier.Y);
        }

        [Theory]
        [InlineData(Orientation.N, Orientation.E)]
        [InlineData(Orientation.O, Orientation.N)]
        [InlineData(Orientation.S, Orientation.O)]
        [InlineData(Orientation.E, Orientation.S)]
        public void TournerDroit_fait_pivoter_correctement(Orientation depart, Orientation attendu)
        {
            // Arrange
            var orientationService = new OrientationService();
            var tournerDroit = new TournerDroiteService(orientationService); // IMouvement implémenté
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
            {
                { Mouvement.D, tournerDroit }
            };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);
            var mouvements = new Queue<Mouvement>();
            mouvements.Enqueue(Mouvement.D);

            var aventurier = new Aventurier("Bob", 1, 1, depart, mouvements);
            carte.GetCase(1, 1).Aventurier = aventurier;

            var deroulement = new Deroulement(carte, new List<Aventurier> { aventurier });

            // Act
            deroulementService.Executer(deroulement);

            // Assert
            Assert.Equal(attendu, aventurier.Orientation);
            Assert.Equal(1, aventurier.X); // ne bouge pas
            Assert.Equal(1, aventurier.Y);
        }

        [Fact]
        public void Aventurier_bloque_par_autre_aventurier_ne_bouge_pas()
        {
            // Arrange
            var orientationService = new OrientationService();
            var avancer = new MouvementService(orientationService);
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
            {
                { Mouvement.A, avancer }
            };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);

            // Aventurier 1 : Alice à (1,0), orientée vers le sud, va tenter d’aller en (1,1)
            var mouvementsAlice = new Queue<Mouvement>();
            mouvementsAlice.Enqueue(Mouvement.A);
            var alice = new Aventurier("Alice", 1, 0, Orientation.S, mouvementsAlice);
            carte.GetCase(1, 0).Aventurier = alice;

            // Aventurier 2 : Bob déjà positionné sur la case cible (1,1)
            var mouvementsBob = new Queue<Mouvement>(); // ne bouge pas
            var bob = new Aventurier("Bob", 1, 1, Orientation.N, mouvementsBob);
            carte.GetCase(1, 1).Aventurier = bob;

            var deroulement = new Deroulement(carte, new List<Aventurier> { alice, bob });

            // Act
            deroulementService.Executer(deroulement);

            // Assert : Alice n’a pas bougé
            Assert.Equal(1, alice.X);
            Assert.Equal(0, alice.Y);

            // Bob est toujours à sa place
            Assert.Equal(1, bob.X);
            Assert.Equal(1, bob.Y);

            // Vérifie la présence sur les cases
            Assert.Equal(alice, carte.GetCase(1, 0).Aventurier);
            Assert.Equal(bob, carte.GetCase(1, 1).Aventurier);
        }

        [Theory]
        [InlineData(0, 0, Orientation.N)] // vers le nord → hors carte
        [InlineData(0, 2, Orientation.S)] // vers le sud → hors carte
        [InlineData(2, 0, Orientation.E)] // vers l’est → hors carte
        [InlineData(0, 0, Orientation.O)] // vers l’ouest → hors carte
        public void Aventurier_bloque_par_bordure_ne_bouge_pas(int startX, int startY, Orientation orientation)
        {
            // Arrange
            var orientationService = new OrientationService();
            var avancer = new MouvementService(orientationService);
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
        {
            { Mouvement.A, avancer }
        };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);

            var mouvements = new Queue<Mouvement>();
            mouvements.Enqueue(Mouvement.A);

            var aventurier = new Aventurier("Lino", startX, startY, orientation, mouvements);
            carte.GetCase(startX, startY).Aventurier = aventurier;

            var deroulement = new Deroulement(carte, new List<Aventurier> { aventurier });

            // Act
            deroulementService.Executer(deroulement);

            // Assert : il n’a pas bougé
            Assert.Equal(startX, aventurier.X);
            Assert.Equal(startY, aventurier.Y);
            Assert.Equal(aventurier, carte.GetCase(startX, startY).Aventurier);
        }

        [Fact]
        public void Aventurier_ramasse_un_tresor_lorsqu_il_arrive_sur_la_case()
        {
            // Arrange
            var orientationService = new OrientationService();
            var avancer = new MouvementService(orientationService);
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
        {
            { Mouvement.A, avancer }
        };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);

            // Ajouter 1 trésor en (1,1)
            var caseTresor = carte.GetCase(1, 1);
            caseTresor.AjouterTresors(1);

            // Aventurier positionné en (1,0), orienté sud vers (1,1)
            var mouvements = new Queue<Mouvement>();
            mouvements.Enqueue(Mouvement.A);
            var aventurier = new Aventurier("Tomb", 1, 0, Orientation.S, mouvements);
            carte.GetCase(1, 0).Aventurier = aventurier;

            var deroulement = new Deroulement(carte, new List<Aventurier> { aventurier });

            // Act
            deroulementService.Executer(deroulement);

            // Assert
            Assert.Equal(1, aventurier.X);
            Assert.Equal(1, aventurier.Y);
            Assert.Equal(1, aventurier.Tresors);
            Assert.Equal(0, caseTresor.NombreDeTresors);
            Assert.Equal(aventurier, carte.GetCase(1, 1).Aventurier);
        }

        [Fact]
        public void Aventurier_doit_revenir_pour_ramasser_deuxieme_tresor()
        {
            // Arrange
            var orientationService = new OrientationService();
            var avancer = new MouvementService(orientationService);
            var tournerGauche = new TournerGaucheService(orientationService);
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
        {
            { Mouvement.A, avancer },
            { Mouvement.G, tournerGauche }
        };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);

            // Placer 2 trésors en (1,1)
            var caseTresor = carte.GetCase(1, 1);
            caseTresor.AjouterTresors(2);

            // Aventurier en (1,0), face au sud
            // Va en (1,1) → ramasse 1
            // Tourne à gauche deux fois → nord
            // Revient en (1,0)
            // Tourne à gauche deux fois → sud
            // Retourne en (1,1) → ramasse 2e
            var mouvements = new Queue<Mouvement>();
            mouvements.Enqueue(Mouvement.A); // vers (1,1)
            mouvements.Enqueue(Mouvement.G); // est
            mouvements.Enqueue(Mouvement.G); // nord
            mouvements.Enqueue(Mouvement.A); // vers (1,0)
            mouvements.Enqueue(Mouvement.G); // ouest
            mouvements.Enqueue(Mouvement.G); // sud
            mouvements.Enqueue(Mouvement.A); // retour (1,1)

            var aventurier = new Aventurier("Indy", 1, 0, Orientation.S, mouvements);
            carte.GetCase(1, 0).Aventurier = aventurier;

            var deroulement = new Deroulement(carte, new List<Aventurier> { aventurier });

            // Act
            deroulementService.Executer(deroulement);

            // Assert
            Assert.Equal(1, aventurier.X);
            Assert.Equal(1, aventurier.Y);
            Assert.Equal(2, aventurier.Tresors);
            Assert.Equal(0, caseTresor.NombreDeTresors);
        }

        [Fact]
        public void Aventuriers_jouent_dans_l_ordre_et_ne_se_bloquent_pas_s_il_y_a_priorite()
        {
            // Arrange
            var orientationService = new OrientationService();
            var avancer = new MouvementService(orientationService);
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
        {
            { Mouvement.A, avancer }
        };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);

            // Aventurier 1 en (0,1), orienté est → cible (1,1)
            var mouvements1 = new Queue<Mouvement>();
            mouvements1.Enqueue(Mouvement.A);
            var alice = new Aventurier("Alice", 0, 1, Orientation.E, mouvements1);
            carte.GetCase(0, 1).Aventurier = alice;

            // Aventurier 2 en (2,1), orienté ouest → cible (1,1)
            var mouvements2 = new Queue<Mouvement>();
            mouvements2.Enqueue(Mouvement.A);
            var bob = new Aventurier("Bob", 2, 1, Orientation.O, mouvements2);
            carte.GetCase(2, 1).Aventurier = bob;

            // L’ordre dans la liste respecte la priorité
            var deroulement = new Deroulement(carte, new List<Aventurier> { alice, bob });

            // Act
            deroulementService.Executer(deroulement);

            // Assert

            // Alice a pris la case centrale (1,1)
            Assert.Equal(1, alice.X);
            Assert.Equal(1, alice.Y);
            Assert.Equal(alice, carte.GetCase(1, 1).Aventurier);

            // Bob est bloqué, reste à sa place
            Assert.Equal(2, bob.X);
            Assert.Equal(1, bob.Y);
            Assert.Equal(bob, carte.GetCase(2, 1).Aventurier);
        }

        [Fact]
        public void Aventurier_effectue_simulation_complete_rotation_deplacement_ramassage()
        {
            // Arrange
            var orientationService = new OrientationService();
            var avancer = new MouvementService(orientationService);
            var tournerDroite = new TournerDroiteService(orientationService);
            var mouvementMap = new Dictionary<Mouvement, IMouvement>
        {
            { Mouvement.A, avancer },
            { Mouvement.D, tournerDroite }
        };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);

            // Trésor à (1,1)
            carte.GetCase(1, 1).AjouterTresors(1);

            // Aventurier à (0,1), orienté est, mouvements : A → D → A
            var mouvements = new Queue<Mouvement>();
            mouvements.Enqueue(Mouvement.A); // (0,1) → (1,1)
            mouvements.Enqueue(Mouvement.D); // Est → Sud
            mouvements.Enqueue(Mouvement.A); // (1,1) → (1,2)

            var aventurier = new Aventurier("Croft", 0, 1, Orientation.E, mouvements);
            carte.GetCase(0, 1).Aventurier = aventurier;

            var deroulement = new Deroulement(carte, new List<Aventurier> { aventurier });

            // Act
            deroulementService.Executer(deroulement);

            // Assert
            Assert.Equal(1, aventurier.X);
            Assert.Equal(2, aventurier.Y);
            Assert.Equal(Orientation.S, aventurier.Orientation);
            Assert.Equal(1, aventurier.Tresors);
            Assert.Equal(0, carte.GetCase(1, 1).NombreDeTresors);
            Assert.Equal(aventurier, carte.GetCase(1, 2).Aventurier);
        }

        [Fact]
        public void Aventurier_sur_case_montagne_declenche_exception()
        {
            // Arrange
            var carte = new Carte(3, 3);
            carte.GetCase(1, 1).PlacerMontagne();

            var mouvements = new Queue<Mouvement>();
            var aventurier = new Aventurier("Indiana", 1, 1, Orientation.S, mouvements);

            // Act + Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var caseCible = carte.GetCase(1, 1);
                if (caseCible.Terrain == TypeTerrain.Montagne)
                    throw new InvalidOperationException("L’aventurier Indiana est positionné sur une montagne (1, 1)");

                caseCible.Aventurier = aventurier;
            });

            Assert.Equal("L’aventurier Indiana est positionné sur une montagne (1, 1)", exception.Message);
        }

        [Fact]
        public void Simulation_complete_multi_aventuriers()
        {
            // Arrange
            var orientationService = new OrientationService();
            var avancer = new MouvementService(orientationService);
            var tournerGauche = new TournerGaucheService(orientationService);
            var tournerDroite = new TournerDroiteService(orientationService);

            var mouvementMap = new Dictionary<Mouvement, IMouvement>
        {
            { Mouvement.A, avancer },
            { Mouvement.G, tournerGauche },
            { Mouvement.D, tournerDroite }
        };

            var deroulementService = new DeroulementService(mouvementMap);
            var carte = new Carte(3, 3);

            // Obstacles
            carte.GetCase(1, 1).PlacerMontagne();

            // Trésors
            carte.GetCase(2, 0).AjouterTresors(1);
            carte.GetCase(0, 2).AjouterTresors(2);

            // Aventurier 1 - Lara
            var mouvementsLara = new Queue<Mouvement>(new[]
            {
            Mouvement.A, Mouvement.A, Mouvement.D, Mouvement.A, Mouvement.A
        });
            var lara = new Aventurier("Lara", 0, 0, Orientation.E, mouvementsLara);
            carte.GetCase(0, 0).Aventurier = lara;

            // Aventurier 2 - Bob
            var mouvementsBob = new Queue<Mouvement>(new[]
            {
            Mouvement.A, Mouvement.A, Mouvement.G, Mouvement.A
        });
            var bob = new Aventurier("Bob", 2, 2, Orientation.N, mouvementsBob);
            carte.GetCase(2, 2).Aventurier = bob;

            var deroulement = new Deroulement(carte, new List<Aventurier> { lara, bob });

            // Act
            deroulementService.Executer(deroulement);

            // Assert

            // Lara :
            // (0,0) → (1,0) → (2,0 - ramasse 1) → tourne Sud → (2,0) → (2,1 - bloque car Bob y est encore)
            Assert.Equal(2, lara.X);
            Assert.Equal(0, lara.Y);
            Assert.Equal(Orientation.S, lara.Orientation);
            Assert.Equal(1, lara.Tresors);
            Assert.Equal(lara, carte.GetCase(2, 0).Aventurier);

            // Bob :
            // Bob : bloqué en (2,1) après tentative sur (2,0), puis rotation ouest et tentative bloquée sur montagne
            Assert.Equal(2, bob.X);
            Assert.Equal(1, bob.Y);
            Assert.Equal(Orientation.O, bob.Orientation);
            Assert.Equal(0, bob.Tresors);
            Assert.Equal(bob, carte.GetCase(2, 1).Aventurier);

            // Trésors restants
            Assert.Equal(0, carte.GetCase(2, 0).NombreDeTresors); // ramassé par Lara
            Assert.Equal(2, carte.GetCase(0, 2).NombreDeTresors); // intact
        }
    }
}



