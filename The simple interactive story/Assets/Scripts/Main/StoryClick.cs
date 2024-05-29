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
            LocalStorage.SetValue("storyId", "3f311d94-1a32-48b7-b7f6-cb04623fef49");
            LocalStorage.SetValue("userId", "rdMc08WAIpMKwZC7l8hdlHIXwUB2");
            LocalStorage.SetValue("language", language);
            
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}