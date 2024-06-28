using Godot;
using System;

public partial class splash_screen : Control
{
	private AnimationPlayer animationPlayer;
    private ColorRect fadeRect;
    private Timer initialDelayTimer;
    private bool isFadingOut = false;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        fadeRect = GetNode<ColorRect>("FadeRect");
        initialDelayTimer = GetNode<Timer>("InitialDelayTimer");

        // Ensure logos are hidden initially
        GetNode<TextureRect>("Logo").Visible = false;
        GetNode<TextureRect>("Warning").Visible = false;

        // Connect the timeout signal of the timer
        initialDelayTimer.Connect("timeout", new Callable(this, nameof(OnInitialDelayTimeout)));

        // Start the initial delay timer
        initialDelayTimer.Start();
    }

    public override void _Process(double delta)
    {
        // Check for key presses to skip the splash screen
        if (Input.IsActionJustPressed("ui_skip"))
        {
            if (!isFadingOut)
            {
                SkipSplashScreen();
            }
        }
    }

    private void OnInitialDelayTimeout()
    {
        // Start the fade-in animation
        animationPlayer.Play("fade_in");
        animationPlayer.Connect("animation_finished", new Callable(this, nameof(OnFadeInFinished)));
    }

    private void OnFadeInFinished(String animName)
    {
        if (animName == "fade_in")
        {
            // Show the first logo and play its animation
            GetNode<TextureRect>("Logo").Visible = true;
            animationPlayer.Play("text1_animation");
            animationPlayer.Connect("animation_finished", new Callable(this, nameof(OnLogo1AnimationFinished)));
        }
    }

    private void OnLogo1AnimationFinished(String animName)
    {
        if (animName == "logo1_animation")
        {
            // Hide the first logo, show the second logo, and play its animation
            GetNode<TextureRect>("Logo").Visible = false;
            GetNode<TextureRect>("Warning").Visible = true;
            animationPlayer.Play("text2_animation");
            animationPlayer.Connect("animation_finished", new Callable(this, nameof(OnLogo2AnimationFinished)));
        }
    }

    private void OnLogo2AnimationFinished(String animName)
    {
        if (animName == "text2_animation")
        {
            // Start the fade-out animation
            animationPlayer.Play("fade_out");
            animationPlayer.Connect("animation_finished", new Callable(this, nameof(OnFadeOutFinished)));
        }
    }

    private void OnFadeOutFinished(String animName)
    {
        if (animName == "fade_out")
        {
            // Switch to the main menu scene
            GetTree().ChangeSceneToFile("res://scenes/main_menu/main_menu.tscn");
        }
    }

    private void SkipSplashScreen()
    {
        isFadingOut = true;
        // Immediately finish the splash screen and switch to the main menu scene
        GetTree().ChangeSceneToFile("res://scenes/main_menu/main_menu.tscn");
    }
}







