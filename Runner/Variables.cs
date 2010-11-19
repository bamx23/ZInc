using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

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

    

    public static class Constants // наверное нужен статик, чтобы не нужно было обьявлять экземпляр класса, а просто обращаться Constants.BOOL к примеру, а еще я их местами слегка поменяю непротив? =)
    {
        /*
        public const int DOUBLE = 2;
        public const int STRING = 3;
        public const int BOOL = 4;*/
        public const int NULLTYPE = 0;
        public const int BOOL = 1;
        public const int INT = 2;
        public const int DOUBLE = 3;
        public const int STRING = 4;

        public const int IO = 100;
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

    public class SystemObject
    {
        public int type;
        public string name;
    }

    public class SystemIO : SystemObject
    {
        public FileStream file;

        public SystemIO()
        {
            type = Constants.IO;
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
            type = Constants.NULLTYPE;
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

        static public bool operator <(VarInt a, VarInt b) 
        {
            return a.data < b.data;
        }
        static public bool operator >(VarInt a, VarInt b)
        {
            return a.data > b.data;
        }
        static public bool operator <=(VarInt a, VarInt b)
        {
            return a.data <= b.data;
        }
        static public bool operator >=(VarInt a, VarInt b)
        {
            return a.data >= b.data;
        }
        static public VarInt operator +(VarInt a, VarInt b)
        {
            return new VarInt(a.data + b.data);
        }
        static public VarInt operator -(VarInt a, VarInt b)
        {
            return new VarInt(a.data - b.data);
        }
        static public VarInt operator *(VarInt a, VarInt b)
        {
            return new VarInt(a.data * b.data);
        }
        static public VarInt operator /(VarInt a, VarInt b)
        {
            return new VarInt( (int)(a.data / b.data) );
        }
        static public VarInt operator %(VarInt a, VarInt b)
        {
            return new VarInt((int)(a.data % b.data));
        }

        public VarInt(): base()
        {
            data = 0;
            type = Constants.INT;
        }

        public VarInt(int Data): base()
        {
            type = Constants.INT;
            data = Data;
        }
    }         

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

        static public bool operator <(VarDouble a, VarDouble b)
        {
            return a.data < b.data;
        }
        static public bool operator >(VarDouble a, VarDouble b)
        {
            return a.data > b.data;
        }
        static public bool operator <=(VarDouble a, VarDouble b)
        {
            return a.data <= b.data;
        }
        static public bool operator >=(VarDouble a, VarDouble b)
        {
            return a.data >= b.data;
        }
        static public VarDouble operator +(VarDouble a, VarDouble b)
        {
            return new VarDouble(a.data + b.data);
        }
        static public VarDouble operator -(VarDouble a, VarDouble b)
        {
            return new VarDouble(a.data - b.data);
        }
        static public VarDouble operator *(VarDouble a, VarDouble b)
        {
            return new VarDouble(a.data * b.data);
        }
        static public VarDouble operator /(VarDouble a, VarDouble b)
        {
            return new VarDouble(a.data / b.data);
        }
        static public VarDouble operator %(VarDouble a, VarDouble b)
        {
            return new VarDouble(a.data % b.data);
        }

        public VarDouble() : base()
        {
            type = Constants.DOUBLE;
            data = 0;
        }

        public VarDouble(double Data) : base()
        {
            type = Constants.DOUBLE;
            data = Data;
        }
    }       

    public class VarString : VarObject
    {
        public string data;

        public override VarObject NewOfType()
        {
            return new VarString();
        }

        public override int ToInt()
        {
            try
            {
                return int.Parse(data);
            }
            catch (Exception e)
            {
                Regex o = new Regex(@"\s*"); // Не уверен что это сработает, нужно проверить =)
                Match someMatch = o.Match(data);
                if(someMatch.Success)
                    return int.Parse(someMatch.Value);
                return 0;
            }
        }

        public override double ToDouble()
        {
            try
            {
                return double.Parse(data);
            }
            catch (Exception e)
            {
                Regex o = new Regex(@"(\s*|\s*.\s*)"); // Не уверен что это сработает, нужно проверить =)
                Match someMatch = o.Match(data);
                if (someMatch.Success)
                    return double.Parse(someMatch.Value);
                return 0;
            }
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

        static public bool operator <(VarString a, VarString b)
        {
            return a.data.Length < b.data.Length;
        }
        static public bool operator >(VarString a, VarString b)
        {
            return a.data.Length > b.data.Length;
        }
        static public bool operator <=(VarString a, VarString b)
        {
            return a.data.Length <= b.data.Length;
        }
        static public bool operator >=(VarString a, VarString b)
        {
            return a.data.Length >= b.data.Length;
        }
        static public VarString operator +(VarString a, VarString b) // string интерпретирует любую операцию как сложение, и в общем то что ему не пиши он все сложит =)
        {
            return new VarString(a.data + b.data);
        }
        static public VarString operator -(VarString a, VarString b)
        {
            return new VarString(a.data + b.data);
        }
        static public VarString operator *(VarString a, VarString b)
        {
            return new VarString(a.data + b.data);
        }
        static public VarString operator /(VarString a, VarString b)
        {
            return new VarString(a.data + b.data);
        }
        static public VarString operator %(VarString a, VarString b)
        {
            return new VarString(a.data + b.data);
        }

        public VarString() : base()
        {
            type = Constants.STRING;
            data = "";
        }

        public VarString(string Data) : base()
        {
            type = Constants.STRING;
            data = Data;
        }
    }      

    public class VarBool : VarObject
    {
        public bool data;

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
            return data ? 1 : 0;
        }

        public override double ToDouble()
        {
            return data ? 1 : 0;
        }

        public override string ToStr()
        {
            return data ? "true" : "false"; // раньше было return data ? "TRUE" : "FALSE"; но мне кажется это не удобно
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

        static public bool operator <(VarBool a, VarBool b)
        {
            return a.data.ToString().Length < b.data.ToString().Length;
        }
        static public bool operator >(VarBool a, VarBool b)
        {
            return a.data.ToString().Length > b.data.ToString().Length;
        }
        static public bool operator <=(VarBool a, VarBool b)
        {
            return a.data.ToString().Length <= b.data.ToString().Length;
        }
        static public bool operator >=(VarBool a, VarBool b)
        {
            return a.data.ToString().Length >= b.data.ToString().Length;
        }
        static public VarBool operator +(VarBool a, VarBool b) // Арифмические действия с булевскими переменнвми соответствуют логическим операциям =) 
        {
            return new VarBool(a.data || b.data);
        } 
        static public VarBool operator -(VarBool a, VarBool b)
        {
            return new VarBool(!(a.data || b.data));
        }
        static public VarBool operator *(VarBool a, VarBool b)
        {
            return new VarBool(a.data && b.data);
        }
        static public VarBool operator /(VarBool a, VarBool b)
        {
            return new VarBool(!(a.data && b.data));
        }
        static public VarBool operator %(VarBool a, VarBool b)
        {
            return new VarBool(a.data && !b.data);
        } 

        public VarBool() : base()
        {
            type = Constants.BOOL;
        }

        public VarBool(bool Data) : base()
        {
            type = Constants.BOOL;
            data = Data;
        }
    }         
}