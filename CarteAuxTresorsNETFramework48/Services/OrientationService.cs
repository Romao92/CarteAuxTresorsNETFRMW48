using CarteAuxTresorsNETFramework48.Enums;
using CarteAuxTresorsNETFramework48.Interfaces;
using System;

namespace CarteAuxTresorsNETFramework48.Services
{
    public class OrientationService : IOrientationService
    {
        public Orientation TournerGauche(Orientation actuelle)
        {
            switch (actuelle)
            {
                case Orientation.N:
                    return Orientation.O;
                case Orientation.O:
                    return Orientation.S;
                case Orientation.S:
                    return Orientation.E;
                case Orientation.E:
                    return Orientation.N;
                default:
                    throw new InvalidOperationException();
            }
        }

        public Orientation TournerDroite(Orientation actuelle)
        {
            switch (actuelle)
            {
                case Orientation.N:
                    return Orientation.E;
                case Orientation.E:
                    return Orientation.S;
                case Orientation.S:
                    return Orientation.O;
                case Orientation.O:
                    return Orientation.N;
                default:
                    throw new InvalidOperationException();
            }
        }

        public (int dx, int dy) ToDelta(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.N:
                    return (0, -1);
                case Orientation.S:
                    return (0, 1);
                case Orientation.E:
                    return (1, 0);
                case Orientation.O:
                    return (-1, 0);
                default:
                    return (0, 0);
            }
        }
    }
}
