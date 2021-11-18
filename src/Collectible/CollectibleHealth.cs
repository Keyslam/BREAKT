using System;
using System.Collections.Generic;
using Love;

public class CollectibleHealth : CollectibleBase {
	private static Image image = Graphics.NewImage("Assets/Powerups/PowerupHealth.png");

	public CollectibleHealth(GameContext gameContext, Vector2 position) : base(gameContext, position) { }

	public override void OnPickup(GameContext gameContext) {
		if (this.pickedUp)
			return;
			
		base.OnPickup(gameContext);

		gameContext.lives = Math.Min(9, gameContext.lives + 1);
		gameContext.audioManager.PlaySource(gameContext.audioManager.powerupExtraLife);
	}

	public override void Draw() {
		Graphics.Draw(image, this.position.X, this.position.Y, 0, 4, 4);
	}
}