using MapSpecific.Editor;
using Xamarin.Forms.GoogleMaps;

namespace MapSpecific.Controls
{
    public class ExtendedMap : Map
    {
        public ExtendedMap()
        {
            Editor = new DrawEditor(this);
        }

        public DrawEditor Editor { get; private set; }
    }
}
