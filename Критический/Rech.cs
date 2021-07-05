using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Критический
{
    public class Rech
    {
        string s = "";
        public int max, maxind;
        public List<List<Rbt>> fnlcn = new List<List<Rbt>>();//Лист путей
        public List<Rbt> ret;
        public List<Rbt> ls = Flrd();//Начальные данные
        /// <summary>
        /// Создание путей
        /// </summary>
        public void put()
        {
            ret = ls.FindAll(x => x.point1 == ls[Minel(ls)].point1);//Точка начала
            foreach (Rbt rb in ret)
            {
                Mv(ls, rb);
                fnlcn.Add(RtPrs(ls, s));
                s = "";
            }
        } 
        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        public static List<Rbt> Flrd()
        {
            List<Rbt> ret = new List<Rbt>();
            try
            {
                using (StreamReader sr = new StreamReader(@"Vvod.csv"))
                {

                    while (sr.EndOfStream != true)
                    {
                        string[] str1 = sr.ReadLine().Split(';');
                        string[] str2 = str1[0].Split('-');
                        ret.Add(new Rbt { point1 = Convert.ToInt32(str2[0]), point2 = Convert.ToInt32(str2[1]), length = Convert.ToInt32(str1[1]) });
                    }
                }
                return ret;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                System.Environment.Exit(0);
                return ret;
            }
        }
        /// <summary>
        /// Поиск критического пути
        /// </summary>
        public void naxodres()
        {
            put();
            maxind = 0;
            max = fnlcn[0][0].length;
            for (int i = 0; i < ret.Count; i++)
            {
                if (FnlMv(fnlcn[i]) >= max)
                {
                    max = FnlMv(fnlcn[i]);
                    maxind = i;
                }
            }
            ViVod();
        }
        

        public struct Rbt
        {
            public int point1;// точки
            public int point2;
            public int length;// длина
            public override string ToString()//Запись пути
            {
                return point1.ToString() + " - " + point2.ToString() + " " + length.ToString();
            }
        }
        /// <summary>
        /// Поиск начала
        /// </summary>
        public int Minel(List<Rbt> ls)
        {
            int min = ls[0].point1, minind = 0;
            foreach (Rbt rb in ls)
            {
                if (rb.point1 <= min)
                {
                    min = rb.point1;
                    minind = ls.IndexOf(rb);
                }
            }
            return minind;
        }
        /// <summary>
        /// Поиск конечной точки
        /// </summary>
        public int Maxel(List<Rbt> ls)
        {
            int min = ls[0].point2, maxind = 0;
            foreach (Rbt rb in ls)
            {
                if (rb.point2 >= min)
                {
                    min = rb.point1;
                    maxind = ls.IndexOf(rb);
                }
            }
            return maxind;
        }
        /// <summary>
        ///Проверка пути
        /// </summary>
        public int Mv(List<Rbt> ls, Rbt minel)
        {
            {
                int ret = 0;
                Rbt rb = ls.Find(x => x.point1 == minel.point1 && x.point2 == minel.point2);
                s += rb.point1.ToString() + "-" + rb.point2.ToString();
                if (rb.point2 == ls[Maxel(ls)].point2)
                {
                    s += ";";
                    return rb.length;
                }
                else
                {
                    for (int i = 0; i < ls.Count; i++)
                    {
                        if (ls[i].point1 == rb.point2)
                        {
                            s += ",";
                            ret = Mv(ls, ls[i]) + rb.length;
                        }
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// Нахождение ветвлений
        /// </summary>
        public List<Rbt> RtPrs(List<Rbt> ls, string s)
        {
            List<List<Rbt>> ret = new List<List<Rbt>>();
            string[] str1 = s.Split(';');
            foreach (string st1 in str1)
            {
                if (st1 != "")
                {
                    ret.Add(new List<Rbt>());
                    string[] str2 = st1.Split(',');
                    foreach (string st2 in str2)
                    {
                        if (st2 != "")
                        {
                            string[] str3 = st2.Split('-');
                            ret[ret.Count - 1].Add(ls.Find(x => x.point1 == Convert.ToInt32(str3[0]) && x.point2 == Convert.ToInt32(str3[1])));
                        }
                    }
                }
            }
            for (int i = 0; i < ret.Count; i++)
            {
                if (i > 0)
                {
                    if (ret[i][0].point1 != ret[i][ret[i].Count - 1].point2)
                    {
                        ret[i].InsertRange(0, ret[i - 1].FindAll(x => ret[i - 1].IndexOf(x) <= ret[i - 1].FindIndex(y => y.point2 == ret[i][0].point1)));
                    }
                }
            }
            int max = ret[0][0].length, maxind = 0;
            for (int i = 0; i < ret.Count; i++)
            {
                if (FnlMv(ret[i]) >= max)
                {
                    max = FnlMv(ret[i]);
                    maxind = i;
                }
            }
            return ret[maxind];
        }
        /// <summary>
        /// Нахождение длины пути
        /// </summary>
        public int FnlMv(List<Rbt> ls)
        {
            int ret = 0;
            foreach (Rbt rb in ls)
            {
                ret += rb.length;
            }
            return ret;
        }
        /// <summary>
        /// Вывод полученных данных в файл
        /// </summary>
        public void ViVod()
        {
            using (StreamWriter sr = new StreamWriter("Otvet.csv"))
            {
                foreach (Rbt rb in fnlcn[maxind])
                {
                    sr.WriteLine(rb.point1 + " - " + rb.point2);
                    Debug.WriteLine(rb.point1 + " - " + rb.point2);
                }
                sr.WriteLine("Rasstoanie: {0}", max);
                Debug.WriteLine(max);
            }
        }
    }
}

