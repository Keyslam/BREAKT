using System;
using System.Collections.Generic;
using Love;

public class CollectibleMultiply : CollectibleBase {
	private static Image image = Graphics.NewImage("Assets/Powerups/PowerupMultiply.png");

	public CollectibleMultiply(GameContext gameContext, Vector2 position) : base(gameContext, position) { }

	public override void OnPickup(GameContext gameContext) {
		if (this.pickedUp)
			return;

		base.OnPickup(gameContext);

		foreach (Ball ball in gameContext.balls) {
			gameContext.ballsToAdd.Add(new Ball(gameContext, ball.position, ball.angle - 0.3f));
			gameContext.ballsToAdd.Add(new Ball(gameContext, ball.position, ball.angle + 0.3f));
		}

		gameContext.audioManager.PlaySource(gameContext.audioManager.powerupMultiBall);
	}

	public override void Draw() {
		Graphics.Draw(image, this.position.X, this.position.Y, 0, 4, 4);
	}
}