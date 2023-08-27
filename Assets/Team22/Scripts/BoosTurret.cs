using System;
using UnityEngine;

namespace Team22.Scripts
{
    public class BoosTurret : MonoBehaviour
    {
        [SerializeField] private BossController _bossController;
        [SerializeField] private Transform _pivotAimPoint;
        [SerializeField] private Transform _pivot;
        [SerializeField] private  float _detectionFireDelay = 1f;
        [SerializeField] private float AimRotationSharpness = 5f;
        [SerializeField] private float LookAtRotationSharpness = 2.5f;
        [SerializeField] private float AimingTransitionBlendTime = 1f;
        [SerializeField] private BossStartShooting _startShooting;

        private Quaternion _previousPivotAimingRotation;
        private Quaternion m_RotationWeaponForwardToPivot;
        private Quaternion m_PivotAimingRotation;
        private float _timeStartedDetection;
        private float m_TimeLostDetection;
        private bool mustShoot;
        private bool _isEnable;

        private void Start()
        {
            Activate();
            mustShoot = false;
            _bossController.Died +=BossControllerOnDied;
            _startShooting.Shooting += StartShootingOnShooting;
            _startShooting.StopShooting += StartShootingOnStopShooting;
            _timeStartedDetection = Mathf.NegativeInfinity;
            _previousPivotAimingRotation = _pivotAimPoint.rotation;
            
            m_RotationWeaponForwardToPivot =
                Quaternion.Inverse(_bossController.GetCurrentWeapon().WeaponMuzzle.rotation) * _pivotAimPoint.rotation;
        }

        private void BossControllerOnDied(object sender, EventArgs e)
        {
            Deactivate();
        }

        private void StartShootingOnStopShooting(object sender, EventArgs e)
        {
            mustShoot = false;
        }

        private void StartShootingOnShooting(object sender, EventArgs e)
        {
            mustShoot = true;
        }

        private void Update()
        {
            if (!_isEnable)
                return;
            
            UpdateCurrentAiState();
        }

        private void LateUpdate()
        {
            if (!_isEnable)
                return;
            
            UpdateAiming();
        }

        public void Activate()
        {
            _isEnable = true;
        }

        public void Deactivate()
        {
            _isEnable = false;
        }

        private void UpdateCurrentAiState()
        {
            Vector3 directionToTarget =
                (_bossController.KnownDetectedTarget.transform.position - _pivotAimPoint.position).normalized;
            Quaternion offsettedTargetRotation =
                Quaternion.LookRotation(directionToTarget) * m_RotationWeaponForwardToPivot;
            m_PivotAimingRotation = Quaternion.Slerp(_previousPivotAimingRotation, offsettedTargetRotation,
                 AimRotationSharpness * Time.deltaTime);
            
            Vector3 correctedDirectionToTarget =
                (m_PivotAimingRotation * Quaternion.Inverse(m_RotationWeaponForwardToPivot)) *
                Vector3.forward;

            // shoot
            if (mustShoot)
            {
                _bossController.TryAttack((_pivotAimPoint.position + correctedDirectionToTarget));
            }
        }

        private void UpdateAiming()
        {
            _pivot.LookAt(_bossController.KnownDetectedTarget.transform.position, Vector3.up);
            
            _pivotAimPoint.rotation = m_PivotAimingRotation;
            _previousPivotAimingRotation = _pivotAimPoint.rotation;
        }
    }
}
