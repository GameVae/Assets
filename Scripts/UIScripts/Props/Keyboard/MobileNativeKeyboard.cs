using UnityEngine;

namespace UI.Keyboard
{
    public class MobileNativeKeyboard : Keyboard
    {
        private TouchScreenKeyboard touchKeyboard;

        protected override void HandleInput()
        {
            if (touchKeyboard != null)
            {
                if (touchKeyboard.status != TouchScreenKeyboard.Status.Visible)
                {
                    Close();
                }
                else
                {
                    if (touchKeyboard.text.CompareTo(InputString) != 0)
                    {
                        InputString = contentValidate?.CheckContent(touchKeyboard.text);
                        touchKeyboard.text = InputString;
                    }
                }
            }
        }

        protected override void Active(bool value, CustomInputField inputField)
        {
            base.Active(value, inputField);
            if (value)
            {
                touchKeyboard = TouchScreenKeyboard.Open(InputString);
            }
            else
            {
                touchKeyboard = null;
            }
        }

        public override void SetInputString(string str)
        {
            InputString = contentValidate?.CheckContent(str);
            touchKeyboard.text = InputString;
        }
    }
}