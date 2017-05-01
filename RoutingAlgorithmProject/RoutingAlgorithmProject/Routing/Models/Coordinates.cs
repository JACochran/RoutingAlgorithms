using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsmSharp.Tags;

namespace RoutingAlgorithmProject.Graph
{
    /// <summary>
    /// Stored Latitude and longitude for a point on the globe
    /// Stores values as integers internally. This is to get around floating point issues. 
    /// </summary>
    public class Coordinates
    {
        private int latitude;
        private int longitude;

        public Coordinates(float? latitude, float? longitude)
        {
            if (latitude == null || longitude == null)
                throw new ArgumentNullException();
            this.latitude =(int) (latitude * 10000000);
            this.longitude = (int) (longitude * 10000000);            
        }

        public override bool Equals(object obj)
        {
            var item = obj as Coordinates;
            if (item == null)
                return false;

            return item.latitude.Equals(this.latitude) && item.longitude.Equals(this.longitude);
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = 67 * hash + (this.latitude/100);
            hash = 67 * hash +( this.longitude/100);
            return hash;
        }

        public float Latitude
        {
            get
            {
                return (float)(latitude / 10000000.0);
            }
        }

        public float Longitude
        {
            get
            {
                return (float)(longitude / 10000000.0);
            }
        }

        public override string ToString()
        {
            return "(" + this.Latitude.ToString() + "," + this.Longitude.ToString() + ") ";
        }

        public float DistanceBetween(Coordinates to)
        {
            var dLat = this.latitude - to.latitude;  // deg2rad below
            var dLon = this.longitude - to.longitude;
            float result = dLat * dLat + dLon * dLon;
            if (result < 0)
            {
                return -1;
            }
            else
                return (float)Math.Sqrt(result);
        }
    }
}
