using HughUIType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughCanvas
{
    public class CanvasManager : MonoBehaviour
    {
        private Canvas canvas;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        public void ShowCanvas()
        {
            this.canvas.enabled = true;
        }

        public void HideCanvas()
        {
            this.canvas.enabled = false;
        }
    }
}
