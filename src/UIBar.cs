using System;
using System.Collections.Generic;
using Love;

public class UIBar {
	public static Image image = Graphics.NewImage("Assets/UIBar.png");
	public static List<Image> imageFont = new List<Image>() {
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

	public void Draw(GameContext gameContext) {
		Graphics.Draw(image, 0, 900, 0, 4, 4);

		Graphics.Draw(imageFont[(int)MathF.Max(0, gameContext.lives)], 824, 916, 0, 4, 4);	

		string scoreString = gameContext.score.ToString();
		for (int i = 0; i < scoreString.Length; i++) {
			Graphics.Draw(imageFont[Convert.ToInt32(new string(scoreString[i], 1))], 196 + i * 32, 916, 0, 4, 4);
		}
	}
}