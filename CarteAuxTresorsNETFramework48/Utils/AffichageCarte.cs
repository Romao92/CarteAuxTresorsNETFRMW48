using CarteAuxTresorsNETFramework48.Models;
using CarteAuxTresorsNETFramework48.Models.Enums;
using System;
using System.Text;

namespace CarteAuxTresorsNETFramework48.Utils
{
    public static class AffichageCarte
    {
        public static void Afficher(Carte carte)
        {
            for (int y = 0; y < carte.Hauteur; y++)
            {
                for (int x = 0; x < carte.Largeur; x++)
                {
                    var c = carte.GetCase(x, y);

                    if (c.Terrain == TypeTerrain.Montagne)
                    {
                        Console.Write(" M ");
                    }
                    else if (c.Aventurier != null)
                    {
                        Console.Write($"A({c.Aventurier.Nom[0]})");
                    }
                    else if (c.NombreDeTresors > 0)
                    {
                        Console.Write($"T({c.NombreDeTresors})");
                    }
                    else
                    {
                        Console.Write(" . ");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine(new string('-', 40)); 
        }

        public static void AfficherLog(Carte carte)
        {
            var sb = new StringBuilder();

            sb.AppendLine("État de la carte :");
            for (int y = 0; y < carte.Hauteur; y++)
            {
                for (int x = 0; x < carte.Largeur; x++)
                {
                    var c = carte.GetCase(x, y);

                    if (c.Terrain == TypeTerrain.Montagne)
                    {
                        sb.Append(" M ");
                    }
                    else if (c.Aventurier != null)
                    {
                        sb.Append($"A({c.Aventurier.Nom[0]})");
                    }
                    else if (c.NombreDeTresors > 0)
                    {
                        sb.Append($"T({c.NombreDeTresors})");
                    }
                    else
                    {
                        sb.Append(" . ");
                    }
                }
                sb.AppendLine();
            }

            sb.AppendLine(new string('-', 40));
            LoggerProvider.Logger.Info(sb.ToString());
        }

        public static string AfficherCarteAsText(Carte carte)
        {
            var sb = new StringBuilder();

            for (int y = 0; y < carte.Hauteur; y++)
            {
                for (int x = 0; x < carte.Largeur; x++)
                {
                    var c = carte.GetCase(x, y);

                    if (c.Terrain == TypeTerrain.Montagne)
                    {
                        sb.Append(" M ");
                    }
                    else if (c.Aventurier != null)
                    {
                        sb.Append($"A({c.Aventurier.Nom[0]})");
                    }
                    else if (c.NombreDeTresors > 0)
                    {
                        sb.Append($"T({c.NombreDeTresors})");
                    }
                    else
                    {
                        sb.Append(" . ");
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
