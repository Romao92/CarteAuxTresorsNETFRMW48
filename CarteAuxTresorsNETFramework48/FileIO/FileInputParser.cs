using CarteAuxTresorsNETFramework48.Enums;
using CarteAuxTresorsNETFramework48.Models;
using CarteAuxTresorsNETFramework48.Models.Enums;
using CarteAuxTresorsNETFramework48.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarteAuxTresorsNETFramework48.FileIO
{
    public class FileInputParser
    {
        public Deroulement Parse(string path)
        {
            Carte carte = null;
            var aventuriers = new List<Aventurier>();
           
            var lignes = File.ReadAllLines(path)
                        .Where(l => !string.IsNullOrWhiteSpace(l) && !l.Trim().StartsWith("#"))
                        .ToList();

            foreach (var ligne in lignes)
            {
                LoggerConsoleHelper.LogInfo($"Ligne traitée: {ligne}");
                var elements = ligne.Split('-').Select(e => e.Trim()).ToArray();

                switch (elements[0])
                {
                    case "C":
                        int largeur = int.Parse(elements[1]);
                        int hauteur = int.Parse(elements[2]);
                        carte = new Carte(largeur, hauteur);
                        break;

                    case "M":
                        int mx = int.Parse(elements[1]);
                        int my = int.Parse(elements[2]);
                        carte?.GetCase(mx, my).PlacerMontagne();
                        break;

                    case "T":
                        int tx = int.Parse(elements[1]);
                        int ty = int.Parse(elements[2]);
                        int nbTresors = int.Parse(elements[3]);
                        carte?.GetCase(tx, ty).AjouterTresors(nbTresors);
                        break;

                    case "A":
                        string nom = elements[1];
                        int ax = int.Parse(elements[2]);
                        int ay = int.Parse(elements[3]);

                        if (carte!=null && carte.GetCase(ax, ay).Terrain == TypeTerrain.Montagne)
                            throw new InvalidOperationException($"L’aventurier {nom} est positionné sur une montagne ({ax}, {ay})");

                        var orientation = (Orientation)Enum.Parse(typeof(Orientation), elements[4]);
                        var mouvements = elements[5]
                            .ToCharArray()
                            .Select(c => (Mouvement)Enum.Parse(typeof(Mouvement), c.ToString()))
                            .ToList();

                        var aventurier = new Aventurier(nom, ax, ay, orientation, mouvements);

                        if (carte != null)
                        {
                            var caseDepart = carte.GetCase(ax, ay);
                            if (caseDepart != null)
                            {
                                caseDepart.Aventurier = aventurier;
                            }
                        }
                        else
                        {
                            LoggerProvider.Logger.Error("Carte non définie avant les aventuriers.");
                            throw new InvalidOperationException("Carte non définie avant les aventuriers.");
                        }

                        aventuriers.Add(aventurier);
                        break;

                    default:
                        LoggerProvider.Logger.Error($"Ligne non reconnue : {ligne}");
                        throw new InvalidOperationException($"Ligne non reconnue : {ligne}");
                }
            }

            return new Deroulement(carte, aventuriers);
        }
    }
}
