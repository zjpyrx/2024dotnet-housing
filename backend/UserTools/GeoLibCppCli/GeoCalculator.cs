using System;

namespace GeoLibCppCli
{
    public static class GeoCalculator
    {
        private const double EarthRadiusKm = 6371;

        public static double GetDist(double lon1, double lat1, double lon2, double lat2)
        {
            double lat1Rad = Math.PI * lat1 / 180.0;
            double lon1Rad = Math.PI * lon1 / 180.0;
            double lat2Rad = Math.PI * lat2 / 180.0;
            double lon2Rad = Math.PI * lon2 / 180.0;

            double dlat = lat2Rad - lat1Rad;
            double dlon = lon2Rad - lon1Rad;
            double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }
    }
}
