using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Love;
using Newtonsoft.Json;
using Retromono.Easings;
using Retromono.Tweens;
using static GameContext;

public abstract class StageBase {
	public delegate void DoneHandler();
	public event DoneHandler Done;

	private static readonly HttpClient client = new HttpClient();

	private static Image GameoverImage = Graphics.NewImage("Assets/Gameover.png");
	private static Image StageClearImage = Graphics.NewImage("Assets/StageClear.png");
	private static Image SelectItem = Graphics.NewImage("Assets/ItemSelect.png");

	private static List<Image> PixelFont = new List<Image>() {
		Graphics.NewImage("Assets/PixelFont/a.png"),
		Graphics.NewImage("Assets/PixelFont/b.png"),
		Graphics.NewImage("Assets/PixelFont/c.png"),
		Graphics.NewImage("Assets/PixelFont/d.png"),
		Graphics.NewImage("Assets/PixelFont/e.png"),
		Graphics.NewImage("Assets/PixelFont/f.png"),
		Graphics.NewImage("Assets/PixelFont/g.png"),
		Graphics.NewImage("Assets/PixelFont/h.png"),
		Graphics.NewImage("Assets/PixelFont/i.png"),
		Graphics.NewImage("Assets/PixelFont/j.png"),
		Graphics.NewImage("Assets/PixelFont/k.png"),
		Graphics.NewImage("Assets/PixelFont/l.png"),
		Graphics.NewImage("Assets/PixelFont/m.png"),
		Graphics.NewImage("Assets/PixelFont/n.png"),
		Graphics.NewImage("Assets/PixelFont/o.png"),
		Graphics.NewImage("Assets/PixelFont/p.png"),
		Graphics.NewImage("Assets/PixelFont/q.png"),
		Graphics.NewImage("Assets/PixelFont/r.png"),
		Graphics.NewImage("Assets/PixelFont/s.png"),
		Graphics.NewImage("Assets/PixelFont/t.png"),
		Graphics.NewImage("Assets/PixelFont/u.png"),
		Graphics.NewImage("Assets/PixelFont/v.png"),
		Graphics.NewImage("Assets/PixelFont/w.png"),
		Graphics.NewImage("Assets/PixelFont/x.png"),
		Graphics.NewImage("Assets/PixelFont/y.png"),
		Graphics.NewImage("Assets/PixelFont/z.png"),
		Graphics.NewImage("Assets/PixelFont/_.png"),
	};

	private static List<Image> PixelFontNumbers = new List<Image>() {
		Graphics.NewImage("Assets/PixelFont/0.png"),
		Graphics.NewImage("Assets/PixelFont/1.png"),
		Graphics.NewImage("Assets/PixelFont/2.png"),
		Graphics.NewImage("Assets/PixelFont/3.png"),
		Graphics.NewImage("Assets/PixelFont/4.png"),
		Graphics.NewImage("Assets/PixelFont/5.png"),
		Graphics.NewImage("Assets/PixelFont/6.png"),
		Graphics.NewImage("Assets/PixelFont/7.png"),
		Graphics.NewImage("Assets/PixelFont/8.png"),
		Graphics.NewImage("Assets/PixelFont/9.png"),
	};

	public Vector2 gameoverPos = new Vector2(450 - 160, -288);
	public Vector2 stageClearPos = new Vector2(450 - 204, -452);
	public int selectIndex = 0;

	public GameContext gameContext = null;

	public List<LevelBase> levels = null;
	public int levelIndex = 0;

	public float gameTimeScale = 1.0f;

	public double coverRadius = 0;

	public AudioManager audioManager = null;

	public List<ITween> tweens = new List<ITween>();
	public List<ITween> tweensToAdd = new List<ITween>();

	public Vector2 namePos = new Vector2(0, -76);

	bool animating = false;
	bool gameover = false;
	bool won = false;

	private List<Highscore> highscores = null;

	public string name = "";

	public int stageIndex = 0;

	public LevelBase level = null;

