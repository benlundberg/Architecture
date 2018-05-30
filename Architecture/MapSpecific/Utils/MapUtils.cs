using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;

namespace MapSpecific.Utils
{
    public class MapUtils
    {
        public static double GetDistance(List<Position> positions, DistanceType distanceType)
        {
            if (positions.Count < 2)
            {
                return 0;
            }

            double totalDist = 0;

            for (int i = 0; i < (positions.Count - 1); i++)
            {
                Position position1 = positions[i];
                Position position2 = positions[i + 1];

                double rlat1 = Math.PI * position1.Latitude / 180;
                double rlat2 = Math.PI * position2.Latitude / 180;
                double theta = position1.Longitude - position2.Longitude;
                double rtheta = Math.PI * theta / 180;
                double dist =
                    Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                    Math.Cos(rlat2) * Math.Cos(rtheta);
                dist = Math.Acos(dist);
                dist = dist * 180 / Math.PI;
                dist = dist * 60 * 1.1515;

                totalDist += dist;
            }

            switch (distanceType)
            {
                case DistanceType.Kilometers:
                    return Math.Round(totalDist * 1.609344, 2);
                case DistanceType.Meters:
                    return Math.Round(totalDist * 1.609344 / 100000, 0);
                case DistanceType.Miles:
                    return Math.Round(totalDist * 1.609344 * 10, 0);
                default:
                    return Math.Round(totalDist * 1.609344, 0);
            }
        }

        public static double GetArea(List<Position> positions)
        {
            if (positions.Count < 3)
            {
                return 0;
            }

            double total = 0;

            Position prev = positions[positions.Count - 1];

            double prevTanLat = Math.Tan((Math.PI / 2 - ConvertToRadian(prev.Latitude)) / 2);

            double prevLng = ConvertToRadian(prev.Longitude);

            foreach (var point in positions)
            {
                double tanLat = Math.Tan((Math.PI / 2 - ConvertToRadian(point.Latitude)) / 2);
                double lng = ConvertToRadian(point.Longitude);
                total += PolarTriangleArea(tanLat, lng, prevTanLat, prevLng);
                prevTanLat = tanLat;
                prevLng = lng;
            }

            double area = total * (EARTH_RADIUS * EARTH_RADIUS);

            area = area / 10000.00;

            area = Math.Round(area, 2);

            return Math.Abs(area);
        }

        private static double PolarTriangleArea(double tan1, double lng1, double tan2, double lng2)
        {
            double deltaLng = lng1 - lng2;
            double t = tan1 * tan2;
            return 2 * Math.Atan2(t * Math.Sin(deltaLng), 1 + t * Math.Cos(deltaLng));
        }

        private static double ConvertToRadian(double latitude)
        {
            return latitude * Math.PI / 180;
        }

        private const double EARTH_RADIUS = 6378137;
    }
}
