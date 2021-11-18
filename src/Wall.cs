using Humper;
using Love;

public class Wall {
	private IBox body;

	public bool isKill = false;

	public Wall(GameContext gameContext, Vector2 position, Vector2 size, bool isKill = false) {
		this.body = gameContext.world.Create(position.X, position.Y, size.X, size.Y);
		this.body.Data = this;

		this.isKill = isKill;

		gameContext.walls.Add(this);
	}

	public void Destroy(GameContext gameContext) {
		gameContext.world.Remove(this.body);
	}
}