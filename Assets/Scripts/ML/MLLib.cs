using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CML
{
    public class MLInput
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

        public bool IsKeyDown(KeyCode key)
        {
            if (AvailableInputs[key] == true || Input.GetKeyDown(key))
            {
                return true;
            }
            else
                return false;
        }

        public List<KeyCode> GetKeysDown()
        {
            List<KeyCode> downKeys = new List<KeyCode>();

            foreach (KeyCode key in AvailableInputs.Keys)
            {
                if (AvailableInputs[key] == true)
                    downKeys.Add(key);
            }
            return downKeys;
        }

        public void Update()
        {
            // Error InvalidOperationException, FIX: Do not edit an arrays contents while iterating through them. Bad Practice
            // FIX: Create temp then make equal NOT WORKING ((Dictionary<KeyCode, bool> tempDictionary = AvailableInputs;))
            // FIX: Create a temp list and readd all variables into the new list (( Dictionary<KeyCode, bool> tempDictionary = new Dictionary<KeyCode, bool>();))
            Dictionary<KeyCode, bool> tempDictionary = new Dictionary<KeyCode, bool>();

            foreach (KeyCode key in AvailableInputs.Keys)
            {
                tempDictionary.Add(key, false);
            }

            AvailableInputs = tempDictionary;
        }
    }


    public class MLEnvioronmentInfo
    {
        private Transform AITransform;

        public void SetObjectTransform(Transform newTransform)
        {
            AITransform = newTransform;
        }
    }

}
