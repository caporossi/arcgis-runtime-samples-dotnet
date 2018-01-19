﻿// Copyright 2018 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific
// language governing permissions and limitations under the License.

using System;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArcGISRuntime.WPF.Samples.FormatCoordinates
{
    public partial class FormatCoordinates
    {
        public FormatCoordinates()
        {
            InitializeComponent();

            // Create the UI, setup the control references and execute initialization
            Initialize();
        }

        private void Initialize()
        {
            // Create the map
            MyMapView.Map = new Map(Basemap.CreateNavigationVector());

            // Add the graphics overlay to the map
            MyMapView.GraphicsOverlays.Add(new GraphicsOverlay());

            // Create the starting point
            MapPoint startingPoint = new MapPoint(0, 0, SpatialReferences.WebMercator);

            // Update the UI with the initial point
            UpdateUiFromMapPoint(startingPoint);

            // Subscribe to text change events
            txtUTM.TextChanged += InputTextChanged;
            txtDMS.TextChanged += InputTextChanged;
            txtDecimalDegrees.TextChanged += InputTextChanged;
            txtUSNG.TextChanged += InputTextChanged;

            // Subscribe to map tap events to enable tapping on map to update coordinates
            MyMapView.GeoViewTapped += (sender, args) => { UpdateUiFromMapPoint(args.Location); };
        }

        private void InputTextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the textbox that sent the event
            TextBox txtSender = (TextBox)sender;

            // Hold the entered point
            MapPoint enteredPoint = null;
            
            // Update the point based on which textbox sent the event
            try
            {
                switch (txtSender.Tag.ToString())
                {
                    case "Decimal Degrees":
                        enteredPoint =
                            CoordinateFormatter.FromLatitudeLongitude(txtSender.Text, MyMapView.SpatialReference);
                        break;

                    case "Degrees, Minutes, Seconds":
                        enteredPoint =
                            CoordinateFormatter.FromLatitudeLongitude(txtSender.Text, MyMapView.SpatialReference);
                        break;

                    case "UTM":
                        enteredPoint =
                            CoordinateFormatter.FromLatitudeLongitude(txtSender.Text, MyMapView.SpatialReference);
                        break;

                    case "USNG":
                        enteredPoint =
                            CoordinateFormatter.FromLatitudeLongitude(txtSender.Text, MyMapView.SpatialReference);
                        break;
                }
            }
            catch (Exception)
            {
                // The coordinate is malformed, return
                // Sample doesn't handle this because coordinates can be invalid while the user is editing
                return;
            }

            // Return if getting the point from text failed
            if (enteredPoint == null)
            {
                return;
            }

            // Update the UI from the MapPoint
            UpdateUiFromMapPoint(enteredPoint);
        }

        private void UpdateUiFromMapPoint(MapPoint startingPoint)
        {
            // Update the decimal degrees textbox
            txtDecimalDegrees.Text =
                CoordinateFormatter.ToLatitudeLongitude(startingPoint, LatitudeLongitudeFormat.DecimalDegrees, 4);

            // Update the degrees, minutes, seconds textbox
            txtDMS.Text = CoordinateFormatter.ToLatitudeLongitude(startingPoint,
                LatitudeLongitudeFormat.DegreesMinutesSeconds, 1);

            // Update the UTM textbox
            txtUTM.Text = CoordinateFormatter.ToUtm(startingPoint, UtmConversionMode.LatitudeBandIndicators, true);

            // Update the USNG textbox
            txtUSNG.Text = CoordinateFormatter.ToUsng(startingPoint, 4, true);

            // Clear existing graphics overlays
            MyMapView.GraphicsOverlays[0].Graphics.Clear();

            // Create a symbol to symbolize the point
            SimpleMarkerSymbol symbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.X, Colors.Yellow, 20);

            // Create the graphic
            Graphic symbolGraphic = new Graphic(startingPoint, symbol);

            // Add the graphic to the graphics overlay
            MyMapView.GraphicsOverlays[0].Graphics.Add(symbolGraphic);
        }
    }
}