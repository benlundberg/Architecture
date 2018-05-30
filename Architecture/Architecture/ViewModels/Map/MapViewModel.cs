using Acr.UserDialogs;
using Architecture.Core;
using MapSpecific.Controls;
using MapSpecific.Extensions;
using MapSpecific.Model;
using MapSpecific.Utils;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Architecture
{
    public class MapViewModel : BaseViewModel, IDisposable
    {
        public MapViewModel()
        {

        }

        public void Initialize(ExtendedMap map, Grid overlayMenu)
        {
            if (this.map != null)
            {
                return;
            }

            this.map = map;
            this.overlayMenu = overlayMenu;

            try
            {
                map.UiSettings.MyLocationButtonEnabled = true;
                map.UiSettings.ZoomControlsEnabled = true;
                map.MyLocationEnabled = true;
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            map.Editor.GraphicChanged += Editor_GraphicChanged;
        }

        private void Editor_GraphicChanged(object sender, Graphic e)
        {
            if (e?.Geometry == null)
            {
                return;
            }

            if (e.GeometryType == GeometryType.Polygon)
            {
                double area = MapUtils.GetArea((e.Geometry as Polygon).Positions.ToList());

                DrawSatusMessage = area.ToString() + " ha";
            }
            else if (e.GeometryType == GeometryType.Polyline)
            {
                double distance = MapUtils.GetDistance((e.Geometry as Polyline).Positions.ToList(), DistanceType.Kilometers);

                if (distance < 0.7)
                {
                    distance = distance * 1000;
                    DrawSatusMessage = distance.ToString() + " meter";
                }
                else
                {
                    DrawSatusMessage = distance.ToString() + " km";
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private ICommand layerCommand;
        public ICommand LayerCommand => layerCommand ?? (layerCommand = new Command(async () =>
        {
            string res = await UserDialogs.Instance.ActionSheetAsync("Karta", "Stäng", null, null, new string[]
            {
                "Satellite",
                "Hybrid",
                "Street",
                "Terrain"
            });

            if (res == "Satellite")
            {
                map.MapType = MapType.Satellite;
            }
            else if (res == "Hybrid")
            {
                map.MapType = MapType.Hybrid;
            }
            else if (res == "Street")
            {
                map.MapType = MapType.Street;
            }
            else if (res == "Terrain")
            {
                map.MapType = MapType.Terrain;
            }
        }));

        private ICommand optionsCommand;
        public ICommand OptionsCommand => optionsCommand ?? (optionsCommand = new Command(() =>
        {
            AnimateMenu?.Invoke(this, null);
        }));

        private ICommand polygonCommand;
        public ICommand PolygonCommand => polygonCommand ?? (polygonCommand = new Command(async () =>
        {
            if (map.Editor.IsActive)
            {
                map.Editor.Complete();
            }

            DrawStatusIsVisible = true;
            DrawSatusMessage = "";

            AnimateMenu?.Invoke(this, null);
            var graphic = await map.Editor.StartEditAsync(GeometryType.Polygon, Color.Red);

            map.RemoveGraphic(graphic);
        }));

        private ICommand polylineCommand;
        public ICommand PolylineCommand => polylineCommand ?? (polylineCommand = new Command(async () =>
        {
            if (map.Editor.IsActive)
            {
                map.Editor.Complete();
            }

            DrawStatusIsVisible = true;
            DrawSatusMessage = "";

            AnimateMenu?.Invoke(this, null);
            var graphic = await map.Editor.StartEditAsync(GeometryType.Polyline, Color.Red);

            map.RemoveGraphic(graphic);
        }));

        private ICommand completeCommand;
        public ICommand CompleteCommand => completeCommand ?? (completeCommand = new Command(() =>
        {
            map.Editor.Complete();
        }));

        private ICommand restartDrawCommand;
        public ICommand RestartDrawCommand => restartDrawCommand ?? (restartDrawCommand = new Command(() =>
        {
            DrawSatusMessage = "";
            map.Editor.Restart();
        }));

        private ICommand closeDrawCommand;
        public ICommand CloseDrawCommand => closeDrawCommand ?? (closeDrawCommand = new Command(() =>
        {
            DrawStatusIsVisible = false;
            DrawSatusMessage = "";
            map.Editor.Complete();
        }));

        public string DrawSatusMessage { get; set; }
        public bool DrawStatusIsVisible { get; set; }

        public event EventHandler AnimateMenu;

        private Grid overlayMenu;
        private ExtendedMap map;
    }
}
