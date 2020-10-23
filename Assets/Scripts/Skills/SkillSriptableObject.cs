using MainController.Operator.Range;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static MainController.Operator.OperatorData;

namespace MainController.Operator.Skills
{

    [CreateAssetMenu(fileName = "Skill Name", menuName = "New Skill")]
    public class SkillSriptableObject : SkillInterface
    {
        [Space(10)]
        public string name;
        public Sprite skillImage;
        public string description;
        public int cost;
        public int initCost;
        public int duration;
        public MySkillEvent SkillEvent;

        public int curSkillPoint;
        public int curDurationTime;
        public bool isActive;
        public bool isCanActive;

        [System.Serializable]
        public class MySkillEvent : UnityEvent<OperatorController> { }

        OperatorData tempData;

        OperatorData tempDataClear;
        public RangeOfOperator rangeOfSkill;

        delegate OperatorData StartSkill(OperatorController operatorController);
        delegate void EndSkill(OperatorController operatorController);

        delegate List<OperatorData> StartSkills(List<OperatorController> operatorController);
        delegate void EndSkills(List<OperatorController> operatorControllers);

        delegate void EachSecondSkill(OperatorController operatorController, int value);

        public void OnEnable()
        {
            if (SkillEvent == null)
            {
                SkillEvent = new MySkillEvent();
            }
            tempData = new OperatorData();
            curSkillPoint = 0;
            curDurationTime = 0;
            isActive = false;
            isCanActive = false;
            tempData = null;
        }

        public override bool SkillActivate(OperatorController operatorController)
        {
            SkillEvent.Invoke(operatorController);
            return true;
        }

        IEnumerator ActiveSkillWithCount(OperatorController operatorController, StartSkill startSkill = null, EachSecondSkill eachSecondSkill = null, EndSkill endSkill = null, int _duration = 0, int eachSecondSkillParametr = 0)
        {
            isActive = true;
            isCanActive = false;
            operatorController.SetStartSkillPoint(duration, duration);

            startSkill?.Invoke(operatorController);

            MainController.mainInterfaceFields.selectOperatorUI.updateSelectInfo(operatorController._operatorData);

            if (duration > 0)
            {
                curDurationTime = duration;
            }
            else
            {
                if (_duration > 0)
                {
                    curDurationTime = _duration;
                }
            }

            while (curDurationTime > 0)
            {

                if (MainController.mainInterfaceFields.selectOperatorUI.operatorController != null)
                {
                    eachSecondSkill?.Invoke(operatorController, eachSecondSkillParametr);
                    operatorController.SetSkillPoint(curDurationTime - 1);

                    MainController.mainInterfaceFields.selectOperatorUI.updateSelectInfoOnlySkill();
                }
                curDurationTime--;
                yield return new WaitForSeconds(1f);
            }

            endSkill?.Invoke(operatorController);

            MainController.mainInterfaceFields.selectOperatorUI.updateSelectInfo(operatorController._operatorData);


            isActive = false;
            isCanActive = true;
            operatorController.StartCoroutine(SkillPointGenerate(operatorController));

            yield return null;
        }
        public IEnumerator SkillPointGenerate(OperatorController operatorController, int startGenerate = 0)
        {
            curSkillPoint = startGenerate;
            isCanActive = false;
            isActive = false;
            operatorController.SetStartSkillPoint(0, cost);

            MainController.mainInterfaceFields.selectOperatorUI.updateSelectInfoOnlySkill();

            while (curSkillPoint < cost)
            {
                operatorController.SetSkillPoint(curSkillPoint + 1);

                MainController.mainInterfaceFields.selectOperatorUI.updateSelectInfoOnlySkill();
                curSkillPoint++;
                yield return new WaitForSeconds(1f);
            }
            isCanActive = true;
            isActive = false;
            yield return null;
        }

        public OperatorData BackUpOperatorData(OperatorController operatorController)
        {
            tempData = operatorController._operatorData;

            tempDataClear = Instantiate(operatorController._operatorData);

            return tempData;
        }

        public void ReBackUpOperatorData(OperatorController operatorController)
        {
            operatorController._operatorData = tempData;
            Destroy(tempDataClear);
            tempData = null;
        }
        private void Heal(OperatorController operatorController, int value)
        {
            operatorController.Heal(value);
        }
        public void reBackUpRange(OperatorController operatorController)
        {
            bool isActiveRange = operatorController.IsRangeShow();
            operatorController.DrowRange(operatorController.setupDirection, true);
            if (isActiveRange)
            {
                operatorController.ShowRange();
            }
            else
            {
                operatorController.HideRange();
            }
        }
        public void Polosis_Skill1(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);
            EndSkill end = ReBackUpOperatorData;

            operatorController._operatorData = operatorController._operatorData = tempDataClear.ModificatorMultiplication(Attribute.AttackPower, 1.65f);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, null, end));
        }
        public void Sora_Skill2(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);
            EndSkill end = ReBackUpOperatorData;

            operatorController._operatorData = tempDataClear.ModificatorMultiplication(Attribute.AttackPower, 1.5f);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, null, end));
        }
        public void Sunbr_Skill1(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);
            EachSecondSkill eachSecondSkill = Heal;
            int healValue = 100;
            EndSkill end = ReBackUpOperatorData;

            operatorController._operatorData = tempDataClear.ModificatorMultiplication(Attribute.AttackPower, 1.33f);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, eachSecondSkill, end, 0, healValue));
        }
        public void Svrash_Skill1(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);
            EndSkill end = ReBackUpOperatorData;

            operatorController._operatorData = tempDataClear.ModificatorMultiplication(Attribute.AttackPower, 2.1f);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, null, end, (int)operatorController._operatorData.ASPD));
        }
        public void Swllow_Skill2(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);
            EndSkill end = ReBackUpOperatorData;

            operatorController._operatorData = tempDataClear.ModificatorMultiplication(Attribute.AttackPower, 1.25f);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, null, end));
        }
        public void Tiger_Skill2(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);
            EndSkill end = ReBackUpOperatorData;

            operatorController._operatorData = tempDataClear.ModificatorMultiplication(Attribute.AttackPower, 1.45f);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, null, end));
        }
        public void Vingna_Skill2(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);
            EndSkill end = ReBackUpOperatorData;

            operatorController._operatorData = tempDataClear.ModificatorMultiplication(Attribute.AttackPower, 1.3f);
            operatorController._operatorData = tempDataClear.ModificatorPlus(Attribute.AttackTime, -0.5f);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, null, end));
        }
        public void Yak_Skill1(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);
            EachSecondSkill eachSecondSkill = Heal;
            int healValue = 25;
            EndSkill end = ReBackUpOperatorData;

            operatorController._operatorData = tempDataClear.ModificatorPlus(Attribute.MaximumHP, operatorController._operatorData.maxHP * 0.33f);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, eachSecondSkill, end, 0, healValue));
        }
        public void Yuki_Skill1(OperatorController operatorController)
        {
            BackUpOperatorData(operatorController);

            EndSkill end = ReBackUpOperatorData;
            end += reBackUpRange;

            tempDataClear.rangeOfOperator = rangeOfSkill;
            operatorController._operatorData = tempDataClear;

            operatorController.DrowRange(operatorController.setupDirection, true);

            operatorController.StartCoroutine(ActiveSkillWithCount(operatorController, null, null, end));
        }
    }
}