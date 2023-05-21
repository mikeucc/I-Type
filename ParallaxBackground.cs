using Godot;
using System;

public partial class ParallaxBackground : Godot.ParallaxBackground
{
	private float scrollSpeed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scrollSpeed= -25f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float newX= this.ScrollOffset.X + (scrollSpeed * (float)delta);

        this.ScrollOffset = new Vector2(newX,0);
		

	}
}
