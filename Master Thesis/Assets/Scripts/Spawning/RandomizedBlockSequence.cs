using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedBlockSequence : BlockSequence
{
    public int countCubePairs;
    public int iterations;
    public bool randomizeElevation = false;
    public int defaultElevation = 90;
    public int offsetElevationToBothSides = 30;
    public int offsetLeftSideRangeMin = 15;
    public int offsetLeftSideRangeMax = 40;
    public int offsetRightSideRangeMin = -40;
    public int offsetRightSideRangeMax = -15;
    public float scaleSpawnedGameObjects;
    public float sphereRadius;
    public bool diagonalArrowsAreUsed = false;
    public bool redAndBlueCubesCanBeInterchanged = false;
    public bool allCubePairsAreRandom = true;
    

    private List<SphereCoordinates> sequenceOfSpawns;
    private int actIndexInList;
    private float lastRotationZLeft = -1, lastRotationZRight = -1;

    public override int getCubePairCount()
    {
        return countCubePairs;
    }

    public override int getIterationCount()
    {
        return iterations;
    }

    public override void Awake()
    {
        base.Awake();
        sequenceOfSpawns = new List<SphereCoordinates>();
        for (int i = 0; i < countCubePairs; ++i)
        {
            generateRandomizedSphereCoordinates();
        } 
    }

    public override bool hasNextSphereCoordinates()
    {
        if (actIndexInList % sequenceOfSpawns.Count == 0 && actIndexInList > 0)
        {
            if (pointScoreActIteration > maxPointScoreOneIteration) maxPointScoreOneIteration = pointScoreActIteration;
            pointScoreAllIterations.Add(pointScoreActIteration);
            pointScoreActIteration = 0;
            if(allCubePairsAreRandom)
            {
                sequenceOfSpawns.Clear();
                for (int i = 0; i < countCubePairs; ++i)
                {
                    generateRandomizedSphereCoordinates();
                }
            }
        }
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

    private void generateRandomizedSphereCoordinates()
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
        while (rotationZLeft == lastRotationZLeft) { rotationZLeft = getRandomRotationZ(); }
        lastRotationZLeft = rotationZLeft;
        rotationZRight = getRandomRotationZ();
        while (rotationZRight == lastRotationZRight) { rotationZRight = getRandomRotationZ(); }
        lastRotationZRight = rotationZRight;

        scaleLeft = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        scaleRight = new Vector3(scaleSpawnedGameObjects, scaleSpawnedGameObjects, scaleSpawnedGameObjects);
        SphereCoordinates sc = new SphereCoordinates();
        sc.phi = checkPhi(degLeftPhi, degRightPhi);
        sc.phi2 = degRightPhi;
        sc.theta = degLeftTheta;
        sc.theta2 = degRightTheta;
        sc.radius = radiusLeft;
        sc.radius2 = radiusRight;
        sc.scale = scaleLeft;
        sc.scale2 = scaleRight;
        sc.rotationZ = rotationZLeft;
        sc.rotationZ2 = rotationZRight;
        if (redAndBlueCubesCanBeInterchanged)
        {
            int z = Random.Range(0, 10);
            if (z > 5) // interchange red and blue cube
            {
                float phiT = sc.phi;
                float thetaT = sc.theta;
                float radiusT = sc.radius;
                Vector3 scaleT = sc.scale;
                float rotationZT = sc.rotationZ;

                sc.phi = sc.phi2;
                sc.theta = sc.theta2;
                sc.radius = sc.radius2;
                sc.scale = sc.scale2;
                sc.rotationZ = sc.rotationZ2;

                sc.phi2 = phiT;
                sc.theta2 = thetaT;
                sc.radius2 = radiusT;
                sc.scale2 = scaleT;
                sc.rotationZ2 = rotationZT;
            }
        }
        swapLeftAndRightCoordinates(ref sc);
        sequenceOfSpawns.Add(sc);
    }

    private void swapLeftAndRightCoordinates(ref SphereCoordinates sc)
    {
        float phiTemp = sc.phi;
        float thetaTemp = sc.theta;
        float radiusTemp = sc.radius;
        float rotationZTemp = sc.rotationZ;
        Vector3 scaleTemp = sc.scale;
        sc.phi = sc.phi2;
        sc.theta = sc.theta2;
        sc.radius = sc.radius2;
        sc.rotationZ = sc.rotationZ2;
        sc.scale = sc.scale2;
        sc.phi2 = phiTemp;
        sc.theta2 = thetaTemp;
        sc.radius2 = radiusTemp;
        sc.rotationZ2 = rotationZTemp;
        sc.scale2 = scaleTemp;
    }

    private float checkPhi(float phi, float phi2)
    {
        if (Mathf.Abs((phi - phi2)) < 30)
            return phi2 + 30;
        else return phi;
    }

    private int getRandomRotationZ()
    {
        int z = Random.Range(0, 8);
        int z2 = Random.Range(0, 4);
        if (diagonalArrowsAreUsed) return z * 45;
        else return z2 * 90;
    }
}
