using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;
using static SerializeData;
using UnityEngine;

public class LoadXml : MonoBehaviour
{
    public int participantsCount = 15;
    public float[] avgPoints;
    public float[] pointsLeftT, pointsRightT;
    public float[] blockLeftT, blockRightT;
    public float[] blockAvg;
    
    public void Start()
    {
        avgPoints = new float[9];
        pointsLeftT = new float[9];
        pointsRightT = new float[9];
        blockLeftT = new float[10];
        blockRightT = new float[10];
        blockAvg = new float[10];
        for(int k=0; k < 9; ++k)
        {
            avgPoints[k] = 0;
            pointsLeftT[k] = 0;
            pointsRightT[k] = 0;
        }

        //_deserializeSportGameDataForSequenceInfoAndWriteCsv();
        _deserializeSportGameDataForBlockInfoAndWriteCsv();

        /* all deserializing methods
          for (int i = 1; i <= participantsCount; ++i)
          {
               _DeserializeBlockGameData("Assets/xml/BlockData/Study/blockData" + i + ".xml");
               _DeserializeSportGameData("Assets/xml/SportGame/Study/sportGameData" + i + ".xml");
               _DeserializeSportGameDataForBlockInfo("Assets/xml/SportGame/Study/sportGameData" + i + ".xml");
               _DeserializeAntizipationTestData("Assets/xml/AntizipationTest/Study/antizipationtest" + i + ".xml");
               _DeserializeSportGameDataForSequenceInfo("Assets/xml/SportGame/Study/sportGameData" + i + ".xml", i);
          }
        */
    }

