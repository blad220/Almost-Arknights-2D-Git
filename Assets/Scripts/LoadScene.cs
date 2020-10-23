using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainController
{
    public class LoadScene : MonoBehaviour
    {
        public Slider slider;
        private AsyncOperation loadingOperation;
        void Start()
        {
            slider.maxValue = 1f;
            slider.minValue = 0f;
            loadingOperation = SceneManager.LoadSceneAsync("GamePlayScene");
        }

        void Update()
        {
            slider.value = loadingOperation.progress;
        }
    }
}