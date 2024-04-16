using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class LevelSO : ScriptableObject
{
    [FormerlySerializedAs("numLevel")]
    public int levelNumber;
    public Sprite levelImage;
}
