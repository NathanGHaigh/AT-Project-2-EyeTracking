using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Light))]
public class LightLOD : MonoBehaviour
{
    private Light _light;
    public bool LightShouldBeOn = true;
    [Range(0,1f)]
    [SerializeField]private float updateDelay = 1f;
    [SerializeField]
    private List<LODAdjustments> LODLevels = new();

    private void Awake()
    {
        _light = GetComponent<Light>();
        Debug.Log("LightLOD Awake: " + _light.name);
        //DesiredLightShadows = _light.shadows;
    }

    private void OnEnable()
    {
        StartCoroutine(AdjustLODQuality());
    }

    private IEnumerator AdjustLODQuality()
    {
        float delay = updateDelay + (updateDelay == 0 ? updateDelay : UnityEngine.Random.value / 20f);

        WaitForSecondsRealtime wait = new(delay);

        while (true)
        {
            if (LightLODCamera.Instance == null)
            {
                yield return wait;
                continue;
            }
            if(LightShouldBeOn)
            {
                Debug.Log("Adjusting LOD for light: " + _light.name);
                float squareDistanceToCamera = Vector3.SqrMagnitude(LightLODCamera.Instance.transform.position - transform.position);

                for(int i = 0; i < LODLevels.Count; i++)
                {
                    if (i == LODLevels.Count - 1 || (
                        squareDistanceToCamera > LODLevels[i].MinSquareDistance
                        && squareDistanceToCamera <= LODLevels[i].MaxSquareDistance
                    ))
                    {
                        _light.enabled = true;
                        _light.shadows = LODLevels[i].LightShadows;
                        if(QualitySettings.shadowResolution <= LODLevels[i].ShadowResolution)
                        {
                            _light.shadowResolution = (LightShadowResolution)QualitySettings.shadowResolution;
                        }
                        else
                        {
                            _light.shadowResolution = (LightShadowResolution)LODLevels[i].ShadowResolution;
                        }

                        break;
                    }
                }

            }
            else
            {
                _light.enabled = false;
            }

            yield return wait;

        }

    }
}
