using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class UIManager : Managers
    {
        protected GameObject miniMap;
        protected GameObject bigMap;

        public bool bigMapOn;

        protected override void Initialization()
        {
            base.Initialization();
            miniMap = FindObjectOfType<MiniMapFinder>().gameObject;
            bigMap = FindObjectOfType<BigMapFinder>().gameObject;
            bigMap.SetActive(false);
        }
        protected virtual void Update()
        {
            if (player.GetComponent<InputManager>().BigMapPressed())
            {
                bigMapOn = !bigMapOn;
            }
            SwitchMaps();
        }
        protected virtual void SwitchMaps()
        {
            if (bigMapOn)
            {
                miniMap.SetActive(false);
                bigMap.SetActive(true);
            }
            else
            {
                miniMap.SetActive(true);
                bigMap.SetActive(false);
            }
        }
    }
}