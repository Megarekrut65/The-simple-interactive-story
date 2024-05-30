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
            LocalStorage.SetValue("storyId", "db37cb6d-e063-400e-b96e-9c0b3df11b62");
            LocalStorage.SetValue("userId", "rdMc08WAIpMKwZC7l8hdlHIXwUB2");
            LocalStorage.SetValue("language", language);
            
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}