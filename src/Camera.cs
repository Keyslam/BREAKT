using System;
using Love;

public class Camera {
	private float trauma = 0;

	public void Update(float dt) {
		this.trauma = Mathf.Max(0.0f, this.trauma - (dt * 2.0f));
	}

	public void AddTrauma(float amount) {
		this.trauma = MathF.Min(1.0f, this.trauma + amount);
	}
	
	public void Apply(GameContext gameContext) {
		Graphics.Push(StackType.All);

		float roffset = ((Love.Mathf.Noise((gameContext.time * 5.0f) + 103200.420f) * 2 - 1) * this.trauma * this.trauma) * 0.025f;
		// Graphics.Translate(450, 450);
		// Graphics.Rotate(roffset);
		// Graphics.Translate(-450, -450);

		float xoffset = ((Love.Mathf.Noise((gameContext.time * 7.0f) + 10540.938f) * 2 - 1) * this.trauma * this.trauma) * 25.0f;
		float yoffset = ((Love.Mathf.Noise((gameContext.time * 7.0f) + 1032.290f) * 2 - 1) * this.trauma * this.trauma) * 25.0f;
		Graphics.Translate(xoffset, yoffset);
	}

	public void Detach() {
		Graphics.Pop();
	}
}