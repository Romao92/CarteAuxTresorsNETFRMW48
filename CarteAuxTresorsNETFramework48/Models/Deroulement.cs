using System.Collections.Generic;

namespace CarteAuxTresorsNETFramework48.Models
{
    public class Deroulement
    {
        public Carte Carte { get; set; }
        public List<Aventurier> Aventuriers { get; set; }

        public Deroulement(Carte carte, List<Aventurier> aventuriers)
        {
            Carte = carte;
            Aventuriers = aventuriers;
        }
    }
}
