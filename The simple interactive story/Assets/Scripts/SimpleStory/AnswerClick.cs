using System;
using UnityEngine;

namespace SimpleStory
{
    public class AnswerClick : MonoBehaviour
    {
        public Action Click { get; set; }

        public void ClickEvent()
        {
            Click?.Invoke();
        }
    }
}