	public StageBase(List<LevelBase> levels, AudioManager audioManager, int stageIndex) {
		this.levels = levels;
		this.audioManager = audioManager;
		this.stageIndex = stageIndex;

		audioManager.PlayMainTrack();

		this.gameContext = new GameContext(
			this.levels[levelIndex],
			new LoggerConsole(LogLevel.ERROR),
			new Humper.World(900, 900),
			new Camera(),
			this.audioManager,
			5,
			0
		);

		level = this.levels[levelIndex];

		tweens.Add(new TweenAnimateDouble(
									TimeSpan.FromSeconds(0.8f),
									0.0f,
									670.0f,
									value => { coverRadius = (float)value; },
									Easings.CubicIn
								));

		FetchHighscores();

		tweens.Add(
			new TweenSequence(() => { }, new List<ITween>() {
				new TweenAnimateDouble(
					TimeSpan.FromSeconds(0.4f),
					namePos.Y,
					250.0f,
					value => { namePos.Y = (float)value; },
					Easings.CubicOut
				),

				new TweenSleep(TimeSpan.FromSeconds(0.85f)),

				new TweenAnimateDouble(
					TimeSpan.FromSeconds(0.4f),
					250,
					-76,
					value => { namePos.Y = (float)value; },
					Easings.CubicIn
				),
			}
		));
	}

	public virtual void LoadNextLevel() {
		audioManager.PlayMainTrack();

		levelIndex++;

		this.gameContext = new GameContext(
			this.levels[levelIndex],
			new LoggerConsole(LogLevel.ERROR),
			new Humper.World(900, 900),
			new Camera(),
			this.audioManager,
			this.gameContext.lives,
			this.gameContext.score
		);

		level = this.levels[levelIndex];

		tweensToAdd.Add(
			new TweenSequence(() => { }, new List<ITween>() {
				new TweenAnimateDouble(
					TimeSpan.FromSeconds(0.3f),
					namePos.Y,
					250.0f,
					value => { namePos.Y = (float)value; },
					Easings.CubicOut
				),

				new TweenSleep(TimeSpan.FromSeconds(0.8f)),

				new TweenAnimateDouble(
					TimeSpan.FromSeconds(0.3f),
					250,
					-76,
					value => { namePos.Y = (float)value; },
					Easings.CubicIn
				),
			}
		));
	}

	public void Update(float dt) {
		tweens.AddRange(tweensToAdd);
		tweensToAdd.Clear();

		foreach (ITween tween in tweens)
			tween.Advance(TimeSpan.FromSeconds(dt));

		GameContextState state = gameContext.Update(dt * gameTimeScale);

		if (!won && !gameover && !animating && (state == GameContextState.WON || state == GameContextState.DEATH)) {
			animating = true;

			if (state == GameContextState.WON) {
				if (levelIndex >= levels.Count - 1) {
					audioManager.PlayScoreGroove();

					tweens.Add(
						new TweenParallel(() => { }, new List<ITween>() {
							new TweenAnimateDouble(
								TimeSpan.FromSeconds(0.5f),
								this.gameTimeScale,
								0.0f,
								value => { this.gameTimeScale = (float)value; }
							),

							new TweenSequence(() => { }, new List<ITween>() {
								new TweenAnimateDouble(
									TimeSpan.FromSeconds(0.8f),
									coverRadius,
									0.0f,
									value => { coverRadius = (float)value; },
									Easings.CubicIn
								),

								new TweenCallback(() => { animating = false; won = true; }),

								new TweenAnimateDouble(
								TimeSpan.FromSeconds(1.5f),
								stageClearPos.Y,
								200f,
								value => { stageClearPos.Y = (float)value; },
								Easings.ElasticOut
							),
							})
						}
					));
				} else {
					tweens.Add(
						new TweenParallel(() => { }, new List<ITween>() {
							new TweenAnimateDouble(
								TimeSpan.FromSeconds(0.5f),
								this.gameTimeScale,
								0.0f,
								value => { this.gameTimeScale = (float)value; }
							),

							new TweenSequence(() => { }, new List<ITween>() {
								new TweenAnimateDouble(
									TimeSpan.FromSeconds(0.8f),
									coverRadius,
									0.0f,
									value => { coverRadius = (float)value; },
									Easings.CubicIn
								),

								new TweenSleep(TimeSpan.FromSeconds(0.2f), () => {
									LoadNextLevel();
								}),

								new TweenCallback(() => { gameTimeScale = 1.0f; }),

								new TweenAnimateDouble(
									TimeSpan.FromSeconds(0.8f),
									0.0f,
									670.0f,
									value => { coverRadius = (float)value; },
									Easings.CubicIn
								),

								new TweenCallback(() => { animating = false; })
							})
						}
					));
				}
			} else if (state == GameContextState.DEATH) {
				audioManager.PlayGameOver();
				selectIndex = 0;
				tweens.Add(
					new TweenParallel(() => { }, new List<ITween>() {
						new TweenAnimateDouble(
							TimeSpan.FromSeconds(0.5f),
							this.gameTimeScale,
							0.0f,
							value => { this.gameTimeScale = (float)value; }
						),

						new TweenSequence(() => { }, new List<ITween>() {
							new TweenAnimateDouble(
								TimeSpan.FromSeconds(0.8f),
								coverRadius,
								0.0f,
								value => { coverRadius = (float)value; },
								Easings.CubicIn
							),

							new TweenCallback(() => { animating = false; gameover = true; }),

							new TweenAnimateDouble(
								TimeSpan.FromSeconds(1.5f),
								gameoverPos.Y,
								300f,
								value => { gameoverPos.Y = (float)value; },
								Easings.ElasticOut
							),
						})
					}
				));
			}
		}
	}

