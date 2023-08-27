using System;
using System.Collections.Generic;
using Unity.FPS.AI;
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
        [SerializeField] private DetectionModule _detectionModule;
        [SerializeField] private Actor _actor;
        [SerializeField] private GameObject _lootPrefab;
        [SerializeField] private Transform _spawnLootPoint;
        [SerializeField] private GameFlowManager _gameFlowManager;


        private bool _wasDamagedThisFrame = false;
        float _lastTimeDamaged = float.NegativeInfinity;
        private List<RendererIndexData> m_BodyRenderers = new List<RendererIndexData>();
        private MaterialPropertyBlock _mBodyFlashMaterialPropertyBlock;
        private Collider[] _selfColliders;
        private WeaponController _currentWeapon;
        private WeaponController[] _weapons;
        private int m_CurrentWeaponIndex;

        public event EventHandler Died;

        public GameObject KnownDetectedTarget => _detectionModule.KnownDetectedTarget;

        private void Start()
        {
            _mBodyFlashMaterialPropertyBlock = new MaterialPropertyBlock();
            _health.OnDie += OnDie;
            _health.OnDamaged += OnDamaged;
            
            _selfColliders = GetComponentsInChildren<Collider>();
            
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
            _detectionModule.HandleTargetDetection(_actor, _selfColliders);
            
            Color currentColor = _onHitBodyGradient.Evaluate((Time.time - _lastTimeDamaged) / _flashOnHitDuration);
            _mBodyFlashMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
            foreach (var data in m_BodyRenderers)
            {
                data.Renderer.SetPropertyBlock(_mBodyFlashMaterialPropertyBlock, data.MaterialIndex);
            }

            _wasDamagedThisFrame = false;
        }

        public bool TryAttack(Vector3 point)
        {
            if (_gameFlowManager.GameIsEnding)
                return false;
            
            bool didFire = GetCurrentWeapon().HandleShootInputs(false, true, false);

            return didFire;
        }

        public WeaponController GetCurrentWeapon()
        {
            FindAndInitializeAllWeapons();
            // Check if no weapon is currently selected
            if (_currentWeapon == null)
            {
                // Set the first weapon of the weapons list as the current weapon
                SetCurrentWeapon(0);
            }

            DebugUtility.HandleErrorIfNullGetComponent<WeaponController, EnemyController>(_currentWeapon, this,
                gameObject);

            return _currentWeapon;
        }
        
        private void FindAndInitializeAllWeapons()
        {
            // Check if we already found and initialized the weapons
            if (_weapons == null)
            {
                _weapons = GetComponentsInChildren<WeaponController>();
                DebugUtility.HandleErrorIfNoComponentFound<WeaponController, EnemyController>(_weapons.Length, this,
                    gameObject);

                for (int i = 0; i < _weapons.Length; i++)
                {
                    _weapons[i].Owner = gameObject;
                }
            }
        }
        
        private void SetCurrentWeapon(int index)
        {
            m_CurrentWeaponIndex = index;
            _currentWeapon = _weapons[m_CurrentWeaponIndex];
        }

        
        private void OnDamaged(float damage, GameObject damageSource)
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
            Died?.Invoke(this, EventArgs.Empty);

            foreach (Collider selfCollider in _selfColliders)
            {
                selfCollider.enabled = false;
            }

            int thrust = 2;
            GameObject loot = Instantiate(_lootPrefab, _spawnLootPoint.position, Quaternion.identity);
            loot.GetComponent<Rigidbody>().AddForce( transform.forward * thrust, ForceMode.Impulse);
        }
    }
}
