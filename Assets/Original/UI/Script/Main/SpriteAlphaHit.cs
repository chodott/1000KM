using UnityEngine;
using UnityEngine.UI;

public class SpriteAlphaHit : MonoBehaviour
{
    Image image;
    Texture2D tex;

    void Awake()
    {
        image = GetComponent<Image>();

        // SpriteÀÇ ÅØ½ºÃ³ ²¨³»¿À±â
        if (image.sprite != null)
            tex = image.sprite.texture;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (tex == null) return false;

        RectTransform rect = GetComponent<RectTransform>();

        // ½ºÅ©¸° ÁÂÇ¥ ¡æ ·ÎÄÃ ÁÂÇ¥
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, sp, eventCamera, out Vector2 localPos);

        Rect r = rect.rect;
        float x = (localPos.x - r.x) / r.width;
        float y = (localPos.y - r.y) / r.height;

        if (x < 0 || x > 1 || y < 0 || y > 1) return false;

        // Sprite ³» ÁÂÇ¥ °è»ê
        int px = Mathf.RoundToInt(image.sprite.rect.x + x * image.sprite.rect.width);
        int py = Mathf.RoundToInt(image.sprite.rect.y + y * image.sprite.rect.height);

        // ÇÈ¼¿ ¾ËÆÄ°ª È®ÀÎ
        Color pixel = tex.GetPixel(px, py);
        return pixel.a > 0.1f; // ¾ËÆÄ 0.1 ÀÌ»óÀÌ¸é Å¬¸¯µÊ
    }
}
