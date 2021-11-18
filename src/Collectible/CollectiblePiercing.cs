using System;
using Love;

public class CollectiblePiercing : CollectibleBase {
	private static Image image = Graphics.NewImage("Assets/Powerups/PowerupPiercing.png");

	public CollectiblePiercing(GameContext gameContext, Vector2 position) : base(gameContext, position) { }

	public override void OnPickup(GameContext gameContext) {
		if (this.pickedUp)
			return;
			
		base.OnPickup(gameContext);
		
		foreach (Ball ball in gameContext.balls) {
			ball.piercingTime = 3.0f;
		}

		gameContext.audioManager.PlaySource(gameContext.audioManager.powerupPiercing, 0.1f);
	}

	public override void Draw() {
		Graphics.Draw(image, this.position.X, this.position.Y, 0, 4, 4);
	}
}