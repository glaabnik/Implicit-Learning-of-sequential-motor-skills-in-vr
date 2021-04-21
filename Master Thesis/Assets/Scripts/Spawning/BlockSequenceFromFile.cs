using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSequenceFromFile : BlockSequence
{
    public string filenameCsv;
    public int iterations;

    private List<SphereCoordinates> sequenceOfSpawns;
    private int actIndexInList;

    public override bool hasNextSphereCoordinates()
    {
        if (actIndexInList % sequenceOfSpawns.Count == 0 && actIndexInList > 0)
        {
            if (pointScoreActIteration > maxPointScoreOneIteration) maxPointScoreOneIteration = pointScoreActIteration;
            pointScoreAllIterations.Add(pointScoreActIteration);
            pointScoreActIteration = 0;
        }
        return actIndexInList < iterations * sequenceOfSpawns.Count;
    }

    public override SphereCoordinates nextSphereCoordinates()
    {
        return sequenceOfSpawns[actIndexInList++ % sequenceOfSpawns.Count];
    }

    public override SphereCoordinates[] twoRandomSphereCoordinatesPairsForWholeSequence()
    {
        SphereCoordinates[] result = new SphereCoordinates[sequenceOfSpawns.Count * 2];
        List<int> usedIndexes = new List<int>();
        int resultIndex = 0;
        for(int i = 0; i < sequenceOfSpawns.Count; ++i)
        {
            int z = Random.Range(0, sequenceOfSpawns.Count);
            while (usedIndexes.Contains(z)) z = Random.Range(0, sequenceOfSpawns.Count);

            usedIndexes.Add(z);
            result[resultIndex++] = sequenceOfSpawns[z];
            result[resultIndex++] = sequenceOfSpawns[(z == sequenceOfSpawns.Count - 1 ? 0 : z + 1)];
        }
        return result;
    }

    public override void Awake()
    {
        base.Awake();
        sequenceOfSpawns = new List<SphereCoordinates>();
        if (!string.IsNullOrEmpty(filenameCsv)) LoadFixedSequenceOfSpawns.loadSpawnSequence(ref sequenceOfSpawns, filenameCsv);
    }
}