	public void Draw() {
		Graphics.Clear();

		Graphics.Stencil(StencilFunc, StencilAction.Replace, 1);

		Graphics.SetStencilTest(CompareMode.Greater, 0);

		Graphics.SetColor(0.922f, 0.863f, 0.647f, 1.000f);
		Graphics.Rectangle(DrawMode.Fill, 0, 0, 900, 976);
		Graphics.SetColor(1.0f, 1.0f, 1.0f, 1.0f);

		gameContext.Draw();

		Graphics.SetStencilTest(CompareMode.Less, 1);

		Graphics.SetColor(0.922f, 0.863f, 0.647f);
		Graphics.Rectangle(DrawMode.Fill, 0, 0, 900, 976);
		Graphics.SetColor(1.0f, 1.0f, 1.0f, 1.0f);

		Graphics.SetStencilTest();

		if (gameover) {
			Graphics.Draw(GameoverImage, gameoverPos.X, gameoverPos.Y, 0, 4, 4);

			Graphics.Draw(SelectItem, gameoverPos.X + 16, gameoverPos.Y + 116 + (selectIndex * 52), 0, 4, 4);
		}

		if (won) {
			Graphics.Draw(StageClearImage, stageClearPos.X, stageClearPos.Y, 0, 4, 4);

			if (highscores != null) {
				for (int i = 0; i < highscores.Count; i++) {
					Highscore highscore = highscores[i];

					for (int j = 0; j < 5; j++) {
						int letterindex = 26;

						if (j < highscore.username.Length) {
							letterindex = (int)(highscore.username[j]) - 97;
							Graphics.Draw(PixelFont[letterindex], stageClearPos.X + 32 * j + 20, stageClearPos.Y + 108 + i * 48, 0, 4, 4);
						}
					}

					string scoreString = highscores[i].score.ToString();
					for (int j = 0; j < scoreString.Length; j++) {
						int letterindex = Convert.ToInt32(new string(scoreString[j], 1));
						Graphics.Draw(PixelFontNumbers[letterindex], stageClearPos.X + 32 * j + 200, stageClearPos.Y + 108 + i * 48, 0, 4, 4);
					}
				}
			}

			for (int j = 0; j < 5; j++) {
				int letterindex = 26;

				if (j < name.Length) {
					letterindex = (int)(name[j]) - 97;
					Graphics.Draw(PixelFont[letterindex], stageClearPos.X + 32 * j + 20, stageClearPos.Y + 380, 0, 4, 4);
				} else {
					Graphics.Draw(PixelFont[letterindex], stageClearPos.X + 32 * j + 20, stageClearPos.Y + 380 - MathF.Sin(Timer.GetTime() * 2.0f + (j * 0.8f)) * 6, 0, 4, 4);
				}
			}

			string scoreStrig = gameContext.score.ToString();
			for (int j = 0; j < scoreStrig.Length; j++) {
				int letterindex = Convert.ToInt32(new string(scoreStrig[j], 1));
				Graphics.Draw(PixelFontNumbers[letterindex], stageClearPos.X + 32 * j + 200, stageClearPos.Y + 380, 0, 4, 4);
			}
		}

		Graphics.Draw(level.GetName(), namePos.X, namePos.Y, 0, 4, 4);
	}

