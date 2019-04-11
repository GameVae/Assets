#if UNITY_EDITOR
using UI.Composites;
using UnityEditor;
#endif

using TMPro;
using UnityEngine;
namespace UI.Composites
{
    public class PlaceholderComp : GUIComposite
    {
        [SerializeField, HideInInspector] private TextMeshProUGUI placeholder;
        [SerializeField] private SingleColorObject textColor;

        public TextMeshProUGUI Placeholder
        {
            get
            {
                return placeholder ?? (placeholder = FindTypeWithCustomMask<TextMeshProUGUI>(CustomLayerMask.CustomMask.Placeholder));
            }
        }

        public float FontSize
        {
            get
            {
                if (Placeholder) return Placeholder.fontSize;
                else return 0;
            }
            set
            {
                if (Placeholder)
                    Placeholder.fontSize = value;
            }
        }

        public string Text
        {
            get { return Placeholder?.text; }
            set { if (Placeholder) Placeholder.text = value; }
        }

        private void Awake()
        {
            Placeholder.raycastTarget = false;
        }

        public override bool ConfirmOffset()
        {     
            
            if(Placeholder != null && 
                Placeholder.color != textColor.Color)
            {
                Placeholder.color = textColor.Color;
                return true;
            }
            return base.ConfirmOffset(); ;
        }

        public override void Refresh()
        {
            base.Refresh();
            FindPlaceholder();
        }

        private void FindPlaceholder()
        {
            placeholder = FindTypeWithCustomMask<TextMeshProUGUI>(CustomLayerMask.CustomMask.Placeholder);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlaceholderComp))]
public class EditorPlaceholder : EditorComposite
{
    private PlaceholderComp owner;
    private bool isChanged;

    private string text;
    private bool autoSize;
    private float fontSize;

    protected override void OnEnable()
    {
        base.OnEnable();
        owner = target as PlaceholderComp;

        text = owner.Text;
        fontSize = owner.FontSize;
    }

    protected override void Draw()
    {
        isChanged = false;

        DrawCustomPlaceholder(ref isChanged);
        DrawCustomFontSize(ref isChanged);

        if (isChanged)
            EditorUtility.SetDirty(owner.Placeholder);
    }

    private void DrawCustomFontSize(ref bool isChanged)
    {
        using (new EditorGUI.IndentLevelScope())
        {
            fontSize = EditorGUILayout.DelayedFloatField("Font size:", fontSize);
            if (!Mathf.Approximately(fontSize, owner.FontSize))
            {
                owner.FontSize = fontSize;
                isChanged = true;
            }
        }
    }

    private void DrawCustomPlaceholder(ref bool isChanged)
    {
        text = EditorGUILayout.DelayedTextField("Placeholder:", text);
        if (text != owner.Text)
        {
            owner.Text = text;
            isChanged = true;
        }
    }
}
#endif