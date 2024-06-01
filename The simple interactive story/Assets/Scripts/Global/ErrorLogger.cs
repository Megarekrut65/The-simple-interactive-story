using UnityEngine;
using UnityEngine.UI;

namespace Global
{
    public class ErrorLogger: MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private GameObject errorObj;

        public void ShowError(string error)
        {
            text.text = error;
            errorObj.SetActive(true);
        }

        public void HideError()
        {
            errorObj.SetActive(false);
        }
    }
}