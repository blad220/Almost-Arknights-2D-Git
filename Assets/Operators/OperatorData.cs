using MainController.Operator.Bullet;
using MainController.Operator.Range;
using MainController.Operator.Skills;
using MainController.Ui;
using UnityEngine;

namespace MainController.Operator
{

    [CreateAssetMenu(fileName = "OperatorName", menuName = "Operators/New operator")]
    public class OperatorData : ScriptableObject
    {
        public string Name;
        public GameObject prefabOfOperator;
        public RangeOfOperator rangeOfOperator;

        [Space(10)]
        public EliteEnum Elite;
        public enum EliteEnum { Elite_0, Elite_1, Elite_2 }
        public int Level;

        [Space(10)]
        public OperatorTypeEnum OperatorType;
        public enum OperatorTypeEnum { Caster, Guard, Medic, Sniper, Specialist, Supporter, Defender, Vanguard }

        public Sprite artBig_Ellite0;
        public Sprite artBig_Ellite2;
        public Sprite artSmall;

        [Space(10)]
        public int curHP;
        public int maxHP;
        public int Attack;
        public bool isHealAttack;
        public int Defense;
        public int RES;

        [Space(10)]
        public int RedeployTyme;
        public int DpCost;
        public int Block;
        public float ASPD;

        [Space(10)]
        public bool isLowGrounded;
        public bool isHighGrounded;

        [Space(10)]
        public ParticleSystem HealParticle;

        [Space(10)]
        public BulletController bulletController;

        [Space(10)]
        public int CurrentEXP;
        public int NextLevelEXP;

        [Space(10)]
        public SkillSriptableObject skill;

        public enum Attribute
        {
            CurrentHP, MaximumHP, RedeployTime, AttackPower, Cost, Defense, Block, MagicResistance, AttackTime
        }

