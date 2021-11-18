using System;
using System.Collections.Generic;
using Love;
using Retromono.Easings;
using Retromono.Tweens;

public class ScorePopup {
	private static List<Image> images = new List<Image>() {
		Graphics.NewImage("Assets/ScorePopup/ScorePopup10.png"),
		Graphics.NewImage("Assets/ScorePopup/ScorePopup25.png"),
		Graphics.NewImage("Assets/ScorePopup/ScorePopup50.png"),
		Graphics.NewImage("Assets/ScorePopup/ScorePopup75.png")
	};

	private Image image = null;
	private Vector2 position = Vector2.Zero;

	public float progress = 0;

	public ScorePopup(GameContext gameContext, Vector2 position, int index) {
		this.image = images[index];

		this.position = position;

		gameContext.tweens.Add(new TweenAnimateDouble(
			TimeSpan.FromSeconds(0.5),
			this.progress,
			1.0f,
			(value) => { this.progress = (float)value; },
			Easings.CubicOut)
		);
	}

	public void Draw() {
		Graphics.Draw(this.image, this.position.X + 6, this.position.Y - progress * 50, 0, 4, 4);
	}
}