    private void _deserializeSportGameDataForBlockInfoAndWriteCsv()
    {
        using (var w = new StreamWriter("Assets/csv/dataanalysis/blockAnalysis" + ".csv"))
        {
            var firstLine = "BlockLeft1;BlockRight1;Avg1;BlockLeft2;BlockRight2;Avg2;BlockLeft3;BlockRight3;Avg3;" +
                "BlockLeft4;BlockRight4;Avg4;BlockLeft5;BlockRight5;Avg5;BlockLeft6;BlockRight6;Avg6;" +
                "BlockLeft7;BlockRight7;Avg7;BlockLeft8;BlockRight8;Avg8;BlockLeft9;BlockRight9;Avg9;BlockLeft10;BlockRight10;Avg10";
            w.WriteLine(firstLine);
            w.Flush();
            for (int i = 1; i <= participantsCount; ++i)
            {
                //loadxml._DeserializeBlockGameData("Assets/xml/BlockData/Study/blockData" + i + ".xml");
                //loadxml._DeserializeSportGameData("Assets/xml/SportGame/Study/sportGameData" + i + ".xml");
                _DeserializeSportGameDataForBlockInfo("Assets/xml/SportGame/Study/sportGameData" + i + ".xml");
                //loadxml._DeserializeAntizipationTestData("Assets/xml/AntizipationTest/Study/antizipationtest" + i + ".xml");

                var line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29}",
                blockLeftT[0], blockRightT[0], blockAvg[0], blockLeftT[1], blockRightT[1], blockAvg[1], blockLeftT[2], blockRightT[2], blockAvg[2],
                blockLeftT[3], blockRightT[3], blockAvg[3], blockLeftT[4], blockRightT[4], blockAvg[4], blockLeftT[5], blockRightT[5], blockAvg[5],
                blockLeftT[6], blockRightT[6], blockAvg[6], blockLeftT[7], blockRightT[7], blockAvg[7], blockLeftT[8], blockRightT[8], blockAvg[8],
                blockLeftT[9], blockRightT[9], blockAvg[9]);
                w.WriteLine(line);
                w.Flush();
            }


        }
    }

    private void _deserializeSportGameDataForSequenceInfoAndWriteCsv()
    {
        using (var w = new StreamWriter("Assets/csv/dataanalysis/pointsSequence" + ".csv"))
        {
            var firstLine = "PointsLeft1;PointsRight1;Avg1;PointsLeft2;PointsRight2;Avg2;PointsLeft3;PointsRight3;Avg3;" +
                "PointsLeft4;PointsRight4;Avg4;PointsLeft5;PointsRight5;Avg5;PointsLeft6;PointsRight6;Avg6;" +
                "PointsLeft7;PointsRight7;Avg7;PointsLeft8;PointsRight8;Avg8;PointsLeft9;PointsRight9;Avg9;";
            w.WriteLine(firstLine);
            w.Flush();
            for (int i = 1; i <= participantsCount; ++i)
            {
                //loadxml._DeserializeBlockGameData("Assets/xml/BlockData/Study/blockData" + i + ".xml");
                //loadxml._DeserializeSportGameData("Assets/xml/SportGame/Study/sportGameData" + i + ".xml");
                //loadxml._DeserializeSportGameDataForBlockInfo("Assets/xml/SportGame/Study/sportGameData" + i + ".xml");
                //loadxml._DeserializeAntizipationTestData("Assets/xml/AntizipationTest/Study/antizipationtest" + i + ".xml");
                _DeserializeSportGameDataForSequenceInfo("Assets/xml/SportGame/Study/sportGameData" + i + ".xml", i);

                var line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26}",
                pointsLeftT[0], pointsRightT[0], avgPoints[0], pointsLeftT[1], pointsRightT[1], avgPoints[1], pointsLeftT[2], pointsRightT[2], avgPoints[2],
                pointsLeftT[3], pointsRightT[3], avgPoints[3], pointsLeftT[4], pointsRightT[4], avgPoints[4], pointsLeftT[5], pointsRightT[5], avgPoints[5],
                pointsLeftT[6], pointsRightT[6], avgPoints[6], pointsLeftT[7], pointsRightT[7], avgPoints[7], pointsLeftT[8], pointsRightT[8], avgPoints[8]);
                w.WriteLine(line);
                w.Flush();
            }


        }
    }

    private void _DeserializeBlockGameData(string filename)
    {
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
        XmlSerializer serializer = new XmlSerializer(typeof(SportGameData));

        // Declare an object variable of the type to be deserialized.
        SportGameData sgd;

        using (Stream reader = new FileStream(filename, FileMode.Open))
        {
            // Call the Deserialize method to restore the object's state.
            sgd = (SportGameData)serializer.Deserialize(reader);
        }

        int pointsLeft = 0;
        int pointsRight = 0;
        float avgPrecisionLeft = 0;
        float avgPrecisionRight = 0;
        float avgPrecisionLeftWithoutFails = 0;
        float avgPrecisionRightWithoutFails = 0;
        int countLeftWithoutFails = sgd.precision_left.Count;
        int countRightWithoutFails = sgd.precision_right.Count;
        float avgTimeLeft = 0;
        float avgTimeRight = 0;
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
            pointsLeft += sgd.points_earned_left[i];
            avgPrecisionLeft += sgd.precision_left[i];
            avgTimeLeft += sgd.time_left[i];
            if (sgd.precision_left[i] > 0) avgPrecisionLeftWithoutFails += sgd.precision_left[i];
            else countLeftWithoutFails -= 1;
        }
        for (int i = 0; i < sgd.points_earned_right.Count; ++i)
        {
            if (sgd.points_earned_right[i] >= 100) verygoodHitsCountRight += 1;
            if (sgd.points_earned_right[i] == 0) badHitsCountRight += 1;
            pointsRight += sgd.points_earned_right[i];
            avgPrecisionRight += sgd.precision_right[i];
            avgTimeRight += sgd.time_right[i];
            if (sgd.precision_right[i] > 0) avgPrecisionRightWithoutFails += sgd.precision_right[i];
            else countRightWithoutFails -= 1;
        }
        avgPrecisionLeft = avgPrecisionLeft / (float) sgd.precision_left.Count;
        avgPrecisionRight = avgPrecisionRight / (float)sgd.precision_right.Count;
        avgPrecisionLeftWithoutFails = avgPrecisionLeftWithoutFails / (float)countLeftWithoutFails;
        avgPrecisionRightWithoutFails = avgPrecisionRightWithoutFails / (float)countRightWithoutFails;
        avgTimeLeft = avgTimeLeft / (float) sgd.time_left.Count;
        avgTimeRight = avgTimeRight / (float)sgd.time_right.Count;
        Debug.Log(
        "Very Good Hits Left: " + verygoodHitsCountLeft  + "\n" +
        "Very Good Hits Right: " + verygoodHitsCountRight + "\n" +
        "Missed Hits Left: " + missedHitsLeft + "\n" +
        "Missed Hits Right: " + missedHitsRight + "\n" +
        "Wrong Hits Left: " + badHitsCountLeft + "\n" +
        "Wrong Hits Right: " + badHitsCountRight + "\n" +
        "Points Left Hand: " + pointsLeft + "\n" +
        "Points Right Hand: " + pointsRight + "\n" +
        "Avg Precision Left Hand: " + avgPrecisionLeft + "\n" +
        "Avg Precision Right Hand: " + avgPrecisionRight + "\n" +
        "Avg Reactiontime Left Hand: " + avgTimeLeft + "\n" +
        "Avg Reactiontime Right Hand: " + avgTimeRight + "\n" +
        "Avg Precision Left Hand without Fails: " + avgPrecisionLeftWithoutFails + "\n" +
        "Avg Precision Right Hand without Fails: " + avgPrecisionRightWithoutFails + "\n"
        );

    }

    private void _DeserializeAntizipationTestData(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(AntizipationTestData));

        AntizipationTestData atd;

        using (Stream reader = new FileStream(filename, FileMode.Open))
        {
            atd = (AntizipationTestData)serializer.Deserialize(reader);
        }
        int sequenceLength = atd.positionCubeOriginalLeft.Count;
        List<float> distanceLeft = new List<float>();
        List<float> distanceRight = new List<float>();
        List<float> distanceLeftHitAndOriginal = new List<float>();
        List<float> distanceRightHitAndOriginal = new List<float>();
        for(int i= 0; i < sequenceLength; ++i)
        {
            float distLeft = Mathf.Abs(atd.positionCubeOriginalLeft[i].y - atd.positionCubePointedLeft[i].y);
            float distRight = Mathf.Abs(atd.positionCubeOriginalRight[i].y - atd.positionCubePointedRight[i].y);
            distanceLeft.Add(distLeft);
            distanceRight.Add(distRight);
            float distLeftHO = (atd.positionCubeOriginalLeft[i] - atd.positionCubeHitLeft[i]).magnitude;
            float distRightHO = (atd.positionCubeOriginalRight[i] - atd.positionCubeHitRight[i]).magnitude;
            distanceLeftHitAndOriginal.Add(distLeftHO);
            distanceRightHitAndOriginal.Add(distRightHO);
        }
        int numberRightChoices = 0;
        int numberRightCubePairs = 0;
        for(int i =0; i < sequenceLength; ++i)
        {
            if ((int) atd.rotationZOriginalLeft[i] == (int) atd.rotationZChoosenLeft[i]) numberRightChoices++;
            if ((int) atd.rotationZOriginalRight[i] == (int) atd.rotationZChoosenRight[i]) numberRightChoices++;
            if ((int) atd.rotationZOriginalLeft[i] == (int) atd.rotationZChoosenLeft[i] && (int)atd.rotationZOriginalRight[i] == (int) atd.rotationZChoosenRight[i]) numberRightCubePairs++;
        }

        for(int i=0; i < sequenceLength; ++i)
        {
            Debug.Log("Distance Left " + i + " : " + distanceLeft[i]+
            " Distance Right " + i + " : " + distanceRight[i]+
            " Distance to Hit Left : " + i + " : " + distanceLeftHitAndOriginal[i]+
           " Distance to Hit Right : " + i + " : " + distanceRightHitAndOriginal[i]);
        }

        Debug.Log("Number right Cube Pairs Rotations: " + numberRightCubePairs);
        Debug.Log("Number right choices all cubes: " + numberRightChoices);
    }

    private void _DeserializeSportGameDataForBlockInfo(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SportGameData));

        // Declare an object variable of the type to be deserialized.
        SportGameData sgd;

        using (Stream reader = new FileStream(filename, FileMode.Open))
        {
            // Call the Deserialize method to restore the object's state.
            sgd = (SportGameData)serializer.Deserialize(reader);
        }

        List<float> precisionleftBlock1, precisionleftBlock2, precisionleftBlock3, precisionleftBlock4, precisionleftBlock5,
            precisionleftBlock6, precisionleftBlock7, precisionleftBlock8, precisionleftBlock9, precisionleftBlock10;
        List<float> precisionRightBlock1, precisionRightBlock2, precisionRightBlock3, precisionRightBlock4, precisionRightBlock5,
            precisionRightBlock6, precisionRightBlock7, precisionRightBlock8, precisionRightBlock9, precisionRightBlock10;
        List<float> timeLeftBlock1, timeLeftBlock2, timeLeftBlock3, timeLeftBlock4, timeLeftBlock5,
             timeLeftBlock6, timeLeftBlock7, timeLeftBlock8, timeLeftBlock9, timeLeftBlock10;
        List<float> timeRightBlock1, timeRightBlock2, timeRightBlock3, timeRightBlock4, timeRightBlock5,
            timeRightBlock6, timeRightBlock7, timeRightBlock8, timeRightBlock9, timeRightBlock10;
       
        precisionleftBlock1 = new List<float>();
        precisionleftBlock2 = new List<float>();
        precisionleftBlock3 = new List<float>();
        precisionleftBlock4 = new List<float>();
        precisionleftBlock5 = new List<float>();
        precisionleftBlock6 = new List<float>();
        precisionleftBlock7 = new List<float>();
        precisionleftBlock8 = new List<float>();
        precisionleftBlock9 = new List<float>();
        precisionleftBlock10 = new List<float>();

        precisionRightBlock1 = new List<float>();
        precisionRightBlock2 = new List<float>();
        precisionRightBlock3 = new List<float>();
        precisionRightBlock4 = new List<float>();
        precisionRightBlock5 = new List<float>();
        precisionRightBlock6 = new List<float>();
        precisionRightBlock7 = new List<float>();
        precisionRightBlock8 = new List<float>();
        precisionRightBlock9 = new List<float>();
        precisionRightBlock10 = new List<float>();

        timeLeftBlock1 = new List<float>();
        timeLeftBlock2 = new List<float>();
        timeLeftBlock3 = new List<float>();
        timeLeftBlock4 = new List<float>();
        timeLeftBlock5 = new List<float>();
        timeLeftBlock6 = new List<float>();
        timeLeftBlock7 = new List<float>();
        timeLeftBlock8 = new List<float>();
        timeLeftBlock9 = new List<float>();
        timeLeftBlock10 = new List<float>();

        timeRightBlock1 = new List<float>();
        timeRightBlock2 = new List<float>();
        timeRightBlock3 = new List<float>();
        timeRightBlock4 = new List<float>();
        timeRightBlock5 = new List<float>();
        timeRightBlock6 = new List<float>();
        timeRightBlock7 = new List<float>();
        timeRightBlock8 = new List<float>();
        timeRightBlock9 = new List<float>();
        timeRightBlock10 = new List<float>();

        int precisionLeftListIndex = 0;
        int timeLeftListIndex = 0;
        int precisionRightListIndex = 0;
        int timeRightListIndex = 0;
        for(int i = 1; i <= 72; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock1.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock1.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if(sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock1.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock1.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 73; i <= 72 * 2; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock2.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock2.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock2.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock2.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 72 * 2 + 1; i <= 72 * 3; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock3.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock3.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock3.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock3.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 72 * 3 + 1; i <= 72 * 4; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock4.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock4.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock4.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock4.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 72 * 4 + 1; i <= 72 * 5; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock5.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock5.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock5.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock5.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 72 * 5 + 1; i <= 72 * 6; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock6.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock6.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock6.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock6.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 72 * 6 + 1; i <= 72 * 7; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock7.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock7.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock7.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock7.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 72 * 7 + 1; i <= 72 * 8; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock8.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock8.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock8.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock8.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 72 * 8 + 1; i <= 72 * 9; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock9.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock9.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock9.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock9.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        for (int i = 72 * 9 + 1; i <= 72 * 10; ++i)
        {
            if (sgd.round_generated_left.Contains(i))
            {
                precisionleftBlock10.Add(sgd.precision_left[precisionLeftListIndex++]);
                timeLeftBlock10.Add(sgd.time_left[timeLeftListIndex++]);
            }
            if (sgd.round_generated_right.Contains(i))
            {
                precisionRightBlock10.Add(sgd.precision_right[precisionRightListIndex++]);
                timeRightBlock10.Add(sgd.time_right[timeRightListIndex++]);
            }
        }

        float avgPrecisionLeftBlock1 = 0, avgPrecisionLeftBlock2 = 0, avgPrecisionLeftBlock3 = 0, avgPrecisionLeftBlock4 = 0, avgPrecisionLeftBlock5 = 0,
            avgPrecisionLeftBlock6 = 0, avgPrecisionLeftBlock7 = 0, avgPrecisionLeftBlock8 = 0, avgPrecisionLeftBlock9 = 0, avgPrecisionLeftBlock10 = 0;
        float avgPrecisionRightBlock1 = 0, avgPrecisionRightBlock2 = 0, avgPrecisionRightBlock3 = 0, avgPrecisionRightBlock4 = 0, avgPrecisionRightBlock5 = 0,
            avgPrecisionRightBlock6 = 0, avgPrecisionRightBlock7 = 0, avgPrecisionRightBlock8 = 0, avgPrecisionRightBlock9 = 0, avgPrecisionRightBlock10 = 0;
        foreach (float f in precisionleftBlock1) avgPrecisionLeftBlock1 += f;
        avgPrecisionLeftBlock1 = avgPrecisionLeftBlock1 / (float) precisionleftBlock1.Count;
        foreach (float f in precisionleftBlock2) avgPrecisionLeftBlock2 += f;
        avgPrecisionLeftBlock2 = avgPrecisionLeftBlock2 / (float)precisionleftBlock2.Count;
        foreach (float f in precisionleftBlock3) avgPrecisionLeftBlock3 += f;
        avgPrecisionLeftBlock3 = avgPrecisionLeftBlock3 / (float)precisionleftBlock3.Count;
        foreach (float f in precisionleftBlock4) avgPrecisionLeftBlock4 += f;
        avgPrecisionLeftBlock4 = avgPrecisionLeftBlock4 / (float)precisionleftBlock4.Count;
        foreach (float f in precisionleftBlock5) avgPrecisionLeftBlock5 += f;
        avgPrecisionLeftBlock5 = avgPrecisionLeftBlock5 / (float)precisionleftBlock5.Count; 
        foreach (float f in precisionleftBlock6) avgPrecisionLeftBlock6 += f;
        avgPrecisionLeftBlock6 = avgPrecisionLeftBlock6 / (float)precisionleftBlock6.Count;
        foreach (float f in precisionleftBlock7) avgPrecisionLeftBlock7 += f;
        avgPrecisionLeftBlock7 = avgPrecisionLeftBlock7 / (float)precisionleftBlock7.Count;
        foreach (float f in precisionleftBlock8) avgPrecisionLeftBlock8 += f;
        avgPrecisionLeftBlock8 = avgPrecisionLeftBlock8 / (float)precisionleftBlock8.Count;
        foreach (float f in precisionleftBlock9) avgPrecisionLeftBlock9 += f;
        avgPrecisionLeftBlock9 = avgPrecisionLeftBlock9 / (float)precisionleftBlock9.Count;
        foreach (float f in precisionleftBlock10) avgPrecisionLeftBlock10 += f;
        avgPrecisionLeftBlock10 = avgPrecisionLeftBlock10 / (float)precisionleftBlock10.Count;

        foreach (float f in precisionRightBlock1) avgPrecisionRightBlock1 += f;
        avgPrecisionRightBlock1 = avgPrecisionRightBlock1 / (float)precisionRightBlock1.Count;
        foreach (float f in precisionRightBlock2) avgPrecisionRightBlock2 += f;
        avgPrecisionRightBlock2 = avgPrecisionRightBlock2 / (float)precisionRightBlock2.Count;
        foreach (float f in precisionRightBlock3) avgPrecisionRightBlock3 += f;
        avgPrecisionRightBlock3 = avgPrecisionRightBlock3 / (float)precisionRightBlock3.Count;
        foreach (float f in precisionRightBlock4) avgPrecisionRightBlock4 += f;
        avgPrecisionRightBlock4 = avgPrecisionRightBlock4 / (float)precisionRightBlock4.Count;
        foreach (float f in precisionRightBlock5) avgPrecisionRightBlock5 += f;
        avgPrecisionRightBlock5 = avgPrecisionRightBlock5 / (float)precisionRightBlock5.Count;
        foreach (float f in precisionRightBlock6) avgPrecisionRightBlock6 += f;
        avgPrecisionRightBlock6 = avgPrecisionRightBlock6 / (float)precisionRightBlock6.Count;
        foreach (float f in precisionRightBlock7) avgPrecisionRightBlock7 += f;
        avgPrecisionRightBlock7 = avgPrecisionRightBlock7 / (float)precisionRightBlock7.Count;
        foreach (float f in precisionRightBlock8) avgPrecisionRightBlock8 += f;
        avgPrecisionRightBlock8 = avgPrecisionRightBlock8 / (float)precisionRightBlock8.Count;
        foreach (float f in precisionRightBlock9) avgPrecisionRightBlock9 += f;
        avgPrecisionRightBlock9 = avgPrecisionRightBlock9 / (float)precisionRightBlock9.Count;
        foreach (float f in precisionRightBlock10) avgPrecisionRightBlock10 += f;
        avgPrecisionRightBlock10 = avgPrecisionRightBlock10 / (float)precisionRightBlock10.Count;

        float avgTimeLeftBlock1 = 0, avgTimeLeftBlock2 = 0, avgTimeLeftBlock3 = 0, avgTimeLeftBlock4 = 0, avgTimeLeftBlock5 = 0,
           avgTimeLeftBlock6 = 0, avgTimeLeftBlock7 = 0, avgTimeLeftBlock8 = 0, avgTimeLeftBlock9 = 0, avgTimeLeftBlock10 = 0;
        float avgTimeRightBlock1 = 0, avgTimeRightBlock2 = 0, avgTimeRightBlock3 = 0, avgTimeRightBlock4 = 0, avgTimeRightBlock5 = 0,
            avgTimeRightBlock6 = 0, avgTimeRightBlock7 = 0, avgTimeRightBlock8 = 0, avgTimeRightBlock9 = 0, avgTimeRightBlock10 = 0;

        foreach (float f in timeLeftBlock1) avgTimeLeftBlock1 += f;
        avgTimeLeftBlock1 = avgTimeLeftBlock1 / (float)timeLeftBlock1.Count;
        foreach (float f in timeLeftBlock2) avgTimeLeftBlock2 += f;
        avgTimeLeftBlock2 = avgTimeLeftBlock2 / (float)timeLeftBlock2.Count;
        foreach (float f in timeLeftBlock3) avgTimeLeftBlock3 += f;
        avgTimeLeftBlock3 = avgTimeLeftBlock3 / (float)timeLeftBlock3.Count;
        foreach (float f in timeLeftBlock4) avgTimeLeftBlock4 += f;
        avgTimeLeftBlock4 = avgTimeLeftBlock4 / (float)timeLeftBlock4.Count;
        foreach (float f in timeLeftBlock5) avgTimeLeftBlock5 += f;
        avgTimeLeftBlock5 = avgTimeLeftBlock5 / (float)timeLeftBlock5.Count;
        foreach (float f in timeLeftBlock6) avgTimeLeftBlock6 += f;
        avgTimeLeftBlock6 = avgTimeLeftBlock6 / (float)timeLeftBlock6.Count;
        foreach (float f in timeLeftBlock7) avgTimeLeftBlock7 += f;
        avgTimeLeftBlock7 = avgTimeLeftBlock7 / (float)timeLeftBlock7.Count;
        foreach (float f in timeLeftBlock8) avgTimeLeftBlock8 += f;
        avgTimeLeftBlock8 = avgTimeLeftBlock8 / (float)timeLeftBlock8.Count;
        foreach (float f in timeLeftBlock9) avgTimeLeftBlock9 += f;
        avgTimeLeftBlock9 = avgTimeLeftBlock9 / (float)timeLeftBlock9.Count;
        foreach (float f in timeLeftBlock10) avgTimeLeftBlock10 += f;
        avgTimeLeftBlock10 = avgTimeLeftBlock10 / (float)timeLeftBlock10.Count;

        foreach (float f in timeRightBlock1) avgTimeRightBlock1 += f;
        avgTimeRightBlock1 = avgTimeRightBlock1 / (float)timeRightBlock1.Count;
        foreach (float f in timeRightBlock2) avgTimeRightBlock2 += f;
        avgTimeRightBlock2 = avgTimeRightBlock2 / (float)timeRightBlock2.Count;
        foreach (float f in timeRightBlock3) avgTimeRightBlock3 += f;
        avgTimeRightBlock3 = avgTimeRightBlock3 / (float)timeRightBlock3.Count;
        foreach (float f in timeRightBlock4) avgTimeRightBlock4 += f;
        avgTimeRightBlock4 = avgTimeRightBlock4 / (float)timeRightBlock4.Count;
        foreach (float f in timeRightBlock5) avgTimeRightBlock5 += f;
        avgTimeRightBlock5 = avgTimeRightBlock5 / (float)timeRightBlock5.Count;
        foreach (float f in timeRightBlock6) avgTimeRightBlock6 += f;
        avgTimeRightBlock6 = avgTimeRightBlock6 / (float)timeRightBlock6.Count;
        foreach (float f in timeRightBlock7) avgTimeRightBlock7 += f;
        avgTimeRightBlock7 = avgTimeRightBlock7 / (float)timeRightBlock7.Count;
        foreach (float f in timeRightBlock8) avgTimeRightBlock8 += f;
        avgTimeRightBlock8 = avgTimeRightBlock8 / (float)timeRightBlock8.Count;
        foreach (float f in timeRightBlock9) avgTimeRightBlock9 += f;
        avgTimeRightBlock9 = avgTimeRightBlock9 / (float)timeRightBlock9.Count;
        foreach (float f in timeRightBlock10) avgTimeRightBlock10 += f;
        avgTimeRightBlock10 = avgTimeRightBlock10 / (float)timeRightBlock10.Count;

        Debug.Log(
        "Avg Precision Left Block1: " + avgPrecisionLeftBlock1 + "\n" +
        "Avg Precision Right Block1: " + avgPrecisionRightBlock1 + "\n" +
        "Avg Precision Left Block2: " + avgPrecisionLeftBlock2 + "\n" +
        "Avg Precision Right Block2: " + avgPrecisionRightBlock2 + "\n" +
        "Avg Precision Left Block3: " + avgPrecisionLeftBlock3 + "\n" +
        "Avg Precision Right Block3: " + avgPrecisionRightBlock3 + "\n" +
        "Avg Precision Left Block4: " + avgPrecisionLeftBlock4 + "\n" +
        "Avg Precision Right Block4: " + avgPrecisionRightBlock4 + "\n" +
        "Avg Precision Left Block5: " + avgPrecisionLeftBlock5 + "\n" +
        "Avg Precision Right Block5: " + avgPrecisionRightBlock5 + "\n" +
        "Avg Precision Left Block6: " + avgPrecisionLeftBlock6 + "\n" +
        "Avg Precision Right Block6: " + avgPrecisionRightBlock6 + "\n" +
        "Avg Precision Left Block7: " + avgPrecisionLeftBlock7 + "\n" +
        "Avg Precision Right Block7: " + avgPrecisionRightBlock7 + "\n" +
        "Avg Precision Left Block8: " + avgPrecisionLeftBlock8 + "\n" +
        "Avg Precision Right Block8: " + avgPrecisionRightBlock8 + "\n" +
        "Avg Precision Left Block9: " + avgPrecisionLeftBlock9 + "\n" +
        "Avg Precision Right Block9: " + avgPrecisionRightBlock9 + "\n" +
        "Avg Precision Left Block10: " + avgPrecisionLeftBlock10 + "\n" +
        "Avg Precision Right Block10: " + avgPrecisionRightBlock10 + "\n"
        );

        Debug.Log(
        "Avg Reactiontime Left Block1: " + avgTimeLeftBlock1 + "\n" +
        "Avg Reactiontime Right Block1: " + avgTimeRightBlock1 + "\n" +
        "Avg Reactiontime Left Block2: " + avgTimeLeftBlock2 + "\n" +
        "Avg Reactiontime Right Block2: " + avgTimeRightBlock2 + "\n" +
        "Avg Reactiontime Left Block3: " + avgTimeLeftBlock3 + "\n" +
        "Avg Reactiontime Right Block3: " + avgTimeRightBlock3 + "\n" +
        "Avg Reactiontime Left Block4: " + avgTimeLeftBlock4 + "\n" +
        "Avg Reactiontime Right Block4: " + avgTimeRightBlock4 + "\n" +
        "Avg Reactiontime Left Block5: " + avgTimeLeftBlock5 + "\n" +
        "Avg Reactiontime Right Block5: " + avgTimeRightBlock5 + "\n" +
        "Avg Reactiontime Left Block6: " + avgTimeLeftBlock6 + "\n" +
        "Avg Reactiontime Right Block6: " + avgTimeRightBlock6 + "\n" +
        "Avg Reactiontime Left Block7: " + avgTimeLeftBlock7 + "\n" +
        "Avg Reactiontime Right Block7: " + avgTimeRightBlock7 + "\n" +
        "Avg Reactiontime Left Block8: " + avgTimeLeftBlock8 + "\n" +
        "Avg Reactiontime Right Block8: " + avgTimeRightBlock8 + "\n" +
        "Avg Reactiontime Left Block9: " + avgTimeLeftBlock9 + "\n" +
        "Avg Reactiontime Right Block9: " + avgTimeRightBlock9 + "\n" +
        "Avg Reactiontime Left Block10: " + avgTimeLeftBlock10 + "\n" +
        "Avg Reactiontime Right Block10: " + avgTimeRightBlock10 + "\n"
        );

        blockLeftT[0] = avgTimeLeftBlock1;
        blockLeftT[1] = avgTimeLeftBlock2;
        blockLeftT[2] = avgTimeLeftBlock3;
        blockLeftT[3] = avgTimeLeftBlock4;
        blockLeftT[4] = avgTimeLeftBlock5;
        blockLeftT[5] = avgTimeLeftBlock6;
        blockLeftT[6] = avgTimeLeftBlock7;
        blockLeftT[7] = avgTimeLeftBlock8;
        blockLeftT[8] = avgTimeLeftBlock9;
        blockLeftT[9] = avgTimeLeftBlock10;
        blockRightT[0] = avgTimeRightBlock1;
        blockRightT[1] = avgTimeRightBlock2;
        blockRightT[2] = avgTimeRightBlock3;
        blockRightT[3] = avgTimeRightBlock4;
        blockRightT[4] = avgTimeRightBlock5;
        blockRightT[5] = avgTimeRightBlock6;
        blockRightT[6] = avgTimeRightBlock7;
        blockRightT[7] = avgTimeRightBlock8;
        blockRightT[8] = avgTimeRightBlock9;
        blockRightT[9] = avgTimeRightBlock10;
        for(int k=0; k < 10; k++)
        {
            blockAvg[k] = (blockLeftT[k] + blockRightT[k]) / 2.0f;
        }


    }



    private void _DeserializeSportGameDataForSequenceInfo(string filename, int number)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SportGameData));

        // Declare an object variable of the type to be deserialized.
        SportGameData sgd;

        using (Stream reader = new FileStream(filename, FileMode.Open))
        {
            // Call the Deserialize method to restore the object's state.
            sgd = (SportGameData)serializer.Deserialize(reader);
        }

        float[,] precisionLeft;
        float[,] precisionRight;
        float[,] timeLeft;
        float[,] timeRight;
        float[,] pointsLeft;
        float[,] pointsRight;

        precisionLeft = new float[80, 9];
        precisionRight = new float[80, 9];
        timeLeft = new float[80, 9];
        timeRight = new float[80, 9];
        pointsLeft = new float[80, 9];
        pointsRight = new float[80, 9];

        

        int precisionLeftListIndex = 0;
        int timeLeftListIndex = 0;
        int precisionRightListIndex = 0;
        int timeRightListIndex = 0;
        int pointsEarnedLeftIndex = 0;
        int pointsEarnedRightIndex = 0;
        for (int i = 0; i < 72 * 10; ++i)
        {
            if (sgd.round_generated_left.Contains(i+1))
            {
                precisionLeft[(int)(i / 9), i % 9] = sgd.precision_left[precisionLeftListIndex++];
                timeLeft[(int) (i / 9), i % 9] = sgd.time_left[timeLeftListIndex++];
                pointsLeft[(int)(i / 9), i % 9] = sgd.points_earned_left[pointsEarnedLeftIndex++];
            }
            else
            {
                precisionLeft[(int)(i / 9), i % 9] = -1;
                timeLeft[(int)(i / 9), i % 9] = -1;
                pointsLeft[(int)(i / 9), i % 9] = -1;
            }
            if (sgd.round_generated_right.Contains(i+1))
            {
                precisionRight[(int)(i / 9), i % 9] = sgd.precision_right[precisionRightListIndex++];
                timeRight[(int)(i / 9), i % 9] = sgd.time_right[timeRightListIndex++];
                pointsRight[(int)(i / 9), i % 9] = sgd.points_earned_right[pointsEarnedRightIndex++];
            }
            else
            {
                precisionRight[(int)(i / 9), i % 9] = -1;
                timeRight[(int)(i / 9), i % 9] = -1;
                pointsRight[(int)(i / 9), i % 9] = -1;
            }
        }



        float avgPrecisionLeftCubePair1 = 0, avgPrecisionLeftCubePair2 = 0, avgPrecisionLeftCubePair3 = 0, avgPrecisionLeftCubePair4 = 0, avgPrecisionLeftCubePair5 = 0,
            avgPrecisionLeftCubePair6 = 0, avgPrecisionLeftCubePair7 = 0, avgPrecisionLeftCubePair8 = 0, avgPrecisionLeftCubePair9 = 0;
        int[] numberPrecisionLeft = new int[9];
        float avgPrecisionRightCubePair1 = 0, avgPrecisionRightCubePair2 = 0, avgPrecisionRightCubePair3 = 0, avgPrecisionRightCubePair4 = 0, avgPrecisionRightCubePair5 = 0,
            avgPrecisionRightCubePair6 = 0, avgPrecisionRightCubePair7 = 0, avgPrecisionRightCubePair8 = 0, avgPrecisionRightCubePair9 = 0;
        int[] numberPrecisionRight = new int[9];
        for(int k = 0; k < 9; ++k)
        {
            numberPrecisionLeft[k] = 0;
            numberPrecisionRight[k] = 0;
        }
        
        for(int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 0] >= 0)
            {
                avgPrecisionLeftCubePair1 += precisionLeft[i, 0];
                numberPrecisionLeft[0]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 1] >= 0)
            {
                avgPrecisionLeftCubePair2 += precisionLeft[i, 1];
                numberPrecisionLeft[1]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 2] >= 0)
            {
                avgPrecisionLeftCubePair3 += precisionLeft[i, 2];
                numberPrecisionLeft[2]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 3] >= 0)
            {
                avgPrecisionLeftCubePair4 += precisionLeft[i, 3];
                numberPrecisionLeft[3]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 4] >= 0)
            {
                avgPrecisionLeftCubePair5 += precisionLeft[i, 4];
                numberPrecisionLeft[4]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 5] >= 0)
            {
                avgPrecisionLeftCubePair6 += precisionLeft[i, 5];
                numberPrecisionLeft[5]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 6] >= 0)
            {
                avgPrecisionLeftCubePair7 += precisionLeft[i, 6];
                numberPrecisionLeft[6]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 7] >= 0)
            {
                avgPrecisionLeftCubePair8 += precisionLeft[i, 7];
                numberPrecisionLeft[7]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionLeft[i, 8] >= 0)
            {
                avgPrecisionLeftCubePair9 += precisionLeft[i, 8];
                numberPrecisionLeft[8]++;
            }
        }
        avgPrecisionLeftCubePair1 = avgPrecisionLeftCubePair1 / numberPrecisionLeft[0];
        avgPrecisionLeftCubePair2 = avgPrecisionLeftCubePair2 / numberPrecisionLeft[1];
        avgPrecisionLeftCubePair3 = avgPrecisionLeftCubePair3 / numberPrecisionLeft[2];
        avgPrecisionLeftCubePair4 = avgPrecisionLeftCubePair4 / numberPrecisionLeft[3];
        avgPrecisionLeftCubePair5 = avgPrecisionLeftCubePair5 / numberPrecisionLeft[4];
        avgPrecisionLeftCubePair6 = avgPrecisionLeftCubePair6 / numberPrecisionLeft[5];
        avgPrecisionLeftCubePair7 = avgPrecisionLeftCubePair7 / numberPrecisionLeft[6];
        avgPrecisionLeftCubePair8 = avgPrecisionLeftCubePair8 / numberPrecisionLeft[7];
        avgPrecisionLeftCubePair9 = avgPrecisionLeftCubePair9 / numberPrecisionLeft[8];


        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 0] >= 0)
            {
                avgPrecisionRightCubePair1 += precisionRight[i, 0];
                numberPrecisionRight[0]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 1] >= 0)
            {
                avgPrecisionRightCubePair2 += precisionRight[i, 1];
                numberPrecisionRight[1]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 2] >= 0)
            {
                avgPrecisionRightCubePair3 += precisionRight[i, 2];
                numberPrecisionRight[2]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 3] >= 0)
            {
                avgPrecisionRightCubePair4 += precisionRight[i, 3];
                numberPrecisionRight[3]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 4] >= 0)
            {
                avgPrecisionRightCubePair5 += precisionRight[i, 4];
                numberPrecisionRight[4]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 5] >= 0)
            {
                avgPrecisionRightCubePair6 += precisionRight[i, 5];
                numberPrecisionRight[5]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 6] >= 0)
            {
                avgPrecisionRightCubePair7 += precisionRight[i, 6];
                numberPrecisionRight[6]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 7] >= 0)
            {
                avgPrecisionRightCubePair8 += precisionRight[i, 7];
                numberPrecisionRight[7]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (precisionRight[i, 8] >= 0)
            {
                avgPrecisionRightCubePair9 += precisionRight[i, 8];
                numberPrecisionRight[8]++;
            }
        }
        avgPrecisionRightCubePair1 = avgPrecisionRightCubePair1 / numberPrecisionRight[0];
        avgPrecisionRightCubePair2 = avgPrecisionRightCubePair2 / numberPrecisionRight[1];
        avgPrecisionRightCubePair3 = avgPrecisionRightCubePair3 / numberPrecisionRight[2];
        avgPrecisionRightCubePair4 = avgPrecisionRightCubePair4 / numberPrecisionRight[3];
        avgPrecisionRightCubePair5 = avgPrecisionRightCubePair5 / numberPrecisionRight[4];
        avgPrecisionRightCubePair6 = avgPrecisionRightCubePair6 / numberPrecisionRight[5];
        avgPrecisionRightCubePair7 = avgPrecisionRightCubePair7 / numberPrecisionRight[6];
        avgPrecisionRightCubePair8 = avgPrecisionRightCubePair8 / numberPrecisionRight[7];
        avgPrecisionRightCubePair9 = avgPrecisionRightCubePair9 / numberPrecisionRight[8];

        float avgTimeLeftCubePair1 = 0, avgTimeLeftCubePair2 = 0, avgTimeLeftCubePair3 = 0, avgTimeLeftCubePair4 = 0, avgTimeLeftCubePair5 = 0,
           avgTimeLeftCubePair6 = 0, avgTimeLeftCubePair7 = 0, avgTimeLeftCubePair8 = 0, avgTimeLeftCubePair9 = 0;
        float avgTimeRightCubePair1 = 0, avgTimeRightCubePair2 = 0, avgTimeRightCubePair3 = 0, avgTimeRightCubePair4 = 0, avgTimeRightCubePair5 = 0,
            avgTimeRightCubePair6 = 0, avgTimeRightCubePair7 = 0, avgTimeRightCubePair8 = 0, avgTimeRightCubePair9 = 0;
        float[] numberTimeLeft, numberTimeRight;
        numberTimeLeft = new float[9]; numberTimeRight = new float[9];
        for (int k = 0; k < 9; ++k)
        {
            numberTimeLeft[k] = 0;
            numberTimeRight[k] = 0;
        }

        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 0] > 0)
            {
                avgTimeLeftCubePair1 += timeLeft[i, 0];
                numberTimeLeft[0]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 1] > 0)
            {
                avgTimeLeftCubePair2 += timeLeft[i, 1];
                numberTimeLeft[1]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 2] > 0)
            {
                avgTimeLeftCubePair3 += timeLeft[i, 2];
                numberTimeLeft[2]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 3] > 0)
            {
                avgTimeLeftCubePair4 += timeLeft[i, 3];
                numberTimeLeft[3]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 4] > 0)
            {
                avgTimeLeftCubePair5 += timeLeft[i, 4];
                numberTimeLeft[4]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 5] > 0)
            {
                avgTimeLeftCubePair6 += timeLeft[i, 5];
                numberTimeLeft[5]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 6] > 0)
            {
                avgTimeLeftCubePair7 += timeLeft[i, 6];
                numberTimeLeft[6]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 7] > 0)
            {
                avgTimeLeftCubePair8 += timeLeft[i, 7];
                numberTimeLeft[7]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeLeft[i, 8] > 0)
            {
                avgTimeLeftCubePair9 += timeLeft[i, 8];
                numberTimeLeft[8]++;
            }
        }
        avgTimeLeftCubePair1 = avgTimeLeftCubePair1 / numberTimeLeft[0];
        avgTimeLeftCubePair2 = avgTimeLeftCubePair2 / numberTimeLeft[1];
        avgTimeLeftCubePair3 = avgTimeLeftCubePair3 / numberTimeLeft[2];
        avgTimeLeftCubePair4 = avgTimeLeftCubePair4 / numberTimeLeft[3];
        avgTimeLeftCubePair5 = avgTimeLeftCubePair5 / numberTimeLeft[4];
        avgTimeLeftCubePair6 = avgTimeLeftCubePair6 / numberTimeLeft[5];
        avgTimeLeftCubePair7 = avgTimeLeftCubePair7 / numberTimeLeft[6];
        avgTimeLeftCubePair8 = avgTimeLeftCubePair8 / numberTimeLeft[7];
        avgTimeLeftCubePair9 = avgTimeLeftCubePair9 / numberTimeLeft[8];


        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 0] > 0)
            {
                avgTimeRightCubePair1 += timeRight[i, 0];
                numberTimeRight[0]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 1] > 0)
            {
                avgTimeRightCubePair2 += timeRight[i, 1];
                numberTimeRight[1]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 2] > 0)
            {
                avgTimeRightCubePair3 += timeRight[i, 2];
                numberTimeRight[2]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 3] > 0)
            {
                avgTimeRightCubePair4 += timeRight[i, 3];
                numberTimeRight[3]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 4] > 0)
            {
                avgTimeRightCubePair5 += timeRight[i, 4];
                numberTimeRight[4]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 5] > 0)
            {
                avgTimeRightCubePair6 += timeRight[i, 5];
                numberTimeRight[5]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 6] > 0)
            {
                avgTimeRightCubePair7 += timeRight[i, 6];
                numberTimeRight[6]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 7] > 0)
            {
                avgTimeRightCubePair8 += timeRight[i, 7];
                numberTimeRight[7]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (timeRight[i, 8] > 0)
            {
                avgTimeRightCubePair9 += timeRight[i, 8];
                numberTimeRight[8]++;
            }
        }
        avgTimeRightCubePair1 = avgTimeRightCubePair1 / numberTimeRight[0];
        avgTimeRightCubePair2 = avgTimeRightCubePair2 / numberTimeRight[1];
        avgTimeRightCubePair3 = avgTimeRightCubePair3 / numberTimeRight[2];
        avgTimeRightCubePair4 = avgTimeRightCubePair4 / numberTimeRight[3];
        avgTimeRightCubePair5 = avgTimeRightCubePair5 / numberTimeRight[4];
        avgTimeRightCubePair6 = avgTimeRightCubePair6 / numberTimeRight[5];
        avgTimeRightCubePair7 = avgTimeRightCubePair7 / numberTimeRight[6];
        avgTimeRightCubePair8 = avgTimeRightCubePair8 / numberTimeRight[7];
        avgTimeRightCubePair9 = avgTimeRightCubePair9 / numberTimeRight[8];



        float avgPointsLeftCubePair1 = 0, avgPointsLeftCubePair2 = 0, avgPointsLeftCubePair3 = 0, avgPointsLeftCubePair4 = 0, avgPointsLeftCubePair5 = 0,
           avgPointsLeftCubePair6 = 0, avgPointsLeftCubePair7 = 0, avgPointsLeftCubePair8 = 0, avgPointsLeftCubePair9 = 0;
        float avgPointsRightCubePair1 = 0, avgPointsRightCubePair2 = 0, avgPointsRightCubePair3 = 0, avgPointsRightCubePair4 = 0, avgPointsRightCubePair5 = 0,
            avgPointsRightCubePair6 = 0, avgPointsRightCubePair7 = 0, avgPointsRightCubePair8 = 0, avgPointsRightCubePair9 = 0;
        float[] numberPointsLeft, numberPointsRight;
        numberPointsLeft = new float[9]; numberPointsRight = new float[9];
        for (int k = 0; k < 9; ++k)
        {
            numberPointsLeft[k] = 0;
            numberPointsRight[k] = 0;
        }

        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 0] >= 0)
            {
                avgPointsLeftCubePair1 += pointsLeft[i, 0];
                numberPointsLeft[0]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 1] >= 0)
            {
                avgPointsLeftCubePair2 += pointsLeft[i, 1];
                numberPointsLeft[1]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 2] >= 0)
            {
                avgPointsLeftCubePair3 += pointsLeft[i, 2];
                numberPointsLeft[2]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 3] >= 0)
            {
                avgPointsLeftCubePair4 += pointsLeft[i, 3];
                numberPointsLeft[3]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 4] >= 0)
            {
                avgPointsLeftCubePair5 += pointsLeft[i, 4];
                numberPointsLeft[4]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 5] >= 0)
            {
                avgPointsLeftCubePair6 += pointsLeft[i, 5];
                numberPointsLeft[5]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 6] >= 0)
            {
                avgPointsLeftCubePair7 += pointsLeft[i, 6];
                numberPointsLeft[6]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 7] >= 0)
            {
                avgPointsLeftCubePair8 += pointsLeft[i, 7];
                numberPointsLeft[7]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsLeft[i, 8] >= 0)
            {
                avgPointsLeftCubePair9 += pointsLeft[i, 8];
                numberPointsLeft[8]++;
            }
        }
        avgPointsLeftCubePair1 = avgPointsLeftCubePair1 / numberPointsLeft[0];
        avgPointsLeftCubePair2 = avgPointsLeftCubePair2 / numberPointsLeft[1];
        avgPointsLeftCubePair3 = avgPointsLeftCubePair3 / numberPointsLeft[2];
        avgPointsLeftCubePair4 = avgPointsLeftCubePair4 / numberPointsLeft[3];
        avgPointsLeftCubePair5 = avgPointsLeftCubePair5 / numberPointsLeft[4];
        avgPointsLeftCubePair6 = avgPointsLeftCubePair6 / numberPointsLeft[5];
        avgPointsLeftCubePair7 = avgPointsLeftCubePair7 / numberPointsLeft[6];
        avgPointsLeftCubePair8 = avgPointsLeftCubePair8 / numberPointsLeft[7];
        avgPointsLeftCubePair9 = avgPointsLeftCubePair9 / numberPointsLeft[8];


        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 0] >= 0)
            {
                avgPointsRightCubePair1 += pointsRight[i, 0];
                numberPointsRight[0]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 1] >= 0)
            {
                avgPointsRightCubePair2 += pointsRight[i, 1];
                numberPointsRight[1]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 2] >= 0)
            {
                avgPointsRightCubePair3 += pointsRight[i, 2];
                numberPointsRight[2]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 3] >= 0)
            {
                avgPointsRightCubePair4 += pointsRight[i, 3];
                numberPointsRight[3]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 4] >= 0)
            {
                avgPointsRightCubePair5 += pointsRight[i, 4];
                numberPointsRight[4]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 5] >= 0)
            {
                avgPointsRightCubePair6 += pointsRight[i, 5];
                numberPointsRight[5]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 6] >= 0)
            {
                avgPointsRightCubePair7 += pointsRight[i, 6];
                numberPointsRight[6]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 7] >= 0)
            {
                avgPointsRightCubePair8 += pointsRight[i, 7];
                numberPointsRight[7]++;
            }
        }
        for (int i = 0; i < 80; ++i)
        {
            if (pointsRight[i, 8] >= 0)
            {
                avgPointsRightCubePair9 += pointsRight[i, 8];
                numberPointsRight[8]++;
            }
        }
        avgPointsRightCubePair1 = avgPointsRightCubePair1 / numberPointsRight[0];
        avgPointsRightCubePair2 = avgPointsRightCubePair2 / numberPointsRight[1];
        avgPointsRightCubePair3 = avgPointsRightCubePair3 / numberPointsRight[2];
        avgPointsRightCubePair4 = avgPointsRightCubePair4 / numberPointsRight[3];
        avgPointsRightCubePair5 = avgPointsRightCubePair5 / numberPointsRight[4];
        avgPointsRightCubePair6 = avgPointsRightCubePair6 / numberPointsRight[5];
        avgPointsRightCubePair7 = avgPointsRightCubePair7 / numberPointsRight[6];
        avgPointsRightCubePair8 = avgPointsRightCubePair8 / numberPointsRight[7];
        avgPointsRightCubePair9 = avgPointsRightCubePair9 / numberPointsRight[8];

        Debug.Log(
       "Avg Points Left CubePair1: " + avgPointsLeftCubePair1 + "\n" +
       "Avg Points Right CubePair1: " + avgPointsRightCubePair1 + "\n" +
       "Avg Points Left CubePair2: " + avgPointsLeftCubePair2 + "\n" +
       "Avg Points Right CubePair2: " + avgPointsRightCubePair2 + "\n" +
       "Avg Points Left CubePair3: " + avgPointsLeftCubePair3 + "\n" +
       "Avg Points Right CubePair3: " + avgPointsRightCubePair3 + "\n" +
       "Avg Points Left CubePair4: " + avgPointsLeftCubePair4 + "\n" +
       "Avg Points Right CubePair4: " + avgPointsRightCubePair4 + "\n" +
       "Avg Points Left CubePair5: " + avgPointsLeftCubePair5 + "\n" +
       "Avg Points Right CubePair5: " + avgPointsRightCubePair5 + "\n" +
       "Avg Points Left CubePair6: " + avgPointsLeftCubePair6 + "\n" +
       "Avg Points Right CubePair6: " + avgPointsRightCubePair6 + "\n" +
       "Avg Points Left CubePair7: " + avgPointsLeftCubePair7 + "\n" +
       "Avg Points Right CubePair7: " + avgPointsRightCubePair7 + "\n" +
       "Avg Points Left CubePair8: " + avgPointsLeftCubePair8 + "\n" +
       "Avg Points Right CubePair8: " + avgPointsRightCubePair8 + "\n" +
       "Avg Points Left CubePair9: " + avgPointsLeftCubePair9 + "\n" +
       "Avg Points Right CubePair9: " + avgPointsRightCubePair9 + "\n"
       );

        Debug.Log(
        "Avg Precision Left CubePair1: " + avgPrecisionLeftCubePair1 + "\n" +
        "Avg Precision Right CubePair1: " + avgPrecisionRightCubePair1 + "\n" +
        "Avg Precision Left CubePair2: " + avgPrecisionLeftCubePair2 + "\n" +
        "Avg Precision Right CubePair2: " + avgPrecisionRightCubePair2 + "\n" +
        "Avg Precision Left CubePair3: " + avgPrecisionLeftCubePair3 + "\n" +
        "Avg Precision Right CubePair3: " + avgPrecisionRightCubePair3 + "\n" +
        "Avg Precision Left CubePair4: " + avgPrecisionLeftCubePair4 + "\n" +
        "Avg Precision Right CubePair4: " + avgPrecisionRightCubePair4 + "\n" +
        "Avg Precision Left CubePair5: " + avgPrecisionLeftCubePair5 + "\n" +
        "Avg Precision Right CubePair5: " + avgPrecisionRightCubePair5 + "\n" +
        "Avg Precision Left CubePair6: " + avgPrecisionLeftCubePair6 + "\n" +
        "Avg Precision Right CubePair6: " + avgPrecisionRightCubePair6 + "\n" +
        "Avg Precision Left CubePair7: " + avgPrecisionLeftCubePair7 + "\n" +
        "Avg Precision Right CubePair7: " + avgPrecisionRightCubePair7 + "\n" +
        "Avg Precision Left CubePair8: " + avgPrecisionLeftCubePair8 + "\n" +
        "Avg Precision Right CubePair8: " + avgPrecisionRightCubePair8 + "\n" +
        "Avg Precision Left CubePair9: " + avgPrecisionLeftCubePair9 + "\n" +
        "Avg Precision Right CubePair9: " + avgPrecisionRightCubePair9 + "\n"
        );

        Debug.Log(
        "Avg Reactiontime Left CubePair1: " + avgTimeLeftCubePair1 + "\n" +
        "Avg Reactiontime Right CubePair1: " + avgTimeRightCubePair1 + "\n" +
        "Avg Reactiontime Left CubePair2: " + avgTimeLeftCubePair2 + "\n" +
        "Avg Reactiontime Right CubePair2: " + avgTimeRightCubePair2 + "\n" +
        "Avg Reactiontime Left CubePair3: " + avgTimeLeftCubePair3 + "\n" +
        "Avg Reactiontime Right CubePair3: " + avgTimeRightCubePair3 + "\n" +
        "Avg Reactiontime Left CubePair4: " + avgTimeLeftCubePair4 + "\n" +
        "Avg Reactiontime Right CubePair4: " + avgTimeRightCubePair4 + "\n" +
        "Avg Reactiontime Left CubePair5: " + avgTimeLeftCubePair5 + "\n" +
        "Avg Reactiontime Right CubePair5: " + avgTimeRightCubePair5 + "\n" +
        "Avg Reactiontime Left CubePair6: " + avgTimeLeftCubePair6 + "\n" +
        "Avg Reactiontime Right CubePair6: " + avgTimeRightCubePair6 + "\n" +
        "Avg Reactiontime Left CubePair7: " + avgTimeLeftCubePair7 + "\n" +
        "Avg Reactiontime Right CubePair7: " + avgTimeRightCubePair7 + "\n" +
        "Avg Reactiontime Left CubePair8: " + avgTimeLeftCubePair8 + "\n" +
        "Avg Reactiontime Right CubePair8: " + avgTimeRightCubePair8 + "\n" +
        "Avg Reactiontime Left CubePair9: " + avgTimeLeftCubePair9 + "\n" +
        "Avg Reactiontime Right CubePair9: " + avgTimeRightCubePair9 + "\n" 
        );


        float avgP1 = (avgTimeLeftCubePair1 + avgTimeRightCubePair1) / 2.0f;
        float avgP2 = (avgTimeLeftCubePair2 + avgTimeRightCubePair2) / 2.0f;
        float avgP3 = (avgTimeLeftCubePair3 + avgTimeRightCubePair3) / 2.0f;
        float avgP4 = (avgTimeLeftCubePair4 + avgTimeRightCubePair4) / 2.0f;
        float avgP5 = (avgTimeLeftCubePair5 + avgTimeRightCubePair5) / 2.0f;
        float avgP6 = (avgTimeLeftCubePair6 + avgTimeRightCubePair6) / 2.0f;
        float avgP7 = (avgTimeLeftCubePair7 + avgTimeRightCubePair7) / 2.0f;
        float avgP8 = (avgTimeLeftCubePair8 + avgTimeRightCubePair8) / 2.0f;
        float avgP9 = (avgTimeLeftCubePair9 + avgTimeRightCubePair9) / 2.0f;
        avgPoints[0] = avgP1;
        avgPoints[1] = avgP2;
        avgPoints[2] = avgP3;
        avgPoints[3] = avgP4;
        avgPoints[4] = avgP5;
        avgPoints[5] = avgP6;
        avgPoints[6] = avgP7;
        avgPoints[7] = avgP8;
        avgPoints[8] = avgP9;
        pointsLeftT[0] = avgTimeLeftCubePair1;
        pointsLeftT[1] = avgTimeLeftCubePair2;
        pointsLeftT[2] = avgTimeLeftCubePair3;
        pointsLeftT[3] = avgTimeLeftCubePair4;
        pointsLeftT[4] = avgTimeLeftCubePair5;
        pointsLeftT[5] = avgTimeLeftCubePair6;
        pointsLeftT[6] = avgTimeLeftCubePair7;
        pointsLeftT[7] = avgTimeLeftCubePair8;
        pointsLeftT[8] = avgTimeLeftCubePair9;
        pointsRightT[0] = avgTimeRightCubePair1;
        pointsRightT[1] = avgTimeRightCubePair2;
        pointsRightT[2] = avgTimeRightCubePair3;
        pointsRightT[3] = avgTimeRightCubePair4;
        pointsRightT[4] = avgTimeRightCubePair5;
        pointsRightT[5] = avgTimeRightCubePair6;
        pointsRightT[6] = avgTimeRightCubePair7;
        pointsRightT[7] = avgTimeRightCubePair8;
        pointsRightT[8] = avgTimeRightCubePair9;


    }
}
