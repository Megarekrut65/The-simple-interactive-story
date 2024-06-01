using System;
using Global.Localization;
using SimpleStory.Data;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleStory
{
    public class Variant: MonoBehaviour
    {
        [SerializeField]
        private AnswerClick answerClick;

        [SerializeField] private Text text;

        public void SetData(Answer answer, Action<string> click, Font font)
        {
            answerClick.Click = () => click(answer.nextScene);

            text.text = answer.text;
            text.font = font;
        } 
    }
}