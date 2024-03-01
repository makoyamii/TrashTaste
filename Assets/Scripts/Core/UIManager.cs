using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   public GameObject gameOverMenu;
   private void  OnEnable() {
        Health.OnPlayerDeath += EnableGamerOverMenu;
   }

    private void OnDisable() {
        Health.OnPlayerDeath -= EnableGamerOverMenu;
    }

   public void EnableGamerOverMenu() {
        gameOverMenu.SetActive(true);
   }
}
