﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using UnityEngine;
using CML;
using System.Linq;

public class MLManager : MonoBehaviour
{
    /* Error codes:
     * 
     * ERR 102 : No inputs in ML Manager, TO FIX : add inputs to list in Inspector
   
     */

    #region INPUTS

    // input management
    public MLInput MLInput = new MLInput();
    [SerializeField]
    public List<KeyCode> Inputs;

    #endregion


    #region AI MANAGEMENT

    // public variables
    public float RequiredStepDistance = 1;

    // target Transform
    public Transform TargetTransform;


    // private variables
    // starting position of the AI
    private Transform StartingTransform;

    // determining a step by distance
    private Vector2 StartStepPos;

    // managing rewards for all inputs 
    // current stage #list < Keycode in stage, reward return for respective input
    private List<SortedDictionary<float, KeyCode>> SavedStages;
    private List<SortedDictionary<float, KeyCode>> FailStages;

    // curremt and targeted stage for rerun
    // current stage starts at 0 = first run :)
    private int CurrentStage = 0;

    // for recalling keys to add to the list
    private KeyCode PreviousKey = KeyCode.F15;
    private KeyCode CurrentKey = KeyCode.F15;

    // letting the code know if it can test steps again
    private bool StepInProgress = false;

    // fail checking to return to the updaate function
    private bool FailOnStep = false;

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        // set the starting position of the AI character 
        StartingTransform = transform;

        // initialise saved stages as a dictionary
        SavedStages = new List<SortedDictionary<float, KeyCode>>();

        // initialise failed stages as a dictionary
        FailStages = new List<SortedDictionary<float, KeyCode>>();


        foreach (KeyCode key in Inputs)
        {
            MLInput.AddInput(key);
        }
    }


    void Update()
    {
        // main look first checks for fails after an update as well as step count
        if (FailOnStep)
        {
            // add step to fail stages
            FailStages[CurrentStage].Add(-2, CurrentKey); // default fail reward = -2

            Restart();
            return;
        }
        else if (!StepInProgress)
        {
            // get the starting position of the step
            StartStepPos = transform.position;

            // key to apply as input
            KeyCode ToBeTested = KeyCode.F15;

            // main AI decision making process 
            // if saved stages are available
            if (SavedStages.Count > CurrentStage)
            {
                try
                {
                    if (SavedStages[CurrentStage].Count > 0)
                    {
                        ToBeTested = SavedStages[CurrentStage].Values.Last();
                    }
                }
                catch
                {
                    Debug.Log("The ML Manager has no inputs to use: ERR 102");
                }
            }

            else if (SavedStages.Count <= 0)
            {
                // add to each list to make room for a new step 
                SavedStages.Add(new SortedDictionary<float, KeyCode>());
                FailStages.Add(new SortedDictionary<float, KeyCode>());

                // if savedstages does not have any available inputs
                if (SavedStages[CurrentStage].Count <= 0)
                {
                    // try random move  
                    // search for key that has not been tested
                    foreach (var key in Inputs)
                    {
                        if (FailStages.Count >= CurrentStage && FailStages[CurrentStage].Count > 0)
                        {
                            if (!FailStages[CurrentStage].ContainsValue(key))
                            { ToBeTested = key; break; }
                        }
                        else
                            ToBeTested = Inputs[0]; break;
                    }
                }
            }

            // apply the Key
            if (ToBeTested != KeyCode.F15)
            {
                StepInProgress = true;
                CurrentKey = ToBeTested;
                MLInput.PressKey(ToBeTested);
            }
            else
            {
                try
                {
                    FailStages[CurrentStage - 1].Add(-2, PreviousKey); // default fail reward = -2
                    Restart();
                }
                catch
                {
                    Debug.Log("Oscar is Monkey");
                }
                }
        }

        // TODO: stage check & calculate reward on stage
        else if (StepInProgress)
        {
            if (Vector2.Distance(StartStepPos, transform.position) > RequiredStepDistance)
            {
                // set step in progress to false as step is completed
                StepInProgress = false;

                // save step and reward
                //if (!SavedStages.Count > CurrentStage)


                SavedStages[CurrentStage].Add(RewardCalculations(), CurrentKey);

                // add 1 to the current stage as the current is complete
                CurrentStage++;

                // reset the step key to none and setting previous key
                PreviousKey = CurrentKey;
                CurrentKey = KeyCode.F15;
            }
        }
    }

    void Restart()
    {
        // resetting position to starting position
        transform.position = StartingTransform.position;
        transform.rotation = StartingTransform.rotation;
        transform.localScale = StartingTransform.localScale;

        StartStepPos = Vector2.zero;

        // resetting vairables
        CurrentStage = 0;
        CurrentKey = KeyCode.F15;
        PreviousKey = KeyCode.F15;

        FailOnStep = false;
        StepInProgress = false;

    } // restart fuction

    private float RewardCalculations()
    {
        return Vector2.Distance(TargetTransform.position, transform.position) * -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FailOnStep = true;
    } // if the AI fails on step


    #region SAVING_AND_LOADING

    static T LoadAISavedStages<T>()
    {
        string savesFilepath = "/AISaves/SavedStages.txt";

        // loading those that are saved AI routes
        var s_fileStream = new FileStream(savesFilepath, FileMode.Open);
        var s_reader = XmlDictionaryReader.CreateTextReader(s_fileStream, new XmlDictionaryReaderQuotas());
        var s_serializer = new DataContractSerializer(typeof(T));
        T s_serializableObject = (T)s_serializer.ReadObject(s_reader, true);
        s_reader.Close();
        s_fileStream.Close();
        return s_serializableObject;
    }    // load saved routes and stages


    static void SaveAISaveStages<T>(T savingObject)
    {
        string savesFilepath = "/AISaves/SavedStages.txt";

        var serializer = new DataContractSerializer(typeof(T));
        var settings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "\t",
        };
        var writer = XmlWriter.Create(savesFilepath, settings);
        serializer.WriteObject(writer, savingObject);
        writer.Close();

    }   // saving those that are saved AI routes 

    #endregion

}
