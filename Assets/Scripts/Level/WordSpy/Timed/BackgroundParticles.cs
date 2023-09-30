using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticles : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private Camera canvasCamera;

    ParticleSystem m_bgParticleSystem;

    float m_cameraHeight;

    private void Awake()
    {
        m_bgParticleSystem = GetComponent<ParticleSystem>();

        // Select Random Sprite

        System.Random random = new System.Random();
        int randomIndex = random.Next(sprites.Count);
        m_bgParticleSystem.textureSheetAnimation.SetSprite(0, sprites[randomIndex]);

        // Scale the Particles Box
        float cameraWidth = canvasCamera.orthographicSize * 2.0f * canvasCamera.aspect;
        float cameraHeight = canvasCamera.orthographicSize * 2f;
        Debug.Log($"cameraWidth: {cameraWidth}");

        var psShape = m_bgParticleSystem.shape;
        psShape.shapeType = ParticleSystemShapeType.Box;
        var oldScale = psShape.scale;
        oldScale.x = cameraWidth;
        psShape.scale = oldScale;
        psShape.enabled = true;

        m_cameraHeight = cameraHeight;
    }

    private void Update()
    {

        // Loop through each particle in the Particle System
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[m_bgParticleSystem.particleCount];
        int particleCount = m_bgParticleSystem.GetParticles(particles);

        for (int i = 0; i < particleCount; i++)
        {
            // this is local position from the particle system
            // particleSystem is at the top of the camera bounds
            // if particles distance is greater than camera height 
            // they should be destroyed
            Vector3 particlePosition = particles[i].position;

            // Check if the particle is outside the camera's viewport
            const float positionErrorMargin = 0.5f;
            if (particlePosition.y < -(m_cameraHeight + positionErrorMargin))
            {
                // If the particle is outside the camera view, destroy it
                particles[i].remainingLifetime = 0f;
            }
        }

        // Apply the modified particle array back to the Particle System
        m_bgParticleSystem.SetParticles(particles, particleCount);
    }
}