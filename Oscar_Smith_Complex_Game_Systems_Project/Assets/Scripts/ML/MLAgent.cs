using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MLAgent : MonoBehaviour
{
    private float CurrentStage = -1;

    void Start()
    {

    }

    void FixedUpdate()
    {

    }

    // used to set the inputs from the manager
    public void SetAvailableInputs(List<KeyCode> newInputs)
    {

    }

    
    public float GetStage()
    {
        return CurrentStage;
    } // returns the current stage of the agent

    
    private void TakeStep(List<KeyCode> input)
    {

    } // goes forward a step

    // checks for when the agent passes into the next stage
    private void CheckForStage()
    {

    }

    // save a step to the ai manager 
    private void SaveStep(float a_reward)
    {

    } // save a step to the ai manager 

    private void CheckForCollision()
    {

    }
}
