using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{

    public Button startButton;
    public Button pauseButton;
    public Button quitButton;
    public Button fasterButton;
    public Button slowerButton;
    public Label speedLabel;
    public Label peakLabel;
    public Label countLabel;

    private bool gameIsRunning;
    private float storedGameSpeed;
    private int peakBugCount = 0;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        gameIsRunning = false;
        Time.timeScale = 0f;
        storedGameSpeed = 1f;

        startButton = root.Q<Button>("StartGame-button");
        startButton.clicked += StartButtonPressed;

        pauseButton = root.Q<Button>("PauseGame-button");
        pauseButton.clicked += PauseButtonPressed;

        fasterButton = root.Q<Button>("GameFaster-button");
        fasterButton.clicked += FasterButtonPressed;

        slowerButton = root.Q<Button>("GameSlower-button");
        slowerButton.clicked += SlowerButtonPressed;

        quitButton = root.Q<Button>("QuitGame-button");
        quitButton.clicked += QuitButtonPressed;

        speedLabel = root.Q<Label>("GameSpeed-label");
        speedLabel.text = "Game speed (1-4): Paused";

        peakLabel = root.Q<Label>("BugPeak-label");
        peakLabel.text = "Peak Bugs: " + peakBugCount;
        countLabel = root.Q<Label>("BugCount-label");
        countLabel.text = "Current Bugs: " + peakBugCount;
    }

    void StartButtonPressed()
    {
        if (!gameIsRunning)
        {
            Time.timeScale = storedGameSpeed;
            gameIsRunning = true;
            speedLabel.text = "Game speed (1-4): " + (int)Time.timeScale;
        }
    }
    void PauseButtonPressed()
    {
        if (gameIsRunning)
        {
            storedGameSpeed = Time.timeScale;
            Time.timeScale = 0f;
            gameIsRunning = false;
            speedLabel.text = "Game speed (1-4): Paused";
        }
    }
    void FasterButtonPressed()
    {
        if (gameIsRunning && 4f > Time.timeScale)
        {
            Time.timeScale += 1f;
            speedLabel.text = "Game speed (1-4): " + (int)Time.timeScale;
        }
    }
    void SlowerButtonPressed()
    {
        if (gameIsRunning && 1f < Time.timeScale)
        {
            Time.timeScale -= 1f;
            speedLabel.text = "Game speed (1-4): " + (int)Time.timeScale;
        }
    }
    void QuitButtonPressed()
    {
        Application.Quit();
    }
    public void UpdateBugCount(int newCount)
    {
        countLabel.text = "Current Bugs: " + newCount;

        if (newCount > peakBugCount)
        {
            peakBugCount = newCount;
            peakLabel.text = "Peak Bugs: " + peakBugCount;
        }
    }
}
