using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public static void DEV_AppendSpecificsToReport(string[] strings)
    {
        CSVManager.AppendToReport(strings);
        //EditorApplication.Beep();
        Debug.Log("<color=green>Specific data added succesfully!</color>");
    }

    [MenuItem("My Tools/Reset-Report %F12")]
    static void DEV_ResetReport()
    {
        CSVManager.CreateReport();
        EditorApplication.Beep();
        Debug.Log("<color=orange>Report has been reset...</color>");
    }
}
