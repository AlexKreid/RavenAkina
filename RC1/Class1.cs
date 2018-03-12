using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC1
{   
    public class Character
    {
        string otvet; // - Название предмета/имя чего-либо и т.д.
        public int n; // Номер ответа
        public int p; // Кол-во раз, которое его загадали
        float Bayes; //Вероятность данной ВЕЩИ при данных ответах на ВОПРОСЫ
        List<Question> qt = new List<Question>(); //база вопросов по данной вещи
        private char[] Separator = new char[] { '^' }; //разделитель ячеек
        private char[] Separator1 = new char[] { '|' }; //разделитель столбцов


        public float BYES => Bayes;

        public string Slovo => otvet;

        public float BayesOP(int nomervoprosa, int otvet) => this.Bayes * this.Nujnii(nomervoprosa).VerOt(otvet); //Вычсиление вероятности данной ВЕЩИ

        public List<Question> SpisokVoprosi => qt;

        public Character(string slovo, int kolvoigr, int kolvootvetov) //Созание нового Определния впервые
        {
            this.otvet = slovo;
            this.p = 1;
            this.n = kolvootvetov;
            float f = p;
            f = f / kolvoigr;
            this.Bayes = f;
        }
        
        public Character(string slovo, int kolvoigr, int kolvootvetov, List<Question> q) //Создание новго определения не в первый раз. Разница в добавлении листа вопросов прошлых определений
        {
            this.otvet = slovo;
            this.p = 1;
            this.n = kolvootvetov;
            float f = p;
            f = f / kolvoigr;
            this.Bayes = f;
            this.qt = q.ToList<Question>();
        }
        
        public Character(string slovo, int kolvoigr)//Считывание базы данных программой
        {    
            string[] str = slovo.Split(Separator1, StringSplitOptions.None);
            string[] d = str[0].Split(Separator,StringSplitOptions.None);
            this.otvet = d[0];
            if (!int.TryParse(d[1], out n)) { throw new Exception("Проблема с базой данных"); }
            if (!int.TryParse(d[2], out p)) { throw new Exception("Проблема с базой данных"); }            
            float f = p;
            f = f / kolvoigr;
            this.Bayes = f;
            for (int i = 1; i < str.Length; i++)
			{                
                Question q = new Question(str[i],Separator);
                this.qt.Add(q);
			}
        }

        public void AddQuestion(string vopros, int kolvovoprosov) //Добавление нового Описания ко всем предыдущим Определениям.
        {
            Question q = new Question(vopros, kolvovoprosov);
            q.Da = q.Da - 1;
            this.qt.Add(q);
        }
        
        public string Vopros() // финальный вопрос
        {
            return "Это ведь "+otvet+", не правда ли?";
        }
        
        public override string ToString() //метод для записи информации об Определении в файл базы данных data.txt
        {
            string s = "";
            foreach (Question x in qt)
	        {
                s = s + Separator1[0] + x.ToString1(Separator);
	        }
            return otvet + Separator[0] + n + Separator[0] + p+s;
        }

        
        public int Kolvo //кол-во раз данное Определение задавалось
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
            }
        }
        
        public Question Nujnii(int nomervoprosa) //Выбор выопроса, наиболее подходящего для задачи прямо сейчас.
        {
            foreach (Question x in qt)
            {
                if (x.Nomer == nomervoprosa)
                {
                    return x;
                }
            }
            return new Question("ia ne znaiu", 100000);
        }
    }// Окончание класса Otvet

    public class Question
    {
        string question; // - текст самого вопроса
        int n=0;// номер ВОПРОСА
        int an0=0; // ответ ДА
        int an1=0; //ответ НЕТ
        int an2=0; // ответ НЕ ЗНАЮ
        int an3=0; //ответ НЕ ИМЕЕТ СМЫСЛА
        float entropy=0;//энтропия вопроса в данный момент

        public int Da { get { return an0; } set { an0 = value; } }

        public int Nomer => n;

        public string Voprosik => question;

        private int KolOt => an0 + an1 + an2 + an3;//кол-во раз на данный вопрос ответили по этой ВЕЩИ

        public Question(string s, int kolvovoprosov)//новое качество
        {
            this.question = s;
            this.n=kolvovoprosov;
            this.an0 = 2; // т.к. это новое качество/описание,  и оно соответсвует только что усвоенному Определению, кол-во ответов "Да" на него больше, чем других вариантов
            this.an1 = 1;
            this.an2 = 1;
            this.an3 = 1;
        }
        
        public Question(string s, char[] separator)//из файла
        {
            
            string[] st = s.Split(separator, StringSplitOptions.None);
            this.question = st[0];
            if (!int.TryParse(st[1], out n)) { throw new Exception("Проблема с базой данных"); }
            if (!int.TryParse(st[2], out an0)) { throw new Exception("Проблема с базой данных"); }
            if (!int.TryParse(st[3], out an1)) { throw new Exception("Проблема с базой данных"); }
            if (!int.TryParse(st[4], out an2)) { throw new Exception("Проблема с базой данных"); }
            if (!int.TryParse(st[5], out an3)) { throw new Exception("Проблема с базой данных"); }
        }

        public float VerOt(int nt) //вероятность определённого ответа на данный вопрос по данной вещи
        {
            float f;
            if (n>=0||n<=3)
            {
                switch (nt)
                {
                    case 0:
                        f = an0;
                        f = f / this.KolOt;
                        return f;
                    case 1:
                        f = an1;
                        f = f / this.KolOt;
                        return f;
                    case 2:
                        f = an2;
                        f = f / this.KolOt;
                        return f;
                    case 3:
                        f = an3;
                        f = f / this.KolOt;
                        return f;
                    default:
                        break;
                }
            }
            else
            {
                throw new Exception("Ошибка вводимого ответа пользователя");
            }
            return nt;
            
        }

        public void VerOtUvel(int nt) //увеличиваем вероятность ответа В БАЗЕ ДАННЫХ
        {
            
                switch (nt)
                {
                    case 0:
                         an0+=1;
                         break;
                    case 1:
                         an1 += 1;
                         break;
                    case 2:
                         an2 += 1;
                         break;
                    case 3:
                         an3+=1;
                         break;
                    default:
                        break;
                }
        }

        public float EntropyOP(Character c, int nomervoprosa) //Вычисляет Информационную энтропию вопроса
        {
            this.entropy = 0;
            for (int i = 0; i < 3; i++)
            {
                this.entropy = this.entropy + c.BYES * c.Nujnii(nomervoprosa).VerOt(i);
            }
            return this.entropy;
        }//Энтропия вопроса - КАЖДЫЙ РАЗ НОВАЯ

        public string ToString1(char[] Separator) => question + Separator[0] + n + Separator[0] + an0 + Separator[0] + an1 + Separator[0] + an2 + Separator[0] + an3;
    }// Vopros


}
