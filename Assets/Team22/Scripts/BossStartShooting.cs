using System;
using UnityEngine;

namespace Team22.Scripts
{
    public class BossStartShooting : MonoBehaviour
    {
        public event EventHandler Shooting;
        public event EventHandler StopShooting;

        public void StartShoot()
        {
            Shooting?.Invoke(this, EventArgs.Empty);
        }

        public void StopShoot()
        {
            StopShooting?.Invoke(this, EventArgs.Empty);
        }
    }
}
