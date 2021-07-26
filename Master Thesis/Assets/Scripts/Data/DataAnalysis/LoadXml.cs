using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;
using static SerializeData;
using UnityEngine;

public class LoadXml : MonoBehaviour
{
    public void Start()
    {
        LoadXml loadxml = new LoadXml();
        for (int i = 1; i < 14; ++i)
        {
            loadxml._DeserializeBlockGameData("Assets/xml/BlockData/Study/blockData" + i + ".xml");
            loadxml._DeserializeSportGameData("Assets/xml/SportGame/Study/sportGameData" + i + ".xml");
        }
    }

    private void _DeserializeBlockGameData(string filename)
    {
        Console.WriteLine("Reading with Stream");
        // Create an instance of the XmlSerializer.
        XmlSerializer serializer = new XmlSerializer(typeof(BlockGameData));

        // Declare an object variable of the type to be deserialized.
        BlockGameData bgd;

        using (Stream reader = new FileStream(filename, FileMode.Open))
        {
            // Call the Deserialize method to restore the object's state.
            bgd = (BlockGameData)serializer.Deserialize(reader);
        }

        int highscoreSum = 0;
        for(int i= 0; i < bgd.listPointScoreAllIterations.Count; ++i)
        {
            highscoreSum += bgd.listPointScoreAllIterations[i];
        }
        int bestScoreOfIterations = 0;
        for (int i = 0; i < bgd.listBestPointScoreAllIterations.Count; ++i)
        {
            if (bgd.listBestPointScoreAllIterations[i] > bestScoreOfIterations) bestScoreOfIterations = bgd.listBestPointScoreAllIterations[i];
        }
        Debug.Log(
        "Highscore: " + highscoreSum + "\n" +
        "Best Iteration Score: " + bestScoreOfIterations + "\n" +
        "Block 7 Best Score vs Block 8 Best Score: " + bgd.listBestPointScoreAllIterations[6] + " ; " + bgd.listBestPointScoreAllIterations[7] +  "\n" +
        "Block 10 Best Score vs Block 9 Best Score: "  + bgd.listBestPointScoreAllIterations[9] + " ; " + bgd.listBestPointScoreAllIterations[8] + "\n" +
        "Block 7 Avg Score vs Block 8 Avg Score: " + bgd.listAvgPointScoreAllIterations[6] + " ; " + bgd.listAvgPointScoreAllIterations[7] + "\n" +
        "Block 10 Avg Score vs Block 9 Avg Score: " + bgd.listAvgPointScoreAllIterations[9] + " ; " + bgd.listAvgPointScoreAllIterations[8] + "\n"
        );
    }

    private void _DeserializeSportGameData(string filename)
    {
        Console.WriteLine("Reading with Stream");
        // Create an instance of the XmlSerializer.
        XmlSerializer serializer = new XmlSerializer(typeof(SportGameData));

        // Declare an object variable of the type to be deserialized.
        SportGameData sgd;

        using (Stream reader = new FileStream(filename, FileMode.Open))
        {
            // Call the Deserialize method to restore the object's state.
            sgd = (SportGameData)serializer.Deserialize(reader);
        }

        int verygoodHitsCountLeft = 0;
        int verygoodHitsCountRight = 0;
        int badHitsCountLeft = 0;
        int badHitsCountRight = 0;
        int missedHitsLeft = 10 * 9 * 8 - sgd.points_earned_left.Count;
        int missedHitsRight = 10 * 9 * 8 - sgd.points_earned_right.Count;
        for(int i= 0; i < sgd.points_earned_left.Count; ++i)
        {
            if (sgd.points_earned_left[i] >= 100) verygoodHitsCountLeft += 1;
            if (sgd.points_earned_left[i] == 0) badHitsCountLeft += 1;
        }
        for (int i = 0; i < sgd.points_earned_right.Count; ++i)
        {
            if (sgd.points_earned_right[i] >= 100) verygoodHitsCountRight += 1;
            if (sgd.points_earned_right[i] == 0) badHitsCountRight += 1;
        }
        Debug.Log(
        "Very Good Hits Left: " + verygoodHitsCountLeft  + "\n" +
        "Very Good Hits Right: " + verygoodHitsCountRight + "\n" +
        "Missed Hits Left: " + missedHitsLeft + "\n" +
        "Missed Hits Right: " + missedHitsRight + "\n" +
        "Wrong Hits Left: " + badHitsCountLeft + "\n" +
        "Wrong Hits Right: " + badHitsCountRight + "\n" 
        );

    }
}
