using UnityEngine;

public class StandaloneKeyboard : Keyboard
{
    protected override void HandleInput()
    {
        HandleStandaloneInput(Input.inputString);
    }

    private void HandleStandaloneInput(string frameInput)
    {
        frameInput = contentValidate?.CheckContent(frameInput);
        if (frameInput != null)
        {
            char[] chars = frameInput.ToCharArray();
            string temp = InputString;

            bool isChanged = chars.Length > 0;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '\b')
                {
                    if (temp != null && temp.Length > 0)
                        temp = temp.Substring(0, temp.Length - 1);
                }
                else if (chars[i] != '\n' && chars[i] != '\r') temp += chars[i];
                else
                {
                    // press Enter
                    Close();
                }
            }
            if (isChanged)
            {
                InputString = temp;
            }
        }
    }
}