	public void Keypressed(KeyConstant key) {
		if (gameover) {
			if (key == KeyConstant.W || key == KeyConstant.Up) {
				selectIndex = Math.Max(0, selectIndex - 1);
			}

			if (key == KeyConstant.S || key == KeyConstant.Down) {
				selectIndex = Math.Min(1, selectIndex + 1);
			}

			if (key == KeyConstant.Return || key == KeyConstant.Space) {
				audioManager.PlaySource(audioManager.menuConfirm);

				animating = true;

				if (selectIndex == 1) {
					tweens.Add(
						new TweenSequence(() => { }, new List<ITween>() {
							new TweenCallback(() => {
								gameContext.score = 0;
								gameContext.lives = 5;

								levelIndex = -1;
								LoadNextLevel();
							}),

							new TweenAnimateDouble(
								TimeSpan.FromSeconds(0.6f),
								300,
								-288,
								value => { gameoverPos.Y = (float)value; },
								Easings.CubicIn
							),

							new TweenAnimateDouble(
								TimeSpan.FromSeconds(0.5f),
								0,
								670f,
								value => { coverRadius = (float)value; }
							),

							new TweenCallback(() => { animating = false; gameover = false; gameTimeScale = 1; }),
						})
					);
				} else {
					tweens.Add(
						new TweenSequence(() => { }, new List<ITween>() {
							new TweenAnimateDouble(
								TimeSpan.FromSeconds(0.6f),
								300,
								-288,
								value => { gameoverPos.Y = (float)value; },
								Easings.CubicIn
							),

							new TweenCallback(() => { Done?.Invoke(); }),
						})
					);
				}
			}
		}

		if (won) {
			if (key == KeyConstant.Backspace && this.name.Length > 0) {
				this.name = this.name.Substring(0, this.name.Length - 1);
				audioManager.PlaySource(audioManager.menuCancel);
			}

			if (key == KeyConstant.Space || key == KeyConstant.Enter) {
				audioManager.PlaySource(audioManager.menuConfirm);

				if (!string.IsNullOrEmpty(this.name)) {
					PutHighscore();
				}

				this.animating = true;

				tweens.Add(
					new TweenSequence(() => { }, new List<ITween>() {
						new TweenAnimateDouble(
							TimeSpan.FromSeconds(0.5f),
							stageClearPos.Y,
							-452,
							value => { stageClearPos.Y = (float)value; },
							Easings.CubicIn
						),

						new TweenCallback(() => { Done?.Invoke(); }),
					})
				);
			}

			string letter = key.ToString().ToLower();

			if (letter.Length > 1)
				return;
			if (!char.IsLetter(letter, 0))
				return;

			if (this.name.Length < 5)
				this.name += letter;
		}

		if (!won && !gameover && !animating) {
			gameContext.keypressed(key);
		}
	}

	private void StencilFunc() {
		Graphics.Circle(DrawMode.Fill, 450, 976 / 2, (float)coverRadius);
	}

	private class Response {
		[JsonProperty("status")]
		public int status;

		[JsonProperty("highscores")]
		public Highscore[] highscores;
	}

	private class Highscore {
		[JsonProperty("username")]
		public string username;

		[JsonProperty("score")]
		public int score;

		[JsonProperty("stageIndex")]
		public int stageIndex;
	}

	private async void FetchHighscores() {
		try {
			var res = await client.PostAsync("https://keyslam.nl/lovejam2021/fetchHighscores.php", new StringContent(JsonConvert.SerializeObject(new {
				stage_index = this.stageIndex
			}), Encoding.UTF8, "application/json"));
			if (res != null && res.StatusCode == System.Net.HttpStatusCode.OK) {
				var str = await res.Content.ReadAsStringAsync();

				Response response = JsonConvert.DeserializeObject<Response>(str);

				if (response != null && response.status == 200) {
					highscores = response.highscores.ToList();
				}
			}
		} catch (Exception e) {

		}
	}

	private bool uploaded = false;

	private async void PutHighscore() {
		try {
			if (uploaded)
				return;

			uploaded = true;

			var res = await client.PostAsync("https://keyslam.nl/lovejam2021/putHighscores.php", new StringContent(JsonConvert.SerializeObject(new {
				username = this.name,
				score = gameContext.score,
				stage_index = this.stageIndex
			}), Encoding.UTF8, "application/json"));
		} catch (Exception e) {

		}
	}
}