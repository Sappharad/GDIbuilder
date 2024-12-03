using System;
using System.ComponentModel;
using Eto.Forms;
using Eto.Drawing;

namespace GDIBuilder2
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Title = "GDIBuilder";
            MinimumSize = new Size(520, 350);
            Resizable = false;
            
            DynamicLayout mainLayout = new DynamicLayout();
            mainLayout.BeginHorizontal();
            mainLayout.AddCentered(new Label { Text = "What would you like to do?", TextAlignment = TextAlignment.Center });
            mainLayout.EndHorizontal();

            TableLayout options = new TableLayout(3, 2);
            Button buildGdi = new Button() { Text = "Build from folder", Height = 30 };
            buildGdi.Click += (sender, args) =>
            {
                BuildView bv = new BuildView();
                this.Visible = false;
                bv.Show();
            };
            options.Add(buildGdi, 0, 0);
            Button rebuildDisc = new Button() { Text = "Patch a copy", Height = 30  };
            rebuildDisc.Click += (sender, args) =>
            {
                RebuildView rv = new RebuildView();
                this.Visible = false;
                rv.Show();
            };
            options.Add(rebuildDisc, 1, 0);
            Button navigator = new Button() { Text = "Navigate or Extract", Height = 30  };
            navigator.Click += (sender, args) =>
            {
                ExtractView ev = new ExtractView();
                this.Visible = false;
                ev.Show();
            };
            options.Add(navigator, 2, 0);
            options.Add(new TextArea
            {
                Text = "Build new data tracks(s) for the high density area of a GD-ROM image. " +
                       "This takes a folder full of files and an IP.BIN bootstrap, along with optional CDDA and generates a new track03.bin along " +
                       "with a final track for discs with CDDA. This app does not generate track01 or track02, those are standard PC readable tracks " +
                       "that you can generate with normal ISO tools like mkisofs or just copy them from an existing image.",
                ReadOnly = true
            }, 0, 1);
            options.Add(new TextArea
            {
                Text = "Using an existing GDI and a folder containing only the files to replace or add in the exact " +
                       "same folder structure as the original disc, create a patched copy of an existing disc " +
                       "image that adds or replaces some files. The rest of the files will be copied from the " +
                       "original image without needing to extract them first.",
                ReadOnly = true
            }, 1, 1);
            options.Add(new TextArea()
            {
                Text = "Open an existing .gdi to browse or extract files from the high density area of a " +
                       "GD-ROM disc. This also supports reading and extracting the high density tracks from GD-ROMs in .bin + .cue format.",
                ReadOnly = true
            }, 2, 1);
            options.Padding = new Padding(6);
            options.Spacing = new Size(6, 6);
            options.SetRowScale(1, true);
            options.SetColumnScale(0, true);
            options.SetColumnScale(1, true);
            options.SetColumnScale(2, true);
            mainLayout.BeginHorizontal();
            mainLayout.Add(options);
            mainLayout.EndHorizontal();
            mainLayout.Padding = new Padding(8);
            
            Content = mainLayout;
            // create a few commands that can be used for the menu and toolbar
            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();
            Menu = new MenuBar { QuitItem = quitCommand };
            
            Closed += KillProgramOnMainWindowExit;
            Application.Instance.Terminating += (s, e) => Closed -= KillProgramOnMainWindowExit;
        }

        private static void KillProgramOnMainWindowExit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }
    }
}