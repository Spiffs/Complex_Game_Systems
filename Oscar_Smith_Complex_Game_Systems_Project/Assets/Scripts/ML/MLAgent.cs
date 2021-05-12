using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CML;
using System.Linq;

public class MLAgent : MonoBehaviour
{
    // input management
    public MLInput MLInput = new MLInput();

    public KeyCode[] Inputs;

    // ai management
    private bool begin;
    public float reward = 0;
    private SortedDictionary<float, SortedDictionary<float, List<KeyCode>>> savedStages
        = new SortedDictionary<float, SortedDictionary<float, List<KeyCode>>>();
    private float currentStage;

    void Start()
    {
        foreach (KeyCode key in Inputs)
        {
            MLInput.AddInput(key);
        }

        StartCoroutine("TryButtons");
    }

    void FixedUpdate()
    {
        if (begin)
        {
            if (savedStages.ContainsKey(currentStage))
            {
                savedStages[currentStage].Last().Key
            }
        }
    }

    IEnumerator TryButtons()
    {
        for (int i = 0; i > Inputs.Length; i++)
        {
            MLInput.PressKey(Inputs[i]);
            yield return null;
            SortedDictionary<float, List<KeyCode>> newStep = savedStages[i];
            newStep.Add(reward, MLInput.GetKeysDown()); 
            savedStages.Add(currentStage, newStep);
        }
    }
}
