using UnityEngine;
using static CustomInputField;

namespace UI.Keyboard
{
    public abstract class Keyboard : MonoBehaviour
    {
        public enum KeyboardType
        {
            MobileNative,
            Standalone,
            Numpad,
        }

        protected abstract void HandleInput();
        public abstract void SetInputString(string str);

        private bool isActive;
        private string inputString;
        protected ContentValidate contentValidate;

        public string InputString
        {
            get
            {
                return inputString;
            }
            protected set
            {
                inputString = value;
                InputField?.SetContent(inputString);
            }
        }

        public bool IsActive { get { return isActive; } }

        public CustomInputField InputField { get; private set; }

        private void UnAcive()
        {
            InputField?.Active(false);
            InputField = null;

            inputString = null;
            contentValidate = null;
        }

        private void Active(CustomInputField inputField)
        {
            InputField?.Active(false); // last input field

            InputField = inputField;
            InputField?.Active(true); // current input field

            inputString = InputField?.Text;
            contentValidate = InputField?.Validator;
        }

        protected void Update()
        {
            if (isActive)
            {
                HandleInput();
            }
        }

        protected virtual void Active(bool value, CustomInputField inputField)
        {
            isActive = value;

            if (isActive)
                Active(inputField);
            else
                UnAcive();
        }

        public void Close()
        {
            Active(false, null);
        }

        public void Open(CustomInputField inputField)
        {
            Active(true, inputField);
        }

    }
}