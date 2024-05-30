using System.Collections;
using Global;
using Global.Sound;
using JetBrains.Annotations;
using SimpleStory.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scene = SimpleStory.Data.Scene;

namespace SimpleStory
{
    public class StoryLoader : MonoBehaviour
    {
        [SerializeField] private Loader loader;
        [SerializeField] private GameObject variantPrefab;
        
        [Header("Parts")]
        [SerializeField] private Transform content;
        [SerializeField] private Image background;

        [Header("Images")] 
        [SerializeField] private Sprite defaultBackground;

        [SerializeField] private ImagesConstructor constructor;

        [Header("Texts")]
        [SerializeField] private GameObject textObj;
        [SerializeField] private Text text;

        private StoryController _controller = new StoryController();
        private Font _currentFont;
        private string _storyId;
        private string _userId;

        private void Start()
        {
            _storyId = LocalStorage.GetValue("storyId", "");
            _userId = LocalStorage.GetValue("userId", "");
            string scene = LocalStorage.GetValue($"{_userId}/{_storyId}", "");
            string fontName = LocalStorage.GetValue("font", "");
            if (_storyId == "" || _userId == "" || scene == "")
            {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
                return;
            }

            _currentFont = FontManager.GetTextFont(fontName);
            text.GetComponent<Text>().font = _currentFont;

            ScenesWrapper wrapper = JsonUtility.FromJson<ScenesWrapper>(scene);
            Scene[] list = wrapper.scenes;

            _controller = new StoryController(list);
            
            string lastScene = LocalStorage.GetValue($"{_userId}/{_storyId}/lastScene", "");
            if (lastScene == "") lastScene = null;
            
            StartCoroutine(LoadScene(_controller.GetScene(lastScene)));
        }

        private IEnumerator LoadScene(Scene scene)
        {
            if(scene == null) yield break;
            
            loader.Close();
            if (!_controller.IsSceneLoaded(scene))
            {
                yield return _controller.LoadScene(scene);
            }
            else
            {
                StartCoroutine(_controller.LoadScene(scene));
            }

            LoadMusic(scene);
            
            LoadText(scene.text);
            LoadAnswers(scene.answers);
            
            LoadImage(background, scene.background);

            constructor.Clear();
            if (scene.images == null)
            {
                loader.Open();
                yield break;
            }
            
            foreach (CanvasImage img in scene.images)
            {
                DrawImage(img);
            }
            loader.Open();
        }

        private void LoadMusic(Scene scene)
        {
            if (scene.music == null)
            {
                MusicManager.ChangeAudioClip(null);
                return;
            }
            MusicManager.ChangeAudioClip(_controller.GetClip(scene.music));
        }
        private void LoadText([CanBeNull] string key)
        {
            textObj.SetActive(true);

            if(key != null) text.GetComponent<Text>().text = _controller.GetWord(key);
            else textObj.SetActive(false);
        }
        
        private void LoadAnswers([CanBeNull] Answer[] answers)
        {
            foreach (Transform child in content) {
                Destroy(child.gameObject);
            }
            if(answers == null) return;

            foreach (Answer answer in answers)
            {
                GameObject obj = Instantiate(variantPrefab, content, false);
                obj.GetComponent<Variant>().SetData(answer, AnswerClick, _currentFont);
            }
        }

        private void AnswerClick(string nextSceneId)
        {
            Debug.Log(nextSceneId);
            if(nextSceneId == null) return;
            LocalStorage.SetValue($"{_userId}/{_storyId}/lastScene", nextSceneId);

            StartCoroutine(LoadScene(_controller.GetScene(nextSceneId)));
        }
        private void LoadImage(Image img, [CanBeNull] string url)
        {
            if (url == null)
            {
                img.sprite = defaultBackground;
                return;
            }

            Sprite sprite = _controller.GetSprite(url);
            if (sprite != null) img.sprite = sprite;
        }

        private void DrawImage(CanvasImage img)
        {
            constructor.DrawImage(img, _controller.GetSprite(img.img));
        }
    }
}