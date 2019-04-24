using Generic.Singleton;
using static CustomInputField;
using static CustomInputField.ContentValidate;

public class ContentValidateFactory : ISingleton
{
    private ContentValidateFactory() { }
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
