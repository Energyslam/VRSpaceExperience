using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public static class MyTools
{
    [MenuItem("My Tools/Add-To-Report %F1")]
    static void AppendDefaultsToReport()
    {
        CSVManager.AppendToReport(
            new string[3]
            {
                "0",
                "200",
                "4"
            });
        EditorApplication.Beep();
        Debug.Log("<color=green>Report updated succesfully!</color>");
    }

    [MenuItem("My Tools/Create-Text-File")]
    static void CreateTextFile()
    {
        string path = Application.dataPath + "/Log.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Excel codes");
        }
        string excelAverage = "";

        for (int j = 0; j < 20; j++)
        {
            int k = 2 + (1050 * j);
            for (int i = 0; i < 50; i++)
            {
                excelAverage += "C" + (k + (21 * i)) + ";";
            }
            excelAverage += "\n";
        }
        File.AppendAllText(path, excelAverage);
        Debug.Log("Success!");
    }
    [MenuItem("My Tools/Reset-Text-File")]
    static void ResetTextFile()
    {
        string path = Application.dataPath + "/Log.txt";
        File.WriteAllText(path, "Excel codes");
    }

    public static void DEV_AppendSpecificsToReport(string[] strings)
    {
        CSVManager.AppendToReport(strings);
        //EditorApplication.Beep();
        Debug.Log("<color=green>Specific data added succesfully!</color>");
    }

    public static void DEV_AppendHeadersToReport()
    {
        CSVManager.CreateHeaders();
    }

    [MenuItem("My Tools/Reset-Report %F12")]
    static void DEV_ResetReport()
    {
        CSVManager.CreateReport();
        EditorApplication.Beep();
        Debug.Log("<color=orange>Report has been reset...</color>");
    }
}
