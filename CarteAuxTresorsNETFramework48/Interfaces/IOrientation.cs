using CarteAuxTresorsNETFramework48.Enums;

namespace CarteAuxTresorsNETFramework48.Interfaces
{
    public interface IOrientationService
    {
        Orientation TournerGauche(Orientation actuelle);
        Orientation TournerDroite(Orientation actuelle);
        (int dx, int dy) ToDelta(Orientation orientation);
    }
}
