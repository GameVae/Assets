using UnityEngine;
using UnityEngine.UI;

namespace UI.Animation
{
    public delegate void FadeAction(AFadeInOut inOut);
    public class AFadeInOut : MonoBehaviour
    {
        private int direction;
        private bool isFadeOut;
        private bool isAnimating;
        private float fadeTimer;
        private Color tempColor;
        private Color defaultColor;

        public bool IsFadeActionNull
        { get { return StartFadingAction == null; } }

        public int LoopCounter { get; private set; }

        public event FadeAction StartFadingAction;

        public bool IsLoop;
        public float Duration;
        public Graphic TargetG;

        public bool IsFadeOut
        {
            get { return isFadeOut; }
        }

        private void Awake()
        {
            defaultColor = TargetG.color;
            Action();
        }

        private void Update()
        {
            if (isAnimating)
            {
                if (isFadeOut)
                {
                    if (fadeTimer <= 0)
                    {
                        FadeIn();
                    }
                }
                else if (fadeTimer >= Duration)
                {
                    if (IsLoop)
                    {
                        Action();
                    }
                    else isAnimating = false;
                }
                fadeTimer += direction * Time.deltaTime;

                tempColor.a = (fadeTimer / Duration);
                TargetG.color = tempColor;
            }
        }

        private void FadeIn()
        {
            isFadeOut = false;
            direction = 1;
        }

        private void FadeOut()
        {
            isFadeOut = true;
            direction = -1;
        }

        public void Action()
        {
            isAnimating = true;
            fadeTimer = Duration;
            FadeOut();
            LoopCounter++;
            StartFadingAction?.Invoke(this);
        }



    }
}