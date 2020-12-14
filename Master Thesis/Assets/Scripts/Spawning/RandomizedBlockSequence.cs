﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedBlockSequence : BlockSequence
{
    public int countSphereCoordinatesOneIteration;
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

    private List<SphereCoordinates> sequenceOfSpawns;
    private int actIndexInList;

    public override void Start()
    {
        sequenceOfSpawns = new List<SphereCoordinates>();
        for (int i = 0; i < countSphereCoordinatesOneIteration; ++i)
        {
            generateRandomizedSphereCoordinates();
        } 
    }

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
        while (rotationZLeft == getLastRotationZ()) rotationZLeft = getRandomRotationZ();
        rotationZRight = getRandomRotationZ();
        while (rotationZRight == getLastRotationZ2()) rotationZRight = getRandomRotationZ();

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
        sequenceOfSpawns.Add(sc);
    }

    private float checkPhi(float phi, float phi2)
    {
        if (Mathf.Abs((phi - phi2)) < 40)
            return phi2 + 50;
        else return phi;
    }

    private float getLastRotationZ()
    {
        if (sequenceOfSpawns.Count > 0) return sequenceOfSpawns[sequenceOfSpawns.Count - 1].rotationZ;
        else return -1;
    }

    private float getLastRotationZ2()
    {
        if (sequenceOfSpawns.Count > 0) return sequenceOfSpawns[sequenceOfSpawns.Count - 1].rotationZ2;
        else return -1;
    }

    private int getRandomRotationZ()
    {
        int z = Random.Range(0, 7);
        return z * 45;
    }
}
