using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContentSizeImage : MonoBehaviour {

    private Image image;
    private LayoutElement le;
    private RectTransform rt;

    private bool ok=true;

    private void Awake()
    {
        image = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
        le = GetComponent<LayoutElement>();
        if(le == null)
        {
            le = gameObject.AddComponent<LayoutElement>();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        if(ok)
        {
            if (rt.rect.width != 0)
            {
                float width = rt.rect.width;
                Texture2D texture = image.sprite.texture;
                float height = texture.height * width / texture.width;
                le.preferredHeight = height;
                ok = false;
            }
        }
	}
}
