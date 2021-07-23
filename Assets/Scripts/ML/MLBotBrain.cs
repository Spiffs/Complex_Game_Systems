using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using UnityEngine;
using CML;
using System.Linq;

[Serializable]
public class MLBotBrain: MonoBehaviour
{
    #region INPUTS

    // input management
    public MLInput MLInput = new MLInput();
    public List<KeyCode> Inputs;

    #endregion


    #region AI MANAGEMENT

    // public variables
    public float RequiredStepDistance;

    // private variables

    // the starting position of the current step
    private Vector3 StartStepPos;

    // setting the starting position of the bot to the starting
    // position of the MLAgent to follow the track accuratly 
    private Transform StartingPos;

    // run path for MLBot
    private List<SortedDictionary<float, KeyCode>> SavedStages;

    // current step of the bot
    private int CurrentStage = 0;

    // the current key pressed by the bot
    private KeyCode CurrentKey = KeyCode.F15;

    // letting the code know if it can test steps again
    private bool StepInProgress = false;

    // access to car
    private MLBotMovement mlBotMovement;

    // if the user wants the bot to play on runtime
    public bool PlayOnRun;

    private bool TargetFound = false;


    #endregion

    // constructor-esk function
    public bool NewMLBot(List<SortedDictionary<float, KeyCode>> newStages, Transform newStartPos, float newSpeed, List<KeyCode> newInputs)
    {
        try
        {
            SavedStages = newStages;
            StartingPos = newStartPos;
            mlBotMovement.SetSpeed(newSpeed);
            Inputs = newInputs;
            return true;
        }
        catch { return false; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // setting the positon to the correct starting position
        transform.position = StartingPos.position;
        transform.rotation = StartingPos.rotation;

        // loading the saved stages
        //SavedStages = new List<SortedDictionary<float, KeyCode>>(); _________________ HAXXOR
        Inputs = new List<KeyCode>();

        // set the car object 
        mlBotMovement = this.GetComponent<MLBotMovement>();

        foreach (KeyCode key in Inputs)
        {
            MLInput.AddInput(key);
        }
    }


    void FixedUpdate()
    {
        if (PlayOnRun && !TargetFound)
        {
            mlBotMovement.HoldForward = true;

            // if there is not a step in progress
            if (!StepInProgress)
            {
                // setting the starting position of the current step
                StartStepPos = transform.position;

                // key to apply as input
                KeyCode ToBeTested = KeyCode.F15;

                // main AI decision making process 
                // if saved stages are available
                if (SavedStages.Count > CurrentStage && SavedStages[CurrentStage].Count > 0)
                {
                    try
                    {
                        ToBeTested = SavedStages[CurrentStage].Values.Last();
                    }
                    catch
                    {
                        Debug.Log("No saved stage at this position " + CurrentStage);
                    }
                }

                // apply the Key
                if (ToBeTested != KeyCode.F15)
                {
                    StepInProgress = true;
                    CurrentKey = ToBeTested;
                    MLInput.PressKey(ToBeTested);
                    Debug.Log("Bot:: " + CurrentStage + " using: " + ToBeTested.ToString());
                }
            }

            // TODO: stage check & calculate reward on stage
            else if (StepInProgress)
            {
                if (Vector2.Distance(StartStepPos, transform.position) > RequiredStepDistance)
                {
                    // set step in progress to false as step is completed
                    StepInProgress = false;

                    // add 1 to the current stage as the current is complete
                    CurrentStage++;

                    // resetting the current key
                    CurrentKey = KeyCode.F15;

                }

                // if not successed, repress the required button to continue the step
                else
                {
                    MLInput.PressKey(CurrentKey);
                }
            }
        }
        // if the AI is not solving
        else
        {
            mlBotMovement.HoldForward = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "MLTarget")
        {
            TargetFound = true;
        } // if the AI fails on step
    }
}
