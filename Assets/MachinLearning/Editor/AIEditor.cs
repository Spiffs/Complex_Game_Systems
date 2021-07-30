using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using CML;

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

        if (GUILayout.Button("Save"))
        {
            // instantiating the new Bot
            BotData data = new BotData();
            data.UsableStages = mLManager.GetSavedStages();
            data.StartPos = mLManager.StartingPosition;
            data.StartRot = mLManager.StartingRotation;
            data.Speed = mLManager.movementObj.Speed;
            data.Inputs = mLManager.Inputs;
            data.StepDisatnce = mLManager.RequiredStepDistance;

            SaveAISaveStages(data);

            try
            {
                string localPath = "Assets/NewMlBot.prefab";

                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                // Create the new Prefab.
                PrefabUtility.SaveAsPrefabAssetAndConnect(mLManager.AIBot, localPath, InteractionMode.UserAction);
            }
            catch
            {

            }
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
                try
                {
                    mLManager = GameObject.Find("MLAgent").GetComponent<MLManager>();
                }
                catch { Debug.Log("There is no MLAgent paired to the editor Window"); }


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

    //private void SaveToFile()
    //{
    //    string filename = "C:/Users/122os/OneDrive/Desktop/Complex_Game_Systems/Assets/SavedStagesXML.text";
    //    // create an instance of the XmlSerializer class specify the type of object to serialize.
    //    XmlSerializer serializer = new XmlSerializer(typeof(List<SortedDictionary<float, KeyCode>>));
    //    TextWriter writer = new StreamWriter(filename);

    //    // stages to save   
    //    List<SortedDictionary<float, KeyCode>> stages = mLManager.GetSavedStages();

    //    // serialize the purchase order, and close the TextWriter.
    //    serializer.Serialize(writer, stages);
    //    writer.Close();
    //}

    // TODO: ADD A BETTER PATH NAME TO WORK WITH BUILT GAME IF IT IS BUILT. _______________________________ HAXXOR

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


    static void SaveAISaveStages<T>(T savingObject)
    {
        string savesFilepath = Application.dataPath + "/SavedStagesXML.text";

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
}
