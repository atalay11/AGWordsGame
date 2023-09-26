using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticles : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;

    void Start()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        System.Random random = new System.Random();
        int randomIndex = random.Next(sprites.Count);
        particleSystem.textureSheetAnimation.SetSprite(0, sprites[randomIndex]);
    }
}
