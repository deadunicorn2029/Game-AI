using System;
using System.Net.Security;
using System.Reflection;

namespace PD1_KseniaDanilets
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static string StartPlayer = "";
        private static string SecondPlayer = "";

        private void button3_Click(object sender, EventArgs e)
        {
            start();
            if (StartPlayer.Equals("AI")) moveAi();
        }
        public void start()
        {
            //btn 1,2,4 unlock
            button1.Enabled = true;
            button2.Enabled = true;
            button4.Enabled = true;
            //btn 3 lock
            button3.Enabled = false;
            //radiobuttons lock
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            //checkBox
            checkBox1.Enabled = false;
            //Random rnd = new Random();
            //textBox 1 set default value
            if (checkBox1.Checked)
            {
                textBox1.Text = GenerateNumber().ToString();
            }
            else
            {
                textBox1.Text = "324";
            }
            //textBox2 set default value(0)
            textBox2.Text = "0";
            //check for available divisions
            checkForAvilableDivisions();
            if (!StartPlayer.Equals("AI"))
            {
                label3.Text = "Please, make a move :)";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            reset();
        }

        public void reset()
        {
            //btn 1,2,4 lock
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            //textBox1 delete text
            textBox1.Text = "";
            //btn 3 unlock
            button3.Enabled = true;
            //radiobuttons unlock
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            //checkBox
            checkBox1.Enabled = true;
            //label 3
            label3.Text = "Press START";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            move(2);
            checkWin();
            var value = int.Parse(textBox1.Text);
            if (value > 3)
            {
                moveAi();
                checkWin();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            move(3);
            checkWin();
            var value = int.Parse(textBox1.Text);
            if (value > 3)
            {
                moveAi();
                checkWin();
            }
        }

        public void move(int devideValue)
        {
            var value = int.Parse(textBox1.Text);
            var result = value / devideValue;
            if (result % 2 == 0)
            {
                addscore(1);
            }
            else
            {
                addscore(-1);
            }
            this.textBox1.Text = result.ToString();
            
            checkForAvilableDivisions();
        }

        public void checkForAvilableDivisions()
        {
            var value = int.Parse(textBox1.Text);
            if (value % 2 == 1)
            {
                button1.Enabled = false;
                // add to label devided value
            }
            else
            {
                button1.Enabled = true;
                //clear info from label
            }

            if (value % 3 == 0)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }
        public void addscore(int value)
        {
            var score = int.Parse(textBox2.Text);
            score = score + value;
            textBox2.Text = score.ToString();
        }
        public void checkWin()
        {
            var value = int.Parse(textBox1.Text);
            var score = int.Parse(textBox2.Text);

            if(value <= 3)
            {
                if(score <= 0)
                {
                    MessageBox.Show($"Win player: {StartPlayer}");
                }
                else
                {
                    MessageBox.Show($"Win player: {SecondPlayer}");
                }
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            StartPlayer = "Player";
            SecondPlayer = "AI";
            button3.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            StartPlayer = "AI";
            SecondPlayer = "Player";
            button3.Enabled = true;
        }

        public void moveAi()
        {
            var value = int.Parse(textBox1.Text);
            var score = int.Parse(textBox2.Text);
            bool isAiStart = StartPlayer.Equals("AI"); //ai Maximizes if starts
            int scoreDiv2 = isAiStart ? int.MinValue : int.MaxValue;
            int scoreDiv3 = isAiStart ? int.MinValue : int.MaxValue;
            int recursionResult;
            if (value % 2 == 0)
            {
                scoreDiv2 = Minimax(value / 2, score + ((value/2) % 2 == 0 ? 1 : -1), !isAiStart);

            }else
            {
                move(3);
                label3.Text = "Computer made move, now its your turn :)";
                return;
            }
            if (value % 3 == 0)
            {
                scoreDiv3 = Minimax(value / 3, score + ((value / 3) % 2 == 0 ? 1 : -1), !isAiStart);
            }
            else
            {
                move(3);
                label3.Text = "Computer made move, now its your turn :)";
                return;
            }
                recursionResult = scoreDiv3 >= scoreDiv2 ? 3:2;
            move(recursionResult);
            label3.Text = "Computer made move, now its your turn :)";
        }

        public int Minimax(int num, int score, bool isMaximizingPlayer)
        {
            // Check if the game has ended
            if (num == 2 || num == 3)
            {
                if (StartPlayer.Equals("AI") && score >= 0)
                {
                    return -1;
                }
                else if (StartPlayer.Equals("AI") && score < 0)
                {
                    return 1;
                }
                else if (!StartPlayer.Equals("AI") && score >= 0)
                {
                    return 1;
                }
                else return -1;
            }

            int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

            // Try dividing the number by 2 and 3 and evaluate the resulting score
            if (num % 2 == 0)
            {
                int currentScore = Minimax(num / 2, score + ((num / 2) % 2 == 0 ? 1 : -1), !isMaximizingPlayer);
                bestScore = isMaximizingPlayer ? Math.Max(bestScore, currentScore) : Math.Min(bestScore, currentScore);
            }

            if (num % 3 == 0)
            {
                int currentScore = Minimax(num / 3, score + ((num / 3) % 2 == 0 ? 1 : -1), !isMaximizingPlayer);
                bestScore = isMaximizingPlayer ? Math.Max(bestScore, currentScore) : Math.Min(bestScore, currentScore);
            }

            return bestScore;
        }
        public int GenerateNumber()
        {
            Random random = new Random();
            int numinit = random.Next(100, 1000000);
            var num = numinit;
            while (num > 3)
            {
                if (num % 2 == 0)
                {
                    num /= 2;
                }
                else if (num % 3 == 0)
                {
                    num /= 3;
                }
                else
                {
                    break;
                }
            }

            if (num == 2 || num == 3)
            {
                return numinit;
            }
            else
            {
                return GenerateNumber();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool isNumber = int.TryParse(textBox1.Text, out _);
            if (isNumber) 
            {
                int number = int.Parse(textBox1.Text);
                if (number % 2 == 1)
                {
                    textBox3.Text = "";
                }
                else
                {
                    textBox3.Text = (number / 2).ToString();
                }

                if (number % 3 == 0)
                {
                    textBox4.Text = (number / 3).ToString();
                }
                else
                {
                    textBox4.Text = "";
                }
            }
            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            about about = new about();
            about.ShowDialog();
        }
    }
    
}