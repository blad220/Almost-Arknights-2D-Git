using MainController.Operator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainController.Ui
{

    public class UISelectOperatorUI : MonoBehaviour
    {
        public OperatorController operatorController;
        public OperatorData operatorDataSelect;

        public GameObject displaySelectedPanel;
        public KillButton killButton;

        [Space(10)]
        public Text Name;

        [Space(10)]
        public Text Level;
        public Image OperatorElitImage;
        public operatorElitImages OperatorElitImages;
        [System.Serializable]
        public class operatorElitImages
        {
            public Sprite Elite_0;
            public Sprite Elite_1;
            public Sprite Elite_2;
        }

        [Space(10)]
        public Image operatorTypeImage;
        public TypeOperatorSprite operatorTypeSprite;
        [System.Serializable]
        public class TypeOperatorSprite
        {
            public Sprite Caster;
            public Sprite Guard;
            public Sprite Medic;
            public Sprite Sniper;
            public Sprite Specialist;
            public Sprite Supporter;
            public Sprite Defender;
            public Sprite Vanguard;
        }
        [Space(10)]
        public Image artBig;

        [Space(10)]
        public UIOperatorHp HPComponent;
        [HideInInspector] public Slider HP;
        [HideInInspector] public Text HPText;
        public Text Attack;
        public Text Defense;
        public Text RES;

        [Space(10)]
        public Text Block;
        public Text BlockBig;

        public Image SkillImage;
        public Text SkillName;
        public Text SkillDescription;
        public Slider SkillSlider;

        void Awake()
        {
            UISelectOperatorUI link = MainController.mainInterfaceFields.selectOperatorUI;
            if (link == null) MainController.mainInterfaceFields.selectOperatorUI = gameObject.GetComponent<UISelectOperatorUI>();
        }

        void Start()
        {
            HP = HPComponent.gameObject.GetComponent<Slider>();
            HPText = HPComponent.textField;

            displaySelectedPanel.SetActive(false);
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

                if (hit)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;

                    if (hitInfo.transform.gameObject.TryGetComponent<OperatorController>(out OperatorController _operatorController))
                    {
                        if (operatorController != null)
                        {
                            operatorController.HideRange();
                        }
                        operatorController = _operatorController;
                        UpdateSelectInfo(operatorController._operatorData);
                        displaySelectedPanel.SetActive(true);
                        operatorController.ShowRange();
                    }
                    else
                    {
                        displaySelectedPanel.SetActive(false);

                        if (operatorController != null)
                        {
                            operatorController.HideRange();
                        }
                    }
                }

            }
        }

        public void UpdateSelectInfoOnlyHP()
        {
            if (operatorDataSelect != null)
            {
                UpdateSelectInfoOnlyHP(operatorDataSelect);
            }
        }
        public void UpdateSelectInfoOnlyHP(OperatorData operatorData)
        {
            HP.maxValue = operatorData.maxHP;
            HP.value = operatorData.curHP;
            HPText.text = $"{operatorData.curHP}/{operatorData.maxHP}";
        }
        public void updateSelectInfoOnlySkill()
        {
            if (operatorDataSelect != null)
            {
                UpdateSelectInfoOnlySkill(operatorDataSelect);
            }
        }
        public void UpdateSelectInfoOnlySkill(OperatorData operatorData)
        {
            if (operatorData.skill.isActive)
            {
                SkillSlider.maxValue = operatorData.skill.duration;
                SkillSlider.value = operatorData.skill.curDurationTime;
            }
            else
            {
                SkillSlider.maxValue = operatorData.skill.cost;
                SkillSlider.value = operatorData.skill.curSkillPoint;
            }
        }
        public void updateSelectInfo()
        {
            if (operatorDataSelect != null)
            {
                UpdateSelectInfo(operatorDataSelect);
            }
        }
        public void updateSelectInfo(OperatorData operatorData)
        {
            if (operatorData != null)
            {
                UpdateSelectInfo(operatorData);
            }
        }
        public void UpdateSelectInfo(OperatorData operatorData)
        {
            operatorDataSelect = operatorData;
            Name.text = operatorData.Name;
            Level.text = operatorData.Level.ToString();

            if (operatorData.OperatorType == OperatorData.OperatorTypeEnum.Caster) operatorTypeImage.sprite = operatorTypeSprite.Caster;
            if (operatorData.OperatorType == OperatorData.OperatorTypeEnum.Guard) operatorTypeImage.sprite = operatorTypeSprite.Guard;
            if (operatorData.OperatorType == OperatorData.OperatorTypeEnum.Medic) operatorTypeImage.sprite = operatorTypeSprite.Medic;
            if (operatorData.OperatorType == OperatorData.OperatorTypeEnum.Sniper) operatorTypeImage.sprite = operatorTypeSprite.Sniper;
            if (operatorData.OperatorType == OperatorData.OperatorTypeEnum.Specialist) operatorTypeImage.sprite = operatorTypeSprite.Specialist;
            if (operatorData.OperatorType == OperatorData.OperatorTypeEnum.Supporter) operatorTypeImage.sprite = operatorTypeSprite.Supporter;
            if (operatorData.OperatorType == OperatorData.OperatorTypeEnum.Defender) operatorTypeImage.sprite = operatorTypeSprite.Defender;
            if (operatorData.OperatorType == OperatorData.OperatorTypeEnum.Vanguard) operatorTypeImage.sprite = operatorTypeSprite.Vanguard;

            if (operatorData.Elite == OperatorData.EliteEnum.Elite_0)
            {
                OperatorElitImage.sprite = OperatorElitImages.Elite_0;
                artBig.sprite = operatorData.artBig_Ellite0;
            }
            if (operatorData.Elite == OperatorData.EliteEnum.Elite_1)
            {
                OperatorElitImage.sprite = OperatorElitImages.Elite_1;
                artBig.sprite = operatorData.artBig_Ellite0;
            }
            if (operatorData.Elite == OperatorData.EliteEnum.Elite_2)
            {
                OperatorElitImage.sprite = OperatorElitImages.Elite_2;
                artBig.sprite = operatorData.artBig_Ellite2;
            }

            HP.maxValue = operatorData.maxHP;
            HP.value = operatorData.curHP;
            HPText.text = $"{operatorData.curHP}/{operatorData.maxHP}";

            Attack.text = operatorData.Attack.ToString();
            Defense.text = operatorData.Defense.ToString();
            RES.text = operatorData.RES.ToString();

            Block.text = operatorData.Block.ToString();
            BlockBig.text = $"Blocks {operatorData.Block} enemies";

            SkillImage.sprite = operatorData.skill.skillImage;
            SkillName.text = operatorData.skill.name;
            SkillDescription.text = operatorData.skill.description;
            if (operatorData.skill.isActive)
            {
                SkillSlider.maxValue = operatorData.skill.duration;
                SkillSlider.value = operatorData.skill.curDurationTime;
            }
            else
            {
                SkillSlider.maxValue = operatorData.skill.cost;
                SkillSlider.value = operatorData.skill.curSkillPoint;
            }
            killButton.killOperatorObject = operatorController;
        }
    }
}