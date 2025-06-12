using CarteAuxTresorsNETFramework48.Enums;
using CarteAuxTresorsNETFramework48.Models;

namespace CarteAuxTresorsNETFramework48.Interfaces
{
    public interface IMouvement
    {
        void Bouger(Carte carte, Aventurier aventurier);
        Mouvement Type { get; }
    }
}
