﻿using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArcGISRuntimeSDKDotNet_DesktopSamples.Samples
{
    /// <summary>
    /// Demonstrates how to create polyline geometries, attach them to graphics and display them on the map. Polyline geometry objects are used to store geographic lines.
    /// </summary>
    /// <title>Create Polylines</title>
	/// <category>Geometry</category>
	public partial class CreatePolylines : UserControl
    {
        /// <summary>Construct Create Polylines sample control</summary>
        public CreatePolylines()
        {
            InitializeComponent();

            var _ = CreatePolylineGraphicsAsync();
        }

        // Create polyline graphics on the map in the center and the center of four equal quadrants
        private async Task CreatePolylineGraphicsAsync()
        {
            await MyMapView.LayersLoadedAsync();

            var height = MyMapView.Extent.Height / 4;
            var width = MyMapView.Extent.Width / 4;
            var length = width / 4;
            var center = MyMapView.Extent.GetCenter();

			var topLeft = new MapPoint(center.X - width, center.Y + height, MyMapView.SpatialReference);
			var topRight = new MapPoint(center.X + width, center.Y + height, MyMapView.SpatialReference);
			var bottomLeft = new MapPoint(center.X - width, center.Y - height, MyMapView.SpatialReference);
			var bottomRight = new MapPoint(center.X + width, center.Y - height, MyMapView.SpatialReference);

            var redSymbol = new SimpleLineSymbol() { 
				Color = System.Windows.Media.Colors.Red, 
				Width = 4, 
				Style = SimpleLineStyle.Solid 
			};
            var blueSymbol = new SimpleLineSymbol() { 
				Color = System.Windows.Media.Colors.Blue, 
				Width = 4, 
				Style = SimpleLineStyle.Solid 
			};

            graphicsOverlay.Graphics.Add(new Graphic() { Geometry = CreatePolylineX(center, length), Symbol = blueSymbol });
			graphicsOverlay.Graphics.Add(new Graphic() { Geometry = CreatePolylineX(topLeft, length), Symbol = redSymbol });
			graphicsOverlay.Graphics.Add(new Graphic() { Geometry = CreatePolylineX(topRight, length), Symbol = redSymbol });
			graphicsOverlay.Graphics.Add(new Graphic() { Geometry = CreatePolylineX(bottomLeft, length), Symbol = redSymbol });
			graphicsOverlay.Graphics.Add(new Graphic() { Geometry = CreatePolylineX(bottomRight, length), Symbol = redSymbol });
        }

        // Creates a polyline with two paths in the shape of an 'X' centered at the given point
        private Polyline CreatePolylineX(MapPoint center, double length)
        {
            var halfLen = length / 2.0;

            PointCollection coords1 = new PointCollection();
            coords1.Add(new MapPoint(center.X - halfLen, center.Y + halfLen));
			coords1.Add(new MapPoint(center.X + halfLen, center.Y - halfLen));

            PointCollection coords2 = new PointCollection();
			coords2.Add(new MapPoint(center.X + halfLen, center.Y + halfLen));
			coords2.Add(new MapPoint(center.X - halfLen, center.Y - halfLen));

            return new Polyline(new PartCollection { coords1, coords2 }, MyMapView.SpatialReference);
        }
    }
}
