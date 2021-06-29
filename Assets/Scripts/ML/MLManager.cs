using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using UnityEngine;
using CML;


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

    // managing rewards for all inputs 
    // current stage #list < Keycode in stage, reward return for respective input
    private List<Dictionary<KeyCode, float>> SavedStages;
    private List<Dictionary<KeyCode, float>> FailStages;

    // curremt and targeted stage for rerun
    // current stage starts at 0 = first run :)
    private int CurrentStage = 0;
    private int TargetRerunStage = 0;

    // letting the code know if it can test steps again
    private bool StepInProgress = false;

    // fail checking to return to the updaate function
    private bool FailOnStep = false;

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        // initialise saved stages as a dictionary
        SavedStages = new List<Dictionary<KeyCode, float>>();

        // initialise failed stages as a dictionary
        FailStages = new List<Dictionary<KeyCode, float>>();


        foreach (KeyCode key in Inputs)
        {
            MLInput.AddInput(key);
        }
    }


    void Update()
    {
        if (!StepInProgress)
        {
            // main look first checks for fails after an update as well as step count
            if (FailOnStep)
            {
                TargetRerunStage = CurrentStage;
                FailOnStep = true;
                Restart();
            }
            else
            {
                // main AI decision making process 
                if (SavedStages[CurrentStage].Count <= 0)
                {
                    // try random move  
                    // list of inputs that are yet to be tested on the current stage
                    KeyCode ToBeTested = KeyCode.None;

                    // search for key that has not been tested
                    foreach (var key in Inputs)
                    {
                        if (!FailStages[CurrentStage].ContainsKey(key))
                        { ToBeTested = key; break; }
                    }
                    MLInput.PressKey(ToBeTested);
                }

            }
        }
        // TODO: stage check & calculate reward on stage
    }




    void Restart()
    {
        // TODO: Restart


    } // restart script


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
