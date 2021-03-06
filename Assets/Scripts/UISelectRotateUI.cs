﻿using MainController.Map.Tile;
using MainController.Operator;
using UnityEngine;
using UnityEngine.UI;

namespace MainController.Ui
{

    public class UISelectRotateUI : MonoBehaviour
    {
        public Color32 hoverColor;

        public GameObject RotateSelectUI;

        public Image topObjectImage;
        public Image leftObjectImage;
        public Image rightObjectImage;
        public Image bottomObjectImage;

        public GameObject target;
        public TileDescription cubeTile;
        void Start()
        {
            UISelectRotateUI link = MainController.mainInterfaceFields.selectRotateUI;
            if (link == null) MainController.mainInterfaceFields.selectRotateUI = gameObject.GetComponent<UISelectRotateUI>();

            RotateSelectUI.SetActive(true);
            SelectRotateOpen(false);
        }
        public void setPosition(GameObject CubeObject)
        {

            Vector3 pos = Camera.main.WorldToScreenPoint(CubeObject.GetComponent<TileDescription>().standByPosition.position);

            transform.position = pos;
        }
        public GameObject getTarget()
        {
            return target;
        }
        public TileDescription getCubeTile()
        {
            return cubeTile;
        }
        public void placeSelectRotate(GameObject operatorObject, TileDescription cube, bool isActive)
        {
            operatorObject.GetComponent<OperatorController>().tilePosition = cube;

            target = operatorObject;
            cubeTile = cube;

            SelectRotateOpen(isActive);
            //operatorObject.GetComponent<OperatorController>().setupDirection = OperatorController.SetupDirection.
        }
        public void unPlaceSelectRotate()
        {
            target.GetComponent<OperatorController>().tilePosition = null;
            target.transform.position = new Vector3(9999f, 9999f, 9999f);

            target = null;
            cubeTile = null;

            SelectRotateOpen(false);

            MainController.TimeScaleReset();
            //operatorObject.GetComponent<OperatorController>().setupDirection = OperatorController.SetupDirection.
        }
        public void SelectRotateOpen(bool isActive)
        {
            if (isActive)
            {
                setPosition(cubeTile.gameObject);
            }
            else
            {
                transform.position = new Vector3(9999f, 9999f, 9999f);
            }
            //RotateSelectUI.SetActive(isActive);
        }

    }
}