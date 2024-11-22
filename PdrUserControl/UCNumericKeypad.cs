using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PdrUserControl
{
    public partial class UCNumericKeypad: UserControl
    {
        Button btn;
        List<Button> numButton = new List<Button>();
        public UCNumericKeypad()
        {
            InitializeComponent();
        }

        private void UCNumericKeypad_Load(object sender, EventArgs e)
        {
            // Determine the size of the pannel
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Convert.ToInt32(screenWidth * 5 / 12);
            this.Height = Convert.ToInt32(screenHeight *5 / 12);

            // Determine number of buttons ⟵ ⌫ ⌦
            string[] btnArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "⟵\nBackspace", "0", "Enter\n⏎" };
            List<string> btnList = new List<string>(btnArray);
            
            // Determine array size for buttoin positioning (Ex. 3x3, 4x3, 4x2)
            int rowNum = Convert.ToInt32(Math.Ceiling(Math.Sqrt(btnList.Count())));
            //int colNum = Convert.ToInt32(Math.Floor(Math.Sqrt(btnList.Count())));
            int colNum = Convert.ToInt32(Math.Round(Math.Sqrt(btnList.Count())));
            
            // Determine Each Button dimension
            int btnWidth = Convert.ToInt32(Math.Floor(Convert.ToDecimal(this.Width / colNum)));
            int btnHeight = Convert.ToInt32(Math.Floor(Convert.ToDecimal(this.Height / rowNum)));
            //MessageBox.Show(colNum.ToString()+"x"+rowNum.ToString());

            //create Buttons as per btn array
            int index = 0;
            foreach (string btnName in btnList)
            {
                btn = new Button();
                //btn.Name = "btnAuto" + btnName;
                btn.Name = "btnAuto" + index;
                btn.Size = new Size(btnWidth,btnHeight);
                btn.Location = new Point(btnWidth*(index%colNum),btnHeight*Convert.ToInt32(Math.Floor(Convert.ToDecimal(index / colNum))));
                btn.Text = btnName;
                btn.Font = new Font(btn.Font.FontFamily, Math.Min(Convert.ToInt32(btn.Height / 4), Convert.ToInt32(btn.Width / 8)));
                //btn.Click += new EventHandler(SoftKeyboard_Click);
                this.Controls.Add(btn);
                index++;
            }
        }

        private void SoftKeyboard_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(GetFocusControl());
            Button btnClickEvent = (Button)sender;
            switch (btnClickEvent.Name)
            {
                case "btnAuto0":
                    SendKeys.Send("{1}");
                    break;
                case "btnAuto1":
                    SendKeys.Send("{2}");
                    break;
                case "btnAuto2":
                    SendKeys.Send("{3}");
                    break;
                case "btnAuto3":
                    SendKeys.Send("{4}");
                    break;
                case "btnAuto4":
                    SendKeys.Send("{5}");
                    break;
                case "btnAuto5":
                    SendKeys.Send("{6}");
                    break;
                case "btnAuto6":
                    SendKeys.Send("{7}");
                    break;
                case "btnAuto7":
                    SendKeys.Send("{8}");
                    break;
                case "btnAuto8":
                    SendKeys.Send("{9}");
                    break;
                case "btnAuto9":
                    SendKeys.Send("{BACKSPACE}");
                    break;
                case "btnAuto10":
                    SendKeys.Send("{0}");
                    break;
                case "btnAuto11":
                    SendKeys.Send("{ENTER}");
                    break;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        private string GetFocusControl()
        {
            Control focusControl = null;
            IntPtr focusHandle = GetFocus();
            if (focusHandle != IntPtr.Zero)
                focusControl = Control.FromHandle(focusHandle);
            if (focusControl.Name.ToString().Length == 0)
                return focusControl.Parent.Parent.Name.ToString();
            else
                return focusControl.Name.ToString();
        }
    }
}
