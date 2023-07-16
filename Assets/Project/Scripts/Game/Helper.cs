using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public enum MantraIndex
    {
        WeaponMantra,
        HealingMantra
    }
    [SerializeField] private static AudioClip[] mantras;
    public static float GetDistanceBetweenTwoObjects(Vector3 element1, Vector3 element2)
    {
        return Vector3.Distance(element1, element2);
    }

    public static IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }

    public static AudioClip[] getMantras()
    {
        if (mantras == null)
        {
            mantras = new AudioClip[2];
            mantras[0] = Resources.Load<AudioClip>("Audio/OmNamoBhagavateVasudevaya");
            mantras[1] = Resources.Load<AudioClip>("Audio/HareKrishnaRama");
        }
        return mantras;
    }
}
