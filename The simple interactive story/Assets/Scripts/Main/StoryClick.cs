using Global;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class StoryClick : MonoBehaviour
    {
        [SerializeField] private string scene;
        [SerializeField] private string storyId;
        [SerializeField] private string language;

        public void Click()
        {
            LocalStorage.SetValue("storyId", "827c621f-e6f9-4502-9d72-6d08fcefb7f0");
            LocalStorage.SetValue("userId", "rdMc08WAIpMKwZC7l8hdlHIXwUB2");
            LocalStorage.SetValue("language", language);
            
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}