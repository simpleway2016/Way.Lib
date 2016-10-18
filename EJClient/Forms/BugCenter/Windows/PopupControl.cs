using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WayControls.Windows.Hook;
using WayControls.Windows;
using System.Runtime.InteropServices;

namespace WayControls.Windows
{
    public class PopupForm : Form
    {

        public const int WS_BORDER = 0x800000;
        public const int WS_DLGFRAME = 0x400000;
        public const int WS_OVERLAPPED = 0x0;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                //createParams.Style = -2147483648;
                //createParams.Style = (createParams.Style & WS_BORDER & WS_DLGFRAME) | 0x40000000 | 0x242;
                createParams.Style = (createParams.Style & WS_OVERLAPPED) | 0x40000000 | 0x242;
                createParams.ExStyle = 0x88;
                return createParams;
            }
        }
    }
    public partial class PopupControl
    {
        public bool Showed = false;
        private Form form;
        private WayControls.Windows.Hook.MouseHook mouseHook;
        private Point position;
        private Control control;
        public event EventHandler OnHide;
        public List<Rectangle> NoHideRectangles;
        public Control ContainerControl;

        private WayControls.Windows.Hook.MouseHook.WayMouseEventHandler _OnMouseActivity;
        public PopupControl()
        {
            mouseHook = new WayControls.Windows.Hook.MouseHook();

            _OnMouseActivity = new WayControls.Windows.Hook.MouseHook.WayMouseEventHandler(mouseHook_OnMouseActivity);
            mouseHook.OnMouseActivity += _OnMouseActivity;
        }

        public Form GetShowingForm()
        {
            return this.form;
        }


        public void Show(System.Windows.Forms.Control control, Point position)
        {
            this.Show(control, position, null);
        }

        public void Show(System.Windows.Forms.Control control, Point position, List<Rectangle> NoHideInRects)
        {
            this.NoHideRectangles = NoHideInRects;
            this.position = position;
            this.control = control;
            if (control is Form)
            {
                this.form = control as Form;
                this.form.ShowInTaskbar = false;

            }
            else
            {

                if (Showed && this.control == control)
                {

                    control.Refresh();
                    //this.form.ClientSize = new Size(control.Width, control.Height);
                    SetWindowPos( position.X, position.Y, control.Width, control.Height);
                    return;
                }
                if (this.form != null)
                {
                    this.form.Controls.Clear();
                    this.form.Dispose();
                }

                this.form = new PopupForm();
                
                Control ccc = new Control();
                ccc.BackColor = Color.White;
                ccc.Dock = DockStyle.Fill;
                //this.form.ShowInTaskbar = false;
                //this.form.FormBorderStyle = FormBorderStyle.None;
                this.control = control;
                this.form.Controls.Add(ccc);
                ccc.Controls.Add(control);
                
            }

            this.form.CreateControl();

            this.SetWindowPos(position.X , position.Y , control.Width , control.Height);
            //this.form.ClientSize = new Size(control.Width, control.Height);
            //SetWindowPos(this.form.Handle, 0, position.X, position.Y, 2, 2, 1);
            //SetWindowPos(this.form.Handle, 0, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

            //this.form.Show();
            //ShowWindow(this.form.Handle, 4);
            this.form.Show();

            this.mouseHook.Start();
            Showed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="nCmdShow">
        /// Long，为窗口指定可视性方面的一个命令。请用下述任何一个常数 
        ///SW_HIDE 隐藏窗口，活动状态给令一个窗口 
        ///SW_MINIMIZE 最小化窗口，活动状态给令一个窗口 
        ///SW_RESTORE 用原来的大小和位置显示一个窗口，同时令其进入活动状态 
        ///SW_SHOW 用当前的大小和位置显示一个窗口，同时令其进入活动状态 
        ///SW_SHOWMAXIMIZED 最大化窗口，并将其激活 
        ///SW_SHOWMINIMIZED 最小化窗口，并将其激活 
        ///SW_SHOWMINNOACTIVE 最小化一个窗口，同时不改变活动窗口 
        ///SW_SHOWNA 用当前的大小和位置显示一个窗口，不改变活动窗口 
        ///SW_SHOWNOACTIVATE 用最近的大小和位置显示一个窗口，同时不改变活动窗口 
        ///SW_SHOWNORMAL 与SW_RESTORE相同 
        /// </param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern int ShowWindow(IntPtr hWnd, short cmdShow);




        [DllImport("USER32.DLL")]
        public static extern int SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x
            , int y, int width, int height, int wFlags);
        public const int HWND_TOPMOST = -1;
        public const int SWP_NOSIZE = 0x1;
        public const int SWP_NOMOVE = 0x2;
        public const int SWP_NOACTIVATE = 0x10;

        /// <summary>
        /// 设置位置和大小
        /// </summary>
        private void SetWindowPos(int left , int top , int width , int height)
        {
            int screentWidth = Screen.FromControl(this.control).Bounds.Width;
            int screentHeight = Screen.FromControl(this.control).Bounds.Height;
            if (left + width > screentWidth)
            {
                left = screentWidth - width - 1;
            }

            if (top + height > screentHeight)
            {
                top = screentHeight - height - 1;
            }
            //this.form.Location = new Point(position.X , position.Y);
            //WayControls.Windows.API.MoveWindow(this.form.Handle.ToInt32(), position.X, position.Y, 0, 0, false);
            WayControls.Windows.API.SetWindowPos(this.form.Handle, 0, left, top, width, height, 0);

        }



        /// <summary>
        /// 设置大小
        /// </summary>
        public void SetSize()
        {
            int screentWidth = Screen.FromControl(this.control).Bounds.Width;
            int screentHeight = Screen.FromControl(this.control).Bounds.Height;
            bool reSetPost = false;
            if (position.X + control.Size.Width > screentWidth)
            {
                reSetPost = true;
                position.X = screentWidth - this.control.Width - 1;
            }

            if (position.Y + control.Size.Height > screentHeight)
            {
                reSetPost = true;
                position.Y = screentHeight - this.control.Height - 1;
            }

            if (!reSetPost)
                WayControls.Windows.API.SetWindowPos(this.form.Handle, -1, 0, 0, control.Size.Width, control.Size.Height, 2);
            else
            {
                WayControls.Windows.API.SetWindowPos(this.form.Handle, -1, position.X, position.Y, control.Size.Width, control.Size.Height, 0);

            }
        }

        public void Hide()
        {
            this.mouseHook.Stop();

            if (this.ContainerControl != null)
            {
                this.ContainerControl.Focus();
            }
            if (this.form != null)
            {
                this.form.Hide();
            }
            Showed = false;

            if (this.control is Form)
            {
                this.form = null;
            }

            if (this.OnHide != null)
            {


                this.OnHide(this, EventArgs.Empty);
            }
        }

        private bool mouseHook_OnMouseActivity(MouseButtons button, int clicks, int x, int y, int delta, MouseHook.MouseActiveType ActiveType)
        {
            if (ActiveType == MouseHook.MouseActiveType.MouseUp)
            {
                Rectangle rect = new Rectangle(this.form.Location, this.form.Size);
                if (NoHideRectangles != null)
                {
                    foreach (Rectangle rect2 in NoHideRectangles)
                    {
                        if (rect2.Contains(new Point(x, y)))
                        {
                            return true;
                        }
                    }
                }
                if (rect.Contains(new Point(x, y)) == false)
                {
                    this.Hide();
                    return true;
                }
            }

            return true;
        }
    }
}
