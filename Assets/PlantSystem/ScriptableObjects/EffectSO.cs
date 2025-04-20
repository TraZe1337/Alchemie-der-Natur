using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EffectSO", menuName = "Scriptable Objects/EffectSO")]
public class EffectSO : ScriptableObject
{
    public PlantHealthStages effectType;
    public Sprite effectImage;

}
