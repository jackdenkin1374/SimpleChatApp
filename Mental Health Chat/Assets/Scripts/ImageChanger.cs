using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    [System.Serializable]
    public struct ImageInfo {
        public string name;
        public Image image;
    }
}
