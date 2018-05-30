using MapSpecific.Model;
using System.Linq;
using Xamarin.Forms.GoogleMaps;

namespace MapSpecific.Extensions
{
    public static class MapExtensions
    {
        public static void RemoveDrawnPins(this Map map)
        {
            var pins = map.Pins?.Where(x => (x.Tag as PinTag)?.Id?.ToString() == "DrawnPin").ToList();

            if (pins?.Any() == true)
            {
                foreach (var pin in pins)
                {
                    map.Pins.Remove(pin);
                }
            }
        }

        public static void RemoveGraphic(this Map map, Graphic graphic)
        {
            map.RemoveDrawnPins();

            if (graphic?.Geometry is Polygon)
            {
                map.Polygons.Remove(graphic.Geometry as Polygon);
            }
            else if (graphic?.Geometry is Polyline)
            {
                map.Polylines.Remove(graphic.Geometry as Polyline);
            }
        }
    }
}
