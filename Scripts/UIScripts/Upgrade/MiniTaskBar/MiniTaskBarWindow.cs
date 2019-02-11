using DB;
using EnumCollect;
using ManualTable;
using ManualTable.Row;
using System.Collections.Generic;
using System.Linq;
using UI.Animation;
using UI.Widget;
using UnityEngine;

namespace UI
{
    public class MiniTaskBarWindow : BaseWindow, IWindow
    {
        private GUIVerticalGird verticalGrid;

        public GUISliderWithBtn HasUpgrade;
        public GUITextWithIcon DontHasUpgrade;
        public GUIOnOffSwitch OpenTaskBtn;

        private List<ListUpgrade> curUpgTypes;

        protected override void Start()
        {
            OpenTaskBtn.InteractableChange(true);
            OpenTaskBtn.On += On;
            OpenTaskBtn.Off += Off;
        }
        public override void Close()
        {
            DontHasUpgrade.gameObject.SetActive(false);
            base.Close();
        }

        public override void Open()
        {
            Debug.Log("open");
            base.Open();
            Load();
        }

        private void On(GUIOnOffSwitch onOff)
        {
            Open();
            onOff.Placeholder.text = "Off";
        }

        private void Off(GUIOnOffSwitch onOff)
        {
            Close();
            onOff.Placeholder.text = "On";
        }

        protected override void Init()
        {
            curUpgTypes = new List<ListUpgrade>();
            HasUpgrade.Slider.Placeholder = HasUpgrade.GetComponentInChildren<GUITextWithIcon>().Placeholder;
            HasUpgrade.Placeholder = HasUpgrade.Slider.Placeholder;
            verticalGrid = Window.GetComponent<GUIVerticalGird>();

            OpenTaskBtn.SetIsOn(true);
            OpenTaskBtn.On += On;
            OpenTaskBtn.Off += Off;
            OpenTaskBtn.InteractableChange(true);

            AddTrainningBar("Trainning");
            AddUpgradeBar("Upgrade");
            AddResearchBar("Research");
        }

        public override void Load(params object[] input)
        {
            if (!verticalGrid.Containts("Trainning") && SyncData.CurrentMainBase.TrainingUnit_ID.IsDefined())
                AddTrainningBar("Trainning");
            if (!verticalGrid.Containts("Upgrade") && SyncData.CurrentMainBase.UpgradeWait_ID.IsDefined())
                AddTrainningBar("Upgrade");
            if (!verticalGrid.Containts("Research") && SyncData.CurrentMainBase.ResearchWait_ID.IsDefined())
                AddTrainningBar("Research");
        }

        private bool AddUpgradeBar(string name)
        {
            ListUpgrade type = SyncData.CurrentMainBase.UpgradeWait_ID;
            if (type.IsDefined())
            {
                RectTransform rect = Instantiate(HasUpgrade.transform as RectTransform);
                AFadeInOut fader = rect.GetComponent<AFadeInOut>();
                GUISliderWithBtn element = rect.GetComponent<GUISliderWithBtn>();
                element.Button.InteractableChange(true);
                fader.StartFadingAction += delegate
                {
                    if (fader.LoopCounter % 2 != 0)
                    {
                        element.Slider.Placeholder.text = "Upgrading " + type.ToString().InsertSpace();
                    }
                    else
                    {
                        int time = (int)SyncData.CurrentMainBase.UpgradeTime;
                        element.Slider.Placeholder.text = System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
                        if (time <= 0)
                        {
                            verticalGrid.Remove(name );
                        }
                    }
                };
                verticalGrid.Add(name , rect);
                return true;
            }
            return false;
        }

        private bool AddResearchBar(string name)
        {
            ListUpgrade type = SyncData.CurrentMainBase.ResearchWait_ID;
            if (type.IsDefined())
            {
                RectTransform rect = Instantiate(HasUpgrade.transform as RectTransform);
                AFadeInOut fader = rect.GetComponent<AFadeInOut>();
                GUISliderWithBtn element = rect.GetComponent<GUISliderWithBtn>();
                fader.StartFadingAction += delegate
                {
                    if (fader.LoopCounter % 2 != 0)
                    {
                        element.Slider.Placeholder.text = "Researching " + type.ToString().InsertSpace();
                    }
                    else
                    {
                        int time = (int)SyncData.CurrentMainBase.ResearchTime;
                        element.Slider.Placeholder.text = System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
                        if (time <= 0)
                        {
                            verticalGrid.Remove(name );
                        }
                    }
                };
                verticalGrid.Add(name , rect);
                return true;
            }
            return false;
        }

        private bool AddTrainningBar(string name)
        {
            ListUpgrade type = SyncData.CurrentMainBase.TrainingUnit_ID;
            if (type.IsDefined())
            {
                RectTransform rect = Instantiate(HasUpgrade.transform as RectTransform);
                AFadeInOut fader = rect.GetComponent<AFadeInOut>();
                GUISliderWithBtn element = rect.GetComponent<GUISliderWithBtn>();
                fader.StartFadingAction += delegate
                {
                    if (fader.LoopCounter % 2 != 0)
                    {
                        element.Slider.Placeholder.text = "Trainning " + type.ToString().InsertSpace();
                    }
                    else
                    {
                        int time = (int)SyncData.CurrentMainBase.TrainingTime;
                        element.Slider.Placeholder.text = System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
                        if (time <= 0)
                        {
                            verticalGrid.Remove(name );
                        }
                    }
                };
                verticalGrid.Add(name , rect);
                return true;
            }
            return false;
        }
    }
}