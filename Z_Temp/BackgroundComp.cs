#if UNITY_EDITOR
using UI.Composites;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace UI.Composites
{
    public class BackgroundComp : GUIComposite
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
            set
            {
                if (BackgroundImg != null)
                    BackgroundImg.sprite = value;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            FindBackgroundImg();
        }

        private void FindBackgroundImg()
        {
            bgImg = FindTypeWithCustomMask<Image>(UI.CustomLayerMask.CustomMask.Background);
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(BackgroundComp))]
public class EditorBackground : EditorUIComposite
{
    private BackgroundComp owner;
    private Sprite bgSprite;

    protected override void OnEnable()
    {
        base.OnEnable();
        owner = target as BackgroundComp;
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