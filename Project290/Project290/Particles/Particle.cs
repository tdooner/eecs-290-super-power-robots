//////__     __               _                 _     _               _   
//////\ \   / /              | |               | |   | |             | |  
////// \ \_/ /__  _   _   ___| |__   ___  _   _| | __| |  _ __   ___ | |_ 
//////  \   / _ \| | | | / __| '_ \ / _ \| | | | |/ _` | | '_ \ / _ \| __|
//////   | | (_) | |_| | \__ \ | | | (_) | |_| | | (_| | | | | | (_) | |_ 
//////   |_|\___/ \__,_| |___/_| |_|\___/ \__,_|_|\__,_| |_| |_|\___/ \__|
//////      _                              _   _     _     _ 
//////     | |                            | | | |   (_)   | |
//////  ___| |__   __ _ _ __   __ _  ___  | |_| |__  _ ___| |
////// / __| '_ \ / _` | '_ \ / _` |/ _ \ | __| '_ \| / __| |
//////| (__| | | | (_| | | | | (_| |  __/ | |_| | | | \__ \_|
////// \___|_| |_|\__,_|_| |_|\__, |\___|  \__|_| |_|_|___(_)
//////                         __/ |                         
//////                        |___/                          

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Clock;
using Microsoft.Xna.Framework;

namespace Project290.Particles
{
    /// <summary>
    /// A single particle.
    /// </summary>
    public class Particle
    {
        /// <summary>
        /// Gets the position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Gets the velocity.
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// Gets the color to overlay the draw image with.
        /// </summary>
        public Color color;

        /// <summary>
        /// The scale of the particle.
        /// </summary>
        public float Scale;

        /// <summary>
        /// The rotation of the particle.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Speed at which an increase in rotation is applied.
        /// </summary>
        public float RotationSpeed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Particle"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the creation time.
        /// </summary>
        public long CreationTime;

        /// <summary>
        /// The action to be performed per update.
        /// </summary>
        public ParticleAction Action;

