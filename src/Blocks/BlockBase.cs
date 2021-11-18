using System;
using System.Collections.Generic;
using Humper;
using Love;
using Retromono.Easings;
using Retromono.Tweens;

public abstract class BlockBase {
	public IBox body = null;
	public bool dead = false;

	public Vector2 position;
	public Vector2 basePosition;

	public Vector2 velocity = Vector2.Zero;

	public bool requiredForFinish = false;

	public BlockBase(Humper.World world, Vector2 position) {
		this.position = position;
		this.basePosition = position;

		this.body = world.Create(position.X, position.Y, 48, 48);
		this.body.Data = this;
	}

	public virtual bool Update(GameContext gameContext, float dt) {
		if (this.dead) {
			this.position += this.velocity * dt;

			this.velocity.Y += 2000.0f * dt;

			if (this.position.Y > 900) {
				return true;
			}
		}

		return false;
	}

	public abstract void Draw();

	public int health = 1;
	private ITween tween = null;

	public virtual bool Hit(GameContext gameContext, Vector2 dir, int damage) {
		this.health -= damage;
		if (this.health <= 0)
			this.dead = true;

		if (!this.dead) {
			if (this.tween != null){
				this.tween.Finish();
			}

			this.tween = new TweenSequence(() => { }, new List<ITween>() {
					new TweenParallel(() => { }, new List<ITween>() {
						new TweenAnimateDouble(
							TimeSpan.FromSeconds(0.2),
							this.position.X,
							this.position.X + dir.X * 10.0f,
							(value) => { this.position.X = (float)value; },
							Easings.CubicOut),

						new TweenAnimateDouble(
							TimeSpan.FromSeconds(0.2),
							this.position.Y,
							this.position.Y + dir.Y * 10.0f,
							(value) => { this.position.Y = (float)value; },
							Easings.CubicOut)
					}),

					new TweenParallel(() => { }, new List<ITween>() {
						new TweenAnimateDouble(
							TimeSpan.FromSeconds(0.2),
							this.position.X + dir.X * 10.0f,
							this.position.X,
							(value) => { this.position.X = (float)value; },
							Easings.CubicOut),

						new TweenAnimateDouble(
							TimeSpan.FromSeconds(0.2),
							this.position.Y + dir.Y * 10.0f,
							this.position.Y,
							(value) => { this.position.Y = (float)value; },
							Easings.CubicOut)
					})
				});

			gameContext.tweens.Add(this.tween);
		} else {
			this.velocity = dir * 600.0f;

			gameContext.world.Remove(this.body);
		}

		if (this.dead) {
			Random random = new Random();

			if (random.NextDouble() >= 0.85f) {
				CollectibleBase collectable = null;

				int index = random.Next(7);

				if (index == 0)
					collectable = new CollectiblePaddleDecrease(gameContext, this.position);
				if (index == 1)
					collectable = new CollectiblePaddleIncrease(gameContext, this.position);
				if (index == 2)
					collectable = new CollectibleMultiply(gameContext, this.position);
				if (index == 3)
					collectable = new CollectiblePiercing(gameContext, this.position);
				if (index == 4 && gameContext.doLivesPowerup)
					collectable = new CollectibleHealth(gameContext, this.position);
				if (index == 5)
					collectable = new CollectibleCoin(gameContext, this.position);
				if (index == 6)
					collectable = new CollectibleCard(gameContext, this.position);
			}
		}

		return this.dead;
	} 
}