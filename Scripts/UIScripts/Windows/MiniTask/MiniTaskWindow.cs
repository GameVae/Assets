using DB;
using EnumCollect;
using Generic.Singleton;

using DataTable;
using DataTable.Row;
using System.Collections.Generic;
using System.Reflection;
using UI.Widget;
using UnityEngine.UI;

namespace UI
{
    public class MiniTaskWindow : BaseWindow
    {
        public VerticalLayoutGroup VerticalGrid;

        public GUITextWithIcon DontHasUpgrade;
        public GUIOnOffSwitch OpenTaskBtn;

        public MiniTaskWDOElement Prefab;

        private BaseInfoRow baseInfo;
        private List<MiniTaskWDOElement> pool;
        private DBReference dbRef;
        private FieldReflection fieldReflection;
        private void Awake()
        {
            OpenTaskBtn.On += On;
            OpenTaskBtn.Off += Off;
        }

        protected override void Start()
        {
            base.Start();
            dbRef = Singleton.Instance<DBReference>();
            fieldReflection = Singleton.Instance<FieldReflection>();
            OpenTaskBtn.IsOn = false;
        }

        public override void Close()
        {
            DontHasUpgrade.gameObject.SetActive(false);
            RefeshElementPool();
            base.Close();
        }

        public override void Open()
        {
            base.Open();
            Load();
        }

        private void On(GUIOnOffSwitch onOff)
        {
            Open();
            //onOff.Placeholder.text = "Off";
        }

        private void Off(GUIOnOffSwitch onOff)
        {
            Close();
            //onOff.Placeholder.text = "On";
        }

        public override void Load(params object[] input)
        {
            bool isHas = false;
            isHas = ValidateTrainingTask(GetElement()) || isHas;
            isHas = ValidateUpgradeTask(GetElement()) || isHas;
            isHas = ValidateResearchTask(GetElement()) || isHas;
            if (!isHas)
            {
                // Don't has any task
                DontHasUpgrade.gameObject.SetActive(true);
            }
        }

        protected override void Init()
        {
            baseInfo = SyncData.CurrentMainBase;
        }

