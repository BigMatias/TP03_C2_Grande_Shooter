using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    private bool _gamePaused = false;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        SetCursorState(locked: true);
    }

    private void OnDestroy()
    {
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_gamePaused)
            SetCursorState(true);
        
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        TogglePause();
    }
    
    public void TogglePause()
    {
        _gamePaused = !_gamePaused;

        Time.timeScale = _gamePaused ? 0f : 1f;
        SetCursorState(locked: !_gamePaused);
        pauseMenu.gameObject.SetActive(_gamePaused);
        if (!_gamePaused) optionsMenu.gameObject.SetActive(false);
    }

    private void SetCursorState(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}