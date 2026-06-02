using UnityEngine;
using System.Collections.Generic;

namespace HollowNight.Player
{
    /// <summary>
    /// Manages player statistics, abilities, and progression
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 5;
        [SerializeField] private int maxSoul = 100;
        [SerializeField] private int baseDamage = 10;
        
        private int currentHealth;
        private int currentSoul;
        private int maskCount;
        private int vesselCount;
        private int attackBonus = 0;
        
        private HashSet<string> unlockedAbilities = new HashSet<string>();
        private HashSet<string> equippedCharms = new HashSet<string>();
        
        private void Start()
        {
            currentHealth = maxHealth;
            currentSoul = 0;
            maskCount = maxHealth;
            vesselCount = maxSoul;
        }
        
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath();
            }
        }
        
        public void Heal(int amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }
        
        public void AddSoul(int amount)
        {
            currentSoul = Mathf.Min(currentSoul + amount, maxSoul);
        }
        
        public void UseSoul(int amount)
        {
            if (currentSoul >= amount)
                currentSoul -= amount;
        }
        
        public void UnlockAbility(string abilityName)
        {
            unlockedAbilities.Add(abilityName);
            Debug.Log($"Ability Unlocked: {abilityName}");
        }
        
        public bool HasAbility(string abilityName)
        {
            return unlockedAbilities.Contains(abilityName);
        }
        
        public void EquipCharm(string charmName)
        {
            equippedCharms.Add(charmName);
        }
        
        public void UnequipCharm(string charmName)
        {
            equippedCharms.Remove(charmName);
        }
        
        public void ApplyAttackBonus(int bonus)
        {
            attackBonus += bonus;
        }
        
        public void AddMaxHealth(int amount)
        {
            maxHealth += amount;
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }
        
        public void AddMaxSoul(int amount)
        {
            maxSoul += amount;
        }
        
        private void OnDeath()
        {
            Debug.Log("Player Died!");
        }
        
        // Getters
        public int GetHealth() => currentHealth;
        public int GetMaxHealth() => maxHealth;
        public int GetSoul() => currentSoul;
        public int GetMaxSoul() => maxSoul;
        public int GetAttackBonus() => attackBonus;
        public int GetMaskCount() => maskCount;
        public int GetVesselCount() => vesselCount;
    }
}
