using CarteAuxTresorsNETFramework48.Models.Enums;

namespace CarteAuxTresorsNETFramework48.Models
{
    public class Case
    {
        public int X { get; }
        public int Y { get; }
        public TypeTerrain Terrain { get; private set; }
        public int NombreDeTresors { get; private set; }
        public Aventurier Aventurier { get; set; }

        public Case(int x, int y)
        {
            X = x;
            Y = y;
            Terrain = TypeTerrain.Plaine;
            NombreDeTresors = 0;
        }

        public void AjouterTresors(int nb) => NombreDeTresors += nb;

        public bool RamasserTresor()
        {
            if (NombreDeTresors > 0)
            {
                NombreDeTresors--;
                return true;
            }
            return false;
        }

        public void PlacerMontagne() => Terrain = TypeTerrain.Montagne;

        public bool EstAccessible() => Terrain != TypeTerrain.Montagne && Aventurier == null;
    }
}
