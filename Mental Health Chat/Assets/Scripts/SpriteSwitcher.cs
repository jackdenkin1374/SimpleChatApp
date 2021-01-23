using UnityEngine;
using System.Collections;
using Yarn.Unity;

[RequireComponent (typeof (SpriteRenderer))]
/// Attach SpriteSwitcher to game object
public class SpriteSwitcher : MonoBehaviour {

    [System.Serializable]
    public struct SpriteInfo {
        public string name;
        public Sprite sprite;
    }

    public SpriteInfo[] sprites;

    /// Create a command to use on a sprite
    [YarnCommand("setsprite")]
    public void UseSprite(string spriteName) {

        Sprite s = null;
        foreach(var info in sprites) {
            if (info.name == spriteName) {
                s = info.sprite;
                break;
            }
            }
        if (s == null) {
            Debug.LogErrorFormat("Can't find sprite named {0}!", spriteName);
            return;
        }

        GetComponent<SpriteRenderer>().sprite = s;
    }
}
