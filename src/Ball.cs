using System;
using System.Collections.Generic;
using Humper;
using Humper.Responses;
using Love;

public class Ball {
	private static List<Image> images = new List<Image>() {
		Graphics.NewImage("Assets/Ball/BallDefault.png"),
		Graphics.NewImage("Assets/Ball/BallCovered1.png"),
		Graphics.NewImage("Assets/Ball/BallCovered2.png"),
		Graphics.NewImage("Assets/Ball/BallCovered3.png"),
	};

	private static Image indicator = Graphics.NewImage("Assets/Ball/IndicatorRight.png");

	private IBox body = null;

	public Vector2 position = Vector2.Zero;
	public float angle = 0;
	public float speed = 0;
	private float targetSpeed = 0;

	public float piercingTime = 0.0f;

	public int blocksBroken = 0;

	public bool locked = false;

	public Ball(GameContext gameContext, Vector2 position, float angle = 0, float speed = 350) {
		this.position = position;
		this.angle = angle;
		this.speed = speed;
		this.targetSpeed = speed;

		this.locked = false;

		this.body = gameContext.world.Create(this.position.X, this.position.Y, 24, 24);
		this.body.Data = this;
	}

	public Ball(GameContext gameContext, bool locked, float speed = 350) {
		this.position = gameContext.paddle.position + new Vector2(((gameContext.paddle.width - 16 - 36) / 2) + 12, -32);
		this.angle = (float)(-Math.PI / 2.0f);
		this.speed = speed;
		this.targetSpeed = speed;

		this.locked = true;

		this.body = gameContext.world.Create(this.position.X, this.position.Y, 24, 24);
		this.body.Data = this;
	}

	public void keypressed(GameContext gameContext, KeyConstant key) {
		if ((key == KeyConstant.Space || key == KeyConstant.Return) && this.locked) {
			this.locked = false;
			this.speed = this.speed = Math.Min(2000, this.speed + 500);
			gameContext.camera.AddTrauma(0.35f);

			gameContext.audioManager.PlaySource(gameContext.audioManager.paddleServe);
		}
	}

	public void Update(GameContext gameContext, float dt) {
		

		if (Keyboard.IsDown(KeyConstant.A) || Keyboard.IsDown(KeyConstant.Left)) {
			this.angle -= 1.5f * dt;
		}

		if (Keyboard.IsDown(KeyConstant.D) || Keyboard.IsDown(KeyConstant.Right)) {
			this.angle += 1.5f * dt;
		}

		Vector2 velocity = new Vector2(
			Mathf.Cos(this.angle),
			Mathf.Sin(this.angle)
		);

		float targetX = this.position.X + velocity.X * speed * dt;
		float targetY = this.position.Y + velocity.Y * speed * dt;

		var result = this.body.Move(targetX, targetY, (collision) => {
			if (collision.Box.Data is CollectibleBase) {
				return CollisionResponses.Cross;
			}

			if (collision.Other.Data is Ball) {
				if (collision.Other.Data != this) {
					return CollisionResponses.Cross;
				}
			}

			if (collision.Other.Data is CollectibleBase) {
				return CollisionResponses.Cross;
			}

			return CollisionResponses.Bounce;
		});

		if (result.HasCollided) {
			foreach (Hit hit in result.Hits) {
				if (hit.Box.Data is BlockBase) {
					BlockBase block = (BlockBase)hit.Box.Data;
					bool died = block.Hit(gameContext, velocity.Normalized(), blocksBroken < 15 ? 1 : 0);


					if (died) {
						gameContext.camera.AddTrauma(0.3f);


						int scoreIndex = (int)MathF.Min(3, Mathf.FloorToInt(gameContext.combo));

						if (scoreIndex == 0)
							gameContext.score += 10;
						if (scoreIndex == 1)
							gameContext.score += 25;
						if (scoreIndex == 2)
							gameContext.score += 50;
						if (scoreIndex == 3)
							gameContext.score += 75;

						gameContext.scorePopups.Add(new ScorePopup(gameContext, block.position + new Vector2(0, 0), scoreIndex));

						gameContext.combo++;

						gameContext.audioManager.PlayRandomSource(gameContext.audioManager.blockBreakSweetener);
						gameContext.audioManager.PlayRandomSource(gameContext.audioManager.blockBreakPerc);

						blocksBroken++;
					} else {
						gameContext.camera.AddTrauma(0.2f);
						gameContext.audioManager.PlayRandomSource(gameContext.audioManager.blockIndestruct);
					}

					if (piercingTime == 0.0f && died) {
						if (hit.Box.Data is BlockStandard)
							this.speed = Math.Min(2000, this.speed + 250);
					}

					if (piercingTime == 0.0f || !died) {
						if (hit.Normal.X != 0) {
							velocity.X = -velocity.X;
						}

						if (hit.Normal.Y != 0) {
							velocity.Y = -velocity.Y;
						}
					}
				}

				if (hit.Box.Data is Paddle) {
					this.speed = Math.Min(2000, this.speed + 450);
					gameContext.camera.AddTrauma(0.35f);

					if (hit.Normal.X != 0) {
						if (this.position.X <= 30 || this.position.X >= 900 - 30) {
							gameContext.ballsToDie.Add(this);
						}

						velocity.X = -velocity.X;
					}

					if (hit.Normal.Y != 0) {
						velocity.Y = -velocity.Y;
					}

					this.blocksBroken = 0;

					gameContext.audioManager.PlayRandomSource(gameContext.audioManager.paddleCollide);
				}

				if (hit.Box.Data is Wall) {
					if ((hit.Box.Data as Wall).isKill) {
						gameContext.ballsToDie.Add(this);
					}

					if (hit.Normal.X != 0) {
						velocity.X = -velocity.X;
					}

					if (hit.Normal.Y != 0) {
						velocity.Y = -velocity.Y;
					}
				}

				if (hit.Box.Data is CollectibleBase) {
					if ((hit.Box.Data as CollectibleBase).invince == 0) {
						(hit.Box.Data as CollectibleBase).OnPickup(gameContext);
					}
				}
			}
		}

		position.X = result.Destination.X;
		position.Y = result.Destination.Y;

		this.angle = (float)Math.Atan2(velocity.Y, velocity.X);

		if (speed > targetSpeed) {
			this.speed = Math.Max(targetSpeed, speed - dt * 2500);
		} else if (speed < targetSpeed) {
			this.speed = Math.Min(targetSpeed, speed + dt * 2500);
		}

		piercingTime = Math.Max(0, piercingTime - dt);

		if (locked) {
			this.position = gameContext.paddle.position + new Vector2(((gameContext.paddle.width - 16 - 36) / 2) + 12, -32);
		}
	}

	public void Draw() {
		int index = (int)MathF.Min(3, Mathf.FloorToInt(blocksBroken / 5));

		Graphics.Draw(images[index], this.position.X, this.position.Y, 0, 4, 4);

		if (locked) {
			for (int i = 0; i < 5; i++) {
				Graphics.Draw(indicator, this.position.X + 13 + MathF.Cos(this.angle) * (12.0f * i), this.position.Y + 13 + MathF.Sin(this.angle) * (12.0f * i), this.angle, 4, 4, 6.5f, 6.5f);
			}
		} else {
			Graphics.Draw(indicator, this.position.X + 13, this.position.Y + 13, this.angle, 4, 4, 6.5f, 6.5f);
		}
	}
}