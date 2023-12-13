using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointLight : MonoBehaviour
{
    private Animator anim;

    private WaitForSeconds wait = new WaitForSeconds(0.15f);

    private List<WeaponPointLight> lights = new List<WeaponPointLight>();

    private void Start()
    {
        anim = GetComponent<Animator>();

        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<WeaponPointLight>();

            if (child)
                lights.Add(child);
        }
    }

    public void TurnOnLight()
    {
        anim.SetTrigger("On");
        StartCoroutine(TurnOnLightChild());
    }

    private IEnumerator TurnOnLightChild()
    {
        yield return wait;

        foreach(var child in lights)
            child.TurnOnLight();
    }
}
