#if UNITY_EDITOR
using UnityEditor;
using UI.Composites;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace UI.Composites
{
    [RequireComponent(typeof(Mask), typeof(Image))]
    public class MaskableComp : GUIComposite
    {
        [SerializeField, HideInInspector] private Mask mask;
        [SerializeField, HideInInspector] private Image maskImg;

        public Mask Mask
        {
            get { return mask ?? (mask = GetComponent<Mask>()); }
        }

        public Image MaskImg
        {
            get
            {
                return maskImg ?? (maskImg = GetComponent<Image>());
            }
        }

        public Sprite Sprite
        {
            get { return MaskImg?.sprite; }
            set
            {
                if (MaskImg)
                    MaskImg.sprite = value;
            }
        }

        public bool ShowTargetGraphic
        {
            get
            {
                if (Mask) return Mask.showMaskGraphic;
                else return false;
            }
            set
            {
                if (Mask)
                    Mask.showMaskGraphic = value;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            FindComponent();
        }

        private void FindComponent()
        {
            maskImg = GetComponent<Image>();
            mask = GetComponent<Mask>();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MaskableComp),false)]
public class EditorMaskable : EditorComposite
{
    private MaskableComp owner;

    private Sprite sprite;
    private bool showTargetGraphic;

    protected override void OnEnable()
    {
        base.OnEnable();
        owner = target as MaskableComp;

        sprite = owner.Sprite;
        showTargetGraphic = owner.ShowTargetGraphic;
    }
    protected override void Draw()
    {
        sprite = EditorGUILayout.ObjectField("Mask Sprite:", sprite, typeof(Sprite), true) as Sprite;
        showTargetGraphic = EditorGUILayout.Toggle("Show Mask Graphic", owner.ShowTargetGraphic);
        bool isChanged = false;
        if (sprite != owner.Sprite)
        {
            owner.Sprite = sprite;
            isChanged = true;
        }

        if (showTargetGraphic != owner.ShowTargetGraphic)
        {
            owner.ShowTargetGraphic = showTargetGraphic;
            isChanged = true;
        }
        if (isChanged)
            EditorUtility.SetDirty(owner);
    }
}
#endif