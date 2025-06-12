using CarteAuxTresorsNETFramework48.Models;
using CarteAuxTresorsNETFramework48.Models.Enums;
using CarteAuxTresorsNETFramework48.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarteAuxTresorsNETFramework48.FileIO
{
    public class FileOutputWriter
    {
        public void Write(string path, Deroulement data)
        {
            var lignes = new List<string>();

            lignes.Add($"{DateTime.Now}---- début retour exec -----");
            // 1. Carte
            lignes.Add($"C - {data.Carte.Largeur} - {data.Carte.Hauteur}");

            // 2. Montagnes
            var montagnes = data.Carte.GetAllCases()
                                      .Where(c => c.Terrain == TypeTerrain.Montagne);
            foreach (var c in montagnes)
            {
                lignes.Add($"M - {c.X} - {c.Y}");
            }

            lignes.Add("# {T comme Trésor} - {Axe horizontal} - {Axe vertical} - {Nb. de trésors restants}");
            // 3. Trésors restants
            var tresors = data.Carte.GetAllCases()
                                    .Where(c => c.NombreDeTresors > 0);
            foreach (var c in tresors)
            {
                lignes.Add($"T - {c.X} - {c.Y} - {c.NombreDeTresors}");
            }

            lignes.Add("# {A comme Aventurier} - {Nom de l’aventurier} - {Axe horizontal} - {Axe vertical} - {Orientation} - {Nb. trésors ramassés}");
            // 4. Aventuriers
            foreach (var a in data.Aventuriers)
            {
                lignes.Add($"A - {a.Nom} - {a.X} - {a.Y} - {a.Orientation} - {a.Tresors}");
            }

            // 5. Ajout carte
            lignes.Add("");
            lignes.Add("---- carte -----");
            lignes.Add("");
            lignes.Add(AffichageCarte.AfficherCarteAsText(data.Carte));
            lignes.Add($"{DateTime.Now}---- fin retour exec -----");

            // 5. Écriture du fichier
            File.WriteAllLines(path, lignes);
        }
    }
}
