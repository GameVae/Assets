using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Animation
{
    public class TextTransform : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDisplay;
        [SerializeField] private List<string> sentences;
        [SerializeField] private float delayTime;

        private bool isPlaying;
        private float timmer;

        private int index;
        private int totalSenetence;

        private void Awake()
        {
            isPlaying = true;
            index = 0;
            totalSenetence = (int)sentences?.Count;
            textDisplay.text = totalSenetence > 0 ? sentences[0] : null;
        }

        private void Update()
        {
            if (isPlaying && totalSenetence > 0)
            {
                timmer += Time.deltaTime;
                if (timmer >= delayTime)
                {
                    index = (index + 1) % totalSenetence;
                    textDisplay.text = sentences[index];
                    timmer = 0;
                }
            }
        }
    }
}