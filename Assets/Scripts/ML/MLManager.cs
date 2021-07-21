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
    /* Error codes:
     * 
     * ERR 102 : No inputs in ML Manager, TO FIX : add inputs to list in Inspector
       ERR 103 : Issue with failing stages, usually a duplicate has occured
       ERR 104 : Issue with removing from position in SavedStages
       ERR 105 : Error removing a previously falsley saved stage at fail
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
    private Vector3 StartingPosition;
    private Quaternion StartingRotation;

    // determining a step by distance
    private Vector2 StartStepPos;

    // managing rewards for all inputs 
    // current stage #list < Keycode in stage, reward return for respective input
    private List<SortedDictionary<float, KeyCode>> SavedStages;
    private List<SortedDictionary<KeyCode, float>> FailStages;

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

    // access to car
    private CarMovement movementObj;

    // path ending
    [NonSerialized]
    public bool TargetFound = false;
    [NonSerialized]
    public bool SolveOnRun = false;
    

    // for creating an AI Bot 


    #endregion



    // Start is called before the first frame update
    void Start()
    {
        // set the starting position of the AI character 
        StartingPosition = transform.position;
        StartingRotation = transform.rotation;

        // initialise saved stages as a dictionary
        SavedStages = new List<SortedDictionary<float, KeyCode>>();

        // initialise failed stages as a dictionary
        FailStages = new List<SortedDictionary<KeyCode, float>>();

        // set the car object 
        movementObj = this.GetComponent<CarMovement>();

        foreach (KeyCode key in Inputs)
        {
            MLInput.AddInput(key);
        }
    }


    void FixedUpdate()
    {
        if (!TargetFound && SolveOnRun)
        {
            movementObj.HoldForward = true;

            // main look first checks for fails after an update as well as step count
            if (FailOnStep)
            {
                try
                {
                    // check for if Fail on stage was called while the AI had no inputs to play
                    if (CurrentKey != KeyCode.F15)
                    {
                        // add step to fail stages
                        FailStages[CurrentStage].Add(CurrentKey, RewardCalculations() - UnityEngine.Random.Range(-11, -9)); // default fail reward = -2
                    }
                    else if (CurrentKey == KeyCode.F15)
                    {
                        FailStages[CurrentStage - 1].Add(PreviousKey, RewardCalculations() - UnityEngine.Random.Range(-11, -9)); // default fail reward = -2
                        if (SavedStages[CurrentStage - 1].Count > 0)
                            try
                            {
                                float tempKey = SavedStages[(CurrentStage - 1)].Where(x => x.Value.Equals(PreviousKey)).Select(x => x.Key).ToArray().First();
                                SavedStages[(CurrentStage - 1)].Remove(tempKey);
                            }
                            catch { Debug.Log("ERR 105 : -----------------------------------------------"); }
                    }
                }
                catch { Debug.Log("ERR 103 : " + "  " + CurrentStage + ", " + CurrentKey.ToString() + " -----------------------------------------------"); }

                Restart();
                return;
            }

            // if there is not a step in progress
            if (!StepInProgress)
            {
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
                        Debug.Log("ERR 102 : -----------------------------------------------");
                    }
                }

                else //if (SavedStages.Count <= 0)
                {
                    // add to each list to make room for a new step 
                    if (SavedStages.Count <= CurrentStage)
                        SavedStages.Add(new SortedDictionary<float, KeyCode>());
                    if (FailStages.Count <= CurrentStage)
                        FailStages.Add(new SortedDictionary<KeyCode, float>());

                    // if savedstages does not have any available inputs
                    if (SavedStages[CurrentStage].Count <= 0)
                    {
                        // try the first move if none are selected

                        if (FailStages[CurrentStage].Count >= Inputs.Count)
                        {
                            // eliminate the stage and remove the saved failstages for the current move to open more possibilities.
                            FailStages[(CurrentStage - 1)].Add(PreviousKey, RewardCalculations() - 10); // default fail reward = -2
                            FailStages[CurrentStage].Clear(); // should clear the current fail stages

                            // gets the key to the Previous key to remove it from the saved stages 
                            try
                            {
                                float tempKey = SavedStages[(CurrentStage - 1)].Where(x => x.Value.Equals(PreviousKey)).Select(x => x.Key).ToArray().First();
                                SavedStages[(CurrentStage - 1)].Remove(tempKey);
                            }
                            catch { Debug.Log("ERR 104 : -----------------------------------------------"); }

                            Restart();
                            return;
                        }

                        // try random move  
                        // search for key that has not been tested
                        else if (FailStages.Count >= CurrentStage && FailStages[CurrentStage].Count > 0)
                        {
                            foreach (var key in Inputs)
                            {
                                if (!FailStages[CurrentStage].ContainsKey(key))
                                { ToBeTested = key; break; }
                            }
                        }
                        else
                        {
                            ToBeTested = Inputs[0];
                        }
                    }
                }

                // apply the Key
                if (ToBeTested != KeyCode.F15)
                {
                    StepInProgress = true;
                    CurrentKey = ToBeTested;
                    MLInput.PressKey(ToBeTested);
                    Debug.Log(CurrentStage + " using: " + ToBeTested.ToString());
                }
            }

            // TODO: stage check & calculate reward on stage
            else if (StepInProgress && !FailOnStep)
            {
                if (Vector2.Distance(StartStepPos, transform.position) > RequiredStepDistance)
                {
                    // set step in progress to false as step is completed
                    StepInProgress = false;

                    float rewardForStep = RewardCalculations();

                    if (SavedStages.Count > CurrentStage && CurrentStage > 0)
                    {
                        if (rewardForStep < SavedStages[CurrentStage - 1].Keys.First())
                        {
                            // fail the step easily by reusing code in the main desicion loop
                            FailOnStep = true;
                            // to go back through and register a failed step 
                            StepInProgress = false;

                            // skip to next statement
                            return;
                        }
                    }

                    if (!SavedStages[CurrentStage].ContainsKey(rewardForStep) && SavedStages[CurrentStage].Count <= 0)
                        SavedStages[CurrentStage].Add(rewardForStep, CurrentKey);

                    // add 1 to the current stage as the current is complete
                    CurrentStage++;

                    // reset the step key to none and setting previous key
                    PreviousKey = CurrentKey;
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
            movementObj.HoldForward = false;
        }
    }

    void Restart()
    {
        // resetting position to starting position
        transform.position = StartingPosition;
        transform.rotation = StartingRotation;

        StartStepPos = transform.position;

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
        if (collision.gameObject.name != "MLTarget")
        {
            FailOnStep = true;
        }

        if (collision.gameObject.name == "MLTarget")
        {
            TargetFound = true;
            Restart();
        } // if the AI fails on step
    }

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
