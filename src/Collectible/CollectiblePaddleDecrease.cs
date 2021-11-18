using System;
using Love;

public class CollectiblePaddleDecrease : CollectibleBase {
	private static Image image = Graphics.NewImage("Assets/Powerups/PowerupPaddleDecrease.png");

	public CollectiblePaddleDecrease(GameContext gameContext, Vector2 position) : base(gameContext, position) { }

	public override void OnPickup(GameContext gameContext) {
		if (this.pickedUp)
			return;
			
		base.OnPickup(gameContext);
		
		gameContext.paddle.targetWidth = Math.Max(70, gameContext.paddle.targetWidth - 50.0f);

		gameContext.audioManager.PlayRandomSource(gameContext.audioManager.powerupPaddlesize);
		gameContext.audioManager.PlaySource(gameContext.audioManager.powerupPaddleSmaller);
	}

	public override void Draw() {
		Graphics.Draw(image, this.position.X, this.position.Y, 0, 4, 4);
	}
}