using UnityEngine;

namespace HollowNight.Enemy
{
    /// <summary>
    /// Example boss enemy with specific attack patterns
    /// </summary>
    public class BossEnemy : Boss
    {
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private Transform[] attackPoints;
        [SerializeField] private GameObject projectilePrefab;
        
        private float lastAttackTime;
        private Transform playerTransform;
        
        protected override void Awake()
        {
            base.Awake();
            maxHealth = 100;
            damage = 15;
            soulDrop = 50;
        }
        
        private void Start()
        {
            Core.GameManager gameManager = Core.GameManager.Instance;
            if (gameManager != null)
            {
                Player.Player player = gameManager.GetPlayer();
                if (player != null)
                    playerTransform = player.transform;
            }
        }
        
        private void Update()
        {
            if (!isAlive || inTransition)
                return;
            
            if (playerTransform != null && CanSeePlayer(playerTransform))
            {
                HandleCombat();
            }
        }
        
        private void HandleCombat()
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            
            // Move towards player
            if (distance > 3f)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
            }
            
            // Attack
            if (Time.time - lastAttackTime > attackCooldown)
            {
                PerformPhaseAttack();
                lastAttackTime = Time.time;
            }
        }
        
        private void PerformPhaseAttack()
        {
            switch (phase)
            {
                case 1:
                    SimpleAttack();
                    break;
                case 2:
                    SimpleAttack();
                    RangedAttack();
                    break;
                case 3:
                    SimpleAttack();
                    RangedAttack();
                    DashAttack();
                    break;
            }
        }
        
        private void SimpleAttack()
        {
            // Simple melee attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);
            
            foreach (Collider2D hit in hitEnemies)
            {
                if (hit.CompareTag("Player"))
                {
                    if (hit.TryGetComponent<Player.Player>(out var player))
                    {
                        player.GetComponent<Player.PlayerStats>().TakeDamage(damage);
                    }
                }
            }
        }
        
        private void RangedAttack()
        {
            // Ranged attack using projectiles
            if (projectilePrefab != null && playerTransform != null)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                
                if (projectile.TryGetComponent<Rigidbody2D>(out var projRb))
                {
                    projRb.velocity = direction * 10f;
                }
            }
        }
        
        private void DashAttack()
        {
            // Dash towards player
            if (playerTransform != null)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                rb.velocity = direction * moveSpeed * 2f;
            }
        }
    }
}
