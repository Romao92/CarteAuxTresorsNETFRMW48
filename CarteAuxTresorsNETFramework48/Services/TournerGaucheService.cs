using CarteAuxTresorsNETFramework48.Enums;
using CarteAuxTresorsNETFramework48.Interfaces;
using CarteAuxTresorsNETFramework48.Models;

namespace CarteAuxTresorsNETFramework48.Services
{
    public class TournerGaucheService : IMouvement
    {
        // injection ici
        private readonly IOrientationService _orientationService;

        public TournerGaucheService(IOrientationService orientationService)
        {
            _orientationService = orientationService;
        }

        public Mouvement Type => Mouvement.G;

        public void Bouger(Carte carte, Aventurier aventurier)
        {
            aventurier.Orientation = _orientationService.TournerGauche(aventurier.Orientation);
        }
    }
}
