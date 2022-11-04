using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using System.Linq;

public enum SelectedProperty
{
   none,
   geometryPrecision,
   colorPrecision,
   affineTextureWarping,
}


public class MaterialPropertyPhaser : MonoBehaviour
{
   public List<PropertyPhaseInfo> PhaseFX;
   void Start()
   {
      foreach (var FX in PhaseFX)
      {
         FX.Initialize(gameObject);
      }
   }
   void Update()
   {
      foreach (var FX in PhaseFX)
      {
         FX.UpdateFX();
      }
   } 
}
