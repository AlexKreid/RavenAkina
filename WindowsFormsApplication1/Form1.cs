using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RC1;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "RavenEye";
            label1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = true;
            button8.Visible = false;
            textBox1.Visible = false;
        }

        int sost = 0;
        int kvz = 0; //кол-во вопросов заданных
        int kolvoigr = 1;
        int voprosov = 0;
        int veshei = 1;
        string svr = "";
        int nomer;
        Character[] ct = new Character[1];
        List<int> vib = new List<int>();
        List<int> Da= new List<int>();
        List<int> Net = new List<int>();
        List<int> NeZnaiu = new List<int>();
        List<int> NeImeetSmisla = new List<int>();
        public char[] Separator = new char[] { '^' };
        string path = @"data.txt";
        public void New_Cycle<T>(List<T> a) //Очищаем все листы для начала нового цикла работы
        {
            a.Clear();
            a.TrimExcess();
        }

        public void Zavershenie(Character c)
        {
            foreach (Question q in c.SpisokVoprosi)
            {
                    if (Da.Contains(q.Nomer)) { q.VerOtUvel(0); }
                    if (Net.Contains(q.Nomer)) { q.VerOtUvel(1); }
                    if (NeZnaiu.Contains(q.Nomer)) { q.VerOtUvel(2); }
                    if (NeImeetSmisla.Contains(q.Nomer)) { q.VerOtUvel(3); }
            }

            New_Cycle(vib);
            New_Cycle(Da);
            New_Cycle(Net);
            New_Cycle(NeZnaiu);
            New_Cycle(NeImeetSmisla);            
        }
        
        public string Konetz()
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            float f = float.MinValue;
            string s = "";
            
            foreach (Character x in ct)
            {
                float fn = 0;
                foreach (Question q in x.SpisokVoprosi)
                {
                    if (Da.Contains(q.Nomer))
                    {
                        fn = x.BayesOP(q.Nomer, 0);
                    }
                    if(Net.Contains(q.Nomer))
                    {
                        fn = x.BayesOP(q.Nomer, 1);
                    }
                    if (NeZnaiu.Contains(q.Nomer))
                    {
                        fn = x.BayesOP(q.Nomer, 2);
                    }
                    if (NeImeetSmisla.Contains(q.Nomer))
                    {
                        fn = x.BayesOP(q.Nomer, 3);
                    }
                    if (fn > f) { f = fn; s = x.Vopros(); }
                }
            }
            return s;
        }
        
        public string MinEntropy( Character[] c, List<int> ii)
        {
            float s = float.MaxValue;
            int i=0;
            string str="";
            foreach (Character x in c)
            {
                foreach(Question q in x.SpisokVoprosi)
                {                    
                        if (!ii.Contains(q.Nomer))
                        {
                            if (q.EntropyOP(x, q.Nomer) < s)
                            {
                                s = q.EntropyOP(x, q.Nomer);
                                i = q.Nomer;
                                str = q.Voprosik;
                            }
                        }
                }
                
            }
            nomer = i;
            vib.Add(i);
            return "Это "+str+"?";
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Da.Add(nomer);
            foreach (Character c in ct)
            {
                c.BayesOP(nomer, 0);
            }
            if ((kvz<=9) && (kvz<voprosov))
            {
                kvz = kvz + 1;
                label1.Text = MinEntropy(ct, vib);
            }
            else
            {
                label1.Text = Konetz();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Net.Add(nomer);
            foreach (Character c in ct)
            {
                c.BayesOP(nomer, 1);
            }
            if ((kvz <= 9) && (kvz < voprosov))
            {
                kvz = kvz + 1;
                label1.Text = MinEntropy(ct, vib);
            }
            else
            {
                label1.Text = Konetz();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NeZnaiu.Add(nomer);
            foreach (Character c in ct)
            {
                c.BayesOP(nomer, 2);
            }
            if ((kvz <= 9) && (kvz < voprosov))
            {
                kvz = kvz + 1;
                label1.Text = MinEntropy(ct, vib);
            }
            else
            {
                label1.Text = Konetz();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NeImeetSmisla.Add(nomer);
            foreach (Character c in ct)
            {
                c.BayesOP(nomer, 3);
            }
            if ((kvz <= 9) && (kvz < voprosov))
            {
                kvz = kvz + 1;
                label1.Text = MinEntropy(ct, vib);
            }
            else
            {
                label1.Text = Konetz();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Visible = false;
            button6.Visible = false;
            foreach (Character c in ct)
            {
                if (c.Vopros() == label1.Text)
                {
                    c.Kolvo = c.Kolvo + 1; //кол-во раз отгадали +1
                }
            }
            label1.Text = "Я молодец!";
            sost = 4;
            button8.Visible = true;
            button8.Text = "Завершить";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button5.Visible = false;
            button6.Visible = false;
            label1.Text = "А что это? Введите данное слово ниже:";
            textBox1.Visible = true;
            button8.Visible = true;
            sost = 2;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Visible = false;
            label1.Visible = true;

            if (!File.Exists(path)) //файла нет - первоначальный ход
            {
                button5.Visible = true;
                button6.Visible = true;
                
                ct[0] = new Character("Кот", kolvoigr, veshei);
                label1.Text = ct[0].Vopros();

                
            }
            else //файл есть - обычный ход
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string[] str = sr.ReadLine().Split(Separator, StringSplitOptions.None);
                    kolvoigr = int.Parse(str[0]);
                    veshei = int.Parse(str[1]);
                    voprosov = int.Parse(str[2]);
                    Array.Resize(ref ct, veshei);
                    for (int i = 0; i < ct.Length; i++)
                    {
                        ct[i] = new Character(sr.ReadLine(), kolvoigr);
                    }
                    sr.Close();
                }
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                label1.Visible = true;
                string s=MinEntropy(ct, vib);
                kvz=kvz+1;
                label1.Text = s;            
            }
        }

        

        private void button8_Click(object sender, EventArgs e)
        {
            if (sost == 1)
            {
                label1.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = true;
                button8.Visible = false;
                textBox1.Visible = false;
                sost = 0;
                kolvoigr += 1;
                kvz = 0;
                File.Delete(path);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(kolvoigr + "^" + veshei + "^"+voprosov);
                    foreach(Character x in ct)
                    {
                        sw.WriteLine(x.ToString());
                    }
                }
            }
            if (sost==2)
            {
                if (textBox1.Text == "")
                {
                    label1.Text = "Не введено новое слово";
                    textBox1.Focus();
                    return;
                }
                svr = textBox1.Text;
                sost = 3;
                textBox1.Clear();
            }
            if (sost==3)
            {
                if (textBox1.Text == "")
                {
                    label1.Text = "Дайте краткое описание:";
                    textBox1.Focus();
                    return;
                }
                voprosov += 1;
                veshei += 1;
                Array.Resize(ref ct, ct.Length + 1);
                ct[ct.Length - 1] = new Character(svr, kolvoigr, veshei, ct[0].SpisokVoprosi);

                for (int i = 0; i < ct.Length-1; i++)
                {
                    ct[i].AddQuestion(textBox1.Text, voprosov);
                }
                ct[ct.Length-1].SpisokVoprosi.Add(new Question(textBox1.Text,voprosov));
                Zavershenie(ct[ct.Length - 1]);
                sost = 1;
                textBox1.Clear();
                textBox1.Visible = false;
                label1.Text = "Всё вышло замечатльно";
            }
            if (sost ==4)
            {
                Application.Exit();
            }
        }    

        
    }
}
