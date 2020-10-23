using System.Collections.Generic;
using UnityEngine;

namespace MainController.Operator.Skills
{

    public class AllSkillsImplementation : MonoBehaviour
    {
        private enum SkillSelect
        {
            Skill_1,
            Skill_2,
            Skill_3,
        };

        [SerializeField]
        private SkillSelect _selectedFunction;
        private Dictionary<SkillSelect, System.Action> _functionLookup;


        private void Awake()
        {
            _functionLookup = new Dictionary<SkillSelect, System.Action>()
        {
            { SkillSelect.Skill_1, Skill_1 },
            { SkillSelect.Skill_2, Skill_2 },
            { SkillSelect.Skill_3, Skill_3 },
        };
        }


        public void ActivateSelectedFunction()
        {
            _functionLookup[_selectedFunction].Invoke();
        }


        private void Skill_1()
        {

        }

        private void Skill_2()
        {

        }

        private void Skill_3()
        {

        }

    }
}