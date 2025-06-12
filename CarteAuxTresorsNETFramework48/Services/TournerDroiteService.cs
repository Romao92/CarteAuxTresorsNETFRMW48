using CarteAuxTresorsNETFramework48.Enums;
using CarteAuxTresorsNETFramework48.Interfaces;
using CarteAuxTresorsNETFramework48.Models;

namespace CarteAuxTresorsNETFramework48.Services
{
    public class TournerDroiteService : IMouvement
    {
        // injection ici
        private readonly IOrientationService _orientationService;

        public TournerDroiteService(IOrientationService orientationService)
        {
            _orientationService = orientationService;
        }

        public Mouvement Type => Mouvement.D;

        public void Bouger(Carte carte, Aventurier aventurier)
        {
            aventurier.Orientation = _orientationService.TournerDroite(aventurier.Orientation);
        }
    }
}
