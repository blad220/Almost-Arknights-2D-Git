using Operator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatorPanelCreate : MonoBehaviour
{
    public Color colorPanel;

    public GameObject operatorsParent;
    public GameObject operatorUIPrefab;

    public List<GameObject> Operators = new List<GameObject>();

    public List<GameObject> OperatorsPanel = new List<GameObject>();

    public RuntimeAnimatorController _AnimatorController;

    public Color colorPanelBefore = new Color();
    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        OperatorPanelCreate link = MainController.mainInterfaceFields.operatorPanelCreate;
        if (link == null) MainController.mainInterfaceFields.operatorPanelCreate = gameObject.GetComponent<OperatorPanelCreate>();
        //colorPanelBefore = GetComponent<Image>().color;

        ((RectTransform)gameObject.transform).anchorMin = new Vector2(1f, 0.5f);
        ((RectTransform)gameObject.transform).anchorMax = new Vector2(1f, 0.5f);
        ((RectTransform)gameObject.transform).pivot = new Vector2(0.5f, 0.5f);
        ((RectTransform)gameObject.transform).sizeDelta = new Vector2(MainController.operatorOnGame.Length*90f, 90f);
        ((RectTransform)gameObject.transform).anchoredPosition = new Vector3(((MainController.operatorOnGame.Length * -90f) * 0.5f), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);

        for (int i = 0; i < MainController.operatorOnGame.Length; i++)
        {
            GameObject imageOperator = Instantiate(operatorUIPrefab);
            //GameObject imageOperator = new GameObject();
            imageOperator.transform.SetParent(this.transform);

            OperatorsPanel.Add(imageOperator);

            GameObject Operator = CreateOperator(MainController.operatorOnGame[i].prefabOfOperator, MainController.operatorOnGame[i]);

            uiSelectAvatar SelectAvatar = imageOperator.GetComponent<uiSelectAvatar>();
            SelectAvatar.operatorObject = Operator;

            GameObject avatar = SelectAvatar.Avatar;
            //avatar.transform.SetParent(imageOperator.transform);

            imageOperator.name = MainController.operatorOnGame[i].Name;


            //Image image = imageOperator.AddComponent<Image>();
            //image.color = colorPanel;

            //Button button = imageOperator.AddComponent<Button>();
            //button.transition = Selectable.Transition.Animation;
            //Navigation tempNavigation = new Navigation();
            //tempNavigation.mode = Navigation.Mode.None;
            //button.navigation = tempNavigation;

            //Animator animator = imageOperator.AddComponent<Animator>();

            //animator.runtimeAnimatorController = _AnimatorController;




            //((RectTransform)imageOperator.transform).sizeDelta = new Vector2(90f, 100f);
            //((RectTransform)imageOperator.transform).anchorMin = new Vector2(1f, 1f);
            //((RectTransform)imageOperator.transform).anchorMax = new Vector2(1f, 1f);
            //((RectTransform)imageOperator.transform).pivot = new Vector2(1f, 1f);
            ((RectTransform)imageOperator.transform).localScale = new Vector3(1f, 1f, 1f);
            ((RectTransform)imageOperator.transform).anchoredPosition = new Vector3((-90f * i), 0f, 0f);

            avatar.name = $"Avatar_{MainController.operatorOnGame[i].Name}";
            Image imageAvatar = avatar.GetComponent<Image>();
            imageAvatar.sprite = MainController.operatorOnGame[i].artSmall;
            //((RectTransform)avatar.transform).sizeDelta = new Vector2(90f, 90f);
            //((RectTransform)avatar.transform).anchorMin = new Vector2(0.5f, 1f);
            //((RectTransform)avatar.transform).anchorMax = new Vector2(0.5f, 1f);
            //((RectTransform)avatar.transform).pivot = new Vector2(0.5f, 1f);

            Text DpCostField = SelectAvatar.DpCost;
            DpCostField.text = MainController.operatorOnGame[i].DpCost.ToString();
        }
        
    }
    public GameObject CreateOperator(GameObject operatorCreate, OperatorData operatorData)
    {
        GameObject OperatorUI = Instantiate(MainController.OperatorInterfaseObject, operatorsParent.transform);
        OperatorUI.name = operatorData.Name;

        GameObject Operator = Instantiate(operatorCreate, OperatorUI.transform);
        Operator.transform.SetSiblingIndex(0);

        OperatorController operatorController = OperatorUI.GetComponent<OperatorController>();
        operatorController._operatorObject = Operator;
        operatorController._operatorData = operatorData;
        operatorController.SetStartHP(operatorController._operatorData.maxHP);
        operatorController.SetStartSkillPoint(
            operatorController._operatorData.skills[(int)operatorController._operatorData.activeSkill].initCost,
            operatorController._operatorData.skills[(int)operatorController._operatorData.activeSkill].cost
            );

        OperatorUI.AddComponent<CapsuleCollider>();

        Operators.Add(OperatorUI);

        OperatorUI.transform.position = new Vector3(9999f, 9999f, 9999f);

        return OperatorUI;
    }
    public void checkDPCostAndUnitLimit()
    {
        foreach (GameObject operatopPanel in OperatorsPanel)
        {
            GameObject operatorObject = operatopPanel.GetComponent<uiSelectAvatar>().operatorObject;
            OperatorController operatorData = operatorObject.GetComponent<OperatorController>();
            if (operatorData._operatorData.DpCost <= MainController._DPcount && MainController.UnitLimit > 0 && !MainController.isDeployedOperator(operatorObject))
            {
                operatopPanel.GetComponent<uiSelectAvatar>().isActive = true;
                operatopPanel.GetComponent<Button>().interactable = true;
                operatopPanel.GetComponent<Image>().color = new Color32(31, 31, 31, 139);
            }
            else
            {
                operatopPanel.GetComponent<uiSelectAvatar>().isActive = false;
                operatopPanel.GetComponent<Button>().interactable = false;
                operatopPanel.GetComponent<Image>().color =  new Color32(118, 118, 118, 139);
            }
        }
    }
}
