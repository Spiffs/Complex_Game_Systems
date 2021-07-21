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
    private bool TargetHasBeenFound = false;

    private MLManager mLManager;

    [MenuItem("Window/ML AI")]
    public static void Init()
    {
        GetWindow<AIEditor>("Machine Learning AI");
        EditorWindow AIE = GetWindow<AIEditor>("Machine Learning AI");
        AIE.minSize = new Vector2(215f, 110f);
    }

    public void OnGUI()
    {

        GUILayout.Label("To start the Agent, Press 'Solve' in runtime");
        GUILayout.Label("");
        GUILayout.Label("   status: " + status);

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
        // update
        if (EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            #region START
            // start functionality
            if (!Start)
            {
                mLManager = GameObject.Find("MLAgent").GetComponent<MLManager>();

                Start = true;
            }
            #endregion

            // status change
            if (SolveOnRun)
                status = "Elapse Time: " + Time.time;
            else
                status = "Not Solving, Press Solve and rerun to Begin";

            // starting the ML Agent
            if (SolveOnRun)
            {
                mLManager.SolveOnRun = true;
                Solving = true;
            }          
            else if (!SolveOnRun)
            {
                mLManager.SolveOnRun = false;
                Solving = false;
            }

            // if paths are found
            if (Solving)
            {
                // if the target has been found
                if (mLManager.TargetFound == true)
                {
                    TargetHasBeenFound = true;
                    Solving = false;
                }
            }

            // if all targets have been found
            if (TargetHasBeenFound)
            {
                status = "Target Found!!";
            }
        }

        // editor update, not update
        else
        {
            Start = false;

            status = "Waiting for Editor to Play";
        }



        Repaint();
    }
}
