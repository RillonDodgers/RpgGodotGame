using Godot;
using System;
using System.ComponentModel;

/// <summary></summary>
public partial class Player : CharacterBody2D {
	[Export]
	[DefaultValue(200f)]
	public float speed { get; set; }

	public bool IsAttacking = false;

	public Vector2 new_direction = new Vector2(0, 1);

	public override void _PhysicsProcess(double delta) {
		Vector2 direction = Velocity;

		GD.Print(direction);

		direction.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		direction.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		
		if (Mathf.Abs(direction.X) == 1 && Mathf.Abs(direction.Y) == 1) {
			direction = direction.Normalized();
		}

		if (Input.IsActionPressed("ui_sprint")) {
			speed = 400f;
		}

		if (Input.IsActionJustReleased("ui_sprint")) {
			speed = 200f;
		}

		Vector2 movement;
		movement.X = direction.X * speed * (float)delta;
		movement.Y = direction.Y * speed * (float)delta;

		if (!IsAttacking) {
			MoveAndCollide(movement);
			PlayerAnimations(direction);
		}
	}

	public void PlayerAnimations(Vector2 direction) {
		var AnimatedSprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;

		string facing = ReturnedDirection(new_direction);

		if (direction != Vector2.Zero) {
			GD.Print(facing);
			new_direction = direction;
			AnimatedSprite.Play($"walk_{facing}");
		} else {
			GD.Print(facing);
			AnimatedSprite.Play($"idle_{facing}");
		}
	}

	public string ReturnedDirection(Vector2 direction) {
		Vector2 normalized_direction = direction.Normalized();

		if (normalized_direction.Y >= 1) {
			return "down";
		} else if (normalized_direction.Y <= -1) {
			return "up";
		} else if (normalized_direction.X >= 1) {
			return "right";
		} else if (normalized_direction.X <= -1) {
			return "left";
		}
		return "down";
	}
}
