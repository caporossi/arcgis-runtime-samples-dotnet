// Copyright 2017 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an 
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific 
// language governing permissions and limitations under the License.

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.UI.GeoAnalysis;
using Foundation;
using System;
using UIKit;

namespace ArcGISRuntimeXamarin.Samples.ViewshedCamera
{
    [Register("ViewshedCamera")]
    public class ViewshedCamera : UIViewController
    {
        // Create and hold reference to the used MapView
        private SceneView _mySceneView = new SceneView();

        // URL for a scene service of buildings in Brest, France
        private string _buildingsServiceUrl = @"http://tiles.arcgis.com/tiles/P3ePLMYs2RVChkJx/arcgis/rest/services/Buildings_Brest/SceneServer/layers/0";

        // URL for an image service to use as an elevation source
        private string _elevationSourceUrl = @"http://scene.arcgis.com/arcgis/rest/services/BREST_DTM_1M/ImageServer";

        // Location viewshed analysis to show visible and obstructed areas from the camera
        private LocationViewshed _viewshedForCamera;

        // Button for updating the analysis viewpoint
        private UIButton _updateViewshedButton;

        public ViewshedCamera()
        {
            Title = "Viewshed from camera";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Create the UI, setup the control references and execute initialization 
            CreateLayout();

            // Create the Scene, basemap, camera, and location viewshed analysis
            Initialize();
        }

        private void Initialize()
        {
            // Create a new Scene with an imagery basemap
            Scene myScene = new Scene(Basemap.CreateImagery());

            // Create a scene layer to show buildings in the Scene
            ArcGISSceneLayer buildingsLayer = new ArcGISSceneLayer(new Uri(_buildingsServiceUrl));
            myScene.OperationalLayers.Add(buildingsLayer);

            // Create an elevation source for the Scene
            ArcGISTiledElevationSource elevationSrc = new ArcGISTiledElevationSource(new Uri(_elevationSourceUrl));
            myScene.BaseSurface.ElevationSources.Add(elevationSrc);

            // Add the Scene to the SceneView
            _mySceneView.Scene = myScene;

            // Set the viewpoint with a new camera focused on the castle in Brest
            Camera observerCamera = new Camera(new MapPoint(-4.49492, 48.3808, 48.2511, SpatialReferences.Wgs84), 344.488, 74.1212, 0.0);
            _mySceneView.SetViewpointCameraAsync(observerCamera);

            // Create a LocationViewshed analysis using the camera as the observer
            _viewshedForCamera = new LocationViewshed(observerCamera, 1, 1000);

            // Create an analysis overlay to contain the viewshed analysis results
            AnalysisOverlay viewshedOverlay = new AnalysisOverlay();

            // Add the location viewshed analysis to the analysis overlay, then add the overlay to the scene view
            viewshedOverlay.Analyses.Add(_viewshedForCamera);
            _mySceneView.AnalysisOverlays.Add(viewshedOverlay);
        }

        private void UpdateObserverWithCamera(object sender, EventArgs e)
        {
            // Use the current camera to update the observer for the location viewshed analysis
            _viewshedForCamera.UpdateFromCamera(_mySceneView.Camera);
        }

        public override void ViewDidLayoutSubviews()
        {
            // Setup the visual frame for the SceneView
            _mySceneView.Frame = new CoreGraphics.CGRect(
                0, 0, View.Bounds.Width, View.Bounds.Height);

            // Update the visual frame for the button
            _updateViewshedButton.Frame = new CoreGraphics.CGRect(0, View.Bounds.Height - 60, View.Bounds.Width, 30);

            base.ViewDidLayoutSubviews();
        }
        
        private void CreateLayout()
        {
            // Create a button to update the viewshed using the current camera
            _updateViewshedButton = new UIButton(new CoreGraphics.CGRect(0,View.Bounds.Height - 60,View.Bounds.Width,30));
            _updateViewshedButton.SetTitle("Viewshed from here", UIControlState.Normal);
            _updateViewshedButton.TouchUpInside += UpdateObserverWithCamera;

            // Add SceneView and button to the page
            View.AddSubviews(_mySceneView, _updateViewshedButton);
        }
    }
}