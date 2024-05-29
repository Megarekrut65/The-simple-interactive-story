using System.Collections;
using Global;
using Global.Sound;
using JetBrains.Annotations;
using Network;
using Network.Data;
using SimpleStory.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scene = SimpleStory.Data.Scene;

namespace SimpleStory
{
    public class StoryLoader : MonoBehaviour
    {
        [SerializeField] private GameObject variantPrefab;
        
        [Header("Parts")]
        [SerializeField] private Transform content;
        [SerializeField] private Image background;
        
        [Header("Images")]

        [Header("Texts")]
        [SerializeField] private GameObject textObj;
        [SerializeField] private Text text;
        [SerializeField] private FontManager fontManager;

        private StoryController _controller = new StoryController();
        private Font _currentFont;

        private void Start()
        {
            string storyId = LocalStorage.GetValue("storyId", "");
            string userId = LocalStorage.GetValue("userId", "");
            string fontName = LocalStorage.GetValue("font", "");
            if (storyId == "" || userId == "")
            {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
                return;
            }

            _currentFont = fontManager.GetFont(fontName);
            text.GetComponent<Text>().font = _currentFont;

            Fetcher.Get(this, Constants.GetScenesPath(userId, storyId),
                "", FetchScenesCallback);
        }

        private void FetchScenesCallback([CanBeNull] string error, [CanBeNull] string result)
        {
            if (error != null)
            {
                FirebaseError err = new FirebaseError(error);
                Debug.Log(err);
                return;
            }

            if (result == null)
            {
                Debug.Log("Story is null");
                return;
            }
            
            FirestoreCollection<SceneFields> scenes = JsonUtility.FromJson<FirestoreCollection<SceneFields>>(result);
            Scene[] list = NetworkConverter.Convert(scenes);

            _controller = new StoryController(list);
            LoadScene(_controller.GetScene());
        }

        private void LoadScene(Scene scene)
        {
            if(scene == null) return;
            Debug.Log(scene);

            LoadMusic(scene);
            
            LoadText(scene.text);
            LoadAnswers(scene.answers);
            
            LoadImage(background, scene.background);

            if(scene.images == null) return;
            
            foreach (CanvasImage img in scene.images)
            {
                DrawImage(img);
            }
        }

        private void LoadMusic(Scene scene)
        {
            if(scene.music == null) return;
            StartCoroutine(_controller.LoadClip(scene.music, MusicManager.ChangeAudioClip));
            
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
            if(nextSceneId == null) return;

            LoadScene(_controller.GetScene(nextSceneId));
        }
        private void LoadImage(Image img, [CanBeNull] string url)
        {
            if( url == null) return;

            StartCoroutine(_controller.LoadSprite(url, (sprite) =>
            {
                if (sprite != null) img.sprite = sprite;
            }));
        }

        private void DrawImage(CanvasImage img)
        {
            
        }
    }
}