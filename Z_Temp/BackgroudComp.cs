#if UNITY_EDITOR
using UI.Composites;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace UI.Composites
{
    public class BackgroudComp : GUIComposite
    {
        [SerializeField, HideInInspector] private Image bgImg;

        public Image BackgroundImg
        {
            get
            {
                return bgImg ?? (bgImg = FindTypeWithCustomMask<Image>(UI.CustomLayerMask.CustomMask.Background));
            }
        }

        public Sprite Sprite
        {
            get { return BackgroundImg?.sprite; }
            set { BackgroundImg.sprite = value; }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(BackgroudComp), true)]
public class EditorBackground : EditorUIComposite
{
    private BackgroudComp owner;
    private Sprite bgSprite;

    protected override void OnEnable()
    {
        base.OnEnable();
        owner = target as BackgroudComp;
        bgSprite = owner.Sprite;

    }

    protected override void Draw()
    {
        bgSprite = EditorGUILayout.ObjectField("Background Sprite:", owner.Sprite, typeof(Sprite), false) as Sprite;
        if (bgSprite != owner.Sprite)
        {
            owner.Sprite = bgSprite;
            EditorUtility.SetDirty(owner.Sprite);
        }
    }
}
#endif