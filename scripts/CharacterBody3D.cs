using Godot;
using System;

public partial class CharacterBody3D : Godot.CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float Acceleration = 0.6f;
	public const float JumpVelocity = 4.5f;
	public const float MouseSensitivity = 0.008f;
	private Node3D _head;
	private Camera3D _cam;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_head = GetNode<Node3D>("Head");
		_cam = GetNode<Camera3D>("Head/Camera3D");
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion m)
		{
			_head.RotateY(-m.Relative.X * MouseSensitivity);
			_cam.RotateX(-m.Relative.Y * MouseSensitivity);

			Vector3 camRot = _cam.Rotation;
			camRot.X = Mathf.Clamp(camRot.X,
				Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			_cam.Rotation = camRot;
		}
		// Exit mouse captured mode with Escape
		else if (@event is InputEventKey k && k.Keycode == Key.Escape)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("movement_left", "movement_right", "movement_forward", "movement_backward");
		Vector3 direction = (_head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Acceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Acceleration);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
