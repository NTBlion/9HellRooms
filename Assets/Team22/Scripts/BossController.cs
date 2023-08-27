using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Team22.Scripts
{
    public class BossController : MonoBehaviour
    {
        [System.Serializable]
        public struct RendererIndexData
        {
            public Renderer Renderer;
            public int MaterialIndex;

            public RendererIndexData(Renderer renderer, int index)
            {
                Renderer = renderer;
                MaterialIndex = index;
            }
        }
        
        private const string DieAnimationTrigger = "Died";
        [SerializeField] private Animator _bossAnimator;
        [SerializeField] private Health _health;
        [SerializeField] private AudioClip _damageTick;
        [SerializeField] private Gradient _onHitBodyGradient;
        [SerializeField] private float _flashOnHitDuration = 0.5f;
        [SerializeField] private Material _bodyMaterial;


        private bool _wasDamagedThisFrame = false;
        float _lastTimeDamaged = float.NegativeInfinity;
        List<RendererIndexData> m_BodyRenderers = new List<RendererIndexData>();
        MaterialPropertyBlock _mBodyFlashMaterialPropertyBlock;

        private void Start()
        {
            _mBodyFlashMaterialPropertyBlock = new MaterialPropertyBlock();
            _health.OnDie += OnDie;
            _health.OnDamaged += OnDamaged;
            
            foreach (var renderer in GetComponentsInChildren<Renderer>(true))
            {
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    if (renderer.sharedMaterials[i] == _bodyMaterial)
                    {
                        m_BodyRenderers.Add(new RendererIndexData(renderer, i));
                    }
                }
            }
        }

        private void Update()
        {
            Color currentColor = _onHitBodyGradient.Evaluate((Time.time - _lastTimeDamaged) / _flashOnHitDuration);
            _mBodyFlashMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
            foreach (var data in m_BodyRenderers)
            {
                data.Renderer.SetPropertyBlock(_mBodyFlashMaterialPropertyBlock, data.MaterialIndex);
            }

            _wasDamagedThisFrame = false;
        }

        private void OnDamaged(float arg0, GameObject damageSource)
        {
            if (damageSource)
            {
                _lastTimeDamaged = Time.time;
                
                if (_damageTick && !_wasDamagedThisFrame)
                    AudioUtility.CreateSFX(_damageTick, transform.position, AudioUtility.AudioGroups.DamageTick, 0f);

                _wasDamagedThisFrame = true;
            }
        }

        private void OnDie()
        {
            _bossAnimator.SetTrigger(DieAnimationTrigger);
        }
    }
}
