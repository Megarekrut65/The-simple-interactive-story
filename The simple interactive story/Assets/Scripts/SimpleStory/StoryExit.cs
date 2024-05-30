using Global.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleStory
{
    public class StoryExit: MonoBehaviour
    {
        [SerializeField] private AudioClip background;
        public void Exit()
        {
            MusicManager.ChangeAudioClip(background);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}