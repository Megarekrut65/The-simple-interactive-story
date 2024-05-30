using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Start
{
    public class Init: MonoBehaviour
    {
        [SerializeField] private AnimationClip clip;
        
        private void Start()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            yield return new WaitForSeconds(clip.length);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}