        public void Awake()
        {
            curHP = maxHP;
        }
        public void SkillActivate(OperatorController operatorController)
        {
            skill.SkillActivate(operatorController);
        }
        public void Copy(OperatorData operatorData)
        {
            Name = operatorData.name;
            prefabOfOperator = operatorData.prefabOfOperator;
            rangeOfOperator = operatorData.rangeOfOperator;
            Elite = operatorData.Elite;
            Level = operatorData.Level;
            OperatorType = operatorData.OperatorType;
            artBig_Ellite0 = operatorData.artBig_Ellite0;
            artBig_Ellite2 = operatorData.artBig_Ellite2;
            artSmall = operatorData.artSmall;
            curHP = operatorData.curHP;
            maxHP = operatorData.maxHP;
            Attack = operatorData.Attack;
            Defense = operatorData.Defense;
            RES = operatorData.RES;
            RedeployTyme = operatorData.RedeployTyme;
            DpCost = operatorData.DpCost;
            Block = operatorData.Block;
            ASPD = operatorData.ASPD;
            isLowGrounded = operatorData.isLowGrounded;
            isHighGrounded = operatorData.isHighGrounded;
            HealParticle = operatorData.HealParticle;
            bulletController = operatorData.bulletController;
            CurrentEXP = operatorData.CurrentEXP;
            NextLevelEXP = operatorData.NextLevelEXP;
            skill = operatorData.skill;
        }
        public static OperatorData operator +(OperatorData operatorData, AttributeDefinition attributeDefinition)
        {
            OperatorData result = new OperatorData();

            Attribute attribute = attributeDefinition.attribute;
            int valueInt = attributeDefinition.valueInt;
            float valueFloat = attributeDefinition.valueFloat;

            if (attribute == Attribute.AttackPower) result.Attack += (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.AttackTime) result.ASPD += (valueFloat != 0f) ? valueFloat : valueInt;
            if (attribute == Attribute.Block) result.Block += (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.Cost) result.DpCost += (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.CurrentHP) result.curHP += (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.Defense) result.Defense += (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.MagicResistance) result.RES += (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.MaximumHP) result.maxHP += (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.RedeployTime) result.RedeployTyme += (valueInt != 0) ? valueInt : (int)valueFloat;

            return result;
        }
        public static OperatorData operator -(OperatorData operatorData, AttributeDefinition attributeDefinition)
        {
            OperatorData result = operatorData;

            Attribute attribute = attributeDefinition.attribute;
            int valueInt = attributeDefinition.valueInt;
            float valueFloat = attributeDefinition.valueFloat;

            if (attribute == Attribute.AttackPower) result.Attack -= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.AttackTime) result.ASPD -= (valueFloat != 0f) ? valueFloat : valueInt;
            if (attribute == Attribute.Block) result.Block -= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.Cost) result.DpCost -= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.CurrentHP) result.curHP -= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.Defense) result.Defense -= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.MagicResistance) result.RES += (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.MaximumHP) result.maxHP -= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.RedeployTime) result.RedeployTyme -= (valueInt != 0) ? valueInt : (int)valueFloat;

            return result;
        }
        public static OperatorData operator *(OperatorData operatorData, AttributeDefinition attributeDefinition)
        {
            OperatorData result = operatorData;

            Attribute attribute = attributeDefinition.attribute;
            int valueInt = attributeDefinition.valueInt;
            float valueFloat = attributeDefinition.valueFloat;

            if (attribute == Attribute.AttackPower) result.Attack *= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.AttackTime) result.ASPD *= (valueFloat != 0f) ? valueFloat : valueInt;
            if (attribute == Attribute.Block) result.Block *= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.Cost) result.DpCost *= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.CurrentHP) result.curHP *= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.Defense) result.Defense *= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.MagicResistance) result.RES *= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.MaximumHP) result.maxHP *= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.RedeployTime) result.RedeployTyme *= (valueInt != 0) ? valueInt : (int)valueFloat;

            return result;
        }
        public static OperatorData operator /(OperatorData operatorData, AttributeDefinition attributeDefinition)
        {
            OperatorData result = operatorData;

            Attribute attribute = attributeDefinition.attribute;
            int valueInt = attributeDefinition.valueInt;
            float valueFloat = attributeDefinition.valueFloat;

            if (attribute == Attribute.AttackPower) result.Attack /= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.AttackTime) result.ASPD /= (valueFloat != 0f) ? valueFloat : valueInt;
            if (attribute == Attribute.Block) result.Block /= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.Cost) result.DpCost /= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.CurrentHP) result.curHP /= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.Defense) result.Defense /= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.MagicResistance) result.RES /= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.MaximumHP) result.maxHP /= (valueInt != 0) ? valueInt : (int)valueFloat;
            if (attribute == Attribute.RedeployTime) result.RedeployTyme /= (valueInt != 0) ? valueInt : (int)valueFloat;

            return result;
        }
        public OperatorData ModificatorPlus(AttributeDefinition attributeDefinition)
        {
            OperatorData result = new OperatorData();

            ModificatorPlus(attributeDefinition.attribute, attributeDefinition.valueFloat, attributeDefinition.valueInt);

            return result;
        }
        public OperatorData ModificatorPlus(AttributeDefinition[] attributeDefinitions)
        {
            OperatorData result = new OperatorData();

            foreach (AttributeDefinition attributeDefinition in attributeDefinitions)
            {
                ModificatorPlus(attributeDefinition.attribute, attributeDefinition.valueFloat, attributeDefinition.valueInt);
            }

            return result;
        }
        public OperatorData ModificatorPlus(Attribute attribute, float valueFloat = 0f, int valueInt = 0)
        {
            OperatorData result = this;

            if (attribute == Attribute.AttackPower) result.Attack = (int)(result.Attack * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.AttackTime) result.ASPD = (result.ASPD + ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.Block) result.Block = (int)(result.Block + ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.Cost) result.DpCost = (int)(result.DpCost + ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.CurrentHP) result.curHP = (int)(result.curHP + ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.Defense) result.Defense = (int)(result.Defense + ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.MagicResistance) result.RES = (int)(result.RES + ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.MaximumHP) result.maxHP = (int)(result.maxHP + ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.RedeployTime) result.RedeployTyme = (int)(result.RedeployTyme + ((valueInt != 0) ? valueInt : valueFloat));

            return result;
        }
        public OperatorData ModificatorPlus(Attribute[] attributes, float valueFloat = 0f, int valueInt = 0)
        {
            OperatorData result = new OperatorData();

            foreach (Attribute attribute in attributes)
            {
                ModificatorPlus(attribute, valueFloat, valueInt);
            }

            return result;
        }
        public OperatorData ModificatorMultiplication(AttributeDefinition attributeDefinition)
        {
            OperatorData result = new OperatorData();

            ModificatorMultiplication(attributeDefinition.attribute, attributeDefinition.valueFloat, attributeDefinition.valueInt);

            return result;
        }
        public OperatorData ModificatorMultiplication(AttributeDefinition[] attributeDefinitions)
        {
            OperatorData result = new OperatorData();

            foreach (AttributeDefinition attributeDefinition in attributeDefinitions)
            {
                ModificatorMultiplication(attributeDefinition.attribute, attributeDefinition.valueFloat, attributeDefinition.valueInt);
            }

            return result;
        }
        public OperatorData ModificatorMultiplication(Attribute attribute, float valueFloat = 0f, int valueInt = 0)
        {
            OperatorData result = this;

            if (attribute == Attribute.AttackPower) result.Attack = (int)(result.Attack * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.AttackTime) result.ASPD = (result.ASPD * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.Block) result.Block = (int)(result.Block * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.Cost) result.DpCost = (int)(result.DpCost * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.CurrentHP) result.curHP = (int)(result.curHP * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.Defense) result.Defense = (int)(result.Defense * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.MagicResistance) result.RES = (int)(result.RES * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.MaximumHP) result.maxHP = (int)(result.maxHP * ((valueInt != 0) ? valueInt : valueFloat));
            if (attribute == Attribute.RedeployTime) result.RedeployTyme = (int)(result.RedeployTyme * ((valueInt != 0) ? valueInt : valueFloat));

            return result;
        }
        public OperatorData ModificatorMultiplication(Attribute[] attributes, float valueFloat = 0f, int valueInt = 0)
        {
            OperatorData result = new OperatorData();

            foreach (Attribute attribute in attributes)
            {
                ModificatorMultiplication(attribute, valueFloat, valueInt);
            }

            return result;
        }
        public void SetCurHP(int value)
        {
            curHP = value;
            MainController.mainInterfaceFields.selectOperatorUI.HP.GetComponent<UIOperatorHp>().SetHP(value);
            MainController.mainInterfaceFields.selectOperatorUI.updateSelectInfo();
        }

        [System.Serializable]
        public class AttributeDefinition
        {
            public Attribute attribute;
            public float valueFloat = 0f;
            public int valueInt = 0;
        }
        [System.Serializable]
        public class Skill
        {
            public string name;
            public Sprite skillImage;
            public string description;
            public int cost;
            public int initCost;
            public int duration;
        }
    }
}