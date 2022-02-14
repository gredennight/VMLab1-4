using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ЛабЛаб1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// получить у для заданого х
        /// </summary>
        public double Function(double x)
        {
            return Math.Pow(x,4)+2*Math.Pow(x,3)-x-1;
        }
        /// <summary>
        /// спрятать все доп элементы
        /// </summary>
        void HideAll()
        {
            textBox1.Visible = false;
            textBox2.Visible=false;
            textBoxX0.Visible=false;
            label1.Visible=false;
            label2.Visible=false;
            label4.Visible=false;
        }
        /// <summary>
        /// получить значение касательной
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double FunctionShtrih(double x)
        {
            return 4*Math.Pow(x,3)+6*Math.Pow(x,2)-1;
        }

        /// <summary>
        /// отрисовка графика
        /// </summary>
        public void PrintGraph()
        {
            Graphics screen;
            screen=CreateGraphics();
            screen.Clear(Color.White);
            int max = 300;
            int x1=0, y1=0, x2=0, y2=0;
            double zoom =Convert.ToDouble( trackBarZoom.Value);
            ///на вход идёт значение х в пикселях, на выход - у тоже в пикселях
            int GetY(int x)
            {
                double mashtab = max/(zoom*2);
                double realX = (x/mashtab)-zoom;
                double realY = Function(realX);
                double y = (realY+zoom)*mashtab;

                return Convert.ToInt32(max-y);
            }
            ///отрисовка координат
            screen.DrawLine(Pens.Black, max/2, 0, max/2, max);
            screen.DrawLine(Pens.Black, 0, max/2, max, max/2);


            ///отрисовка графика
            for (int i = 0; i < max; i++)
            {
                y1 =GetY(i);
                x1=i;
                screen.DrawLine(Pens.Red, x1, y1, x2, y2);
                y2=y1;
                x2=x1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        //метод половинного деления
        private void Polovinki()
        {
            PrintGraph();
            double a0 = Convert.ToDouble(textBox1.Text);
            double b0 = Convert.ToDouble(textBox2.Text);
            double E = Convert.ToDouble(textBox3.Text);
            int count = 0;

            richTextBox1.Text+="итерация а b f1 f2 f3";

            while (Math.Abs(b0-a0)>2*E)
            {
                double c = (a0+ b0)/2;
                double f1 = Function(a0);
                double f2 = Function(c);
                double f3 = Function(b0);

                //вывод в консоль
                richTextBox1.Text+="\n№"+count+" a:"+Math.Round(a0, 4)+" b:"+Math.Round(b0, 4)+" F(a):"+Math.Round(f1, 4)+" F(c):"+Math.Round(f2, 4)+" F(b):"+Math.Round(f3, 4);

                //выбор интервала для следующей итерации
                if (f1<0 && f2>0)
                {
                    double a1 = a0;
                    double b1 = c;
                    a0=a1;
                    b0=b1;

                }
                else
                {
                    double a1 = c;
                    double b1 = b0;
                    a0=a1;
                    b0=b1;

                }
                count++;

            }
            richTextBox1.Text+="\nВ результате треша и экшена получаем, что х="+(a0+b0)/2;
        }

        //метод ньютона
        private void Newton()
        {
            double x0=Convert.ToDouble( textBoxX0.Text);
            double x1 = x0;
            double E = Convert.ToDouble(textBox3.Text);
            do
            {
                x0 = x1;
                double f = Function(x0);
                double F = FunctionShtrih(x0);
                x1=x0-(f/F);
                //вставить сюда вывод значений
                richTextBox1.Text+="\nx0:"+Math.Round(x0, 3)+" x1:"+Math.Round(x1, 3)+" f:"+Math.Round(f, 3)+" F:"+Math.Round(F, 3);

            }
            while (Math.Abs(x1-x0)>E);
            richTextBox1.Text+="\nВ результате треша и экшена получаем, что х="+x1;
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            PrintGraph();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void trackBarZoom_Scroll(object sender, EventArgs e)
        {
            PrintGraph();
            ZoomText.Text=Convert.ToString( trackBarZoom.Value);
        }

        private void trackBarX_Scroll(object sender, EventArgs e)
        {
            PrintGraph();
        }

        /// <summary>
        /// скрытие ненужных элементов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0://половинного деления
                    HideAll();
                    textBox1.Visible = true;
                    textBox2.Visible=true;
                    textBox3.Visible=true;
                    label1.Visible=true;
                    label2.Visible=true;
                    label3.Visible=true;
                    break;
                case 1://ньютона
                    HideAll();
                    textBoxX0.Visible=true;
                    label4.Visible=true;
                    break;

                case 2://секущих
                    break;

                case 3://итераций
                    break;
            }
        }
        /// <summary>
        /// вычисление значения на основе выбранного режима
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Compute_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0://половинного деления
                    Polovinki();
                    break;
                case 1://ньютона
                    Newton();
                    break;

                case 2://секущих
                    break;

                case 3://итераций
                    break;
            }
        }
    }
}
