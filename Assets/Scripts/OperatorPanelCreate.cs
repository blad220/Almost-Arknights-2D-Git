using MainController.Operator;
using MainController.Ui;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainController
{
    public class OperatorPanelCreate : MonoBehaviour
    {
        public Color colorPanel;

        public GameObject operatorsParent;
        public GameObject operatorUIPrefab;

        public List<GameObject> Operators = new List<GameObject>();

        public List<GameObject> OperatorsPanel = new List<GameObject>();

        public RuntimeAnimatorController _AnimatorController;

        public Color colorPanelBefore = new Color();
        public Color32 activeAvatarColor = new Color32(31, 31, 31, 139);
        public Color32 NonActiveAvatarColor = new Color32(118, 118, 118, 139);

        public ParticleSystem hitSelfParticle;
        public ParticleSystem healSelfParticle;

        void Start()
        {
            OperatorPanelCreate link = MainController.mainInterfaceFields.operatorPanelCreate;
            if (link == null) MainController.mainInterfaceFields.operatorPanelCreate = gameObject.GetComponent<OperatorPanelCreate>();

            ((RectTransform)gameObject.transform).anchorMin = new Vector2(1f, 0.5f);
            ((RectTransform)gameObject.transform).anchorMax = new Vector2(1f, 0.5f);
            ((RectTransform)gameObject.transform).pivot = new Vector2(0.5f, 0.5f);
            ((RectTransform)gameObject.transform).sizeDelta = new Vector2(MainController.operatorOnGame.Length * 90f, 90f);
            ((RectTransform)gameObject.transform).anchoredPosition = new Vector3(((MainController.operatorOnGame.Length * -90f) * 0.5f), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);

            for (int i = 0; i < MainController.operatorOnGame.Length; i++)
            {
                GameObject imageOperator = Instantiate(operatorUIPrefab);
                imageOperator.transform.SetParent(this.transform);

                OperatorsPanel.Add(imageOperator);

                UISelectAvatar SelectAvatar = imageOperator.GetComponent<UISelectAvatar>();

                GameObject Operator = CreateOperator(MainController.operatorOnGame[i].prefabOfOperator, MainController.operatorOnGame[i], SelectAvatar);

                SelectAvatar.operatorObject = Operator;
                SelectAvatar.RedeployField.transform.parent.gameObject.SetActive(false);

                GameObject avatar = SelectAvatar.Avatar;

                imageOperator.name = MainController.operatorOnGame[i].Name;

                ((RectTransform)imageOperator.transform).localScale = new Vector3(1f, 1f, 1f);
                ((RectTransform)imageOperator.transform).anchoredPosition = new Vector3((-90f * i), 0f, 0f);

                avatar.name = $"Avatar_{MainController.operatorOnGame[i].Name}";
                Image imageAvatar = avatar.GetComponent<Image>();
                imageAvatar.sprite = MainController.operatorOnGame[i].artSmall;

                Text DpCostField = SelectAvatar.DpCost;
                DpCostField.text = MainController.operatorOnGame[i].DpCost.ToString();

                GameObject operatorObject = SelectAvatar.operatorObject;
                OperatorController operatorController = operatorObject.GetComponent<OperatorController>();

                if (hitSelfParticle != null)
                {
                    ParticleSystem particleSystem = Instantiate(hitSelfParticle, operatorObject.transform);
                    particleSystem.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                    operatorController.hitSelfParticle = particleSystem;
                }
                if (healSelfParticle != null)
                {
                    ParticleSystem particleSystemHeal = Instantiate(healSelfParticle, operatorObject.transform);
                    particleSystemHeal.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                    operatorController.healSelfParticle = particleSystemHeal;
                }

            }
            checkDPCostAndUnitLimit();

        }
        public GameObject CreateOperator(GameObject operatorCreate, OperatorData operatorData, UISelectAvatar selectAvatar)
        {
            GameObject OperatorUI = Instantiate(MainController.OperatorInterfaseObject, operatorsParent.transform);
            OperatorUI.name = operatorData.Name;

            GameObject Operator = Instantiate(operatorCreate, OperatorUI.transform);
            Operator.transform.SetSiblingIndex(0);

            OperatorController operatorController = OperatorUI.GetComponent<OperatorController>();
            operatorController.selectAvatar = selectAvatar;
            operatorController._operatorObject = Operator;
            operatorController._operatorData = operatorData;


            operatorController.SetStartHP(operatorController._operatorData.maxHP);
            operatorController.SetStartSkillPoint(operatorController._operatorData.skill.initCost, operatorController._operatorData.skill.cost);

            CapsuleCollider capsuleCollider = OperatorUI.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = 0.35f;

            Rigidbody rigidbody = OperatorUI.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            Operators.Add(OperatorUI);

            OperatorUI.transform.position = new Vector3(9999f, 9999f, 9999f);

            return OperatorUI;
        }
        public void checkDPCostAndUnitLimit()
        {
            foreach (GameObject operatopPanel in OperatorsPanel)
            {
                UISelectAvatar selectAvatar = operatopPanel.GetComponent<UISelectAvatar>();
                GameObject operatorObject = selectAvatar.operatorObject;
                OperatorController operatorData = operatorObject.GetComponent<OperatorController>();
                if (operatorData._operatorData.DpCost <= MainController._DPcount && MainController.UnitLimit > 0 && !MainController.isDeployedOperator(operatorObject) && !selectAvatar.isRedeploy)
                {
                    operatopPanel.GetComponent<UISelectAvatar>().isActive = true;
                    operatopPanel.GetComponent<Button>().interactable = true;
                    operatopPanel.GetComponent<Image>().color = activeAvatarColor;
                }
                else
                {
                    operatopPanel.GetComponent<UISelectAvatar>().isActive = false;
                    operatopPanel.GetComponent<Button>().interactable = false;
                    operatopPanel.GetComponent<Image>().color = NonActiveAvatarColor;
                }
            }
        }
        public void SetAllActive(bool value)
        {
            foreach (GameObject operatopPanel in OperatorsPanel)
            {
                operatopPanel.GetComponent<UISelectAvatar>().isActive = value;
                operatopPanel.GetComponent<Button>().interactable = value;
            }
        }
    }
}