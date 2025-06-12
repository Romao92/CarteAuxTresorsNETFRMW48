using CarteAuxTresorsNETFramework48.Enums;
using System.Collections.Generic;

namespace CarteAuxTresorsNETFramework48.Models
{
    public class Aventurier
    {
        public string Nom { get; }
        public int X { get; set; }
        public int Y { get; set; }
        public Orientation Orientation { get; set; }
        public Queue<Mouvement> Mouvements { get; }
        public int Tresors { get; private set; }
        public Aventurier(string nom, int x, int y, Orientation orientation, IEnumerable<Mouvement> mouvements)
        {
            Nom = nom;
            X = x;
            Y = y;
            Orientation = orientation;
            Mouvements = new Queue<Mouvement>(mouvements);
        }

        public void RamasserTresor()
        {
            LoggerProvider.Logger.Info($"Trésor trouvé!");
            Tresors++;
            LoggerProvider.Logger.Info($"{Tresors} trésor pour {Nom}");
        }
    }
}
