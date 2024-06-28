using Godot;
using System;
using System.Threading;

public partial class MainMenu : Control
{
	private AudioStreamPlayer _audioStreamPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        // Get the AudioStreamPlayer node
        _audioStreamPlayer = GetNode<AudioStreamPlayer>("ClickSound");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayButtonPressed()
	{
		if (_audioStreamPlayer != null)
        {
            _audioStreamPlayer.Play();
        }
		Thread.Sleep(200);
		GetTree().ChangeSceneToFile("res://scenes/game/game.tscn");
	}
	
	public void OnOptionsButtonPressed()
	{
		if (_audioStreamPlayer != null)
        {
            _audioStreamPlayer.Play();
        }
		Thread.Sleep(200);
	}

	public void OnQuitButtonPressed()
	{
		if (_audioStreamPlayer != null)
        {
            _audioStreamPlayer.Play();
        }
		Thread.Sleep(200);
		GetTree().Quit();
	}
}
