using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapSpecific.Model
{
    public class Graphic
    {
        public string Id { get; set; }
        public object Geometry { get; set; }
        public object Tag { get; set; }
        public Color Color { get; set; }
        public GeometryType GeometryType
        {
            get
            {
                if (Geometry is Polygon)
                {
                    return GeometryType.Polygon;
                }
                else if (Geometry is Polyline)
                {
                    return GeometryType.Polyline;
                }
                else if (Geometry is Circle)
                {
                    return GeometryType.Circle;
                }
                else
                {
                    return GeometryType.Point;
                }
            }
        }
    }
}
