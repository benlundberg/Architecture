using MapSpecific.Controls;
using MapSpecific.Extensions;
using MapSpecific.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapSpecific.Editor
{
    public class DrawEditor : IDisposable
    {
        public DrawEditor(ExtendedMap map)
        {
            this.map = map;
            this.map.PinDragging += Map_PinDragging;
        }

        private void Map_PinDragging(object sender, PinDragEventArgs e)
        {
            if (e.Pin.Tag == null)
            {
                return;
            }

            int indexOfMovingPin = int.Parse(((PinTag)e.Pin.Tag)?.Position?.ToString() ?? "0");

            indexOfMovingPin = indexOfMovingPin > 0 ? indexOfMovingPin - 1 : indexOfMovingPin;

            if (graphic.GeometryType == GeometryType.Polygon)
            {
                ((Polygon)graphic.Geometry).Positions.RemoveAt(indexOfMovingPin);
                ((Polygon)graphic.Geometry).Positions.Insert(indexOfMovingPin, e.Pin.Position);
            }
            else if (graphic.GeometryType == GeometryType.Polyline)
            {
                ((Polyline)graphic.Geometry).Positions.RemoveAt(indexOfMovingPin);
                ((Polyline)graphic.Geometry).Positions.Insert(indexOfMovingPin, e.Pin.Position);
            }
            else if (graphic.GeometryType == GeometryType.Circle)
            {
                ((Circle)graphic.Geometry).Center = e.Pin.Position;
            }

            GraphicChanged?.Invoke(this, graphic);
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            DrawPointForGraphic(e.Point);
        }

        public async Task<Graphic> StartEditAsync(GeometryType geometryType, Color color, Graphic graphic = null)
        {
            if (IsActive)
            {
                return null;
            }

            this.graphic = graphic;
            this.geometryType = geometryType;
            this.color = color;

            if (graphic != null)
            {
                DrawPins();
            }

            taskCompletionSource = new TaskCompletionSource<Graphic>();

            map.MapClicked += Map_MapClicked;

            var finishedGraphic = await taskCompletionSource.Task;

            IsActive = false;

            map.RemoveDrawnPins();

            map.MapClicked -= Map_MapClicked;

            return finishedGraphic;
        }

        public bool ReplaceGraphic(Graphic graphic)
        {
            this.graphic = graphic;

            return true;
        }

        public void Complete()
        {
            IsActive = false;
            taskCompletionSource.TrySetResult(graphic);
        }

        public void Restart()
        {
            map.RemoveGraphic(graphic);

            graphic = null;
        }

        public void Cancel()
        {
            IsActive = false;
            taskCompletionSource = null;
        }

        public void RemovePins()
        {
            map.RemoveDrawnPins();
        }

        public void RemoveCircle(string tagName)
        {
            var circles = map.Circles?.Where(x => x.Tag?.ToString() == tagName).ToList();

            if (circles?.Any() == true)
            {
                foreach (var circle in circles)
                {
                    map.Circles.Remove(circle);
                }
            }
        }

        private void DrawPins()
        {
            switch (graphic.GeometryType)
            {
                case GeometryType.Polyline:
                    int lineIndex = 0;

                    foreach (var position in ((Polyline)graphic.Geometry).Positions)
                    {
                        lineIndex++;
                        AddPin(position, lineIndex.ToString());
                    }
                    break;
                case GeometryType.Polygon:
                    int polyIndex = 0;

                    foreach (var position in ((Polygon)graphic.Geometry).Positions)
                    {
                        polyIndex++;
                        AddPin(position, polyIndex.ToString());
                    }
                    break;
            }
        }

        private void AddPin(Position position, string index)
        {
            map.Pins.Add(new Pin()
            {
                Tag = new PinTag("DrawnPin", index),
                Label = "",
                Position = position,
                IsDraggable = true,
                ZIndex = 40,
                Icon = BitmapDescriptorFactory.DefaultMarker(color)
            });
        }

        private void DrawPointForGraphic(Position position)
        {
            switch (geometryType)
            {
                case GeometryType.Polyline:
                    HandlePolyline(position);
                    break;
                case GeometryType.Polygon:
                    HandlePolygon(position);
                    break;
                case GeometryType.Circle:
                    HandleCircle(position);
                    break;
                case GeometryType.Point:
                    HandlePoint(position);
                    break;
                default:
                    break;
            }

            GraphicChanged?.Invoke(this, graphic);
        }

        private void HandlePoint(Position position)
        {
            // RemovePins("AddedPin");

            var pin = new Pin()
            {
                Tag = "AddedPin",
                Label = "",
                Position = position,
                IsDraggable = true,
                ZIndex = 40,
                Icon = BitmapDescriptorFactory.DefaultMarker(color)
            };

            graphic = new Graphic
            {
                Geometry = pin,
                Color = color
            };

            map.Pins.Add(pin);
        }

        private void HandleCircle(Position position)
        {
            //RemovePins("AddedPin");

            var circlePin = new Pin()
            {
                Tag = "AddedPin",
                Label = "",
                Position = position,
                IsDraggable = true,
                ZIndex = 40,
                Icon = BitmapDescriptorFactory.DefaultMarker(color)
            };

            graphic = new Graphic
            {
                Geometry = new Circle()
                {
                    Tag = "DrawnCircle",
                    Center = position,
                    FillColor = Color.Transparent,
                    StrokeColor = color,
                    StrokeWidth = 2,
                    ZIndex = 30,
                    Radius = Distance.FromMeters(100)
                },
                Color = color
            };

            RemoveCircle("DrawnCircle");
            map.Circles.Add((Circle)graphic.Geometry);
            map.Pins.Add(circlePin);
        }

        private void HandlePolygon(Position position)
        {
            map.Pins.Add(new Pin()
            {
                Tag = new PinTag("DrawnPin", graphic?.Geometry == null ? "0" : (((Polygon)graphic.Geometry).Positions?.Count + 1)?.ToString()),
                Label = "",
                Position = position,
                IsDraggable = true,
                ZIndex = 40,
                Icon = BitmapDescriptorFactory.DefaultMarker(color)
            });

            if (graphic == null)
            {
                graphic = new Graphic
                {
                    Geometry = new Polygon()
                    {
                        StrokeColor = color,
                        FillColor = Color.Transparent,
                        StrokeWidth = 2,
                        ZIndex = 30
                    },
                    Color = color
                };
            }

            ((Polygon)graphic.Geometry).Positions.Add(position);

            if (((Polygon)graphic.Geometry).Positions.Count >= 3)
            {
                map.Polygons.Remove(((Polygon)graphic.Geometry));
                map.Polygons.Add(((Polygon)graphic.Geometry));
            }
        }

        private void HandlePolyline(Position position)
        {
            map.Pins.Add(new Pin()
            {
                Tag = new PinTag("DrawnPin", graphic?.Geometry == null ? "0" : (((Polyline)graphic.Geometry).Positions?.Count + 1)?.ToString()),
                Label = "",
                Position = position,
                IsDraggable = true,
                ZIndex = 40,
                Icon = BitmapDescriptorFactory.DefaultMarker(color)
            });

            if (graphic == null)
            {
                graphic = new Graphic
                {
                    Geometry = new Polyline()
                    {
                        StrokeColor = color,
                        StrokeWidth = 2,
                        ZIndex = 30
                    },
                    Color = color
                };
            }

            ((Polyline)graphic.Geometry).Positions.Add(position);

            if (((Polyline)graphic.Geometry).Positions.Count >= 2)
            {
                map.Polylines.Remove(((Polyline)graphic.Geometry));
                map.Polylines.Add(((Polyline)graphic.Geometry));
            }
        }

        public void Dispose()
        {
            map.PinDragging -= Map_PinDragging;
            GC.SuppressFinalize(this);
        }

        public bool IsActive { get; private set; }

        private Graphic graphic;
        private GeometryType geometryType;
        private Color color;
        private ExtendedMap map;

        public event EventHandler<Graphic> GraphicChanged;

        private TaskCompletionSource<Graphic> taskCompletionSource;
    }
}
