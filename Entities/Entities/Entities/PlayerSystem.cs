using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using EntityComponents;

namespace Entities
{
    public class PlayerSystem : EntityProcessingSystem
    {

        GraphicsDevice _graphicsDevice;
        SpriteBatch _spriteBatch;

        int xSpeed = 200;

        ComponentMapper <Vector2D> _vector2DMapper;

        public PlayerSystem(GraphicsDevice graphicsDevice)
            : base(Aspect.All(typeof(Vector2D)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _vector2DMapper = mapperService.GetMapper<Vector2D>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var vector2d = _vector2DMapper.Get(entityId);
            vector2d.X += (int) (xSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            Console.WriteLine("Pat Was Here:Vector2d:" + vector2d.X +", " + vector2d.Y);
        }
    }
}
