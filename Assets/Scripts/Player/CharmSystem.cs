using UnityEngine;
using System.Collections.Generic;

namespace HollowNight.Player
{
    /// <summary>
    /// Manages charm equipping and effects
    /// </summary>
    public class CharmSystem : MonoBehaviour
    {
        [System.Serializable]
        public class Charm
        {
            public string charmName;
            public int notchCost = 1;
            public int damageBonus = 0;
            public int healthBonus = 0;
            public float attackSpeedBonus = 0;
            public float movementSpeedBonus = 0;
            public int soulBonus = 0;
        }
        
        [SerializeField] private int maxNotches = 10;
        [SerializeField] private List<Charm> availableCharms = new List<Charm>();
        
        private List<Charm> equippedCharms = new List<Charm>();
        private int usedNotches = 0;
        private PlayerStats playerStats;
        
        private void Start()
        {
            playerStats = GetComponent<PlayerStats>();
        }
        
        public bool EquipCharm(string charmName)
        {
            Charm charm = availableCharms.Find(c => c.charmName == charmName);
            if (charm == null)
            {
                Debug.LogWarning($"Charm {charmName} not found!");
                return false;
            }
            
            // Check if we have enough notches
            if (usedNotches + charm.notchCost > maxNotches)
            {
                Debug.Log("Not enough notches to equip charm!");
                return false;
            }
            
            equippedCharms.Add(charm);
            usedNotches += charm.notchCost;
            
            // Apply charm effects
            ApplyCharmEffects(charm);
            
            Debug.Log($"Charm {charmName} equipped!");
            return true;
        }
        
        public bool UnequipCharm(string charmName)
        {
            Charm charm = equippedCharms.Find(c => c.charmName == charmName);
            if (charm == null)
                return false;
            
            equippedCharms.Remove(charm);
            usedNotches -= charm.notchCost;
            
            // Remove charm effects
            RemoveCharmEffects(charm);
            
            Debug.Log($"Charm {charmName} unequipped!");
            return true;
        }
        
        private void ApplyCharmEffects(Charm charm)
        {
            if (playerStats == null)
                return;
            
            if (charm.damageBonus > 0)
                playerStats.ApplyAttackBonus(charm.damageBonus);
            
            if (charm.healthBonus > 0)
                playerStats.AddMaxHealth(charm.healthBonus);
            
            if (charm.soulBonus > 0)
                playerStats.AddMaxSoul(charm.soulBonus);
        }
        
        private void RemoveCharmEffects(Charm charm)
        {
            if (playerStats == null)
                return;
            
            if (charm.damageBonus > 0)
                playerStats.ApplyAttackBonus(-charm.damageBonus);
        }
        
        public List<Charm> GetEquippedCharms() => equippedCharms;
        public int GetAvailableNotches() => maxNotches - usedNotches;
        public int GetUsedNotches() => usedNotches;
    }
}
