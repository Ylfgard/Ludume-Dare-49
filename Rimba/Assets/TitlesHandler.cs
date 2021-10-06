using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlesHandler : MonoBehaviour
{
    [SerializeField] private float delayOnStart;
    [SerializeField] private float speed;
    [SerializeField] private Scrollbar titlesScrollbar;

    private void Start()
    {
        speed *= 0.01f;
        titlesScrollbar.value = 1;
        StartCoroutine(DelaySlidingOnStart());
    }

    private IEnumerator DelaySlidingOnStart()
    {
        yield return new WaitForSeconds(delayOnStart);
        StartCoroutine(InitSliding());
    }

    private IEnumerator InitSliding()
    {
        if(titlesScrollbar.value > 0)
        {
            titlesScrollbar.value -= speed * Time.deltaTime;
            yield return null;
            StartCoroutine(InitSliding());
        }
        else
        {
            titlesScrollbar.value = 0;
            yield return null;
        }
    }
}
