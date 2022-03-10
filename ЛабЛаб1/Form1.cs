using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace ЛабЛаб1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HideAll();
            PrintGraphV2(Function);
            List<List<double>> xy = new List<List<double>>();
            richTextBox1.Text+="\nПеред использованием стоит убедиться, что правильно задана функция и её производная в коде функций Function и FunctionShtih соответственно.";
        }

        /// <summary>
        /// массив из координат, первая позиция х, вторая - у
        /// </summary>
        public List<List<double>> coordinates =new List<List<double>>();
       

        /// <summary>
        /// получить у для заданого х
        /// </summary>
        public double Function(double x)
        {
            //return Math.Pow(x, 5)-x-1;
            return Math.Pow(x,4)+2*Math.Pow(x,3)-x-1;
        }
        /// <summary>
        /// спрятать все доп элементы на форме
        /// </summary>
        void HideAll()
        {
            textBox1.Visible = false;
            textBox2.Visible=false;
            textBox3.Visible=false;
            label1.Visible=false;
            label2.Visible=false;
            label3.Visible=false;
            buttonOpenFile.Visible=false;
        }
        /// <summary>
        /// получить значение касательной
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double FunctionShtrih(double x)
        {
            //return 5*Math.Pow(x, 4)-1;
            return 4*Math.Pow(x,3)+6*Math.Pow(x,2)-1;
        }

        /// <summary>
        /// крутая отрисовка графика, на вход засовывать функцию получения у для х
        /// </summary>
        /// <param name="GetY"></param>
        public void PrintGraphV2(Func<double,double>GetY)
        {
            //тут идёт такая дичь, что лучше чекнуть записи в блокноте...


            //всякая всячина для рисования
            Graphics screen;
            screen=CreateGraphics();
            screen.Clear(Color.White);
            //рисуем рамку чёрного цвета двадцатого века
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
                y=GetY(x);
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


            //отрисовка осей
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
                //отрисовка шкал
                for(int i=0; i < max; i+=max/10)
                {
                    screen.DrawLine(Pens.Black, vertical-5,i,vertical+5,i);
                    screen.DrawString(Convert.ToString(Math.Round((Convert.ToDouble(i)*step+sy),2)), new Font("Arial", 8), new SolidBrush(Color.Black), vertical, i-12);
                }
                
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
                //отрисовка шкал
                for (int i = 0; i < max; i+=max/10)
                {
                    screen.DrawLine(Pens.Black, i, max-horizontal-5, i, max-horizontal+5);
                    screen.DrawString(Convert.ToString(Math.Round((Convert.ToDouble(i)*step+sx), 2)), new Font("Arial", 8), new SolidBrush(Color.Black), i, max-horizontal-12);

                }

            }



        }

        //очистка консоли
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        //метод половинного деления
        private void Polovinki()
        {
            PrintGraphV2(Function);
            double a0 = Convert.ToDouble(textBox1.Text);
            double b0 = Convert.ToDouble(textBox2.Text);
            double E = Convert.ToDouble(textBox3.Text);
            int count = 0;

            richTextBox1.Text+="итерация а b f1 f2 f3";

            while (Math.Abs(b0-a0)>2*E)//проверка условия окончания
            {
                double c = (a0+ b0)/2;//среднее значение границ
                double f1 = Function(a0);// функция левой границы
                double fc = Function(c);//функция середины
                double f2 = Function(b0);//функция правой границы

                //вывод в консоль
                richTextBox1.Text+="\n№"+count+" a:"+Math.Round(a0, 3)+" b:"+Math.Round(b0, 3)+" c:"+Math.Round(c, 3)+" F(a):"+Math.Round(f1, 3)+" F(c):"+Math.Round(fc, 3)+" F(b):"+Math.Round(f2, 3);

                //выбор интервала для следующей итерации
                if (f1*fc<0)//выбирается левый интервал
                {
                    double a1 = a0;
                    double b1 = c;
                    a0=a1;
                    b0=b1;

                }
                else//правый
                {
                    double a1 = c;
                    double b1 = b0;
                    a0=a1;
                    b0=b1;

                }
                count++;

            }
            //в конце выводится значение х
            richTextBox1.Text+="\nх="+(a0+b0)/2;
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
                double fF = -f/F;
                x1=x0+fF;
                //вставить сюда вывод значений
                richTextBox1.Text+="\nx0:"+Math.Round(x0, 3)+" x1:"+Math.Round(x1, 3)+" f:"+Math.Round(f, 3)+" F:"+Math.Round(F, 3)+" -f/F:"+fF;

            }
            while (Math.Abs(x1-x0)>E);
            richTextBox1.Text+="\nх="+x1;
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
                richTextBox1.Text+="\nx:"+Math.Round(x1,3)+" f(x):"+fx1;
                x0=x1;
                x1=x2;
            }
            while (Math.Abs(x1-x0)>E);
            richTextBox1.Text+="\nх="+x1;
        }

        //метод итераций
        private void Iteration()
        {
            PrintGraphV2(Function);
            double Fx0 = FunctionShtrih(Convert.ToDouble(textBox1.Text));
            double lambda = 1/Fx0;
            double x0 = Convert.ToDouble(textBox1.Text), x1 = 0, xtemp = x0 ;
            //x0=0;
            double E = Convert.ToDouble(textBox3.Text);
            int count = 0;
            do
            {
                x0=xtemp;
                double u= x0-lambda*Function(x0);
                x1=u;
                richTextBox1.Text+="\n№:"+count+ " x:"+Math.Round( x0,3)+" u(x):"+Math.Round(u, 3);
                xtemp = x1;
                count++;
            }
            while (Math.Abs(x1-x0)>E);

            
            richTextBox1.Text+="\nх="+Math.Round(x1, 3);

            //x2=x1-lambda*function(x1);*/
        }

        //всякие ползунки
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                
                case 4:
                    Kubik();
                    break;
                default://всё остальнре
                    PrintGraphV2(Function);
                    break;
            }
        }

        //интерполяция кубическим сплайном
        public void Kubik()
        {
            List<List<double>> coords = coordinates;
            //List<List<double>> coords = new List<List<double>>();
            /*
            //вставить сюда получение данных в массив
            coords.Add(new List<double> { 0, 1 });
            coords.Add(new List<double> { 1.5, 2.52 });
            coords.Add(new List<double> { 4.0, 4.16 });
            coords.Add(new List<double> { 5.2, 3.2 });
            coords.Add(new List<double> { 7.0, 5.1 });
            coords.Add(new List<double> { 9.3, 6.2 });
            */

            int mas = coords.Count+1;

            //поиск высот h
            List<double> h = new List<double>(mas);
            h.Add(0);
            for (int i = 1; i<coords.Count; i++)
            {
                h.Add(coords[i][0]-coords[i-1][0]);
                Console.WriteLine(h[i]+"    "+i);
            }

            //перевод h в u (хз зачем я это сделал, но пусть будет)
            List<double> u = new List<double>(mas);
            u.Add(-9080889);
            u.Add(-9080889);
            Console.WriteLine("\n");
            for (int i = 2; i<coords.Count; i++)
            {
                u.Add(h[i]);
                Console.WriteLine(u[i]+"    "+i);
            }

            //определение v из h
            List<double> v = new List<double>(mas);
            Console.WriteLine("\n");
            v.Add(-98142);
            v.Add(-98142);
            for (int i = 2; i<coords.Count; i++)
            {
                v.Add(-2*(h[i-1]+h[i]));
                Console.WriteLine(v[i]+"    "+i);

            }

            //определение w, которые так-то прост h с задержкой в развитии
            List<double> w = new List<double>(mas);
            w.Add(-9080889);
            w.Add(-9080889);
            Console.WriteLine("\n");
            for (int i = 2; i<coords.Count; i++)
            {
                w.Add(h[i-1]);
                Console.WriteLine(w[i]+"    "+i);

            }

            //определение F из y и h
            List<double> F = new List<double>(mas);
            F.Add(-9080889);
            F.Add(-9080889);
            Console.WriteLine("\n");
            for (int i = 2; i<coords.Count; i++)
            {
                F.Add(3*(((coords[i][1]-coords[i-1][1])/h[i])-((coords[i-1][1]-coords[i-2][1])/h[i-1])));
                Console.WriteLine(F[i]+"    "+i);

            }

            //определение коефа а
            List<double> a = new List<double>(mas);
            a.Add(-9080889);
            Console.WriteLine("\n");
            for (int i = 1; i<coords.Count; i++)
            {
                a.Add(coords[i-1][1]);
                Console.WriteLine(a[i]+"    "+i);

            }

            //определение недоматриц А
            List<double> A = new List<double>(mas);
            A.Add(0);
            A.Add(0);
            A.Add(u[2]/v[2]);

            Console.WriteLine("\n");
            for (int i = 3; i<coords.Count; i++)
            {
                A.Add(u[i]/(v[i]-w[i]*A[i-1]));
                Console.WriteLine(A[i]+"    "+i);

            }

            //определение недоматриц В
            List<double> B = new List<double>(mas);
            B.Add(0);
            B.Add(0);
            B.Add(-F[2]/v[2]);

            Console.WriteLine("\n");
            for (int i = 3; i<coords.Count; i++)
            {
                B.Add(((w[i]*B[i-1])-F[i])/(v[i]-(w[i]*A[i-1])));
                Console.WriteLine(B[i]+"    "+i);

            }

            //определение коефа с
            List<double> c = new List<double>(mas);
            for (int i = 0; i<=coords.Count; i++)
            {
                c.Add(08348130591);
            }
            c[mas-1]=0;
            c[1]=0;
            for (int i = c.Count-2; i>1; i--)
            {
                c[i]=A[i]*c[i+1]+B[i];
            }

            //а теперь внезапно определение коефа b
            List<double> b = new List<double>(mas);
            for (int i = 0; i<=coords.Count; i++)
            {
                b.Add(0);
            }
            for (int i = 1; i<b.Count-1; i++)
            {
                b[i]=((1/h[i])*(coords[i][1]-coords[i-1][1]))-((1.0/3.0)*((2*c[i])+c[i+1])*h[i]);
                //Console.WriteLine("\nb"+i+"     ci:"+c[i]+" ci+1:"+c[i+1]+"   hi:"+h[i]+"   sjfasf "+(((2*c[i])+c[i+1])*h[i])*(1.0/3.0) +" а вместе:"+((1/3)*((2*c[i])+c[i+1])*h[i]));
            }

            //ну и почему бы и да, определение коефа d
            List<double> d = new List<double>(mas);
            for (int i = 0; i<=coords.Count; i++)
            {
                d.Add(0);
            }
            for (int i = 1; i<d.Count-1; i++)
            {
                d[i]=(c[i+1]-c[i])/(3*h[i]);
            }

            //преобразование полученных массивов в функцию кубического сплайна
            //на вход кидаешь х, а получаешь у
            double Kubaster(double x)
            {
                //смотрим к какому промежутку принадлежит х
                int i = 1;

                for (int n = 0; n<coords.Count; n++)
                {

                    if (x>=coords[n][0] && x<coords[coords.Count-1][0])
                    {
                        i=n+1;
                    }

                }

                //если он вне промежутка то прост рисуем прямую
                if (x>=coords[coords.Count-1][0])
                {
                    return 0;
                }
                if (x<coords[0][0])
                {
                    return 0;
                }

                //выводим у для х, узнав нужные для этого коефы
                return (a[i]+b[i]*(x-coords[i-1][0])+c[i]*Math.Pow((x-coords[i-1][0]), 2)+d[i]*Math.Pow((x-coords[i-1][0]), 3));
            }
            
            //вывод коефов в консольку
            richTextBox1.Clear();
            for(int i = 1; i<coords.Count; i++)
            {
                string xtemp = "(x-"+Convert.ToString(Math.Round(coords[i-1][0], 3))+")";
                richTextBox1.Text+="\nu"+i+"="+Math.Round(a[i], 3)+"+"+Math.Round(b[i], 3)+xtemp+"+"+Math.Round(c[i], 3)+xtemp+"^2+"+Math.Round(d[i], 3)+xtemp+"^3";
            }

            //рисование графика
            PrintGraphV2(Kubaster);


        }

        //крутилка масштаба
        private void trackBarZoom_Scroll(object sender, EventArgs e)
        {
            ZoomText.Text=Convert.ToString( trackBarZoom.Value);
            PrintGraphV2(Function);
            switch (comboBox1.SelectedIndex)
            {
                
                case 4:
                    Kubik();
                    break;
                default://всё остальнре
                    PrintGraphV2(Function);
                    break;
            }
        }

        //крутилка по иксам
        private void trackBarX_Scroll(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                
                case 4:
                    Kubik();
                    break;
                default://всё остальнре
                    PrintGraphV2(Function);
                    break;
            }
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
                case 4://интерчтототамянеспал20часовнихуянепонимаю
                    HideAll();
                    buttonOpenFile.Visible=true;
                    break;
                case 5://заготовка на чёт ещё
                    HideAll();
                    buttonOpenFile.Visible=true;
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
                    if(Convert.ToDouble(textBox1.Text)!=0)
                    {
                        Newton();

                    }
                    break;

                case 2://секущих
                    sekushie();
                    break;

                case 3://итераций
                    Iteration();
                    break;
                case 4://интерполяция сплайном
                    Kubik();
                    break;
                
                default://всё остальнре
                    PrintGraphV2(Function);
                    richTextBox1.Text+="\nВыберите метод.";
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// тип это самое, попытка считать данные из текстового файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //openFileDialog1.FileName; от так вот можно получить путь к файлу
                coordinates.Clear();
                string input = File.ReadAllText(openFileDialog1.FileName);
                int i = 0, j = 0;

                //считывание файла в массив
                foreach(var row in input.Split('\n'))
                {
                    j=0;
                    coordinates.Add(new List<double> {0,0});
                    foreach(var col in row.Trim().Split(' '))
                    {
                        //coordinates[i][j]=double.Parse(col.Trim());
                        coordinates[i][j]=Convert.ToDouble(col.Replace(".",","));//оно может преобразовать ток 2,2, так что даём возможность заменить 2.2
                        j++;
                    }
                    i++;
                }

                //добавь сюда сортировку массива по иксам, пж
                //coordinates.Sort();//пахпахпаыхвп лул, ну ты хотя бы пытался :D
                //coordinates = coordinates.OrderBy(x => x.Count > 1 ? x[1] : null).ToList();
            }

        }
    }
}
