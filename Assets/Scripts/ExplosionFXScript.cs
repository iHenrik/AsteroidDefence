using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFXScript : MonoBehaviour
{
    private const float WAIT_ANIMATION_TO_END = 5f;
    
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(WAIT_ANIMATION_TO_END);
        StopCoroutine(DestroySelf());
        GameObject.Destroy(gameObject);
    }
}
