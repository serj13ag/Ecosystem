using UnityEngine;

namespace UI
{
    public abstract class BaseWindow : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        protected void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}