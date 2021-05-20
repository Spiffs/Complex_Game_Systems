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

    IEnumerator TryButtons()
    {
        for (int i = 0; i > Inputs.Count; i++)
        {
            MLInput.PressKey(Inputs[i]);
            yield return null;
            SortedDictionary<float, List<KeyCode>> newStep = savedStages[i];
            newStep.Add(reward, MLInput.GetKeysDown()); 
            savedStages.Add(currentStage, newStep);
        }
        begin = true;
    }

    // goes forward a step
    private void TakeStep()
    {

    }

    // goes back a step on fail 
    private void BackStep()
    {

    }

    // checks for when the agent passes into the next stage
    private void CheckForUnit()
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
