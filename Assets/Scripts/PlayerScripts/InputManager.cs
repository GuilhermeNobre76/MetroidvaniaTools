﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        protected KeyCode crouchHeld;
        [SerializeField]
        protected KeyCode dashPressed;
        [SerializeField]
        protected KeyCode sprintingHeld;
        [SerializeField]
        protected KeyCode jump;
        [SerializeField]
        protected KeyCode weaponFired;
        [SerializeField]
        protected KeyCode upHeld;
        [SerializeField]
        protected KeyCode downHeld;
        [SerializeField]
        protected KeyCode tiltedUpHeld;
        [SerializeField]
        protected KeyCode tiltedDownHeld;
        [SerializeField]
        protected KeyCode aimingHeld;

        // Update is called once per frame
        void Update()
        {
            CrouchHeld();
            DashPressed();
            SprintingHeld();
            JumpPressed();
            JumpHeld();
            WeaponFired();
            UpHeld();
            DownHeld();
            TiltedUpHeld();
            TiltedDownHeld();
            AimingHeld();
        }
        public virtual bool CrouchHeld()
        {
            if (Input.GetKey(crouchHeld))
            {
                return true;
            }
            return false;
        }
        public virtual bool DashPressed()
        {
            if (Input.GetKeyDown(dashPressed))
            {
                return true;
            }
            else
                return false;
        }
        public virtual bool SprintingHeld()
        {
            if (Input.GetKey(sprintingHeld))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool JumpHeld()
        {
            if (Input.GetKey(jump))
            {
                return true;
            }
            else
                return false;
        }
        public virtual bool JumpPressed()
        {
            if (Input.GetKeyDown(jump))
            {
                return true;
            }
            else
                return false;
        }
        public virtual bool WeaponFired()
        {
            if (Input.GetKeyDown(weaponFired))
            {
                return true;
            }
            else
                return false;
        }
        public virtual bool UpHeld()
        {
            if (Input.GetKey(upHeld))
            {
                return true;
            }
            else
                return false;
        }
        public virtual bool DownHeld()
        {
            if (Input.GetKey(downHeld))
            {
                return true;
            }
            else
                return false;
        }
        public virtual bool TiltedUpHeld()
        {
            if (Input.GetKey(tiltedUpHeld))
            {
                return true;
            }
            else
                return false;
        }
        public virtual bool TiltedDownHeld()
        {
            if (Input.GetKey(tiltedDownHeld))
            {
                return true;
            }
            else
                return false;
        }
        public virtual bool AimingHeld()
        {
            if (Input.GetKey(aimingHeld))
            {
                return true;
            }
            else
                return false;
        }
    }
}