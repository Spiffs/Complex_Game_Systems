using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using UnityEngine;
using CML;
using System.Linq;

public class MLManager : MonoBehaviour
{
    #region INPUTS

    // input management

    [SerializeField]
    private MLInput MLInput = new MLInput();
    [SerializeField]
    public List<KeyCode> Inputs;

    #endregion


    #region AI MANAGEMENT

    // public variables
    public float RequiredStepDistance = 1;


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
    private KeyCode PreviousKey = KeyCode.None;
    private KeyCode CurrentKey = KeyCode.None;

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
            KeyCode ToBeTested = KeyCode.None;

            // main AI decision making process 
            // if saved stages are available
            if (SavedStages[CurrentStage].Count > 0 )
            {
                ToBeTested = SavedStages[CurrentStage].Values.Last();
            }

            // if savedstages does not have any available inputs
            else if (SavedStages[CurrentStage].Count <= 0)
            {
                // try random move  
                // search for key that has not been tested
                foreach (var key in Inputs)
                {
                    if (!FailStages[CurrentStage].ContainsValue(key))
                    { ToBeTested = key; break; }
                }
            }

            if (ToBeTested != KeyCode.None)
            {
                StepInProgress = true;
                CurrentKey = ToBeTested;
                MLInput.PressKey(ToBeTested);
            }
            else
            {
                // adds the current step if all inputs are failed
                FailStages[CurrentStage--].Add(-2, PreviousKey); // default fail reward = -2
                Restart();
            }
        }

        // TODO: stage check & calculate reward on stage
        else if (StepInProgress)
        {
            if (Vector2.Distance(StartStepPos, transform.position) > RequiredStepDistance)
            {
                // reset the step key to none and setting previous key
                PreviousKey = CurrentKey;
                CurrentKey = KeyCode.None;

                // add 1 to the current stage as the current is complete
                CurrentStage++;

                // set step in progress to false as step is completed
                StepInProgress = false;

                // TODO: save step and reward and restart

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
        CurrentKey = KeyCode.None;
        PreviousKey = KeyCode.None;

        FailOnStep = false;
        StepInProgress = false;

    } // restart fuction

    private float RewardCalculations()
    {
        return Vector2.Distance(StartStepPos, transform.position) * -1;
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
