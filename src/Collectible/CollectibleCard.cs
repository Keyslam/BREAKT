using System;
using System.Collections.Generic;
using Love;

public class CollectibleCard : CollectibleBase {
	private static Image image = Graphics.NewImage("Assets/Powerups/PowerupCard.png");

	public CollectibleCard(GameContext gameContext, Vector2 position) : base(gameContext, position) { }

	public override void OnPickup(GameContext gameContext) {
		if (this.pickedUp)
			return;

		base.OnPickup(gameContext);

		gameContext.score += 1000;
		gameContext.audioManager.PlaySource(gameContext.audioManager.powerupScoreCard);
	}

	public override void Draw() {
		Graphics.Draw(image, this.position.X, this.position.Y, 0, 4, 4);
	}
}