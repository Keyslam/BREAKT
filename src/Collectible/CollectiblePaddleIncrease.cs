using System;
using Love;

public class CollectiblePaddleIncrease : CollectibleBase {
	private static Image image = Graphics.NewImage("Assets/Powerups/PowerupPaddleIncrease.png");

	public CollectiblePaddleIncrease(GameContext gameContext, Vector2 position) : base(gameContext, position) { }

	public override void OnPickup(GameContext gameContext) {
		if (this.pickedUp)
			return;
			
		base.OnPickup(gameContext);
		
		gameContext.paddle.targetWidth = Math.Min(320, gameContext.paddle.targetWidth + 50.0f);

		gameContext.audioManager.PlayRandomSource(gameContext.audioManager.powerupPaddlesize);
	}

	public override void Draw() {
		Graphics.Draw(image, this.position.X, this.position.Y, 0, 4, 4);
	}
}