using UnityEngine;

namespace UI
{
    public sealed class CustomLayerMask : MonoBehaviour
    {
        public enum CustomMask
        {
            Default,
            Background,
            MaskImg,
        }

        public CustomMask Mask;
        public int SameTypePiority;
    }
}
