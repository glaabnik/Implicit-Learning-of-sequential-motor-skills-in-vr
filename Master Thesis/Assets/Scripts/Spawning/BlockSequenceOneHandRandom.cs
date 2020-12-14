using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSequenceOneHandRandom : BlockSequence
{
    public string filenameCsv;
    public int iterations;
    public bool leftHandRandomized = false;
    public bool rightHandRandomized = false;

    public bool randomizeElevation = false;
    public int defaultElevation = 90;
    public int offsetElevationToBothSides = 30;
    public int offsetLeftSideRangeMin = 15;
    public int offsetLeftSideRangeMax = 40;
    public int offsetRightSideRangeMin = -40;
    public int offsetRightSideRangeMax = -15;
    public float scaleSpawnedGameObjects;
    public float sphereRadius;

    private List<SphereCoordinates> sequenceOfSpawns;
    private int actIndexInList;

    public override bool hasNextSphereCoordinates()
    {
        return actIndexInList < iterations * sequenceOfSpawns.Count;
    }

    public override SphereCoordinates nextSphereCoordinates()
    {
        return sequenceOfSpawns[actIndexInList++ % sequenceOfSpawns.Count];
    }

    public override SphereCoordinates[] twoRandomSphereCoordinatesPairsForWholeSequence()
    {
        SphereCoordinates[] result = new SphereCoordinates[sequenceOfSpawns.Count];
        List<int> usedIndexes = new List<int>();
        int resultIndex = 0;
        for (int i = 0; i < sequenceOfSpawns.Count; ++i)
        {
            int z = Random.Range(0, sequenceOfSpawns.Count);
            while (usedIndexes.Contains(z)) z = Random.Range(0, sequenceOfSpawns.Count);

            usedIndexes.Add(z);
            result[resultIndex++] = sequenceOfSpawns[z];
            result[resultIndex++] = sequenceOfSpawns[(z == sequenceOfSpawns.Count - 1 ? 0 : z + 1)];
        }
        return result;
    }

    public override void Start()
    {
        sequenceOfSpawns = new List<SphereCoordinates>();
        if (!string.IsNullOrEmpty(filenameCsv)) LoadFixedSequenceOfSpawns.loadSpawnSequence(ref sequenceOfSpawns, filenameCsv);
        if (leftHandRandomized) generateRandomizedSphereCoordinates(true);
        if (rightHandRandomized) generateRandomizedSphereCoordinates(false);
    }

    private void generateRandomizedSphereCoordinates(bool leftHand)
    {
        for (int i = 0; i < sequenceOfSpawns.Count; ++i)
        {
            float degLeftPhi, degRightPhi, degLeftTheta, degRightTheta;
            float radiusLeft, radiusRight;
            float rotationZLeft, rotationZRight;
            Vector3 scaleLeft, scaleRight;
            degLeftPhi = Random.Range(offsetLeftSideRangeMin, offsetLeftSideRangeMax);
            degRightPhi = Random.Range(offsetRightSideRangeMin, offsetRightSideRangeMax);
            degLeftTheta = defaultElevation;
            degRightTheta = defaultElevation;
            if (randomizeElevation)
            {
                degLeftTheta = defaultElevation - offsetElevationToBothSides + Random.Range(0, offsetElevationToBothSides * 2);
                degRightTheta = defaultElevation - offsetElevationToBothSides + Random.Range(0, offsetElevationToBothSides * 2);
            }
            radiusLeft = sphereRadius;
            radiusRight = sphereRadius;
            rotationZLeft = getRandomRotationZ();
            rotationZRight = getRandomRotationZ();
            scaleLeft = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
            scaleRight = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
            SphereCoordinates sc = sequenceOfSpawns[i];
            if(leftHand)
            {
                sc.phi2 = degLeftPhi;
                sc.theta2 = degLeftTheta;
                sc.radius2 = radiusLeft;
                sc.scale2 = scaleLeft;
                sc.rotationZ2 = rotationZLeft;
            }
            if(!leftHand)
            {
                sc.phi = degRightPhi;
                sc.theta = degRightTheta;
                sc.radius = radiusRight;
                sc.scale = scaleRight;
                sc.rotationZ = rotationZRight;
            }
        }
    }

    private int getRandomRotationZ()
    {
        int z = Random.Range(0, 7);
        return z * 45;
    }
}
