using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveShaderController : MonoBehaviour
{

    [SerializeField] private float shockwaveTime = 0.75f;
    private Coroutine shockwaveCoroutine;
    [SerializeField] private Material material;
    private static int distance = Shader.PropertyToID("_distance");


    public void startShockwave()
    {
        shockwaveCoroutine = StartCoroutine(ShockwaveEnumerator(-0.1f, 0.1f));
    }

    private IEnumerator ShockwaveEnumerator(float startPos, float endPos)
    {
        float lerpedAmount = 0f;
        
        float elapsedTime = 0f;

        while (elapsedTime < shockwaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / shockwaveTime);
            material.SetFloat(distance, lerpedAmount);

            yield return null;
        }
    }
}
