﻿using System.Collections;
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

    //private void OnEnable()
    //{
    //    mLManager = GameObject.Find("MLAgent").GetComponent<MLManager>();
    //    if (mLManager == null)
    //        Debug.Log("There is no MLAgent paired to the editor Window");
    //}

    public void OnGUI()
    {
        #region Editor Buttons

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
        if (SolveOnRun == false)
            GUI.enabled = false;

        if (GUILayout.Button("Cancel"))
        {
            SolveOnRun = false;
        }

        #endregion

        #region TargetFoundButtons

        GUI.enabled = false;
        if (TargetHasBeenFound)
            GUI.enabled = true;

        GUILayout.Label("");
        GUILayout.Label("Once the MLAgent has found");
        GUILayout.Label("a path, it can be saved by");
        GUILayout.Label("applying it to a new ML Bot");

        if (GUILayout.Button("Create Bot"))
        {
            // instantiating the new Bot
            GameObject newobj = Instantiate(mLManager.AIBot);
            newobj.GetComponent<MLBotBrain>().NewMLBot(mLManager.GetSavedStages(), mLManager.transform, mLManager.movementObj.Speed, mLManager.Inputs);
            
            try
            {
                // creating a new bot
                //AssetDatabase.CopyAsset("Assets/Prefabs/MLBot.prefab", "Assets/MLBots/NewMlBot.prefab");
                //using (var editingScope = new PrefabUtility.EditPrefabContentsScope())
                //{
                //    var prefabRoot = editingScope.prefabContentsRoot;

                //    prefabRoot.GetComponent<MLBotBrain>().NewMLBot(mLManager.GetSavedStages(), mLManager.StartingPosition, mLManager.movementObj.Speed, mLManager.Inputs);
                //}
                string localPath = "Assets/NewMlBot.prefab";

                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                // Create the new Prefab.
                PrefabUtility.SaveAsPrefabAssetAndConnect(newobj, localPath, InteractionMode.UserAction);
            }
            catch
            {
                // creating a folder as well as creating a new bot
                //status = ("Creating Folder");
                //AssetDatabase.CreateFolder("Assets", "MLBots");
                //AssetDatabase.CopyAsset("Assets/Prefabs/MLBot.prefab", "Assets /MLBots/NewMlBot.prefab");
                //using (var editingScope = new PrefabUtility.EditPrefabContentsScope())
                //{
                //    var prefabRoot = editingScope.prefabContentsRoot;

                //    prefabRoot.GetComponent<MLBotBrain>().NewMLBot(mLManager.GetSavedStages(), mLManager.StartingPosition, mLManager.movementObj.Speed, mLManager.Inputs);
                //}

            }

            Destroy(newobj);
        }
        GUI.enabled = true;

        #endregion


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
                if (mLManager == null)
                    Debug.Log("There is no MLAgent paired to the editor Window");

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
            TargetHasBeenFound = false;

            status = "Waiting for Editor to Play";
        }



        Repaint();
    }
}
