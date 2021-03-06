﻿using MainController.Operator;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainController.Ui
{

    public class UISelectRotatePointerUI : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Color32 topColor;
        public Color32 rightColor;
        public Color32 leftColor;
        public Color32 bottomColor;

        UISelectRotateUI selectRotateUI;

        void Start()
        {
            selectRotateUI = MainController.mainInterfaceFields.selectRotateUI;
            topColor = selectRotateUI.topObjectImage.color;
            rightColor = selectRotateUI.rightObjectImage.color;
            leftColor = selectRotateUI.leftObjectImage.color;
            bottomColor = selectRotateUI.bottomObjectImage.color;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            isEndDrag = false;
        }

        public void OnDrag(PointerEventData eventData)
        {

            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            Vector3 prePosition = transform.localPosition;
            float Min = -80f;
            float Max = 80f;
            transform.localPosition = new Vector3(Mathf.Clamp(prePosition.x, Min, Max), Mathf.Clamp(prePosition.y, Min, Max), 0f);
            bool notSelectDirection = false;
            if (transform.localPosition.x > 40f && transform.localPosition.y < 40f && transform.localPosition.y > -40f)
            {
                selectRotateUI.rightObjectImage.color = selectRotateUI.hoverColor;
                selectRotateUI.target.GetComponent<OperatorController>().DrowRange(OperatorController.SetupDirection.Right);
                notSelectDirection = true;
            }
            else
            {
                selectRotateUI.rightObjectImage.color = rightColor;
            }

            if (transform.localPosition.x < -40f && transform.localPosition.y < 40f && transform.localPosition.y > -40f)
            {
                selectRotateUI.leftObjectImage.color = selectRotateUI.hoverColor;
                selectRotateUI.target.GetComponent<OperatorController>().DrowRange(OperatorController.SetupDirection.Left);
                notSelectDirection = true;
            }
            else
            {
                selectRotateUI.leftObjectImage.color = leftColor;
            }

            if (transform.localPosition.y > 40f && transform.localPosition.x < 40f && transform.localPosition.x > -40f)
            {
                selectRotateUI.topObjectImage.color = selectRotateUI.hoverColor;
                selectRotateUI.target.GetComponent<OperatorController>().DrowRange(OperatorController.SetupDirection.Top);
                notSelectDirection = true;
            }
            else
            {
                selectRotateUI.topObjectImage.color = topColor;
            }

            if (transform.localPosition.y < -40f && transform.localPosition.x < 40f && transform.localPosition.x > -40f)
            {
                selectRotateUI.bottomObjectImage.color = selectRotateUI.hoverColor;
                selectRotateUI.target.GetComponent<OperatorController>().DrowRange(OperatorController.SetupDirection.Bottom);
                notSelectDirection = true;
            }
            else
            {
                selectRotateUI.bottomObjectImage.color = bottomColor;
            }
            if (!notSelectDirection)
            {
                selectRotateUI.target.GetComponent<OperatorController>().HideRange();
            }
            //Debug.Log(prePosition.x);
            //Debug.Log(Input.mousePosition.x);
            //Debug.Log(Mathf.Clamp(Input.mousePosition.x - prePosition.x, -60f, 60f));
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            selectRotateUI.rightObjectImage.color = rightColor;
            selectRotateUI.leftObjectImage.color = leftColor;
            selectRotateUI.topObjectImage.color = topColor;
            selectRotateUI.bottomObjectImage.color = bottomColor;

            isEndDrag = true;

            if (transform.localPosition.x > 40f && transform.localPosition.y < 40f && transform.localPosition.y > -40f)
            {
                selectRotateUI.getTarget().GetComponent<OperatorController>().StartOperator(OperatorController.SetupDirection.Right);
                selectRotateUI.SelectRotateOpen(false);
                MainController.DeployOperator(selectRotateUI.getTarget(), selectRotateUI.getCubeTile());
            }

            if (transform.localPosition.x < -40f && transform.localPosition.y < 40f && transform.localPosition.y > -40f)
            {
                selectRotateUI.getTarget().GetComponent<OperatorController>().StartOperator(OperatorController.SetupDirection.Left);
                selectRotateUI.SelectRotateOpen(false);
                MainController.DeployOperator(selectRotateUI.getTarget(), selectRotateUI.getCubeTile());
            }

            if (transform.localPosition.y > 40f && transform.localPosition.x < 40f && transform.localPosition.x > -40f)
            {
                selectRotateUI.getTarget().GetComponent<OperatorController>().StartOperator(OperatorController.SetupDirection.Top);
                selectRotateUI.SelectRotateOpen(false);
                MainController.DeployOperator(selectRotateUI.getTarget(), selectRotateUI.getCubeTile());
            }

            if (transform.localPosition.y < -40f && transform.localPosition.x < 40f && transform.localPosition.x > -40f)
            {
                selectRotateUI.getTarget().GetComponent<OperatorController>().StartOperator(OperatorController.SetupDirection.Bottom);
                selectRotateUI.SelectRotateOpen(false);
                MainController.DeployOperator(selectRotateUI.getTarget(), selectRotateUI.getCubeTile());
            }

        }

        private bool isEndDrag;
        // Update is called once per frame
        void Update()
        {

            if (isEndDrag && transform.localPosition != Vector3.zero) transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 0.1f);

        }
    }
}