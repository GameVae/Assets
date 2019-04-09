using UnityEngine;

namespace Animation
{
    public class Winker : MonoBehaviour
    {
        private float counter;
        private bool isActive;

        public GameObject TargetObject;
        public float Duration;

        private void Update()
        {
            if (isActive)
            {
                counter += Time.deltaTime;
                if (Duration <= counter)
                {
                    counter -= Duration;
                    TargetObject.SetActive(!TargetObject.activeInHierarchy);
                }
            }
        }

        public void SetActive(bool value)
        {
            isActive = value;
            if (!value)
                TargetObject.SetActive(false);
        }
    }
}