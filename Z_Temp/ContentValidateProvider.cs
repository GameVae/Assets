﻿using Generic.Singleton;
using static InputFieldv2;
using static InputFieldv2.ContentValidate;

public class ContentValidateProvider : ISingleton
{
    private ContentValidateProvider() { }
    public ContentValidate GetValidator(ContentType type)
    {
        switch (type)
        {
            case ContentType.Interger:
                return new IntergerValidate();
            case ContentType.Text:
                return new TextValidate();
            default:
                return null;
        }
    }
}
