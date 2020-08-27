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
        return actIndexInList < iterations * sequenceOfSpawns.Count;
    }

    public override SphereCoordinates nextSphereCoordinates()
    {
        return sequenceOfSpawns[actIndexInList++ % sequenceOfSpawns.Count];
    }

    public override void Start()
    {
        sequenceOfSpawns = new List<SphereCoordinates>();
        if (!string.IsNullOrEmpty(filenameCsv)) LoadFixedSequenceOfSpawns.loadSpawnSequence(ref sequenceOfSpawns, filenameCsv);
    }
}
