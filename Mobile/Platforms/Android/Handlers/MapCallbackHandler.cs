
using Android.Gms.Maps;
using IMap = Microsoft.Maui.Maps.IMap;

namespace Mobile.Platforms.Android.Handlers
{
    class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
    {
        private readonly Microsoft.Maui.Maps.Handlers.IMapHandler mapHandler;

        public MapCallbackHandler(Microsoft.Maui.Maps.Handlers.IMapHandler mapHandler)
        {
            this.mapHandler = mapHandler;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mapHandler.UpdateValue(nameof(IMap.Pins));
        }
    }

}
