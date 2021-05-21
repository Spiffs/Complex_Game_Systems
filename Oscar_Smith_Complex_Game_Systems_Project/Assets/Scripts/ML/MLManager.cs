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

    // TODO reward calculator:: _________________________________________________HAXXOR
}
