using System;
using System.Collections.Generic;
using Humper;
using Love;
using Retromono.Easings;
using Retromono.Tweens;

public class Program : Scene {
	private static Image menu = Graphics.NewImage("Assets/Menu.png");
	private static Image itemSelect = Graphics.NewImage("Assets/ItemSelect.png");

	private Vector2 menuPos = new Vector2(208, -492);

	private StageBase stage = null;

	private Source track = null;

	private AudioManager audioManager = null;

	private int index = 0;

	private List<ITween> tweens = new List<ITween>();

	public override void Load() {
		Window.SetIcon(Image.NewImageData("Assets/Blocks/ChocolateStandard.png"));

		Graphics.SetDefaultFilter(FilterMode.Nearest, FilterMode.Nearest);

		Graphics.SetBackgroundColor(0.922f, 0.863f, 0.647f, 1.000f);

		audioManager = new AudioManager();

		audioManager.PlayMenuGroove();
		Enter();

		//DoStage(new StageTest(audioManager));
	}

	public void Enter() {
		tweens.Add(
			new TweenParallel(() => { }, new List<ITween>() {
				new TweenAnimateDouble(
					TimeSpan.FromSeconds(1.5f),
					menuPos.Y,
					204f,
					value => { menuPos.Y = (float)value; },
					Easings.ElasticOut
				),
			}
		));
	}

	public override void Update(float dt) {
		foreach (ITween tween in tweens)
			tween.Advance(TimeSpan.FromSeconds(dt));

		if (stage != null)
			this.stage.Update(dt);
	}

	public override void Draw() {
		if (stage != null)
			this.stage.Draw();
		else {
			Graphics.Draw(menu, menuPos.X, menuPos.Y, 0, 4, 4);
			Graphics.Draw(itemSelect, menuPos.X + 16, menuPos.Y + 120 + (index * 48 + (index == 6 ? 36 : 0)), 0, 4, 4);
		}
	}

	bool animating = false;

	private bool bGot = false;
	private bool oGot = false;
	private bool nGot = false;
	private bool eGot = false;

	public override void KeyPressed(KeyConstant key, Scancode scancode, bool isRepeat) {
		if (stage != null)
			this.stage.Keypressed(key);
		else {

			if (key == KeyConstant.E && eGot == false && bGot == true && oGot == true && nGot == true) {
				eGot = true;
			} else {
				if (key == KeyConstant.N && nGot == false && bGot == true && oGot == true) {
					
					nGot = true;
				} else {
					if (key == KeyConstant.O && oGot == false && bGot == true) {
						oGot = true;
						
					} else {
						if (key == KeyConstant.B && bGot == false) {
							bGot = true;

						} else {
							bGot = false;
							oGot = false;
							nGot = false;
							eGot = false;
						}
					}
				}
			}

			if (bGot && oGot && nGot && eGot) {
				bGot = false;
				oGot = false;
				nGot = false;
				eGot = false;

				Console.Write("Engage b o n e m o d e");

				DoStage(new StageBadTime(audioManager));
			}

			if (key == KeyConstant.Space || key == KeyConstant.Return) {
				if (animating)
					return;

				audioManager.PlaySource(audioManager.menuConfirm);


				if (index == 0)
					DoStage(new StageHazelnut(audioManager));
				if (index == 1)
					DoStage(new StageLove(audioManager));
				if (index == 2)
					DoStage(new StageChip(audioManager));
				if (index == 3)
					DoStage(new StageCocoa(audioManager));
				if (index == 4)
					DoStage(new StageMelt(audioManager));
				if (index == 5)
					DoStage(new StageCrunch(audioManager));
				if (index == 6)
					DoStage(new StageEndless(audioManager));

			}

			if (key == KeyConstant.S || key == KeyConstant.Down) {
				index = (int)MathF.Min(index + 1, 6);
			}

			if (key == KeyConstant.W || key == KeyConstant.Up) {
				index = (int)MathF.Max(index - 1, 0);
			}
		}
	}

	public void DoStage(StageBase stage) {
		animating = true;

		tweens.Add(
					new TweenSequence(() => { }, new List<ITween>() {
						new TweenAnimateDouble(
							TimeSpan.FromSeconds(0.8f),
							menuPos.Y,
							 -592,
							value => { menuPos.Y = (float)value; },
							Easings.CubicIn
						),

						new TweenCallback(() => {
							this.stage = stage;
							this.stage.Done += OnDone;

							this.animating = false;
						})
					})
				);
	}

	public void OnDone() {
		this.stage.Done -= OnDone;
		this.stage = null;

		audioManager.PlayMenuGroove();
		Enter();
	}
}