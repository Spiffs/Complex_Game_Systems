using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AIEditor : EditorWindow
{
    // solve on run bool tracking to execute in Update
    private bool SolveOnRun = false;
    private bool Solving = false;

    // starting functionality
    private bool Start = false; 

    // statis string for editor
    private string status = "Default!";

    private MLManager[] mLManager = GameObject.FindObjectsOfType<MLManager>();

    [MenuItem("Window/ML AI")]
    public static void Init()
    {
        GetWindow<AIEditor>("Machine Learning AI");
    }

    private void OnGUI()
    {
        GUILayout.Label("To start the Agent, Press 'Solve' in runtime");
        GUILayout.Label("");
        GUILayout.Label("status: " + status);

        // disable the button if it is selected
        GUI.enabled = true;
        if (SolveOnRun == true)
            GUI.enabled = false;

        // solve on run button
        if (GUILayout.Button("Solve"))
        {
            SolveOnRun = true;
        }

        // return to gui enabled
        GUI.enabled = true;
        GUI.enabled = true;
        if (SolveOnRun == false)
            GUI.enabled = false;

        if (GUILayout.Button("Cancel"))
        {
            SolveOnRun = false;
        }
    }

    void Update()
    {
        // start functionality
        if (!Start)
        {
            if (SolveOnRun)
            {
                Solving = true;
            }

            Start = true;
        }

        if (EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            status = "Elapse Time: " + Time.time;
        }
        else
        {
            status = "Waiting for Editor to Play";
        } 
            
        Repaint();
    }
}
