using System;
using GBGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Components;

namespace GBGame.Items;

public class Bomb(Game windowData, Player player) : Item(windowData)
{
    private readonly Color _overlayColour = new Color(40, 56, 24);

    private int _bombCount = 5;
    public bool CanPlace = true;
    public AnimatedSpriteSheet Sheet = null!;
    
    private Vector2 _pos;

    public bool Exploded = false;
    public Rectangle KillRadius;
    public Rectangle RadiusData;

    private Shapes _shapes = null!;

    public int BombCount
    {
        get => _bombCount;
        set
        {
            _bombCount = value;
            Description = _bombCount == 1 ? "1 bomb left." : $"{(_bombCount == 0 ? "No" : _bombCount)} bombs left."; 
        }
    }

    public override void LoadContent()
    {
        InventorySprite = WindowData.Content.Load<Texture2D>("Sprites/UI/Bomb");

        Name = "Bomb";
        Description = $"{_bombCount} bombs left.";

        Sheet = new AnimatedSpriteSheet(WindowData.Content.Load<Texture2D>("Sprites/SpriteSheets/BombPlaced"), new Vector2(5, 1), 0.25f);
    
        _shapes = new Shapes(WindowData.GraphicsDevice);
        RadiusData = new Rectangle(16, 16, 40, 40);
    }

    public override void Use()
    {
        if (_bombCount - 1 < 0 || !CanPlace) 
        {
            Console.WriteLine("Cannot use Bomb!");
            return;
        }

        _bombCount--;
        Description = _bombCount == 1 ? "1 bomb left." : $"{(_bombCount == 0 ? "No" : _bombCount)} bombs left."; 

        CanPlace = false;
        Sheet.Finished = false;
        _pos = Vector2.Floor(player.Position / 8) * 8;
        KillRadius = new Rectangle((int)_pos.X - RadiusData.X, (int)_pos.Y - RadiusData.Y, RadiusData.Width, RadiusData.Height);
        
        Console.WriteLine("Used Bomb.");
    }

    public void Draw(SpriteBatch batch)
    { 
        Sheet.Draw(batch, _pos);
        _shapes.DrawRectangleMinimal(KillRadius, _overlayColour, 3, batch);
    }
}
