namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
    using static AttributesReader;
    using System;

    public class MySpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;
        AttributesReader s1;

        /*Loupas Code*/
        public bool once;
        public GameObject point_prefab;
        public GameObject cylinder_prefab;
        /* end */
        
        void Start()
		{
            once = true;
			_locations = new Vector2d[_locationStrings.Length];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				
				_spawnedObjects.Add(instance);
			}

            /*Loupas Code Starts Here*/
            s1 = GetComponent<AttributesReader>();



            /*Loupas Code Ends Here*/
        }

        private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
            /*Loupas Code Starts Here*/

            if (s1.initialised && once)
            {

                List<Pipe> testList = s1.Pipes;

                if (_map.InitialZoom != 0) { 
                SpawnPoints(testList);
                once = false;
                    MoveToLayer(GameObject.Find("ArAlignedMap").transform, 11);
                }
            }
            /*Loupas Code Ends Here*/
        }

        /*Loupas Code Starts Here*/
        void SpawnPoints(List<Pipe> pipes)
        {



            //Debug.Log("I m in 1");

            //Debug.Log("Number of elements "+pipes.Count);
            int i = 0;
            Vector3 start = new Vector3(1,1,1);
            Vector3 end;
        
            foreach (var pipe in pipes)
            {
                //Debug.Log("I m in 2");
                //Debug.Log(pipe.ToString());
                var instance = Instantiate(point_prefab);
                instance.name = pipe.getId().ToString()+pipe.getType().ToString()+"(StartPoint)";
                var instance2 = Instantiate(point_prefab);
                instance2.name = pipe.getId().ToString() + pipe.getType().ToString() + "(EndPoint)";




                var location_start_str = pipe.getStart_Y().ToString() + "," + pipe.getStart_X().ToString();
                var location_end_str = pipe.getEnd_Y().ToString() + "," + pipe.getEnd_X().ToString();
                //Debug.Log("Pipe loco : "+location_str);
                Vector2d start_loc = Conversions.StringToLatLon(location_start_str);
                Vector2d end_loc = Conversions.StringToLatLon(location_end_str);
                //Debug.Log("Vec print : " + loc.ToString() );
                instance.transform.localPosition = new Vector3( _map.GeoToWorldPosition(start_loc,false).x , _map.GeoToWorldPosition(start_loc, false).y-1, _map.GeoToWorldPosition(start_loc, false).z);
                instance2.transform.localPosition = new Vector3(_map.GeoToWorldPosition(end_loc, false).x, _map.GeoToWorldPosition(end_loc, false).y-1, _map.GeoToWorldPosition(end_loc, false).z);

                //instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                instance.transform.parent = GameObject.Find("PipeStartPoints").transform;

                instance2.transform.parent = GameObject.Find("PipeEndPoints").transform;
                
                CreateCylinderBetweenPoints(instance.transform.localPosition, instance2.transform.localPosition, 1);


                /*
                if (i == 0)
                {
                    i = 1;
                    start = instance.transform.localPosition;

                }
                else
                {
                    i = 0;
                    end = instance.transform.localPosition;
                    CreateCylinderBetweenPoints(start, end, 2f);


                }*/


            }
            Vector3 old = GameObject.Find("PipeSystem").transform.position;

            //GameObject.Find("Pipes").transform.position= new Vector3(old.x+10,old.y-4,old.z-1);



        }

       void CreateCylinderBetweenPoints(Vector3 start , Vector3 end,float width)
        {
            var offset = end - start;
            var scale = new Vector3(width + 1, (offset.magnitude / 2.0f)+1, width + 1);
            var position = start + (new Vector3 ( offset.x / 2.0f , offset.y / 2.0f , offset.z / 2.0f ) );


            var cylinder = Instantiate(cylinder_prefab, position, Quaternion.identity);
            cylinder.transform.up = offset;
            cylinder.transform.localScale = scale;
            cylinder.transform.parent = GameObject.Find("Pipes").transform;

        }

        /// <summary>
        /// Converts WGS84 lat/lon to x/y meters in reference to a center point
        /// </summary>
        /// <param name="lat"> The latitude. </param>
        /// <param name="lon"> The longitude. </param>
        /// <param name="refPoint"> A <see cref="T:UnityEngine.Vector2d"/> center point to offset resultant xy</param>
        /// <param name="scale"> Scale in meters. (default scale = 1) </param>
        /// <returns> A <see cref="T:UnityEngine.Vector2d"/> xy tile ID. </returns>
        /// <example>
        /// Converts a Lat/Lon of (37.7749, 122.4194) into Unity coordinates for a map centered at (10,10) and a scale of 2.5 meters for every 1 Unity unit 
        /// <code>
        /// var worldPosition = Conversions.GeoToWorldPosition(37.7749, 122.4194, new Vector2d(10, 10), (float)2.5);
        /// // worldPosition = ( 11369163.38585, 34069138.17805 )
        /// </code>
        /// </example>
        public static Vector2d GeoToWorldPositionWGS(double lat, double lon, Vector2d refPoint, float scale = 1)
        {
            int EarthRadius = 6378137;
            double OriginShift = 2 * Math.PI * EarthRadius / 2;
            var posx = lon * OriginShift / 180;
            var posy = Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180);
            posy = posy * OriginShift / 180;
            return new Vector2d((posx - refPoint.x) * scale, (posy - refPoint.y) * scale);
        }

        /*Loupas Code Ends Here*/
        //recursive calls
        void MoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                MoveToLayer(child, layer);
        }
    }
}