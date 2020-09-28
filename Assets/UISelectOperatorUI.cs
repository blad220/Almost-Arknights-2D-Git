using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectOperatorUI : MonoBehaviour
{
    public OperatorData operatorData;

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
    public Slider HP;
    public Text Attack;
    public Text Defense;
    public Text RES;

    [Space(10)]
    //public Text RedeployTyme;
    //public Text DpCost;
    public Text Block;
    public Text BlockBig;
    //public Text ASPD;

    public Image SkillImage;
    public Text SkillName;
    public Text SkillDescription;

    // Start is called before the first frame update
    void Start()
    {
        Name.text = operatorData.Name;
        Level.text = operatorData.Level.ToString();
        
        if(operatorData.OperatorType == OperatorData.OperatorTypeEnum.Caster) operatorTypeImage.sprite = operatorTypeSprite.Caster;
        if(operatorData.OperatorType == OperatorData.OperatorTypeEnum.Guard) operatorTypeImage.sprite = operatorTypeSprite.Guard;
        if(operatorData.OperatorType == OperatorData.OperatorTypeEnum.Medic) operatorTypeImage.sprite = operatorTypeSprite.Medic;
        if(operatorData.OperatorType == OperatorData.OperatorTypeEnum.Sniper) operatorTypeImage.sprite = operatorTypeSprite.Sniper;
        if(operatorData.OperatorType == OperatorData.OperatorTypeEnum.Specialist) operatorTypeImage.sprite = operatorTypeSprite.Specialist;
        if(operatorData.OperatorType == OperatorData.OperatorTypeEnum.Supporter) operatorTypeImage.sprite = operatorTypeSprite.Supporter;
        if(operatorData.OperatorType == OperatorData.OperatorTypeEnum.Defender) operatorTypeImage.sprite = operatorTypeSprite.Defender;
        if(operatorData.OperatorType == OperatorData.OperatorTypeEnum.Vanguard) operatorTypeImage.sprite = operatorTypeSprite.Vanguard;

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

        Attack.text = operatorData.Attack.ToString();
        Defense.text = operatorData.Defense.ToString();
        RES.text = operatorData.RES.ToString();

        Block.text = operatorData.Block.ToString();
        BlockBig.text = $"Blocks {operatorData.Block} enemies";

        SkillImage.sprite = operatorData.skills[(int)operatorData.activeSkill].skillImage;
        SkillName.text = operatorData.skills[(int)operatorData.activeSkill].name;
        SkillDescription.text = operatorData.skills[(int)operatorData.activeSkill].description;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
