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
            if (AvailableInputs[key] || Input.GetKeyDown(key))
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
            foreach (KeyValuePair<KeyCode, bool> input in AvailableInputs)
            {
                AvailableInputs[input.Key] = false;
            }
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