        /// <summary>
        /// This is used to identify what "type" in a custom action this particle belongs to.
        /// For example, you may want to have one action which could draw one of several different
        /// images. You can set this value to indicate which image to draw. This does not have
        /// to be used.
        /// </summary>
        public byte ActionUniqueIdentifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="Particle"/> class.
        /// </summary>
        /// <param name="xPosition">The x position.</param>
        /// <param name="yPosition">The y position.</param>
        /// <param name="xVelocity">The x velocity.</param>
        /// <param name="yVelocity">The y velocity.</param>
        /// <param name="color">The color to overlay the drawn image with.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="actionUniqueIdentifier">This is used to identify what "type" in a custom action this particle belongs to.
        /// For example, you may want to have one action which could draw one of several different
        /// images. You can set this value to indicate which image to draw. This does not have
        /// to be used.</param>
        /// <param name="action">The action. This should be a reference to a STATIC object, for memory sake.</param>
        public Particle(float xPosition, float yPosition, float xVelocity, float yVelocity, Color color, float scale, float rotation, byte actionUniqueIdentifier, ParticleAction action)
        {
            this.Position = new Vector2();
            this.Velocity = new Vector2();
            this.Reset(xPosition, yPosition, xVelocity, yVelocity, color, scale, rotation, actionUniqueIdentifier, action);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Particle"/> class.
        /// </summary>
        /// <param name="xPosition">The x position.</param>
        /// <param name="yPosition">The y position.</param>
        /// <param name="xVelocity">The x velocity.</param>
        /// <param name="yVelocity">The y velocity.</param>
        /// <param name="color">The color to overlay the drawn image with.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="rotationSpeed">The rotaiton speed.</param>
        /// <param name="actionUniqueIdentifier">This is used to identify what "type" in a custom action this particle belongs to.
        /// For example, you may want to have one action which could draw one of several different
        /// images. You can set this value to indicate which image to draw. This does not have
        /// to be used.</param>
        /// <param name="action">The action. This should be a reference to a STATIC object, for memory sake.</param>
        public Particle(float xPosition, float yPosition, float xVelocity, float yVelocity, Color color, float scale, float rotation, float rotationSpeed, byte actionUniqueIdentifier, ParticleAction action)
        {
            this.Position = new Vector2();
            this.Velocity = new Vector2();
            this.Reset(xPosition, yPosition, xVelocity, yVelocity, color, scale, rotation, rotationSpeed, actionUniqueIdentifier, action);
        }

        /// <summary>
        /// Resets the specified x position.
        /// </summary>
        /// <param name="xPosition">The x position.</param>
        /// <param name="yPosition">The y position.</param>
        /// <param name="xVelocity">The x velocity.</param>
        /// <param name="yVelocity">The y velocity.</param>
        /// <param name="color">The color to overlay the drawn image with.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="actionUniqueIdentifier">This is used to identify what "type" in a custom action this particle belongs to.
        /// For example, you may want to have one action which could draw one of several different
        /// images. You can set this value to indicate which image to draw. This does not have
        /// to be used.</param>
        /// <param name="action">The action. This should be a reference to a STATIC object, for memory sake.</param>
        public void Reset(float xPosition, float yPosition, float xVelocity, float yVelocity, Color color, float scale, float rotation, byte actionUniqueIdentifier, ParticleAction action)
        {
            this.Position.X = xPosition;
            this.Position.Y = yPosition;
            this.Velocity.X = xVelocity;
            this.Velocity.Y = yVelocity;
            this.Disposed = false;
            this.CreationTime = GameClock.Now;
            this.Action = action;
            this.color = color;
            this.Scale = scale;
            this.Rotation = rotation;
            this.ActionUniqueIdentifier = actionUniqueIdentifier;
            this.RotationSpeed = 0;
        }

        /// <summary>
        /// Resets the specified x position.
        /// </summary>
        /// <param name="xPosition">The x position.</param>
        /// <param name="yPosition">The y position.</param>
        /// <param name="xVelocity">The x velocity.</param>
        /// <param name="yVelocity">The y velocity.</param>
        /// <param name="color">The color to overlay the drawn image with.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="rotationSpeed">The rotation speed.</param>
        /// <param name="actionUniqueIdentifier">This is used to identify what "type" in a custom action this particle belongs to.
        /// For example, you may want to have one action which could draw one of several different
        /// images. You can set this value to indicate which image to draw. This does not have
        /// to be used.</param>
        /// <param name="action">The action. This should be a reference to a STATIC object, for memory sake.</param>
        public void Reset(float xPosition, float yPosition, float xVelocity, float yVelocity, Color color, float scale, float rotation, float rotationSpeed, byte actionUniqueIdentifier, ParticleAction action)
        {
            this.Position.X = xPosition;
            this.Position.Y = yPosition;
            this.Velocity.X = xVelocity;
            this.Velocity.Y = yVelocity;
            this.Disposed = false;
            this.CreationTime = GameClock.Now;
            this.Action = action;
            this.color = color;
            this.Scale = scale;
            this.Rotation = rotation;
            this.ActionUniqueIdentifier = actionUniqueIdentifier;
            this.RotationSpeed = rotationSpeed;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            if (!this.Disposed)
            {
                this.Position.X += this.Velocity.X;
                this.Position.Y += this.Velocity.Y;
                this.Rotation += this.RotationSpeed;
                this.Action.Update(this);
            }
        }

        /// <summary>
        /// Applies the acceloration to the velcoity of the particle.
        /// </summary>
        /// <param name="acceloration">The acceloration.</param>
        public void ApplyAcceloration(ref Vector2 acceloration)
        {
            this.Velocity.X += acceloration.X;
            this.Velocity.Y += acceloration.Y;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            this.Disposed = true;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            if (!this.Disposed)
            {
                this.Action.Draw(this);
            }
        }
    }
}
