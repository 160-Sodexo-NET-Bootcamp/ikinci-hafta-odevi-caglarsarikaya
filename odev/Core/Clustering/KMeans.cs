using Data.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Clustering
{
    public class KMeans
    {
        public List<ContainerCluster> Cluster(List<ContainerCluster> containerList, int numClusters)
        {
            List<ContainerCluster> data = Normalized(containerList); // so large values don't dominate

            bool changed = true; // was there a change in at least one cluster assignment?
            bool success = true; // were all means able to be computed? (no zero-count clusters)

            data = InitClustering(data, numClusters, 0); // semi-random initialization
            List<ContainerCluster> means = Allocate(numClusters); // small convenience

            int maxCount = data.Count() * 10; // sanity check
            int ct = 0;
            while (changed == true && success == true && ct < maxCount)
            {
                ++ct; // k-means typically converges very quickly
                success = UpdateMeans(data, means); // compute new cluster means if possible. no effect if fail
                changed = UpdateClustering(data, means); // (re)assign tuples to clusters. no effect if fail
            }
            return data;
        }

        private List<ContainerCluster> Normalized(List<ContainerCluster> rawData)
        {
            var json = JsonConvert.SerializeObject(rawData);
            List<ContainerCluster> result = JsonConvert.DeserializeObject<List<ContainerCluster>>(json); ;

            double colSumX = 0.0;
            double colSumY = 0.0;
            foreach (var element in result)
            {
                colSumX += element.LocationX;
                colSumY += element.LocationY;
            }

            double meanX = colSumX / result.Count();
            double meanY = colSumY / result.Count();

            double sumX = 0.0;
            double sumY = 0.0;

            foreach (var element in result)
            {
                sumX += Math.Pow((element.LocationX - meanX), 2);
                sumY += Math.Pow((element.LocationY - meanY), 2);
            }

            double sdX = sumX / result.Count();
            double sdY = sumY / result.Count();

            foreach (var element in result)
            {
                element.LocationX = (element.LocationX - meanX) / sdX;
                element.LocationY = (element.LocationY - meanY) / sdY;
            }
            return result;
        }

        private List<ContainerCluster> InitClustering(List<ContainerCluster> data, int numClusters, int randomSeed)
        {
            //   int numTuples count
            Random random = new Random(randomSeed);
            int[] clustering = new int[data.Count()];
            for (int i = 0; i < numClusters; ++i) // make sure each cluster has at least one tuple
                data[i].GroupId = i;
            for (int i = numClusters; i < clustering.Length; ++i)
                data[i].GroupId = random.Next(0, numClusters); // other assignments random
            return data;
        }
        private List<ContainerCluster> Allocate(int numClusters)
        {
            // convenience matrix allocator for Cluster()
            List<ContainerCluster> result = new List<ContainerCluster>();
            for (int k = 0; k < numClusters; ++k)
                result.Add(new ContainerCluster { });
            return result;
        }

        private bool UpdateMeans(List<ContainerCluster> data, List<ContainerCluster> means)
        {

            int numClusters = means.Count();
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Count(); ++i)
            {
                int cluster = data[i].GroupId;
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to means[][]

            // update, zero-out means so it can be used as scratch matrix 
            for (int k = 0; k < means.Count(); ++k)
            {
                means[k].LocationX = 0.0;
                means[k].LocationY = 0.0;
            }


            for (int i = 0; i < data.Count(); ++i)
            {
                int cluster = data[i].GroupId;
                means[cluster].LocationX += data[i].LocationX; // accumulate sum
                means[cluster].LocationY += data[i].LocationY; // accumulate sum
            }

            for (int k = 0; k < means.Count(); ++k)
            {
                means[k].LocationX /= clusterCounts[k]; // danger of div by 0
                means[k].LocationY /= clusterCounts[k]; // danger of div by 0
            }

            return true;
        }

        private bool UpdateClustering(List<ContainerCluster> data, List<ContainerCluster> means)
        {
            // (re)assign each tuple to a cluster (closest mean)
            // returns false if no tuple assignments change OR
            // if the reassignment would result in a clustering where
            // one or more clusters have no tuples.

            int numClusters = means.Count();
            bool changed = false;

            double[] distances = new double[numClusters]; // distances from curr tuple to each mean

            for (int i = 0; i < data.Count(); ++i) // walk thru each tuple
            {
                for (int k = 0; k < numClusters; ++k)
                    distances[k] = Distance(data[i], means[k]); // compute distances from curr tuple to all k means

                int newClusterID = MinIndex(distances); // find closest mean ID
                if (newClusterID != data[i].GroupId)
                {
                    changed = true;
                    data[i].GroupId = newClusterID; // update
                }
            }

            if (changed == false)
                return false; // no change so bail and don't update clustering[][]

            // check proposed clustering[] cluster counts
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Count(); ++i)
            {
                int cluster = data[i].GroupId;
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to clustering[][]

            return true; // good clustering and at least one change
        }

        private double Distance(ContainerCluster first, ContainerCluster second)
        {
            // Euclidean distance between two vectors for UpdateClustering()
            // consider alternatives such as Manhattan distance
            double sumSquaredDiffs;
            sumSquaredDiffs = Math.Pow((first.LocationX - second.LocationX), 2) + Math.Pow((first.LocationY - second.LocationY), 2);
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
