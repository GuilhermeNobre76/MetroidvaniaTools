using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class UIManager : Managers
    {
        protected GameObject miniMap;
        protected GameObject bigMap;
        protected float originalTimeScale;

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
                SwitchMaps();
            }
            if (bigMapOn)
            {
                MoveMap();
            }
        }
        protected virtual void SwitchMaps()
        {
            if (bigMapOn)
            {
                miniMap.SetActive(false);
                bigMap.SetActive(true);
                gameManager.gamePaused = true;
                originalTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                miniMap.SetActive(true);
                bigMap.SetActive(false);
                gameManager.gamePaused = false;
                Time.timeScale = originalTimeScale;
            }
        }
        protected virtual void MoveMap()
        {
            float vertical = new float();
            float horizontal = new float();
            if (input.UpHeld())
            {
                vertical = .25f;
            }
            if (input.DownHeld())
            {
                vertical = -.25f;
            }
            if (input.LeftHeld())
            {
                horizontal = -.25f;
            }
            if (input.RightHeld())
            {
                horizontal = .25f;
            }
            Vector3 currentPosition = bigMapCamera.transform.position;
            bigMapCamera.transform.position = new Vector3(currentPosition.x + horizontal, currentPosition.y + vertical, -10);
        }
    }
}