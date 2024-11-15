using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private TextMeshPro objectToFadeIn;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            objectToFadeIn.DOFade(1, 0.3f);
        }
    }
}
