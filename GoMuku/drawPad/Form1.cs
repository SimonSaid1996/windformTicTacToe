using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace drawPad
{
   public partial class Form1 : Form           
    {
        private Panel panel1;
        private Image baseImg;
        private Button bt1;
        private Button bt2;
        private Button bt3;
        private Button bt4;
        private int xLoc;
        private int yLoc;
        private bool gameOver = false;
        private bool currentWhite = false;                 //to judge which color plays first
        private int[ , ] data = new int[16, 16];           //to automatically create a 15*15 2d array of 0s to store the game data, 2d array
        private int pieceCount = 0;                        //count the number of pieces to judge if the board is full
        private Label label1; 

        public Form1()                          
        {                                       //the board is a 15*15 board with 4 buttons and a lable on it
            InitializeComponent();

            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.Dock = System. Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(30, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 600);               //15*15 board, each side is 40, not enough
            this.Controls.Add(this.panel1);
            
            //adding buttons
            this.bt1 = new Button();//initialize 4 buttons
            bt1.Text = "start game";
            bt1.Location = new Point(800,100);
            bt1.Size = new Size(70,70);
            this.panel1.Controls.Add(this.bt1);
            this.bt2 = new Button();
            bt2.Text = "end game";
            bt2.Location = new Point(800, 300);
            bt2.Size = new Size(70,70);
            this.panel1.Controls.Add(this.bt2);
            this.bt3 = new Button();
            bt3.Text = "white first";
            bt3.Location = new Point(800,400);
            bt3.Size = new Size(70,70);
            this.panel1.Controls.Add(this.bt3);
            this.bt4 = new Button();
            bt4.Text = "black first";
            bt4.Location = new Point(800,500);
            bt4.Size = new Size(70,70);
            this.panel1.Controls.Add(this.bt4);

             //there is a glitch i didn't fix in the game, the buttons and the label can't show up unless fixing the game window, i am not sure why
            this.label1 = new Label();
            this.label1.Location = new System.Drawing.Point(800,200);                       //label to show the game situations
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 80);
            this.label1.Text = "game not started";
            this.panel1.Controls.Add(this.label1);

            //adding functions to each buttons
            this.bt1.MouseClick += new MouseEventHandler(btnStart_Click);
            this.bt2.MouseClick += new MouseEventHandler(btnEnd_Click);
            
            this.Load += new EventHandler(Form1_Load);                                      //to print out the game board
                      
        }

        private void whiteStarter(object sender, MouseEventArgs e)
        {
            this.panel1.MouseClick += new MouseEventHandler(panel1_MouseClick);             //can only play the game after starting, white pieces first
            this.bt3.Enabled = false;
            this.bt4.Enabled = false;
            currentWhite = true;
        }


        private void blackStarter(object sender, MouseEventArgs e)
        {
            this.panel1.MouseClick += new MouseEventHandler(panel1_MouseClick);             //can only play the game after starting, black pieces first
            this.bt3.Enabled = false;
            this.bt4.Enabled = false;
            currentWhite = false;
        }

        private void btnStart_Click(object sender, MouseEventArgs e)                        //to start the game we need to choose which player goes first
        {
            label1.Text = "game start";
            bt1.Enabled = false;
            bt2.Enabled = true;
            MessageBox.Show("please choose which player to start first");
            this.bt3.MouseClick += new MouseEventHandler(whiteStarter);
            this.bt4.MouseClick += new MouseEventHandler(blackStarter);                     
        }

        private void btnEnd_Click(object sender, MouseEventArgs e)                          //the end of the game, restart with board cleared
        {
            label1.Text = "game ends";
            this.bt3.Enabled = true;
            this.bt4.Enabled = true;
            data = new int[16, 16];                                                         //to clear out the original data
            Draw();                                                                         //redraw
            MessageBox.Show("game restart, please choose which player play first");         //didn't get an easy way to delete the event handler, a cheat way to restart the game
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            baseImg = new Bitmap(panel1.Width, panel1.Height);              
        }

        protected override void OnPaint(PaintEventArgs e)                                   //will be called automatically, calling draw here to draw the back side of the board
        {
            Draw();
            base.OnPaint(e);
        }

        public void Draw()                                      
        {
            Graphics g = Graphics.FromImage(baseImg);                                      //adding graph on the bitmap
            g.Clear(this.BackColor);                                                       //clear the background color
            g.FillRectangle(Brushes.Orange, new Rectangle(new Point(10, 10), new Size(600, 600)));
            for (int i = 0; i < 15; i++){
                for(int j = 0; j < 15; j++){
                    g.DrawRectangle( new Pen(Brushes.Black), i * 40 + 10, j * 40 + 10, 40, 40); //choosing for example 40 as the length for each sides
                }
            }
            Graphics panelGraph = panel1.CreateGraphics();
            panelGraph.DrawImage(baseImg, 0, 0);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)                     //painting out each player's movement
        {           
            if( gameOver )                                                                  //end if the game is over
                return;
            Graphics g = panel1.CreateGraphics();                                           //create a graphics so that we can add more details on the board
            int yPosi = (e.Y - 10) % 40;                                                    //because starting position is 50,50 and each rectangle is 40*40 on the graph
            int xPosi = (e.X - 10) % 40;       
            yPosi = (yPosi > 20) ? (yPosi = (e.Y - 10) / 40 + 1) : (yPosi = (e.Y - 10) / 40);
            xPosi = (xPosi > 20) ? (xPosi = (e.X - 10) / 40 + 1) : (xPosi = (e.X - 10) / 40);   //trianry operation to see the location
            
            if( xPosi < 16 && yPosi < 16)
            {                                                                               //to make sure index not out of bound
                if(data[xPosi, yPosi] == 0)                                                 //can only place a piece if the place is originally blank
                {       
                    if(currentWhite)                                                        //white pieces move
                    {
                        currentWhite = false;        
                        drawPiece(g, xPosi, yPosi, new Pen(Brushes.White), Brushes.White, 1);
                        if( hasWinner() )
                        {                                                                   //printout the white one wins, and end the game
                            MessageBox.Show("white wins");
                            gameOver = true;
                            this.label1.Text = "white wins, game ends";
                        }
                    }
                    else                                                                    //black pieces move
                    {
                        currentWhite = true;
                        drawPiece(g, xPosi, yPosi, new Pen(Brushes.Black), Brushes.Black, 2);
                        if( hasWinner() )
                        {                                                                   //printout the black one wins, and end the game
                            MessageBox.Show("Black wins");
                            gameOver = true;
                            this.label1.Text = "Black wins, game ends";
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("please click on the game board");    
            }
            if( boardFull() ){                                                               //game draw, if some player wins the game before, it will show up previously
                MessageBox.Show("Game draw");
                gameOver = true;
                return;
            }
        }

        private void drawPiece(Graphics g, int x, int y, Pen p, Brush b, int color)         //color to indicate which piece it is
        {
            xLoc = x; yLoc = y;                                                             //setting up the current x and y location
            data[x, y] = color;
            g.DrawEllipse(p, x*40, y*40, 20, 20);
            g.FillEllipse(b, x*40, y*40, 20, 20);               
            pieceCount += 1;
        }

        private bool hasWinner( )                                                           //figure out which player wins the game
        { 
            int x = xLoc; int y = yLoc;
            if( vertWin(x, y) )
                return true;
            if( horiWin(x, y) )
                return true;
            if( diagnWin(x, y) )
                return true;
            return false;
        }

        private bool vertWin(int x, int y)
        {                                                                                   //i modified the original function a little bit, changing into te top-->down approach
            int top = (y + 4)< 15 ? ( y + 4 ):15;
            int val = data[x,y];
            for (int i = top; i > 3; i--){                  
                if(data[x ,i] == val && data[x, i-1] == val && data[x, i-2] == val && data[x, i-3] == val && data[x, i-4] == val)
                    return true;
            }
            return false;
        }

         private bool horiWin(int x, int y)             
         {
            int top = (x + 4)< 15 ? ( x + 4 ):15;
            int val = data[x,y];
            for (int i = top; i > 3; i--){                  
                if(data[i, y] == val && data[i-1, y] == val && data[i-2, y] == val && data[i-3, y] == val && data[i-4, y] == val)
                    return true;
            }
            return false;
         }

         private bool diagnWin(int x, int y)
         {                                                                                 //the original code has issues, i put all 4 situations in the game and adjust how to decide winining 
            int x1 = (x + 4) < 15 ? (x + 4) : 15;
            int y1 = (y + 4) < 15 ? (y + 4) : 15;
            int val = data[x, y];
            for (int i = x1, j = y1; i > 0 && j > 0; i--, j--)
            {
                                                                                           //from left to right, y=kx, k is positive 
                if( i > 3)
                {
                    if (data[i, j] == val && data[i - 1, j + 1] == val && data[i - 2, j + 2] == val && data[i - 3, j + 3] == val && data[i - 4, j + 4] == val)
                     return true;
                }
                
                                                                                            //from left to right, y=kx, k is negative
                if( i >3 && j > 3) 
                {  
                    if (data[i, j] == val && data[i - 1, j - 1] == val && data[i - 2, j - 2] == val && data[i - 3, j - 3] == val && data[i - 4, j - 4] == val)
                        return true;    
                }
                
            }
            x1 = (x - 4) > 0 ? (x - 4) : 0;
            y1 = (y - 4) > 0 ? (y - 4) : 0;
            for (int i = x1,j = y1; i < 16  && j < 16; i++,j++)
            {
                                                                                            //from right to left, y=kx, k is positive 
                 if( i < 12 && j < 12)
                 {                                                                          //to avoid out of bound exceptions
                    if (data[i, j] == val && data[i + 1, j + 1] == val && data[i + 2, j + 2] == val && data[i + 3, j + 3] == val && data[i + 4, j + 4] == val)
                     return true;                
                 }

                                                                                            //from right to left, y=kx, k is negative
                if( i < 12 )
                {
                    if (data[i, j] == val && data[i + 1, j - 1] == val && data[i + 2, j - 2] == val && data[i + 3, j - 3] == val && data[i + 4, j - 4] == val)
                     return true;
                }
            }

            return false;
         }

        private bool boardFull()                                                           //judging if the game is draw, checking if no more places to put new pieces
        {
            if(pieceCount == 225){
                return true;
            }
            return false;
        }
       
        
    }
}
