using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointLightStarter : MonoBehaviour
{
    private WaitForSeconds wait = new WaitForSeconds(1f);

    private List<WeaponPointLight> lights = new List<WeaponPointLight>();

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++) 
        { 
            var child = transform.GetChild(i).GetComponent<WeaponPointLight>();

            if (child)
                lights.Add(child);
        }

        StartCoroutine(TurnOnPointLight());
    }

    private IEnumerator TurnOnPointLight()
    {
        while (true)
        {
            yield return wait;

            foreach(var child in lights)
                child.TurnOnLight();
        }
    }
}
