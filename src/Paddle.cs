using System;
using System.Linq;
using Humper;
using Humper.Responses;
using Love;

public class Paddle {
	private static Image edgeCenter = Graphics.NewImage("Assets/Paddle/PaddleCenter.png");
	private static Image edgeLeft = Graphics.NewImage("Assets/Paddle/PaddleEdgeLeft.png");
	private static Image edgeRight = Graphics.NewImage("Assets/Paddle/PaddleEdgeRight.png");
	private static Image edgeExtension = Graphics.NewImage("Assets/Paddle/PaddleExtension.png");
	
	private static Image emoteSad = Graphics.NewImage("Assets/Emotes/EmoteSad.png");
	private static Image emoteHappy = Graphics.NewImage("Assets/Emotes/EmoteHappy.png");

	public Vector2 position = Vector2.Zero;
	public float width = 0;

	public float targetWidth = 0;

	public float velocity = 0;

	public IBox body = null;

	public float targetX = 0;

	public float emoteTime = 0.0f;
	public Image emote = null;

	public Paddle(GameContext gameContext, Vector2 position, float width) {
		this.position = position;
		this.width = width;
		this.targetWidth = width;

		this.body = gameContext.world.Create(this.position.X, this.position.Y, this.width, 40);
		this.body.Data = this;
	}

	public void Update(GameContext gameContext, float dt) {
		bool changed = false;
		float deltaMov = 0;

		if (this.targetWidth > this.width) {
			float oldWidth = this.width;
			this.width = Mathf.Min(this.targetWidth, this.width + dt * 300.0f);
			changed = true;
			deltaMov = (this.width - oldWidth) / 2 * -1;
		} else if (this.targetWidth < this.width) {
			float oldWidth = this.width;
			this.width = Mathf.Max(this.targetWidth, this.width - dt * 300.0f);
			changed = true;
			deltaMov = (this.width - oldWidth) / 2 * -1;
		}

		if (changed) {
			gameContext.world.Remove(this.body);

			this.position.X += deltaMov;
			if (this.position.X < 0.0f)
				this.position.X = 0.0f;
			if (this.position.X + this.width > 900.0f)
				this.position.X = 900.0f - this.width;

			this.body = gameContext.world.Create(this.position.X, this.position.Y, this.width, 40);
			this.body.Data = this;
		}

		emoteTime = Math.Max(0, emoteTime - dt);
		if (emoteTime == 0.0f) {
			emote = null;
		}

		if (gameContext.balls.Count > 0) {
			float minDistance = gameContext.balls.Min(ball => Mathf.Abs(Vector2.DistanceSquared(this.position, ball.position)));
			Ball closestBall = gameContext.balls.First(ball => Mathf.Abs(Vector2.DistanceSquared(this.position, ball.position)) == minDistance);

			targetX = closestBall.position.X;			
		} else {
			if (emoteTime == 0.0f) {
				emote = emoteSad;
				emoteTime = 0.515f;
			}
		}
		
		if (targetX < this.position.X + width / 5) {
			this.velocity = Math.Max(-900, this.velocity - dt * 500);
		}

		if (targetX > this.position.X + width - width / 5) {
			this.velocity = Math.Min(900, this.velocity + dt * 500);
		}

		if (this.velocity > 0.0f) {
			this.velocity = Math.Max(0.0f, this.velocity - 200.0f * dt);
		} else if (this.velocity < 0.0f) {
			this.velocity = Math.Min(0.0f, this.velocity + 200.0f * dt);
		}

		this.position.X += velocity * dt;

		var res = this.body.Move(this.position.X, this.position.Y, (collision) => {
			if (collision.Other.Data is Ball) {
				return CollisionResponses.Cross;
			}

			if (collision.Other.Data is CollectibleBase) {
				return CollisionResponses.Cross;
			}

			return CollisionResponses.Bounce;
		});

		foreach (var hit in res.Hits) {
			if (hit.Box.Data is Wall) {
				this.velocity = this.velocity * -0.5f;
			}

			if (hit.Box.Data is CollectibleBase) {
				(hit.Box.Data as CollectibleBase).OnPickup(gameContext);
			}
		}

		this.position.X = res.Destination.X;
	}

	public void Draw() {
		Graphics.Draw(edgeExtension, this.position.X, this.position.Y, 0, this.width, 4);

		Graphics.Draw(edgeLeft, this.position.X, this.position.Y, 0, 4, 4);
		Graphics.Draw(edgeRight, this.position.X + width - 8, this.position.Y, 0, 4, 4);

		Graphics.Draw(edgeCenter, this.position.X + ((width - 16 - 36) / 2) + 8, this.position.Y, 0, 4, 4);

		if (emote != null) {
			//Graphics.Draw(emote, this.position.X + ((width - 16 - 36) / 2) + 4, this.position.Y - 60, 0, 4, 4);
		}
	}
}