using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Runner
{
   /* public class VarList
    {
        public List<VarObject> list;
        public string text;
        private bool dyn;

        public void Complete()
        {
            if (!dyn && list.Count() != 0)
                return;

            //формирование массива переменных
        }

        public int Length()
        {
            return list.Count();
        }

        public VarList(string s)
        {
            dyn = false;
            Complete();
        }
    }*/

    public class Constants
    {
        
        public const int DOUBLE = 2;
        public const int STRING = 3;
        public const int BOOL = 4;
    }

    public class CodeObject
    {
        public string Name;

        protected short type;
        public short Type
        {
            get
            {
                return type;
            }
        }
    }

    public class DynVarObject : CodeObject
    {
        private string val;

        public string Val
        {
            get
            {
                return val;
            }
        }

        public VarObject Get(List<VarObject> L, string v = "")
        {
            string text = (v == "")?val:v;

            Regex o = new Regex(@"\((?=[^\(]*\)).*?\)");
            while (o.IsMatch(text))
                text = o.Replace(text, Get(L, o.Match(text).Value.Trim('(',')')).ToStr(), 1);

            MatchCollection r = (new Regex(@"\d+")).Matches(text);

            VarObject R = L[int.Parse(r[0].Value)];
            for (int i = 1; i < r.Count; i++)
                R = R[int.Parse(r[i].Value)];

            return R;
        }

        public DynVarObject(string val)
        {
            this.val = val;
            type = -1;
        }
    }

    public class VarObject : CodeObject
    {
        public List<VarObject> sub;

        protected int deep;
        public int Deep
        {
            get
            {
                return deep;
            }
        }

        public VarObject this[int index]
        {
            get
            {
                if (index >= 0 && index < deep)
                    return sub[index];
                else
                {
                    for (int i = deep; i < index; i++)
                        sub.Add(NewOfType());
                    deep = index + 1;
                    VarObject V = NewOfType();
                    sub.Add(V);
                    return V;
                }
            }
            set
            {
                if (index >= 0 && index < deep)
                    sub[index] = value;
                else
                {
                    for (int i = deep; i < index; i++)
                        sub.Add(NewOfType());
                    deep = index + 1;
                    sub.Add(value);
                }
            }
        }

        public virtual VarObject NewOfType()
        {
            return new VarObject();
        }

        public virtual string ToStr()
        {
            return "0";
        }

        public virtual int ToInt()
        {
            return 0;
        }

        public virtual double ToDouble()
        {
            return 0.0;
        }

        public virtual bool ToBool()
        {
            return false;
        }

        public virtual void Parse(string S) 
        {
        }

        public VarObject()
        {
            sub = new List<VarObject>();
            sub.Add(this);
            deep = 1;
            type = 0;
        }
    }                   // type = 0

    public class VarInt : VarObject
    {
        public int data;

        public override VarObject NewOfType()
        {
            return new VarInt();
        }

        public override int ToInt()
        {
            return data;
        }

        public override double ToDouble()
        {
            return (double)data;
        }

        public override string ToStr()
        {
            return data.ToString();
        }

        public override bool ToBool()
        {
            return data != 0;
        }

        public override void Parse(string S)
        {
            data = int.Parse(S);
        }

        public VarInt(): base()
        {
            data = 0;
            type = 1;
        }

        public VarInt(int Data): base()
        {
            type = 1;
            data = Data;
        }
    }          // type = 1

    public class VarDouble : VarObject
    {
        public double data;

        public override VarObject NewOfType()
        {
            return new VarDouble();
        }

        public override int ToInt()
        {
            return (int)data;
        }

        public override double ToDouble()
        {
            return data;
        }

        public override string ToStr()
        {
            return data.ToString();
        }

        public override bool ToBool()
        {
            return data != 0.0;
        }

        public override void Parse(string S)
        {
            data = double.Parse(S);
        }

        public VarDouble() : base()
        {
            type = 2;
            data = 0;
        }

        public VarDouble(int Data) : base()
        {
            type = 2;
            data = Data;
        }
    }       // type = 2

    public class VarString : VarObject
    {
        public string data;

        public override VarObject NewOfType()
        {
            return new VarString();
        }

        public override int ToInt()
        {
            return int.Parse(data);
        }

        public override double ToDouble()
        {
            return double.Parse(data);
        }

        public override string ToStr()
        {
            return data;
        }

        public override bool ToBool()
        {
            return (data != "" || data.ToUpper() == "TRUE");
        }

        public override void Parse(string S)
        {
            data = S;
        }

        public VarString() : base()
        {
            type = 3;
            data = "";
        }

        public VarString(string Data) : base()
        {
            type = 3;
            data = Data;
        }
    }       // type = 3

    public class VarBool : VarObject
    {
        bool data;

        public override VarObject NewOfType()
        {
            return new VarBool();
        }

        public override bool ToBool()
        {
            return data;
        }

        public override int ToInt()
        {
            return data?1:0;
        }

        public override double ToDouble()
        {
            return data?1:0;
        }

        public override string ToStr()
        {
            return data?"TRUE":"FALSE";
        }

        public override void Parse(string S)
        {
            switch(S.ToUpper())
            {
                case "TRUE":
                    data = true;
                    return;
                case "FALSE":
                    data = false;
                    return;
            }
        }

        public VarBool() : base()
        {
            type = 4;
        }

        public VarBool(bool Data) : base()
        {
            type = 4;
            data = Data;
        }
    }         // type = 4
}