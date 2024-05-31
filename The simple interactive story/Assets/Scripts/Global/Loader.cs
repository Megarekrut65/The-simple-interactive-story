using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Global
{
    public class Loader: MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject icon;
        [SerializeField] private Animator animator;
        private static readonly int IsOpened = Animator.StringToHash("IsOpened");

        private void Start()
        {
            background.SetActive(true);
            icon.SetActive(true);
        }

        public void Open()
        {
            animator.SetBool(IsOpened, true);
            icon.SetActive(false);
        }
        
        public void Close()
        {
            background.SetActive(true);
            icon.SetActive(true);
            animator.SetBool(IsOpened, false);
        }

        private void Update()
        {
            background.SetActive(backgroundImage.color.a > 0.05);
        }
    }
}