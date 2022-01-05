using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Clustering
{
    public class Clustering
    {
        public static List<Cluster> Cluster(List<Container> rawData, int numClusters)
        {
            // k-means clustering
            // index of return is tuple ID, cell is cluster ID
            // ex: [2 1 0 0 2 2] means tuple 0 is cluster 2, tuple 1 is cluster 1, tuple 2 is cluster 0, tuple 3 is cluster 0, etc.
            // an alternative clustering DS to save space is to use the .NET BitArray class
            List<Cluster> data = Normalized(rawData); // so large values don't dominate

            bool changed = true; // was there a change in at least one cluster assignment?
            bool success = true; // were all means able to be computed? (no zero-count clusters)

            var clustering =  Enumerable.Range(0, numClusters).Select(x => new Cluster {GroupId = x }).ToList();

            int maxCount = data.Count() * 10; // sanity check
            int ct = 0;
            while (changed == true && success == true && ct < maxCount)
            {
                ++ct; // k-means typically converges very quickly
                success = UpdateMeans(data, clustering); // compute new cluster means if possible. no effect if fail
                changed = UpdateClustering(data, clustering); // (re)assign tuples to clusters. no effect if fail
            }
            // consider adding means[][] as an out parameter - the final means could be computed
            // the final means are useful in some scenarios (e.g., discretization and RBF centroids)
            // and even though you can compute final means from final clustering, in some cases it
            // makes sense to return the means (at the expense of some method signature uglinesss)
            //
            // another alternative is to return, as an out parameter, some measure of cluster goodness
            // such as the average distance between cluster means, or the average distance between tuples in 
            // a cluster, or a weighted combination of both
            return clustering;
        }

        private static List<Cluster> Normalized(List<Container> rawData)
        {
            // normalize raw data by computing (x - mean) / stddev
            // primary alternative is min-max:
            // v' = (v - min) / (max - min)

            // make a copy of input data
            var clusterList = new List<Cluster>();
            foreach (var item in rawData)
            {
                clusterList.Add(new Cluster { Id = item.Id, LocationX = item.LocationX, LocationY = item.LocationY });
            }

            double colSumX = 0;
            double colSumY = 0;
            foreach (var element in clusterList)
            {
                colSumX += element.LocationX;
                colSumY += element.LocationY;
            }

            double meanX = colSumX / clusterList.Count(); // dikine ort
            double meanY = colSumY / clusterList.Count(); // dikine ort

            double sumX = 0.0;
            double sumY = 0.0;

            foreach (var element in clusterList)
            {
                sumX += (element.LocationX - meanX) * (element.LocationX - meanX); // (butun dikine - ort)^2
                sumY += (element.LocationY - meanY) * (element.LocationY - meanY); // (butun dikine - ort)^2
            }

            double sdX = sumX / clusterList.Count();
            double sdY = sumY / clusterList.Count();

            foreach (var element in clusterList)
            {
                sumX += (element.LocationX - meanX) * (element.LocationX - meanX); // (butun dikine - ort)^2
                sumY += (element.LocationY - meanY) * (element.LocationY - meanY); // (butun dikine - ort)^2
            }

            foreach (var element in clusterList)
            {
                element.LocationX = (element.LocationX - meanX) / sdX;
                element.LocationY = (element.LocationY - meanY) / sdY;
            }

            return clusterList;
        }



        private static bool UpdateMeans(List<Cluster> data, List<Cluster> clustering)
        {
 
            int numClusters = clustering.Select(x => x.GroupId).Distinct().Count();
            int[] clusterMemberCounts = clustering.Select(x => x.GroupId).Distinct().ToArray();

            for (int k = 0; k < numClusters; ++k)
                if (clustering.Where(x => x.GroupId == clusterMemberCounts[k]).Count() == 0)
                    return false; // bad clustering. no change to means[][]
 
            for (int i = 0; i < data.Count(); ++i)
            {
                int cluster = clustering[i].GroupId;
                clustering[i].GroupId = cluster;
                clustering[i].LocationX = data[i].LocationX;  
                clustering[i].LocationY = data[i].LocationY;  
            }

            for (int k = 0; k < clustering.Count(); ++k)
            {
                clustering[k].LocationX /= clusterMemberCounts[k]; 
                clustering[k].LocationY /= clusterMemberCounts[k]; 
            }
            return true;
        }

        private static bool UpdateClustering(List<Cluster> data, List<Cluster> clustering)
        {
            // (re)assign each tuple to a cluster (closest mean)
            // returns false if no tuple assignments change OR
            // if the reassignment would result in a clustering where
            // one or more clusters have no tuples.

            int numClusters = clustering.Select(x => x.GroupId).Distinct().Count();
            bool changed = false;

            var newClustering = clustering;

            double[] distances = new double[numClusters]; // distances from curr tuple to each mean

            for (int i = 0; i < data.Count(); ++i) // walk thru each tuple
            {
                for (int k = 0; k < numClusters; ++k)
                    distances[k] = Distance(data[i].LocationX, data[i].LocationY, clustering[k].LocationX , clustering[k].LocationY); // compute distances from curr tuple to all k means

                int newClusterID = MinIndex(distances); // find closest mean ID
                if (newClusterID != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newClusterID; // update
                }
            }

            if (changed == false)
                return false; // no change so bail and don't update clustering[][]

            // check proposed clustering[] cluster counts
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = newClustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to clustering[][]

            Array.Copy(newClustering, clustering, newClustering.Length); // update
            return true; // good clustering and at least one change
        }

        private static double Distance(double x1, double y1, double x2, double y2)
        {
            // Euclidean distance between two vectors for UpdateClustering()
            // consider alternatives such as Manhattan distance
            double sumSquaredDiffs = 0.0;
            sumSquaredDiffs = Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        private static int MinIndex(double[] distances)
        {
            // index of smallest value in array
            // helper for UpdateClustering()
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }

    }
}
