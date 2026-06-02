using UnityEngine;
using System;

namespace HollowNight.Input
{
    /// <summary>
    /// Centralized input handling for the entire game
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        
        // Input events
        public event Action<Vector2> OnMove;
        public event Action OnJump;
        public event Action OnDash;
        public event Action OnAttack;
        public event Action OnCast;
        public event Action OnPause;
        
        private bool inputEnabled = true;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        private void Update()
        {
            if (!inputEnabled)
                return;
            
            // Movement input
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            OnMove?.Invoke(moveInput);
            
            // Jump input
            if (Input.GetKeyDown(KeyCode.Space))
                OnJump?.Invoke();
            
            // Dash input
            if (Input.GetKeyDown(KeyCode.LeftShift))
                OnDash?.Invoke();
            
            // Attack input
            if (Input.GetMouseButtonDown(0))
                OnAttack?.Invoke();
            
            // Cast spell input
            if (Input.GetMouseButtonDown(1))
                OnCast?.Invoke();
            
            // Pause input
            if (Input.GetKeyDown(KeyCode.Escape))
                OnPause?.Invoke();
        }
        
        public void EnableInput(bool enable)
        {
            inputEnabled = enable;
        }
        
        public bool IsInputEnabled() => inputEnabled;
    }
}
