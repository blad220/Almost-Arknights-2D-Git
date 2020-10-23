using UnityEngine;

namespace MainController.Operator.Skills
{
    public abstract class SkillInterface : ScriptableObject
    {
        public abstract bool SkillActivate(OperatorController operatorController);
    }
}