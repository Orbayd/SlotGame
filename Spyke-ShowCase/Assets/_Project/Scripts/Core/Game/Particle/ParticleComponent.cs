using System;
using UnityEngine;
namespace SpykeGames.Showcase.Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleComponent : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        public event Action ParticleSystemStopped;
        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            //gameObject.SetActive(false);
        }
        public void OnParticleSystemStopped()
        {  
            Stop();
        }

        public void Play(int maxParticles)
        {
            //gameObject.SetActive(true);
            var main = _particleSystem.main;
            main.maxParticles = maxParticles;
            _particleSystem.Play();
        }

        public void Stop()
        {
            _particleSystem.Stop();
            //gameObject.SetActive(false);
            ParticleSystemStopped?.Invoke();
        }
    }
}
