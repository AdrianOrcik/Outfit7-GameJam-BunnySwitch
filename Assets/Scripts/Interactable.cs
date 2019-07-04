using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public BoxCollider2D BoxCollider2D;
    private YieldInstruction fadeInstruction = new YieldInstruction();

    public void SetTransparent(bool toActive)
    {
        if (toActive)
        {
            if (SpriteRenderer != null)
            {
                StartCoroutine(FadeIn());
                SpriteRenderer.sortingOrder = Constants.ACTIVE_SORTING_LAYER;
            }

            BoxCollider2D.enabled = true;
        }
        else
        {
            if (SpriteRenderer != null)
            {
                StartCoroutine(FadeOut());
                SpriteRenderer.sortingOrder = Constants.DEACTIVE_SORTING_LAYER;
            }

            BoxCollider2D.enabled = false;
        }
    }

    IEnumerator FadeOut()
    {
        //yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0.0f;
        Color c = SpriteRenderer.color;
        while (elapsedTime < Constants.SPRITE_FADE_OUT_TIME)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / Constants.SPRITE_FADE_OUT_TIME);
            SpriteRenderer.color = c;
            if (c.a < Constants.DEACTIVE_LAYER_ALPHA)
            {
                break;
            }
        }
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        Color c = SpriteRenderer.color;
        while (elapsedTime < Constants.SPRITE_FADE_IN_TIME)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / Constants.SPRITE_FADE_IN_TIME);
            SpriteRenderer.color = c;
        }
    }
}