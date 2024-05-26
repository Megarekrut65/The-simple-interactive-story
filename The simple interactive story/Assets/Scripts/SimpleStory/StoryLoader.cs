using System.Collections;
using Global;
using Global.Sound;
using JetBrains.Annotations;
using Network;
using Network.Data;
using SimpleStory.Data;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleStory
{
    public class StoryLoader : MonoBehaviour
    {
        [SerializeField] private GameObject variantPrefab;
        
        [Header("Parts")]
        [SerializeField] private Transform content;
        [SerializeField] private Image background;
        
        [Header("Images")]
        [SerializeField] private Image left;
        [SerializeField] private Image right;
        [SerializeField] private Image centerUnder;
        [SerializeField] private Image centerOver;
        
        [Header("Texts")]
        [SerializeField] private GameObject textObj;
        [SerializeField] private Text text;
        [SerializeField] private Font font;
        [SerializeField] private FontManager fontManager;

        private StoryController _controller;

        private void Start()
        {
            string url = Constants.UserStories +
                         "rdMc08WAIpMKwZC7l8hdlHIXwUB2/stories/3f311d94-1a32-48b7-b7f6-cb04623fef49/scenes";
            Debug.Log(url);
            Fetcher.Get(this, url, 
                "", (e, s) =>
                {
                    Debug.Log(new FirebaseError(e));
                    Debug.Log(s);

                    FirestoreCollection<SceneFields> scenes = JsonUtility.FromJson<FirestoreCollection<SceneFields>>(s);
                    Scene[] scenesList = NetworkConverter.Convert(scenes);
                    foreach (var scene in scenesList)
                    {
                        Debug.Log(scene);
                    }
                });
            /*string storyId = LocalStorage.GetValue("storyId", "");
            string language = LocalStorage.GetValue("language", "en_US");
            if(storyId == "") return;
            
            StartCoroutine(Load(storyId, language));*/
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator Load(string storyId, string language)
        {
            _controller = new StoryController(storyId, language);

            //Font frameFont = fontManager.GetFont(_controller.Config.font);
            //if (frameFont != null) font = frameFont;
            text.GetComponent<Text>().font = font;
            
            //LoadFrame(_controller.GetFrame(_controller.Config.mainFrame));
            yield return null;
        }
        private void LoadFrame(Scene scene)
        {
            if(scene == null) return;

            LoadMusic(scene);
            
            //LoadText(scene.textKey);
            LoadAnswers(scene.answers);
            
            LoadImage(background, "Backgrounds",scene.background);
            
            //LoadImage(left, "Images",scene.images?.left);
            //LoadImage(right, "Images",scene.images?.right);
            //LoadImage(centerUnder, "Images",scene.images?.centerUnder);
            //LoadImage(centerOver, "Images",scene.images?.centerOver);
        }

        private void LoadMusic(Scene scene)
        {
            if(scene.music == null) return;
            
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
                
                //answer.textKey = _controller.GetWord(answer.textKey);
                obj.GetComponent<Variant>().SetData(answer, AnswerClick, font);
            }
        }

        private void AnswerClick(string frameId, string action)
        {
            if(frameId == null) return;

            LoadFrame(_controller.GetFrame(frameId));
            
            if(action == null) return;
            Debug.Log("Action");

        }
        private void LoadImage(Image img, string folder, [CanBeNull] string filename)
        {
            if(filename == null) return;

            Sprite sprite = _controller.GetSprite($"{folder}/{filename}");

            if (sprite != null) img.sprite = sprite;
        }
    }
}