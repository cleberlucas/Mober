﻿namespace Mobile.Utils
{
    public class DistanceCalculator
    {
        public static double CalculateDistance(Location location1, Location location2)
        {
            double earthRadius = 6371000;

            double dLat = DegreesToRadians(location2.Latitude - location1.Latitude);
            double dLon = DegreesToRadians(location2.Longitude - location1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(location1.Latitude)) * Math.Cos(DegreesToRadians(location2.Latitude)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = earthRadius * c;

            return distance;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
