using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
    public List<Sprite> sprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
        spriteRenderer.color = new Color(Random.Range(.3f, .6f), 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
}
