using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExampleGame;

/// <summary>
/// Implementation of a dummy <see cref="Game"/> instance.
/// </summary>
public class Game1 : Game
{
  private GraphicsDeviceManager _graphics;
  private SpriteBatch _spriteBatch;

  private Texture2D _image;

  /// <summary>
  /// Initializes a new instance of the <see cref="Game1"/> class.
  /// </summary>
  public Game1()
  {
    _graphics = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
  }

  /// <inheritdoc />
  protected override void LoadContent()
  {
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    _image = Content.Load<Texture2D>("Images/Image");
  }

  /// <inheritdoc />
  protected override void Update(GameTime gameTime)
  {
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
        Keyboard.GetState().IsKeyDown(Keys.Escape))
    {
      Exit();
    }

    base.Update(gameTime);
  }

  /// <inheritdoc />
  protected override void Draw(GameTime gameTime)
  {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _spriteBatch.Draw(_image, Vector2.Zero, Color.White);

    base.Draw(gameTime);
  }
}
