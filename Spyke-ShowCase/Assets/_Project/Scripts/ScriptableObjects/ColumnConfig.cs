using SpykeGames.Showcase.Core.Enums;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "ColumnConfig", menuName = "ScriptableObjects/ColumnConfig", order = 1)]
public class ColumnConfig : ScriptableObject
{
    public float MinLimit = -5.0f;
    public float MaxLimit = 7.5f;
    public float DefaultRollTime = 0.75f;
    public float MaxSpintTime = 10;
    public SlotType[] SlotOrder;
    public SpriteAtlas Atlas;
}
