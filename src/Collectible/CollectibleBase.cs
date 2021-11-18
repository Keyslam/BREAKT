using System;
using Humper;
using Humper.Responses;
using Love;

public abstract class CollectibleBase {
	public Vector2 position = Vector2.Zero;
	
	public IBox body = null;

	public float invince = 1.0f;
	public bool pickedUp = false;

	public CollectibleBase(GameContext gameContext, Vector2 position) {
		this.position = position;

		this.body = gameContext.world.Create(this.position.X, this.position.Y, 52, 52);
		this.body.Data = this;

		gameContext.collectibles.Add(this);
	}

	public void Update(GameContext gameContext, float dt) {
		this.position.Y += dt * 50.0f;

		var result = this.body.Move(this.position.X, this.position.Y, (collision) => CollisionResponses.Cross);
		foreach (var hit in result.Hits) {
			if (this.invince > 0)
				continue;

			if (hit.Box.Data is Ball) {
				this.OnPickup(gameContext);
			}

			if (hit.Box.Data is Paddle) {
				this.OnPickup(gameContext);
			}
		}

		this.position.X = result.Destination.X;
		this.position.Y = result.Destination.Y;

		this.invince = Math.Max(0, this.invince - dt);
	}

	public void Destroy(GameContext gameContext) {
		gameContext.world.Remove(this.body);
		pickedUp = true;
	}

	public abstract void Draw();

	public virtual void OnPickup(GameContext gameContext) {
		this.Destroy(gameContext);
	}
}