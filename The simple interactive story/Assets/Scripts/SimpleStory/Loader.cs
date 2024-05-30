using System.Collections;
using UnityEngine;

namespace SimpleStory
{
    public class Loader: MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private Animator animator;
        private static readonly int IsOpened = Animator.StringToHash("IsOpened");

        public void Open()
        {
            animator.SetBool(IsOpened, true);
            StartCoroutine(CheckAnimationEnd());
        }

        private IEnumerator CheckAnimationEnd()
        {
            yield return new WaitForSeconds(1.5f);
            
            background.SetActive(false);
        }

        public void Close()
        {
            background.SetActive(true);
            animator.SetBool(IsOpened, false);
        }
    }
}