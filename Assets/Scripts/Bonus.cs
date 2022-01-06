using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bonus
{
    [Range(0f, 1f)] public float chance;
    public BonusType type;
    public int rewardAmount;

    public void ApplyBonus(Player player, PlayerLoadout loadout)
    {
        if (type == BonusType.Mana)
        {
            if (loadout != null)
                loadout.AddMana(rewardAmount);
        }

        else if (type == BonusType.Health)
        {
            if (player != null)
                player.Heal(rewardAmount);
        }

        else if (type == BonusType.Gold)
            Progression.Instance.AddScore(rewardAmount);

        else
            Debug.Log("Bonus type not found!");

        Progression.Instance.SetBonus(rewardAmount, type.ToString());
    }
}

public enum BonusType
{
    Mana,
    Health,
    Gold
}