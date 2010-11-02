using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Runner
{
    public class SystemVars
    {
    
    }

    public class VarList
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
    }

    public class VarObject
    {
        protected byte type;
        public byte Type
        {
            get
            {
                return type;
            }
        }

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

        public virtual void Parse(string S) { }

        public VarObject()
        {
            sub = new List<VarObject>();
            sub.Add(this);
            deep = 0;
            type = 0;
        }
    }                   // type = 0

    public class VarInt : VarObject
    {
        public int data;

        public VarInt()
            : base()
        {
            data = 0;
            type = 1;
        }

        public VarInt(int Data)
            : base()
        {
            type = 1;
            data = Data;
        }
    }          // type = 1

    public class VarDouble : VarObject
    {
        public double data;

        public VarDouble()
            : base()
        {
            type = 2;
            data = 0;
        }

        public VarDouble(int Data)
            : base()
        {
            type = 2;
            data = Data;
        }
    }       // type = 2

    public class VarString : VarObject
    {
        public string data;

        public VarString()
            : base()
        {
            type = 3;
            data = "";
        }

        public VarString(string Data)
            : base()
        {
            type = 3;
            data = Data;
        }
    }       // type = 3
}