        private MiniTaskWDOElement GetElement()
        {
            if (pool != null)
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    if (!pool[i].gameObject.activeInHierarchy)
                        return pool[i];
                }
            }
            else
            {
                pool = new List<MiniTaskWDOElement>();
            }
            MiniTaskWDOElement newElement = Instantiate(Prefab, VerticalGrid.transform);
            pool.Add(newElement);
            return newElement;
        }

        private void RefeshElementPool()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].gameObject.activeInHierarchy)
                    pool[i].gameObject.SetActive(false);
            }
        }

        #region Training task
        private bool ValidateTrainingTask(MiniTaskWDOElement taskElement)
        {
            ListUpgrade type = baseInfo.TrainingUnit_ID;
            if (type.IsDefined())
            {
                taskElement.SetGetTimeFunc
                    (
                    delegate { return GetTrainingTimeString(taskElement); },
                    GetTraingMaxTime()
                    );
                taskElement.SetSliderValue(GetTrainingTime);
                taskElement.SetTitleFunc(delegate { return "Training " + type.ToString().InsertSpace() + "..."; });
                taskElement.gameObject.SetActive(true);
                return true;
            }
            return false;
        }

        private string GetTrainingTimeString(MiniTaskWDOElement taskElement)
        {
            int time = (int)baseInfo.TrainingTime;
            if (time <= 0) taskElement.gameObject.SetActive(false);
            return "Remain " + System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
        }

        private int GetTrainingTime()
        {
            return GetTraingMaxTime() - (int)baseInfo.TrainingTime;
        }

        private int GetTraingMaxTime()
        {
            ListUpgrade type = baseInfo.TrainingUnit_ID;
            if (type.IsDefined())
            {
                int level = SyncData.CurrentBaseUpgrade[type].Level;
                ITableData data = dbRef[type][level - 1];
                int trainingTime = fieldReflection.GetFieldValue<int>(data, "TrainingTime", BindingFlags.Public | BindingFlags.Instance);
                return trainingTime * baseInfo.TrainingQuality;
            }
            return 0;
        }
        #endregion

        #region Upgrade task
        private bool ValidateUpgradeTask(MiniTaskWDOElement taskElement)
        {
            ListUpgrade type = baseInfo.UpgradeWait_ID;
            if (type.IsDefined())
            {
                taskElement.SetGetTimeFunc
                    (
                    delegate { return GetUpgradeTimeString(taskElement); },
                    GetUpgradeMaxTime()
                    );
                taskElement.SetTitleFunc(delegate { return "Upgrading " + type.ToString().InsertSpace() + "..."; });
                taskElement.SetSliderValue(GetUpgradeTime);
                taskElement.gameObject.SetActive(true);
                return true;
            }
            return false;
        }

        private string GetUpgradeTimeString(MiniTaskWDOElement taskElement)
        {
            int time = (int)baseInfo.UpgradeTime;
            if (time <= 0) taskElement.gameObject.SetActive(false);
            return "Remain " + System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
        }

        private int GetUpgradeTime()
        {
            return GetUpgradeMaxTime() - (int)baseInfo.UpgradeTime;
        }

        private int GetUpgradeMaxTime()
        {
            ListUpgrade type = baseInfo.UpgradeWait_ID;
            if (type.IsDefined())
            {
                int level = SyncData.CurrentBaseUpgrade[type].Level;
                ITableData data = dbRef[type][level - 1];
                int upgradeTime = fieldReflection.GetPublicField<int>(data, "TimeInt");
                return upgradeTime;
            }
            return 0;
        }
        #endregion

        #region Research task
        private bool ValidateResearchTask(MiniTaskWDOElement taskElement)
        {
            ListUpgrade type = baseInfo.ResearchWait_ID;
            if (type.IsDefined())
            {
                taskElement.SetGetTimeFunc
                    (
                    delegate { return GetResearchTimeString(taskElement); },
                    GetResearchMaxTime()
                    );
                taskElement.SetTitleFunc(delegate { return "Researching " + type.ToString().InsertSpace() + "..."; });
                taskElement.SetSliderValue(GetResearchTime);
                taskElement.gameObject.SetActive(true);
                return true;
            }
            return false;
        }

        private string GetResearchTimeString(MiniTaskWDOElement taskElement)
        {
            int time = (int)baseInfo.ResearchTime;
            if (time <= 0) taskElement.gameObject.SetActive(false);
            return "Remain " + System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
        }

        private int GetResearchTime()
        {
            return GetResearchMaxTime() - (int)baseInfo.ResearchTime;
        }

        private int GetResearchMaxTime()
        {
            ListUpgrade type = baseInfo.ResearchWait_ID;
            if (type.IsDefined())
            {
                int level = SyncData.CurrentBaseUpgrade[type].Level;
                ITableData data = dbRef[type][level - 1];
                return fieldReflection.GetPublicField<int>(data, "TimeInt");             
            }
            return 0;
        }
        #endregion

        /// //////////////////////
        #region Old
        //private bool AddUpgradeBar(string name)
        //{
        //    ListUpgrade type = SyncData.CurrentMainBase.UpgradeWait_ID;
        //    if (type.IsDefined())
        //    {
        //        RectTransform rect = Instantiate(HasUpgrade.transform as RectTransform);
        //        AFadeInOut fader = rect.GetComponent<AFadeInOut>();
        //        GUISliderWithBtn element = rect.GetComponent<GUISliderWithBtn>();
        //        element.Button.InteractableChange(true);
        //        fader.StartFadingAction += delegate
        //        {
        //            if (fader.LoopCounter % 2 != 0)
        //            {
        //                element.Slider.Placeholder.text = "Upgrading " + type.ToString().InsertSpace();
        //            }
        //            else
        //            {
        //                int time = (int)SyncData.CurrentMainBase.UpgradeTime;
        //                element.Slider.Placeholder.text = System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
        //                if (time <= 0)
        //                {
        //                    verticalGrid.Remove(name);
        //                }
        //            }
        //        };
        //        verticalGrid.Add(name, rect);
        //        return true;
        //    }
        //    return false;
        //}

        //private bool AddResearchBar(string name)
        //{
        //    ListUpgrade type = SyncData.CurrentMainBase.ResearchWait_ID;
        //    if (type.IsDefined())
        //    {
        //        RectTransform rect = Instantiate(HasUpgrade.transform as RectTransform);
        //        AFadeInOut fader = rect.GetComponent<AFadeInOut>();
        //        GUISliderWithBtn element = rect.GetComponent<GUISliderWithBtn>();
        //        fader.StartFadingAction += delegate
        //        {
        //            if (fader.LoopCounter % 2 != 0)
        //            {
        //                element.Slider.Placeholder.text = "Researching " + type.ToString().InsertSpace();
        //            }
        //            else
        //            {
        //                int time = (int)SyncData.CurrentMainBase.ResearchTime;
        //                element.Slider.Placeholder.text = System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
        //                if (time <= 0)
        //                {
        //                    verticalGrid.Remove(name);
        //                }
        //            }
        //        };
        //        verticalGrid.Add(name, rect);
        //        return true;
        //    }
        //    return false;
        //}

        //private bool AddTrainningBar(string name)
        //{
        //    ListUpgrade type = SyncData.CurrentMainBase.TrainingUnit_ID;
        //    if (type.IsDefined())
        //    {
        //        RectTransform rect = Instantiate(HasUpgrade.transform as RectTransform);
        //        AFadeInOut fader = rect.GetComponent<AFadeInOut>();
        //        GUISliderWithBtn element = rect.GetComponent<GUISliderWithBtn>();
        //        fader.StartFadingAction += delegate
        //        {
        //            if (fader.LoopCounter % 2 != 0)
        //            {
        //                element.Slider.Placeholder.text = "Trainning " + type.ToString().InsertSpace();
        //            }
        //            else
        //            {
        //                int time = (int)SyncData.CurrentMainBase.TrainingTime;
        //                element.Slider.Placeholder.text = System.TimeSpan.FromSeconds(time).ToString().Replace(".", "d ");
        //                if (time <= 0)
        //                {
        //                    verticalGrid.Remove(name);
        //                }
        //            }
        //        };
        //        verticalGrid.Add(name, rect);
        //        return true;
        //    }
        //    return false;
        //}

        //protected override void Init()
        //{
        //    curUpgTypes = new List<ListUpgrade>();
        //    HasUpgrade.Slider.Placeholder = HasUpgrade.GetComponentInChildren<GUITextWithIcon>().Placeholder;
        //    HasUpgrade.Placeholder = HasUpgrade.Slider.Placeholder;
        //    //verticalGrid = Window.GetComponent<GUIVerticalGird>();

        //    OpenTaskBtn.On += On;
        //    OpenTaskBtn.Off += Off;
        //    OpenTaskBtn.InteractableChange(true);

        //    AddTrainningBar("Trainning");
        //    AddUpgradeBar("Upgrade");
        //    AddResearchBar("Research");
        //}

        //public override void Load(params object[] input)
        //{
        //    if (!verticalGrid.Containts("Trainning") && SyncData.CurrentMainBase.TrainingUnit_ID.IsDefined())
        //        AddTrainningBar("Trainning");
        //    if (!verticalGrid.Containts("Upgrade") && SyncData.CurrentMainBase.UpgradeWait_ID.IsDefined())
        //        AddTrainningBar("Upgrade");
        //    if (!verticalGrid.Containts("Research") && SyncData.CurrentMainBase.ResearchWait_ID.IsDefined())
        //        AddTrainningBar("Research");
        //}
        #endregion
    }
}