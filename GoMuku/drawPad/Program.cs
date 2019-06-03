using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

///please note that this program is inspired by https://blog.csdn.net/Jakie_Zhan/article/details/79985923, the basic structor is from this site and i improvised and added some additonal features on the original code 
///i have also used parital code from https://www.jb51.net/article/133856.htm to form the final program
///i fixed the errors within those two codes, however, i have issues with the starting game window problem and didn't find a perfect solution to delete the existing game event, i could put those events into 
///Form1.Designer.cs, but i realize that it might be harder for me to get the informations from the class since they are private, so i decide to leave those issues there and feel welcome to upgrade my code if you want
///as long as it is not for commercial uses
///writer of the code: ziran cao

namespace drawPad
{
    static class program 
    {
        

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 testing = new Form1();
            Application.Run( new Form1());
        }
    }
}
