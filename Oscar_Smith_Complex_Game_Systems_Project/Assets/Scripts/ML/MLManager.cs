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
    public MLInput MLInput = new MLInput();
    public List<KeyCode> Inputs;

    #endregion


    #region AI MANAGEMENT

    // managing routes and rewards for all inputs 
    // routes<input, reward>
    List<SortedDictionary<KeyCode, float>> SavedGenerations;
    List<SortedDictionary<KeyCode, float>> FailStagesInRoutes;
    
    #endregion


    #region AGENTS

    public int AmountOfAgents = 1;
    List<GameObject> Agents;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        SavedGenerations = new List<SortedDictionary<KeyCode, float>>();
        FailStagesInRoutes = new List<SortedDictionary<KeyCode, float>>();

        foreach (KeyCode key in Inputs)
        {
            MLInput.AddInput(key);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    { 



    }

    // TODO reward calculator:: _________________________________________________HAXXOR
    public float CalculateReward()
    {

    }


    static T LoadAISavedStages<T>()
    {
        string savesFilepath = "/AISaves/SavedGenerations.txt";

        // loading those that are saved AI routes
        var s_fileStream = new FileStream(savesFilepath, FileMode.Open);
        var s_reader = XmlDictionaryReader.CreateTextReader(s_fileStream, new XmlDictionaryReaderQuotas());
        var s_serializer = new DataContractSerializer(typeof(T));
        T s_serializableObject = (T)s_serializer.ReadObject(s_reader, true);
        s_reader.Close();
        s_fileStream.Close();
        return s_serializableObject;
    }    // load saved routes and stages

    static T LoadAIFailedStages<T>()
    {
        string failFilepath = "/AISaves/FailStagesInRoutes.txt";

        // loading those that are failed AI routes
        var f_fileStream = new FileStream(failFilepath, FileMode.Open);
        var f_reader = XmlDictionaryReader.CreateTextReader(f_fileStream, new XmlDictionaryReaderQuotas());
        var f_serializer = new DataContractSerializer(typeof(T));
        T f_serializableObject = (T)f_serializer.ReadObject(f_reader, true);
        f_reader.Close();
        f_fileStream.Close();
        return f_serializableObject;
    }    // load saved routes and stages

    static void SaveAISaveStages<T>(T savingObject)
    {
        string savesFilepath = "/AISaves/SavedGenerations.txt";

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

    static void SaveAIFailsStages<T>(T savingObject)
    { 
        string failFilepath = "/AISaves/FailStagesInRoutes.txt";

        var serializer = new DataContractSerializer(typeof(T));
        var settings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "\t",
        };
        var writer = XmlWriter.Create(failFilepath, settings);
        serializer.WriteObject(writer, savingObject);
        writer.Close();

        
    }   // saving those that are failed AI routes
}
