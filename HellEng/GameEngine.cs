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
    public RenderWindow Window;

    // check if sdl window is in focus
    public bool IsFocused
    {
        get => Window.HasFocus();
    }

    public void Start()
    {
        // sdl renderer
        VideoMode mode = new VideoMode(800, 600);
        Window = new RenderWindow(mode, "Game Engine");
        Window.Closed += (s, e) => Window.Close();
        Window.Resized += (s, e) => Size = new Vector2u(e.Width, e.Height);

        Window.SetActive();

        OnLoad(); // load event

        long targetTicksPerFrame = TimeSpan.TicksPerSecond / targetFPS;
        long prevTicks = DateTime.UtcNow.Ticks;

        long u_targetTicksPer = TimeSpan.TicksPerSecond / updateRate;
        long u_prevTicks = DateTime.UtcNow.Ticks;

        long accumulatedTicks = 0;

        //Task.Factory.StartNew(() =>
        //{
        //    while (window.IsOpen)
        //    {
        //        // handle update (60 u_targetTicksPer)
        //        long u_currTicks = DateTime.Now.Ticks;
        //        long u_elapsedTicks = u_currTicks - u_prevTicks;

        //        if (u_elapsedTicks >= u_targetTicksPer)
        //        {
        //            u_prevTicks = u_currTicks;
        //            OnUpdate(); // update game
        //        }
        //    }
        //});

        while (Window.IsOpen)
        {
            // handle draw (165 targetFPS)
            long currTicks = DateTime.UtcNow.Ticks;
            long elapsedTicks = currTicks - prevTicks;

            if (elapsedTicks >= targetTicksPerFrame)
            {
                prevTicks = currTicks;
                OnDraw(Window); // redraw window
            }


            // handle update (60 u_targetTicksPer)
            long u_currTicks = DateTime.UtcNow.Ticks;
            long u_elapsedTicks = u_currTicks - u_prevTicks;

            if (u_elapsedTicks >= u_targetTicksPer)
            {
                u_prevTicks = u_currTicks;
                OnUpdate(); // update game
            }

            SystemTimer.Sleep(1);
        }

        OnUnload();

        // clean up
        Window.Close();
        Window.Dispose();
    }

    protected virtual void OnDraw(RenderWindow ctx) { }// draw event
    protected virtual void OnUpdate() { }// update event
    protected virtual void OnLoad() { }// load event
    protected virtual void OnUnload() { }// unload event

    #region Easy Game Properties

    public Vector2u Size
    {
        get => Window.Size;
        set
        {
            View view = new View(new FloatRect(0, 0, value.X, value.Y));
            Window.SetView(view);
        }
    }

    public String Title
    { set => Window.SetTitle(value); }

    #endregion
}