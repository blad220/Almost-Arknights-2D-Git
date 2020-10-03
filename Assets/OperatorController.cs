using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using DragonBones;
using Transform = UnityEngine.Transform;
//#endif


public class OperatorController : MonoBehaviour
{
    public OperatorData _operatorData;
    public GameObject _operatorObject;
    public GameObject _underlayObject;

    public GameObject _operatorHP;
    public GameObject _operatorSkill;

    public enum SetupDirection { Right, Left, Top, Bottom }
    public SetupDirection setupDirection;

    public bool isAttack;
    public bool isAttackLoop;
    private bool isAttacking;

    DragonBones.UnityArmatureComponent operatorObject;
    // Start is called before the first frame update
    void Start()
    {
        if (_operatorObject.TryGetComponent( out DragonBones.UnityArmatureComponent component))
        {
            operatorObject = component;
            //StartOperator();
        }
        //operatorObject = _operatorObject.GetComponent<DragonBones.UnityArmatureComponent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttack)
        {
            if(!isAttacking)
            {
                StartCoroutine(AnimationAttackOperator());
            }
        }
        if (isAttackLoop)
        {
            if (!isAttacking)
            {
                StartCoroutine(AnimationAttackLoopOperator());
            }
        }
        
    }
    public void SetMaxHP(int hp)
    {
        _operatorHP.GetComponent<UIOperatorHp>().SetMaxHP(hp);
    }
    public void SetHP(int hp)
    {
        _operatorHP.GetComponent<UIOperatorHp>().SetHP(hp);
    }
    public void SetMaxSkillPoint(int maxSkillPoint)
    {
        _operatorSkill.GetComponent<UIOperatorSkill>().SetMaxSkillPoint(maxSkillPoint);
    }
    public void SetSkillPoint(int skillPoint)
    {
        _operatorSkill.GetComponent<UIOperatorSkill>().SetSkillPoint(skillPoint);
    }

    public void SetStartHP(int hp)
    {
        SetMaxHP(hp);
        SetHP(hp);
    }
    public void SetStartSkillPoint(int skillPoint, int maxSkillPoint)
    {
        SetMaxSkillPoint(maxSkillPoint);
        SetSkillPoint(skillPoint);
    }

    public static void ChangeArmatureData(UnityArmatureComponent _armatureComponent, string armatureName, string dragonBonesName)
    {
        bool isUGUI = _armatureComponent.isUGUI;
        UnityDragonBonesData unityData = null;
        Slot slot = null;
        if (_armatureComponent.armature != null)
        {
            unityData = _armatureComponent.unityData;
            slot = _armatureComponent.armature.parent;
            _armatureComponent.Dispose(false);

            UnityFactory.factory._dragonBones.AdvanceTime(0.0f);

            _armatureComponent.unityData = unityData;
        }

        _armatureComponent.armatureName = armatureName;
        _armatureComponent.isUGUI = isUGUI;

        _armatureComponent = UnityFactory.factory.BuildArmatureComponent(_armatureComponent.armatureName, dragonBonesName, null, _armatureComponent.unityData.dataName, _armatureComponent.gameObject, _armatureComponent.isUGUI);
        if (slot != null)
        {
            slot.childArmature = _armatureComponent.armature;
        }

        _armatureComponent.sortingLayerName = _armatureComponent.sortingLayerName;
        _armatureComponent.sortingOrder = _armatureComponent.sortingOrder;
    }
    public void StartOperator(SetupDirection direction)
    {
        //transform.position;
        setupDirection = direction;
        string lastAnimation = operatorObject._armature.animation.lastAnimationName;
        DragonBones.AnimationState lastAnimationState = operatorObject._armature.animation.lastAnimationState;
        if (direction == SetupDirection.Bottom)
        {
            operatorObject.armature.flipX = false;

            _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 270f, _underlayObject.transform.localEulerAngles.z);


        }
        if (direction == SetupDirection.Top)
        {
            //Transform transformOperator = operatorObject.transform;
            //operatorObject.armature.Dispose();
            string  armatureName= operatorObject.armatureName;
            
            ChangeArmatureData(operatorObject, armatureName.Replace("_Front", "_Back"), operatorObject.unityData.dataName);
            //DragonBones.UnityFactory.factory.BuildArmatureComponent("Default_Skin_Back", "Tiger_Front", null, null, gameObject, false);

            
            operatorObject._armature.animation.Play("Idle", 0);

            operatorObject.armature.flipX = false;
            operatorObject.DBUpdate();
            _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 90f, _underlayObject.transform.localEulerAngles.z);
        }
        if (direction == SetupDirection.Left)
        {
            operatorObject.armature.flipX = true;

            _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 0f, _underlayObject.transform.localEulerAngles.z);

        }
        if (direction == SetupDirection.Right)
        {
            operatorObject.armature.flipX = false;

            _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 180f, _underlayObject.transform.localEulerAngles.z);

        }
        //if (!string.IsNullOrEmpty(operatorObject.animationName))
        //{
            StartCoroutine(AnimationStartOperator());
        //}
    }
    IEnumerator AnimationStartOperator()
    {
        operatorObject._armature.animation.Play("Start", 1);
        //Debug.Log("Animation Start");
        //Debug.Log(operatorObject._armature.animation.isCompleted);
        //yield return new WaitForSeconds(2f);

        while (!operatorObject._armature.animation.isCompleted)
        {
            yield return new WaitForSeconds(.1f);
        }
        //Debug.Log(operatorObject._armature.animation.isCompleted);

        operatorObject._armature.animation.Play("Idle", 0);
        //Debug.Log("Animation Idle");
        yield return null;   
    }
    IEnumerator AnimationAttackOperator()
    {
        Debug.Log("Animation AnimationAtackOperator");
        isAttacking = true;
        operatorObject._armature.animation.Play("Attack", 1);
        
        while (!operatorObject._armature.animation.isCompleted)
        {
            yield return new WaitForSeconds(.1f);
        }

        //AtackDamage()
        isAttacking = false;
        isAttack = false;

        operatorObject._armature.animation.Play("Idle", 0);
        yield return null;
    }
    IEnumerator AnimationAttackLoopOperator()
    {

        isAttacking = true;
        while (isAttackLoop) {
            operatorObject._armature.animation.Play("Attack", 1);

            while (!operatorObject._armature.animation.isCompleted)
            {
                yield return new WaitForSeconds(.1f);
            }

            //AtackDamage()

        }
        isAttacking = false;
        

        operatorObject._armature.animation.Play("Idle", 0);
        yield return null;
    }
}
