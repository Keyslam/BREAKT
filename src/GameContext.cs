using System;
using System.Collections.Generic;
using System.Linq;
using Humper;
using Love;
using Retromono.Tweens;

public class GameContext {
	public enum GameContextState {
		PLAYING,
		WON,
		DEATH,
	};

	public ILogger logger = null;

	public Humper.World world = null;
	public Camera camera = null;
	public AudioManager audioManager = null;

	public List<BlockBase> blocks = null;
	public List<Ball> balls = null;
	public Paddle paddle = null;
	public List<Wall> walls = null;
	public List<ScorePopup> scorePopups = null;
	public List<CollectibleBase> collectibles = null;

	public List<Ball> ballsToAdd = null;
	public List<Ball> ballsToDie = null;

	public UIBar uiBar = null;

	public List<ITween> tweens = null;

	public float time = 0.0f;

	private Image backgroundImage = null;
	private Quad backgroundQuad = null;

	public float combo = 0.0f;

	public int lives = 0;

	public int score = 0;

	public bool doLivesPowerup = true;

	public GameContext(LevelBase level, ILogger logger, Humper.World world, Camera camera, AudioManager audioManager, int lives, int score) {
		this.logger = logger;

		this.world = world;
		this.camera = camera;
		this.audioManager = audioManager;

		this.blocks = new List<BlockBase>();
		this.balls = new List<Ball>();
		this.walls = new List<Wall>();
		this.scorePopups = new List<ScorePopup>();
		this.collectibles = new List<CollectibleBase>();

		this.ballsToAdd = new List<Ball>();
		this.ballsToDie = new List<Ball>();

		this.uiBar = new UIBar();

		this.tweens = new List<ITween>();

		this.lives = lives;
		this.score = score;

		backgroundImage = Graphics.NewImage("Assets/BackgroundStripes.png");
		backgroundImage.SetWrap(WrapMode.Repeat, WrapMode.Repeat);
		backgroundQuad = Graphics.NewQuad(0, 0, 450, 450, 32, 32);

		blocks.Clear();
		List<BlockBase> newBlocks = level.GetLevel(world);
		blocks.AddRange(newBlocks);

		paddle = new Paddle(this, new Vector2(400, 852), 120);
		//balls.Add(new Ball(this, new Vector2(350 - 14f, 600), 0.0f));
		balls.Add(new Ball(this, true));

		new Wall(this, new Vector2(-10, -10), new Vector2(10, 920));
		new Wall(this, new Vector2(900, -10), new Vector2(10, 920));
		new Wall(this, new Vector2(-10, -10), new Vector2(920, 10));
		new Wall(this, new Vector2(-10, 900), new Vector2(920, 10), true);
	}

	private bool respawning = false;

	public GameContextState Update(float dt) {
		this.time += dt;

		backgroundQuad.SetViewport(this.time * 20.0f, this.time * 30.0f, 450, 450);

		camera.Update(dt);

		paddle.Update(this, dt);

		foreach (Ball ball in balls)
			ball.Update(this, dt);

		List<BlockBase> toRemove = new List<BlockBase>();

		foreach (BlockBase block in blocks) {
			if (block.Update(this, dt))
				toRemove.Add(block);
		}

		foreach (BlockBase block in toRemove)
			blocks.Remove(block);

		foreach (CollectibleBase collectible in collectibles)
			collectible.Update(this, dt);

		collectibles.RemoveAll(collectible => collectible.pickedUp);
		scorePopups.RemoveAll(popup => popup.progress == 1.0f);

		this.balls.AddRange(this.ballsToAdd);
		this.ballsToAdd.Clear();

		foreach (Ball ball in ballsToDie)
			balls.Remove(ball);
		ballsToDie.Clear();

		foreach (ITween tween in tweens)
			tween.Advance(TimeSpan.FromSeconds(dt));

		combo = Math.Max(0, combo - dt);

		bool wonLevel = false;
		if (blocks.Count == 0)
			wonLevel = true;
		else {
			if (blocks.Where(block => block.requiredForFinish).Count() == 0)
				wonLevel = true;
		}

		if (wonLevel) {
			return GameContextState.WON;
		}

		if (balls.Count == 0) {
			if (!respawning) {
				respawning = true;

				if (lives == 0) {
					return GameContextState.DEATH;
				} else {
					lives--;

					balls.Add(new Ball(this, true));

					respawning = false;
				}
			}
		}

		return GameContextState.PLAYING;
	}

	public void Draw() {
		camera.Apply(this);

		Graphics.Draw(this.backgroundQuad, this.backgroundImage, 0, 0, 0, 4, 4);

		foreach (BlockBase block in blocks)
			if (!block.dead)
				block.Draw();

		foreach (BlockBase block in blocks)
			if (block.dead)
				block.Draw();

		foreach (CollectibleBase collectible in collectibles)
			collectible.Draw();

		paddle.Draw();

		foreach (Ball ball in balls)
			ball.Draw();

		foreach (ScorePopup scorePopup in scorePopups)
			scorePopup.Draw();

		var b = world.Bounds;
		//world.DrawDebug((int)b.X, (int)b.Y, (int)b.Width, (int)b.Height, DrawCell, DrawBox, DrawString);

		camera.Detach();

		this.uiBar.Draw(this);
	}

	public void keypressed(KeyConstant key) {
		foreach (Ball ball in balls)
			ball.keypressed(this, key);
	}

	private void DrawCell(int x, int y, int w, int h, float alpha) {
		Graphics.SetColor(1, 1, 1, alpha);
		Graphics.Rectangle(DrawMode.Line, x, y, w, h);
		Graphics.SetColor(1, 1, 1, 1);
	}

	private void DrawBox(IBox box) {
		Graphics.SetColor(1, 0, 0, 1);
		Graphics.Rectangle(DrawMode.Line, box.X, box.Y, box.Width, box.Height);
		Graphics.SetColor(1, 1, 1, 1);
	}

	private void DrawString(string message, int x, int y, float alpha) {
		Graphics.SetColor(1, 1, 1, alpha);
		Graphics.Print(message, x, y);
		Graphics.SetColor(1, 1, 1, 1);
	}
}