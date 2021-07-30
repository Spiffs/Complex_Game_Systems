using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using UnityEngine;
using CML;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable]
public class MLBotBrain : MonoBehaviour
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

    // waiting for seconds before starting logic 
    public float SecondsBeforeStart = 1;
    private float SecondsPassed = 0;

    private bool RunStart = false;

    #endregion

    public TextMeshPro CountdownText;

    public GameObject Player;

    public Canvas canvas;


    // constructor-esk function
    public bool NewMLBot(BotData newData)
    {
        try
        {
            SavedStages = newData.UsableStages;
            StartingPos.position = newData.StartPos;
            StartingPos.rotation = newData.StartRot;
            mlBotMovement.SetSpeed(newData.Speed);
            Inputs = newData.Inputs;
            RequiredStepDistance = newData.StepDisatnce;
            return true;
        }
        catch { return false; }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartingPos = transform;

        // setting the positon to the correct starting position
        transform.position = StartingPos.position;
        transform.rotation = StartingPos.rotation;

        Inputs = new List<KeyCode>();

        // set the car object 
        mlBotMovement = this.GetComponent<MLBotMovement>();

        foreach (KeyCode key in Inputs)
        {
            MLInput.AddInput(key);
        }

        NewMLBot(LoadAISavedStages<BotData>());

        canvas.gameObject.SetActive(false);

        StartCoroutine("ThreeSecondCountdown");
    }


    void FixedUpdate()
    {
        if (PlayOnRun && !TargetFound && RunStart)
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
            canvas.gameObject.SetActive(true);
            Player.GetComponent<PlayerMovement>().CanPlay = false;
        } // if the AI fails on step
    }

    static T LoadAISavedStages<T>()
    {
       string savesFilepath = Application.dataPath + "/SavedStagesXML.text";

        // loading those that are saved AI routes
        var s_fileStream = new FileStream(savesFilepath, FileMode.Open);
        var s_reader = XmlDictionaryReader.CreateTextReader(s_fileStream, new XmlDictionaryReaderQuotas());
        var s_serializer = new DataContractSerializer(typeof(T));
        T s_serializableObject = (T)s_serializer.ReadObject(s_reader, true);
        s_reader.Close();
        s_fileStream.Close();
        return s_serializableObject;
    }    // load saved routes and stages

    IEnumerator ThreeSecondCountdown()
    {
        while (SecondsPassed != SecondsBeforeStart)
        {
            CountdownText.text = (SecondsBeforeStart - SecondsPassed).ToString();
            yield return new WaitForSeconds(1);
            SecondsPassed++;
        }
        RunStart = true;
        CountdownText.gameObject.SetActive(false);
        Player.GetComponent<PlayerMovement>().CanPlay = true;
    }

    public void Restart() { Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name); }
    public void AppExit() { Application.Quit(); }
}
