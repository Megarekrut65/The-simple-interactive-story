using UnityEngine;

namespace SimpleStory
{
    public class ActivityController : MonoBehaviour
    {
        [SerializeField] private GameObject obj;

        public void Activity()
        {
            obj.SetActive(!obj.activeInHierarchy);
        }
        public void SetActivity(bool value)
        {
            obj.SetActive(value);
        }
    }
}