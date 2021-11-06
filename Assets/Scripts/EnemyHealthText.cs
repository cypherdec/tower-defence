using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealthText : MonoBehaviour
{
   EnemyHealth enemyHealth;   
   Camera mCamera;
   [SerializeField] TextMeshPro hitpointText;

   void Start(){
       mCamera = FindObjectOfType<Camera>();
       enemyHealth = GetComponentInParent<EnemyHealth>();
   }
    void Update()
    {
        //textMeshTransform.rotation = Quaternion.LookRotation(textMeshTransform.position - Camera.main.transform.position);
        gameObject.transform.rotation = mCamera.transform.rotation;
        hitpointText.text = enemyHealth.CurrentHitPoints.ToString();
    }
}
