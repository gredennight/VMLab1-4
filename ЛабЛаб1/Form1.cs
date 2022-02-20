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
            PrintGraphV2();
            richTextBox1.Text+="\nПеред использованием стоит убедиться, что правильно задана функция и её производная в коде функций Function и FunctionShtih соответственно.";
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
            textBox3.Visible=false;
            label1.Visible=false;
            label2.Visible=false;
            label3.Visible=false;
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
        /// рисует у=2х
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double FunctionDebug(double x)
        {
            return (2*x);
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
        
        public void PrintGraphV2()
        {
            Graphics screen;
            screen=CreateGraphics();
            screen.Clear(Color.White);
            screen.DrawRectangle(Pens.Black, 0, 0, 300, 300);
            //широта экрана вывода
            int max = 300;
            //диапазон вывода графика (от -zoom до zoom)
            double zoom = Convert.ToDouble(trackBarZoom.Value);
            //смещение графика по оси х
            double moveX=Convert.ToDouble(-trackBarX.Value)/10;
            //смещение графика по оси у
            double moveY=Convert.ToDouble(-trackBarY.Value)/10;
            //шаг в координатах для каждого нового пикселя
            double step = (2*zoom)/max;
            //начальные точки вывода графика
            double sx = -zoom+moveX;
            double sy = -zoom+moveY;
            
            //конечные точки вывода графика
            double fx = zoom+moveX;
            double fy = zoom+moveY;
            //richTextBox1.Text+="\nmoveX:"+moveX+" moveY:"+moveY+" sx:"+sx+" sy:"+sy+" fx:"+fx+" fy:"+fy+" step:"+step;
            //высота и широта в пикселях
            int h = 0, w = 0;
            //координаты
            double x=0, y=0;
            //точка на отрисовку
            Point dot = new Point(0, 0);
            Point dotPrev = new Point(0, 0);
            //переменная ну короч это самое, тип если точку низя отрисовать, то она переключается и тип теперь эта точка не будет рисоваться
            bool lostDot=false;


            //получить значение координаты из пикселей с учитываемым смещением
            double getX()
            {
                return (Convert.ToDouble( w)*step)+sx;
            }

            //получить из координаты значение в пикселях
            int getHeight()
            {
                return Convert.ToInt32((y-moveY+zoom)/step);
            }

            //преобразовать координаты в пикселях в конченные координаты отрисовки
            Point ConvertToDraw()
            {
                return new Point(w, max-h);
            }

            //тело цикла отрисовки
            for(w=0; w < max; w++)
            {
                x = getX();
                y=Function(x);
                if(sy<=y && y<=fy)
                {
                    h=getHeight();
                    dot = ConvertToDraw();
                    if (lostDot)
                    {
                        dotPrev=dot;
                    }
                    lostDot=false;
                    screen.DrawLine(Pens.Red,dotPrev,dot);
                    dotPrev=dot;

                }
                else
                {
                    lostDot=true;
                }
                //richTextBox1.Text+="\nx:"+Math.Round( x,2)+" y:"+Math.Round(y, 2)+" w:"+w+" h:"+h;
            }

            x=0;y=0;
            int vertical=0, horizontal=0;
            Point point1=new Point(0,0);
            Point point2=new Point(0,0);
            //если вертикаль попадает в поле зрения
            if(sx<=0 && fx>=0)
            {
                vertical=Convert.ToInt32((x-moveX+zoom)/step);
                point1.X=vertical;point2.X=vertical;
                point1.Y=0;point2.Y=max;
                point1.Y=max-point1.Y;
                point2.Y=max-point2.Y;
                screen.DrawLine(Pens.Black,point1,point2);
                
            }
            //если горизонталь попадает в поле зрения
            if (sy<=0 && fy>=0)
            {
                horizontal=Convert.ToInt32((y-moveY+zoom)/step);
                point1.Y=horizontal; point2.Y=horizontal;
                point1.X=0; point2.X=max;
                point1.Y=max-point1.Y;
                point2.Y=max-point2.Y;
                screen.DrawLine(Pens.Black, point1, point2);

            }



        }
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        //метод половинного деления
        private void Polovinki()
        {
            PrintGraphV2();
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
                richTextBox1.Text+="\n№"+count+" a:"+Math.Round(a0, 4)+" b:"+Math.Round(b0, 4)+" c:"+c+" F(a):"+Math.Round(f1, 4)+" F(c):"+Math.Round(f2, 4)+" F(b):"+Math.Round(f3, 4);

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
            double x0=Convert.ToDouble( textBox1.Text);
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

        //метод секущих
        private void sekushie()
        {
            double x0 = Convert.ToDouble(textBox1.Text);
            double x1 = Convert.ToDouble(textBox2.Text);
            double x2 = 0;
            double E = Convert.ToDouble(textBox3.Text);
            do
            {
                double fx0 = Function(x0);
                double fx1 = Function(x1);
                x2=((x0*fx1)-(x1*fx0))/(fx1-fx0);
                richTextBox1.Text+="\nx:"+Math.Round(x1,3)+" f(x):"+Math.Round(fx1,3);
                x0=x1;
                x1=x2;
            }
            while (Math.Abs(x1-x0)>E);
            richTextBox1.Text+="\nВ результате треша и экшена получаем, что х="+x1;
        }

        //метод итераций
        private void Iteration()
        {
            PrintGraphV2();
            /*double x0 = Convert.ToDouble(textBox1.Text);
            double E = Convert.ToDouble(textBox3.Text);
            double lambda = 1/FunctionShtrih(x0);
            double x1=x0,x2=0;
            do
            {
                x2=x1-lambda*Function(x1);
                richTextBox1.Text+="\nx2:"+x2+" f(x1):"+Function(x1);
                x1=x2;
            }
            while (Math.Abs(x2-x1)>E);
            richTextBox1.Text+="\nВ результате треша и экшена получаем, что х="+x2;

            //x2=x1-lambda*function(x1);*/
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            PrintGraphV2();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void trackBarZoom_Scroll(object sender, EventArgs e)
        {
            PrintGraphV2();
            ZoomText.Text=Convert.ToString( trackBarZoom.Value);
        }

        private void trackBarX_Scroll(object sender, EventArgs e)
        {
            PrintGraphV2();
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
                    label1.Text="a0";
                    label2.Text="b0";
                    textBox1.Visible = true;
                    textBox2.Visible=true;
                    textBox3.Visible=true;
                    label1.Visible=true;
                    label2.Visible=true;
                    label3.Visible=true;
                    break;
                case 1://ньютона
                    HideAll();
                    label1.Text="x0";
                    textBox1.Visible=true;
                    label1.Visible = true;
                    textBox3.Visible=true;
                    label3.Visible = true;
                    break;

                case 2://секущих
                    HideAll();
                    label1.Text="x0";
                    label2.Text="x1";

                    textBox1.Visible=true;
                    label1.Visible = true;
                    textBox2.Visible=true;
                    label2.Visible = true;
                    textBox3.Visible=true;
                    label3.Visible = true;
                    break;

                case 3://итераций
                    HideAll();
                    label1.Text="x0";

                    textBox1.Visible=true;
                    label1.Visible = true;
                    textBox3.Visible=true;
                    label3.Visible = true;
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
                    sekushie();
                    break;

                case 3://итераций
                    Iteration();
                    break;
            }
        }
    }
}
