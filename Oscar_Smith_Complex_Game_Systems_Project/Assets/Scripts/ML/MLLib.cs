using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CML
{
    public class MLInput : MonoBehaviour
    { 
        private Dictionary<KeyCode, bool> AvailableInputs = new Dictionary<KeyCode, bool>();

        public void AddInput(KeyCode newInput)
        {
            AvailableInputs.Add(newInput, false);
        }

        public void PressKey(KeyCode pressedKey)
        {
            AvailableInputs[pressedKey] = true;
        }

        public bool GetKeyDown(KeyCode key)
        {
            if (AvailableInputs[key] || Input.GetKeyDown(key))
            {
                return true;
            }
            else
                return false;
        }

        public void Update()
        {
            foreach(KeyValuePair<KeyCode, bool> input in AvailableInputs)
            {
                AvailableInputs[input.Key] = false;
            }
        }
    }

    public class MLEnvioronmentInfo : MonoBehaviour
    {
        private Transform AITransform;

        public void SetObjectTransform(Transform newTransform)
        {
            AITransform = newTransform;
        }
    }
}
