using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private YieldInstruction fadeInstruction = new YieldInstruction();

    public void SetTransparent(bool toDeactive)
    {
        if (toDeactive)
        {
            StartCoroutine(FadeOut());
            spriteRenderer.sortingOrder = Constants.DEACTIVE_SORTING_LAYER;
        }
        else
        {
            StartCoroutine(FadeIn());
            spriteRenderer.sortingOrder = Constants.ACTIVE_SORTING_LAYER;
        }
    }

    IEnumerator FadeOut()
    {
        //yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0.0f;
        Color c = spriteRenderer.color;
        while (elapsedTime < Constants.SPRITE_FADE_OUT_TIME)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / Constants.SPRITE_FADE_OUT_TIME);
            spriteRenderer.color = c;
            if (c.a < Constants.DEACTIVE_LAYER_ALPHA)
            {
                break;
            }
        }
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        Color c = spriteRenderer.color;
        while (elapsedTime < Constants.SPRITE_FADE_IN_TIME)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / Constants.SPRITE_FADE_IN_TIME);
            spriteRenderer.color = c;
        }
    }
}