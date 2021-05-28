using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MLAgent : MonoBehaviour
{
    #region STAGE CHANGES 
    
    // trakcs the current stage of the agent
    private float CurrentStage = -1;
    
    // timer
    // timer count for privatly tracking timer
    private float TimerCount = 0;
    // for setting the time goal in the editor
    public float Timer = 0;

    // for the mlBrain to detect whether the agent is completing a stage or not
    public bool CompleteStage = true; // true when not moving and ready to recieve a new input. 

    #endregion


    // saves position to calculate whether the agent has taken a step
    private Vector3 PreStepPosition;

    // available inputs and prepares the availaqble inputs list 
    List<KeyCode> CurrentInputs = new List<KeyCode>();
     

    void Start() {

    }

    public bool ControlledUpdate(List<KeyCode> input)
    {
        if (CheckForStage())
        {
            return true;
        }


    } // returns a bool for the mlBrain to detect whether the agent is completing a stage or not

    public void SetAvailableInputs(List<KeyCode> newInputs)
    {

    } // sets the available inputs from the mlBrain                                            _____________________________________________________HAXXOR

    public float GetStage()
    {
        return CurrentStage;
    } // returns the current stage of the agent

    private void TakeStep(List<KeyCode> input)
    {

    } // goes forward a step                       _____________________________________________________HAXXOR

    // checks for when the agent passes into the next stage
    private bool CheckForStage()
    {
        if (Vector3.Distance(transform.position, PreStepPosition) >= 1)
        {
            CurrentStage++;
            CurrentInputs.Clear();

            return true;
        }

        return false;
    } // checks for completed stage and returns true or false

    // save a step to the ai manager 
    private void SaveStep(float a_reward)
    {

    } // save a step to the ai manager                  _____________________________________________________HAXXOR

    private void CheckForCollision()
    {

    } // checks for collisions and applys a fail to the agent_____________________________________________________HAXXOR
     
    
    public float CalculateReward()
    {

    } // TODO reward calculator                                                                _____________________________________________________HAXXOR
}
