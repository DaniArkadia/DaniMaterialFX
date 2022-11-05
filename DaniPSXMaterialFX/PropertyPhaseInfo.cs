using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using System.Linq;
using System;

[System.Serializable]
public class PropertyPhaseInfo
{
   public SelectedProperty selectedProperty;
   public AnimationCurve curve;
   public float speed = 1;
   public List<int> materialsToIgnore;
   int currentMaterialIndex;
   string name;
   Action onPhaseProperty;
   float currentCurveTime;
   Renderer meshRenderer;

   public void Initialize(GameObject owner)
   {
      meshRenderer = owner.GetComponent<Renderer>();
      switch (selectedProperty)
      {
         case SelectedProperty.geometryPrecision:
         {
            onPhaseProperty = PhaseGeometryPrecision;
            break;
         } 
         case SelectedProperty.colorPrecision:
         {
            onPhaseProperty = PhaseColorPrecision;
            break;
         }
         case SelectedProperty.affineTextureWarping:
         {
            onPhaseProperty = PhaseAffineTextureWarping;
            break;
         }
      }
   }
   public void UpdateFX()
   {
      if (curve.keys.Count() == 0) return;
      for (int i = 0; i < meshRenderer.materials.Count(); i++)
      {
         if (shouldIgnoreIndex(i))
         {
            continue;
         }
         currentMaterialIndex = i;
         onPhaseProperty?.Invoke(); 
      }

      if (currentCurveTime < curve.keys.Last().time)
      {
         currentCurveTime += Time.deltaTime * speed;
      }
      else
      {
         currentCurveTime = 0;
      } 

      bool shouldIgnoreIndex(int index)
      {
         foreach (var ignoredIndex in materialsToIgnore)
         {
            if (index == ignoredIndex)
            {
               return true;
            }
         }
         return false;
      }  
   } 
   void PhaseGeometryPrecision()
   {
      var e = curve.Evaluate(currentCurveTime);
      meshRenderer.materials[currentMaterialIndex].SetColor("_PrecisionGeometryOverrideParameters", new Color(e,e,e,1));
   }

   void PhaseColorPrecision()
   {
      var e = curve.Evaluate(currentCurveTime);
      meshRenderer.materials[currentMaterialIndex].SetColor("_PrecisionColorOverrideParameters", new Color(e,e,e,1));
   }

   void PhaseAffineTextureWarping()
   {
      var e = curve.Evaluate(currentCurveTime);
      meshRenderer.materials[currentMaterialIndex].SetFloat("_AffineTextureWarpingWeight", e);
   }

}
