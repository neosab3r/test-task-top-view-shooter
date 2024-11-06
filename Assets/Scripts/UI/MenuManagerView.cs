using System.Collections;
using System.Collections.Generic;
using BeeGood.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeeGood.UI
{
    public enum DifficultType
    {
        None,
        Easy,
        Hard
    }
    
    public class MenuManagerView : MonoBehaviour
    {
        [SerializeField] private Sprite selectedImage;
        [SerializeField] private Sprite unselectedImage;
        [SerializeField] private Button easyButton;
        [SerializeField] private TextMeshProUGUI easyButtonText;
        [SerializeField] private Image easyButtonImage;
        [SerializeField] private Button hardButton;
        [SerializeField] private TextMeshProUGUI hardButtonText;
        [SerializeField] private Image hardButtonImage;

        [SerializeField] private Button startButton;

        [SerializeField] private List<TextMeshProUGUI> descriptionObj;

        public DifficultType Difficult { get; private set; }
        
        private void Awake()
        {
            Difficult = DifficultType.None;
            easyButton.onClick.AddListener(() =>
            {
                Difficult = DifficultType.Easy;
                hardButtonText.color = Color.white;
                easyButtonText.color = Color.black;
                easyButtonImage.sprite = selectedImage;
                hardButtonImage.sprite = unselectedImage;
                SetDifficultDescription(Difficult);

            });
            hardButton.onClick.AddListener(() =>
            {
                Difficult = DifficultType.Hard;
                hardButtonText.color = Color.black;
                easyButtonText.color = Color.white;
                hardButtonImage.sprite = selectedImage;
                easyButtonImage.sprite = unselectedImage;
                SetDifficultDescription(Difficult);
            });

            startButton.onClick.AddListener(() =>
            {
                if (Difficult != DifficultType.None)
                {
                    Debug.LogError("START GAME");
                    GameManager.Initialize();
                    GameManager.Instance.StartGame(botCount: 1, stopSystems: true);
                    SetVisible(false);
                }
            });
            
            easyButton.onClick?.Invoke();
            StartCoroutine(UpdateSizesAndEnable());
        }

        public void SetVisible(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private IEnumerator UpdateSizesAndEnable()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return new WaitForSeconds(0.01F);
                
                var compsArray = gameObject.transform.GetComponentsInChildren<ContentSizeFitter>(true);
                foreach (var comp in compsArray)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(comp.transform as RectTransform);
                }
            }
        }

        private void SetDifficultDescription(DifficultType type)
        {
            if (type == DifficultType.Easy)
            {
                descriptionObj[0].text = "Средняя скорость";
                descriptionObj[1].text = "Боты уклоняются медленно";
            }
            else
            {
                descriptionObj[0].text = "Высокая скорость";
                descriptionObj[1].text = "Боты очень осторожны";
            }
        }
    }
}