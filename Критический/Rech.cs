using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Критический
{
    public class Rech
    {
        string s = "";
        public int max, mind;
        public List<List<Rbt>> fnlcn = new List<List<Rbt>>();
        public List<Rbt> ret;
        public  List<Rbt> ls = Flrd(path);
        public void put()
        {
            ret = ls.FindAll(x => x.point1 == ls[Minel(ls)].point1);
            foreach (Rbt rb in ret)
            {
                Mv(ls, rb);
                fnlcn.Add(RtPrs(ls, s));
                s = "";
            }
        }
        public struct Rbt
        {//Точки, длина
            public int point1;
            public int point2;
            public int length;

            //Запись пути
            public override string ToString()
            {
                return point1.ToString() + " - " + point2.ToString() + " " + length.ToString();
            }
        }

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

        public void ViVod()
        {
            using (StreamWriter sr = new StreamWriter("Otvet.csv"))
            {
                foreach (Rbt rb in fnlcn[maxind])
                {
                    sr.WriteLine(rb.point1 + " - " + rb.point2);
                }
                sr.WriteLine("Rasstoanie: {0}", max);
            }
        }
        public void naxodres()
        {
            mind = 0;
            max = fnlcn[0][0].length;
            ret();
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

    }
}
