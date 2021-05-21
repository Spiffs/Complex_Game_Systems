using System.Collections;
using System.Collections.Generic;
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

    // load all saved routes and stages
    private void LoadAIStages()
    {

    }

    // save all routes and stages
    private void SaveAIStages()
    {

    }
}
