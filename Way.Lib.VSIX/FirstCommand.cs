//------------------------------------------------------------------------------
// <copyright file="FirstCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using EnvDTE80;

namespace Way.Lib.VSIX
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class FirstCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("8d988374-15d5-47c1-a0db-bae06ae45f77");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private FirstCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static FirstCommand Instance
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
            Instance = new FirstCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
          
            IntPtr hierarchyPtr, selectionContainerPtr;
                    uint projectItemId;
                    IVsMultiItemSelect mis;

            IVsMonitorSelection monitorSelection = (IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection));

            monitorSelection.GetCurrentSelection(out hierarchyPtr, out projectItemId, out mis, out selectionContainerPtr);
          

            DTE2 dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            if (dte == null)
                return;
 
            Projects projects = dte.Solution.Projects;
            for (int i = 1; i <= projects.Count; i++)
            {
                Project project = projects.Item(i);
                checkElements(project.ProjectItems);
            }
            //string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            //string title = "FirstCommand";

            //// Show a message box to prove we were here
            //VsShellUtilities.ShowMessageBox(
            //    this.ServiceProvider,
            //    message,
            //    title,
            //    OLEMSGICON.OLEMSGICON_INFO,
            //    OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        void checkElements(ProjectItems projectitems)
        {
            for (int i = 1; i <= projectitems.Count; i++)
            {
                ProjectItem proitem = projectitems.Item(i);
                string filename = proitem.FileNames[1];
                if (proitem.FileCodeModel != null)
                {
                    string language = proitem.FileCodeModel.Language; 
                   foreach( CodeElement2 codeitem in proitem.FileCodeModel.CodeElements)
                    {
                        try
                        {
                            if(codeitem.Kind == vsCMElement.vsCMElementNamespace)
                            {
                                string fullname = codeitem.FullName;
                                string name = codeitem.Name;

                                checkNamespace(codeitem.Children);
                            }
                        }
                        catch { }
                    }
                }
                try { 
                checkElements(proitem.ProjectItems);
                 }
                catch { }
            }
        }

        void checkNamespace(CodeElements elements)
        {
            foreach(CodeElement2 codeitem in elements)
            {
              
                try
                {
                    if (codeitem.Kind == vsCMElement.vsCMElementClass)
                    {
                        
                        CodeClass2 codeclass = (CodeClass2)codeitem;
                        if (codeclass.Access != vsCMAccess.vsCMAccessPrivate)
                        {
                            bool isderived = codeclass.IsDerivedFrom["System.Windows.Forms.ContainerControl"];
                            string fullname = codeitem.FullName;
                            string name = codeitem.Name;
                            var kind = codeitem.Kind.ToString();
                        }
                    }
                }
                catch { }
            }
        }
    }
}
