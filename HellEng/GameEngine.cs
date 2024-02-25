#region Includes

using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View = SFML.Graphics.View;

#endregion

internal class GameEngine
{
    /// <summary>
    /// The games target framerate
    /// </summary>
    private int targetFPS = 165;
    private int updateRate = 60;

    // sdl stuff
    public RenderWindow window;

    // check if sdl window is in focus
    public bool IsFocused
    {
        get => window.HasFocus();
    }

    public void Start()
    {
        // sdl renderer
        VideoMode mode = new VideoMode(800, 600);
        window = new RenderWindow(mode, "Game Engine");
        window.Closed += (s, e) => window.Close();
        window.Resized += (s, e) => Size = new Vector2u(e.Width, e.Height);

        window.SetActive();

        OnLoad(); // load event

        long targetTicksPerFrame = TimeSpan.TicksPerSecond / targetFPS;
        long prevTicks = DateTime.Now.Ticks;

        long u_targetTicksPer = TimeSpan.TicksPerSecond / updateRate;
        long u_prevTicks = DateTime.Now.Ticks;

        long accumulatedTicks = 0;

        Task.Factory.StartNew(() =>
        {
            while (window.IsOpen)
            {
                // handle update (60 u_targetTicksPer)
                long u_currTicks = DateTime.Now.Ticks;
                long u_elapsedTicks = u_currTicks - u_prevTicks;

                if (u_elapsedTicks >= u_targetTicksPer)
                {
                    u_prevTicks = u_currTicks;
                    OnUpdate(); // update game
                }
            }
        });

        while (window.IsOpen)
        {
            // handle draw (165 targetFPS)
            long currTicks = DateTime.Now.Ticks;
            long elapsedTicks = currTicks - prevTicks;

            if (elapsedTicks >= targetTicksPerFrame)
            {
                prevTicks = currTicks;
                OnDraw(window); // redraw window
            }

            //Thread.Sleep(1);
        }

        OnUnload();

        // clean up
        window.Close();
        window.Dispose();
    }

    protected virtual void OnDraw(RenderWindow ctx) { }// draw event
    protected virtual void OnUpdate() { }// update event
    protected virtual void OnLoad() { }// load event
    protected virtual void OnUnload() { }// unload event

    #region Easy Game Properties

    public Vector2u Size
    {
        get => window.Size;
        set
        {
            View view = new View(new FloatRect(0, 0, value.X, value.Y));
            window.SetView(view);
        }
    }

    public String Title
    { set => window.SetTitle(value); }

    #endregion
}