using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainController
{

    public class ButtonsClick : MonoBehaviour
    {
        [Header("Play/Pause Button Field")]
        public Sprite PlaySprite;
        public Sprite PauseSprite;

        [Header("Speed Game Button Field")]
        public Text textFieldFast;

        [Header("Settings Game Button Field")]
        public Image SettingsPanelShow;

        private bool isFast;
        private bool isPlay;
        private float TimeStateBeforePlay = 1f;

        private void Start()
        {
            if (textFieldFast != null)
            {
                textFieldFast.text = "1X";
            }

        }
        public void SpeedOfGame()
        {
            if (textFieldFast != null && Time.timeScale != 0f)
            {
                isFast = !isFast;
                if (isFast)
                {
                    MainController.SetCurrentTimeScale(2f);
                    textFieldFast.text = "2X";
                }
                else
                {
                    MainController.SetCurrentTimeScale(1f);
                    textFieldFast.text = "1X";
                }
            }
        }
        public void WinGame()
        {
            SceneManager.LoadScene("Win");
        }
        public void LooseGame()
        {
            SceneManager.LoadScene("Loose");
        }
        public void LoadMainGame()
        {
            SceneManager.LoadScene("GamePlayScene");
        }
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }
        public void LoadingScene()
        {
            SceneManager.LoadScene("Loading");
        }
        public void PlayPauseGameTogle()
        {
            isPlay = !isPlay;
            if (isPlay)
            {

                Pause();
                GetComponent<Image>().sprite = PlaySprite;
            }
            else
            {
                Play();
                GetComponent<Image>().sprite = PauseSprite;
            }

        }
        private void Pause()
        {
            TimeStateBeforePlay = Time.timeScale;
            Time.timeScale = 0f;
            MainController.mainInterfaceFields.operatorPanelCreate.SetAllActive(false);
        }
        private void Play()
        {
            Time.timeScale = TimeStateBeforePlay;
            MainController.mainInterfaceFields.operatorPanelCreate.SetAllActive(true);
        }
        public void SettingsShow()
        {
            MainController.SetCurrentTimeScale(0f);
            SettingsPanelShow.gameObject.SetActive(true);
            MainController.mainInterfaceFields.operatorPanelCreate.SetAllActive(false);

        }
        public void SettingsHide()
        {
            MainController.TimeScaleReset();
            SettingsPanelShow.gameObject.SetActive(false);
            MainController.mainInterfaceFields.operatorPanelCreate.SetAllActive(true);
        }

    }
}