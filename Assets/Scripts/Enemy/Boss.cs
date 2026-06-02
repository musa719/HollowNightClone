using UnityEngine;

namespace HollowNight.Enemy
{
    /// <summary>
    /// Base class for boss enemies with multiple phases
    /// </summary>
    public abstract class Boss : Enemy
    {
        [SerializeField] protected int phase = 1;
        [SerializeField] protected int maxPhases = 3;
        [SerializeField] protected float phaseTransitionHealth = 0.66f;
        
        protected bool inTransition = false;
        
        protected override void Awake()
        {
            base.Awake();
            moveSpeed = 2f;
            detectionRange = 30f;
        }
        
        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            
            CheckPhaseTransition();
        }
        
        protected virtual void CheckPhaseTransition()
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            
            if (healthPercentage < (1f - (phaseTransitionHealth * phase)) && phase < maxPhases)
            {
                TransitionToPhase(phase + 1);
            }
        }
        
        protected virtual void TransitionToPhase(int newPhase)
        {
            if (inTransition)
                return;
            
            inTransition = true;
            phase = newPhase;
            
            if (animator != null)
                animator.SetInteger("Phase", phase);
            
            Debug.Log($"Boss transitioned to phase {phase}");
            
            Invoke(nameof(EndTransition), 1.5f);
        }
        
        protected void EndTransition()
        {
            inTransition = false;
        }
        
        public int GetCurrentPhase() => phase;
        public bool IsInTransition() => inTransition;
    }
}
