using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;
namespace MetroidvaniaTools
{
    public class AimManager : Abilities
    {
        public Solver2D aimingGun;
        public Solver2D aimingLeftHand;
        public Solver2D notAimingGun;
        public Solver2D notAimingLeftHand;
        public Transform whereToAim;
        public Transform whereToPlaceHand;
        public Transform origin;
        public Bounds bounds;

        protected override void Initialization()
        {
            base.Initialization();
            aimingGun.enabled = false;
            aimingLeftHand.enabled = false;
            bounds.center = origin.position;
        }
        protected virtual void FixedUpdate()
        {
            ChangeArms();
            bounds.center = origin.position;
        }
        public virtual void ChangeArms()
        {
            if(weapon.currentTimeTillChangeArms > 0)
            {
                notAimingGun.enabled = false;
                notAimingLeftHand.enabled = false;
                aimingGun.enabled = true;
                aimingLeftHand.enabled = true;
            }
            if (weapon.currentTimeTillChangeArms < 0)
            {
                notAimingGun.enabled = true;
                notAimingLeftHand.enabled = true;
                aimingGun.enabled = false;
                aimingLeftHand.enabled = false;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(origin.position, bounds.size);
        }
    }
}