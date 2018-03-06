using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using iTunesLib;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VSIX_MusicControl
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class PlayCommand
    {
        private iTunesApp iTunes;
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int PlayCommandId = 0x0100;
        public const int PrevCommandId = 0x0101;
        public const int NextCommandId = 0x0102;
        public const int TextCommandId = 0x0103;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("3ad5129b-26dd-4fe7-9d56-aac3109ffb8d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private PlayCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            iTunes = new iTunesApp();

            if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                commandService.AddCommand(
                    new MenuCommand(this.PlayButtonCallback, new CommandID(CommandSet, PlayCommandId)));
                commandService.AddCommand(
                    new MenuCommand(this.PrevButtonCallback, new CommandID(CommandSet, PrevCommandId)));
                commandService.AddCommand(
                    new MenuCommand(this.NextButtonCallback, new CommandID(CommandSet, NextCommandId)));
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static PlayCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new PlayCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void PlayButtonCallback(object sender, EventArgs e)
        {
            iTunes.PlayPause();
        }

        private void PrevButtonCallback(object sender, EventArgs e)
        {
            iTunes.PreviousTrack();
        }

        private void NextButtonCallback(object sender, EventArgs e)
        {
            iTunes.NextTrack();
        }
    }
}
