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

    // ai management
    public float reward = 0;
    private bool begin = false;

    // confirmed positive steps
    // <step, <reward, list<keycode>>>
    private SortedDictionary<float, SortedDictionary<float, List<KeyCode>>> ConfirmedStages
        = new SortedDictionary<float, SortedDictionary<float, List<KeyCode>>>();

    // bad steps ruled out 
    private Dictionary<float, List<KeyCode>> DisabledKeys;

    private float currentStage;

    #endregion


    #region AGENTS

    public int AmountOfAgents = 1;
    List<GameObject> Agents;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        foreach (KeyCode key in Inputs)
        {
            MLInput.AddInput(key);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
