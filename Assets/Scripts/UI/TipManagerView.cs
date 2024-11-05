using System.Collections;
using UnityEngine;

namespace BeeGood.UI
{
    public class TipManagerView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        private Coroutine coroutine;
        
        private void Start()
        {
            Show();
        }

        public void Show()
        {
            canvasGroup.alpha = 0;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            coroutine = StartCoroutine(ShowCoroutine());
        }

        private IEnumerator ShowCoroutine()
        {
            for (var i = 0; i < 3; i++)
            {
                while (canvasGroup.alpha < 1)
                {
                    canvasGroup.alpha += Time.deltaTime * 2;
                    yield return null;
                }

                while (canvasGroup.alpha > 0)
                {
                    canvasGroup.alpha -= Time.deltaTime * 2;
                    yield return null;
                }
            }

            yield return null;
        }
    }
}