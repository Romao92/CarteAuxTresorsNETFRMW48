using CarteAuxTresorsNETFramework48.Enums;
using CarteAuxTresorsNETFramework48.Interfaces;
using CarteAuxTresorsNETFramework48.Models;
using CarteAuxTresorsNETFramework48.Utils;
using System;
using System.Collections.Generic;

namespace CarteAuxTresorsNETFramework48.Services
{
    public class DeroulementService
    {
        private readonly Dictionary<Mouvement, IMouvement> _mouvementStrategies;

        public DeroulementService(Dictionary<Mouvement, IMouvement> mouvementStrategies)
        {
            _mouvementStrategies = mouvementStrategies;
        }

        public void Executer(Deroulement enonce)
        {
            var aventuriers = enonce.Aventuriers;

            bool mouvementsRestants;

            do
            {
                mouvementsRestants = false;

                foreach (var aventurier in aventuriers)
                {
                    if (aventurier.Mouvements.Count == 0)
                        continue;

                    LoggerConsoleHelper.LogInfo($"Aventurier {aventurier.Nom} commence en ({aventurier.X}, {aventurier.Y}) orientation {aventurier.Orientation}");
                    var mouvement = aventurier.Mouvements.Dequeue();
                    if (_mouvementStrategies.TryGetValue(mouvement, out var deplacement))
                    {
                        deplacement.Bouger(enonce.Carte, aventurier);
                        mouvementsRestants = true;

                        LoggerConsoleHelper.LogInfo($"Après mouvement {mouvement} de {aventurier.Nom}, orientation {aventurier.Orientation}, position: ({aventurier.X},{aventurier.Y}) :");
                        AffichageCarte.Afficher(enonce.Carte);
                        AffichageCarte.AfficherLog(enonce.Carte);
                    }
                    else
                    {
                        // Mouvement inconnu ou non supporté (sécurité)
                        LoggerConsoleHelper.LogWarn($"{aventurier.Nom} ne peut effectuer le mouvement: {mouvement}");
                        continue;
                    }
                }

            } while (mouvementsRestants);
        }
    }
}
