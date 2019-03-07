
namespace CustomAttr
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class UpgradeAttribute : System.Attribute
    {
        public UpgradeAttribute() { }
    }

}