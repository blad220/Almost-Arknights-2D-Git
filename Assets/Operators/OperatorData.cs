using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="OperatorName", menuName ="Operators/New operator")]
public class OperatorData : ScriptableObject
{
    public string Name;
    
    
    [Space(10)]
    public EliteEnum Elite;
    public enum EliteEnum { Elite_0, Elite_1, Elite_2 }
    public int Level;

    [Space(10)]
    public OperatorTypeEnum OperatorType;
    public enum OperatorTypeEnum { Caster, Guard, Medic, Sniper, Specialist, Supporter, Defender, Vanguard  }

    public Sprite artBig_Ellite0;
    public Sprite artBig_Ellite2;
    public Sprite artSmall;

    [Space(10)]
    public int curHP;
    public int maxHP;
    public int Attack;
    public int Defense;
    public int RES;

    [Space(10)]
    public int RedeployTyme;
    public int DpCost;
    public int Block;
    public int ASPD;

    [Space(10)]
    public int Range;

    [Space(10)]
    public int CurrentEXP;
    public int NextLevelEXP;

    [Space(10)]
    public SkillEnum activeSkill;
    public enum SkillEnum { Skil_1, Skil_2, Skil_3 }

    [Space(10)]
    public Skill[] skills = new Skill[3];
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
