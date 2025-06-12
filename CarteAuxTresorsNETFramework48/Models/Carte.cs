using System;
using System.Collections.Generic;

namespace CarteAuxTresorsNETFramework48.Models
{
    public class Carte
    {
		public int Largeur;
		public int Hauteur;
        private readonly Dictionary<(int x, int y), Case> _grille;

        public Carte(int largeur, int hauteur)
        {
            Largeur = largeur;
            Hauteur = hauteur;
            _grille = new Dictionary<(int, int), Case>();

            for (int y = 0; y < Hauteur; y++)
            {
                for (int x = 0; x < Largeur; x++)
                {
                    _grille[(x, y)] = new Case(x, y); 
                }
            }
        }

        internal bool EstDansLesBornes(int newX, int newY)
        {
            return newX >= 0 && newX < Largeur && newY >= 0 && newY < Hauteur;
        }

        public Case GetCase(int x, int y)
        {
            if (!EstDansLesBornes(x, y))
                throw new ArgumentOutOfRangeException($"Coordonnées en dehors des limites de la carte : ({x}, {y})");

            return _grille[(x, y)];
        }

        public IEnumerable<Case> GetAllCases() => _grille.Values;
    }
}
