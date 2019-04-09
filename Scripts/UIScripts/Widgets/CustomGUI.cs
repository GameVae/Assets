using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public abstract class CustomGUI : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected TextMeshProUGUI placeholder;
        [SerializeField, HideInInspector] protected Mask mask;
        [SerializeField, HideInInspector] protected Image maskImage;
        [SerializeField, HideInInspector] protected bool maskable;
        [SerializeField, HideInInspector] protected bool interactable;
        [SerializeField, HideInInspector] protected bool isPlaceholder;

        [SerializeField, HideInInspector] protected Image backgroundImg;
        [SerializeField, HideInInspector] protected bool isBackground;

        [SerializeField, HideInInspector] protected bool isUIDependent;

        #region Placeholder
        public float FontSize
        {
            get { return Placeholder ? Placeholder.fontSize : 0; }
            protected set { if (Placeholder) Placeholder.fontSize = value; }
        }

        public bool IsPlaceholder
        {
            get { return isPlaceholder; }
            protected set { isPlaceholder = value; }
        }

        public Color PlaceholderColor
        {
            get
            {
                if (Placeholder) return Placeholder.color;
                return default(Color);
            }
            protected set { if (Placeholder) Placeholder.color = value; }
        }

        public virtual TextMeshProUGUI Placeholder
        {
            get { return placeholder ?? (placeholder = GetComponentInChildren<TextMeshProUGUI>()); }
            set { placeholder = value; }
        }

        public void PlaceholderValueChange(string text)
        {
            if (Placeholder != null)
                Placeholder.text = text;
        }

        public void FontSizeChange(float fontSize)
        {
            FontSize = fontSize;
        }

        public void PlaceholderColorChange(Color newColor)
        {
            PlaceholderColor = newColor;
        }

        public void IsPlaceholderChange(bool value)
        {
            IsPlaceholder = value;
            if (Placeholder != null)
                Placeholder.enabled = value;
        }

        #endregion

        public bool UIDependent
        {
            get { return isUIDependent; }
            set { isUIDependent = value; }
        }

        #region Maskable

        public Mask Mask
        {
            get
            {
                if (mask == null) mask = GetComponent<Mask>();
                return mask;
            }
            protected set { mask = value; }
        }

        public Image MaskImage
        {
            get { return maskImage ?? (maskImage = GetComponent<Image>()); }
            protected set { maskImage = value; }
        }

        public bool Maskable
        {
            get { return maskable; }
            protected set { maskable = value; }
        }

        public virtual Sprite MaskSprite
        {
            get { return MaskImage?.sprite; }
            protected set
            {
                if (MaskImage != null)
                    MaskImage.sprite = value;
            }
        }

        public void MaskableChange(bool value)
        {
            Maskable = value;
            if (Maskable)
            {
                if (Mask == null)
                    Mask = gameObject.AddComponent<Mask>();
                else Mask.enabled = true;
            }
            else
            {
                if (Mask != null)
                    Mask.enabled = false;
            }
        }

        public void MaskSpriteChange(Sprite value)
        {
            MaskSprite = value;
        }

        #endregion

        #region Backgroud
        public virtual bool IsBackground
        {
            get { return isBackground; }
            protected set { isBackground = value; }
        }

        public Sprite BackgroudSprite
        {
            get
            {
                return BackgroundImg?.sprite;
            }
            protected set
            {
                if (BackgroundImg)
                    BackgroundImg.sprite = value;
            }
        }

        public virtual Image BackgroundImg
        {
            get
            {
                if (backgroundImg == null)
                {
                    return backgroundImg = FindTypeWithCustomMask<Image>(CustomLayerMask.CustomMask.Background);
                }
                else return backgroundImg;
            }
            protected set { backgroundImg = value; }
        }

        public void BackgroundChange(Sprite sprite)
        {
            BackgroudSprite = sprite;
        }

        public void IsBackgroudChange(bool value)
        {
            IsBackground = value;
            if (BackgroundImg) BackgroundImg.enabled = value;
        }
        #endregion

        public virtual bool Interactable
        {
            get { return interactable; }
            protected set { interactable = value; }
        }

        public virtual void InteractableChange(bool value)
        {
            Interactable = value;
        }

        public virtual void SetChildrenDependence() { }

        public T FindTypeWithCustomMask<T>(CustomLayerMask.CustomMask maskType)
            where T : Component
        {
            CustomLayerMask[] marks = GetComponentsInChildren<CustomLayerMask>();
            List<CustomLayerMask> sameType = new List<CustomLayerMask>();
            int length = marks.Length;

            for (int i = 0; i < length; i++)
            {
                if (marks[i].Mask == maskType)
                    sameType.Add(marks[i]);
            }

            length = sameType.Count;
            CustomLayerMask r;
            r = length > 0 ? sameType[0] : null;

            for (int i = 1; i < length; i++)
            {
                if (r.SameTypePiority < marks[i].SameTypePiority)
                    r = marks[i];
            }
            return r?.GetComponent<T>();
        }

    }
}
