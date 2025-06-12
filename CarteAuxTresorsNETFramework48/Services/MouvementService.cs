using CarteAuxTresorsNETFramework48.Enums;
using CarteAuxTresorsNETFramework48.Interfaces;
using CarteAuxTresorsNETFramework48.Models;
using CarteAuxTresorsNETFramework48.Utils;
using System;

namespace CarteAuxTresorsNETFramework48.Services
{
    public class MouvementService : IMouvement
    {
        // injection ici
        private readonly IOrientationService _orientationService;

        public MouvementService(IOrientationService orientationService)
        {
            _orientationService = orientationService;
        }

        public Mouvement Type => Mouvement.A;

        public void Bouger(Carte carte, Aventurier aventurier)
        {
            var (dx, dy) = _orientationService.ToDelta(aventurier.Orientation);
            int newX = aventurier.X + dx;
            int newY = aventurier.Y + dy;


            if (carte.EstDansLesBornes(newX, newY) &&
                carte.GetCase(newX, newY).EstAccessible())
            {
                var caseActuelle = carte.GetCase(aventurier.X, aventurier.Y);
                caseActuelle.Aventurier = null;

                aventurier.X = newX;
                aventurier.Y = newY;

                var nouvelleCase = carte.GetCase(newX, newY);
                LoggerConsoleHelper.LogInfo($"{aventurier.Nom} avance vers ({newX}, {newY})");
                if (nouvelleCase.RamasserTresor())
                    aventurier.RamasserTresor();

                nouvelleCase.Aventurier = aventurier;
            }
            else
            {
                if (!carte.EstDansLesBornes(newX, newY))
                {
                    LoggerConsoleHelper.LogWarn($"Mouvement impossible pour {aventurier.Nom} vers ({newX}, {newY}) : hors des bornes.");
                }
                else if (!carte.GetCase(newX, newY).EstAccessible())
                {
                    if(carte.GetCase(newX, newY).Terrain == Models.Enums.TypeTerrain.Montagne)
                    {
                        LoggerConsoleHelper.LogWarn($"Mouvement impossible pour {aventurier.Nom} vers ({newX}, {newY}) : Bloqué par un montagne.");
                    }
                    else if (carte.GetCase(newX, newY).Aventurier != null)
                    {
                        LoggerConsoleHelper.LogWarn($"Mouvement impossible pour {aventurier.Nom} vers ({newX}, {newY}) : {carte.GetCase(newX, newY).Aventurier.Nom} est déjà présent.");
                    }
                }
               
            }
        }
    }
}
