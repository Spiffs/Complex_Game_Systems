 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MLAgent : MonoBehaviour
{
    // input management


    public List<KeyCode> Inputs;

    // ai management
    public float reward = 0;
    private bool begin = false;

    private float currentStage;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (begin)
        {
            
        }
    }

    // used to set the inputs from the manager
    public void SetAvailableInputs(List<KeyCode> newInputs)
    {
        Inputs = newInputs;
    }


    // goes forward a step
    private void TakeStep()
    {

    }

    // checks for when the agent passes into the next stage
    private void CheckForStage()
    {

    }

    private void ApplyRewards(float a_reward)
    {

    }

    private void CheckForFail()
    {

    }

    private void LoadAIStages()
    {

    }

    private void SaveAIStages()
    {

    }
}
