using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Weapon : Abilities
    {
        [SerializeField]
        protected List<WeaponTypes> weaponTypes;
        [SerializeField]
        protected Transform gunBarrel;
        [SerializeField]
        protected Transform gunRotation;

        public List<GameObject> currentPool = new List<GameObject>();
        public GameObject currentProjectile;
        public WeaponTypes currentWeapon;
        public float currentTimeTillChangeArms;

        private GameObject projectileParentFolder;

        protected override void Initialization()
        {
            base.Initialization();
            foreach(WeaponTypes weapon in weaponTypes)
            {
                GameObject newPool = new GameObject();
                projectileParentFolder = newPool;
                objectPooler.CreatePool(weapon, currentPool, projectileParentFolder);
            }
            currentWeapon = weaponTypes[0];
        }
        protected virtual void Update()
        {
            if (input.WeaponFired())
            {
                FireWeapon();
            }
        }
        protected virtual void FixedUpdate()
        {
            PointGun();
            NegateTimeTillChangeArms();
        }
        protected virtual void FireWeapon()
        {
            currentTimeTillChangeArms = currentWeapon.lifeTime;
            currentProjectile = objectPooler.GetObject(currentPool);
            if(currentProjectile != null)
            {
                Invoke("PlaceProjectile", .1f);
            }
        }
        protected virtual void PointGun()
        {
            if (!character.isFacingLeft)
            {
                aimManager.whereToAim.position = new Vector2(aimManager.bounds.max.x, aimManager.bounds.center.y);
            }
            else
            {
                aimManager.whereToAim.position = new Vector2(aimManager.bounds.min.x, aimManager.bounds.center.y);
            }
            aimManager.aimingGun.transform.GetChild(0).position = aimManager.whereToAim.position;
            aimManager.aimingLeftHand.transform.GetChild(0).position = aimManager.whereToPlaceHand.position;
        }
        protected virtual void NegateTimeTillChangeArms()
        {
            currentTimeTillChangeArms -= Time.deltaTime;
        }
        protected virtual void PlaceProjectile()
        {
            currentProjectile.transform.position = gunBarrel.position;
            currentProjectile.transform.rotation = gunRotation.rotation;
            currentProjectile.SetActive(true);
            if (!character.isFacingLeft)
            {
                currentProjectile.GetComponent<Projectile>().left = false;
            }
            else
            {
                currentProjectile.GetComponent<Projectile>().left = true;
            }
            currentProjectile.GetComponent<Projectile>().fired = true;
        }
    }
}