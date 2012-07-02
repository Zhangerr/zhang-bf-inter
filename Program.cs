using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace bfinter
{
    enum Op { //operators
        GT,
        LT,
        PL,
        MIN,
        DOT,
        COM,
        LB,
        RB
    }

    class Program
    {
        static Dictionary<char, Op> map = new Dictionary<char, Op>()
        {
            {'>', Op.GT},
            {'<', Op.LT},
            {'+',Op.PL},
            {'-',Op.MIN},
            {'.',Op.DOT},
            {',',Op.COM},
            {'[',Op.LB},
            {']',Op.RB},
        };
        delegate void doStuff(ref char[] d, ref int e);

        static Dictionary<char, doStuff> ops = new Dictionary<char,doStuff> () {
            {'>',delegate(ref char[] chars, ref int index) {index++;}},
            {'<',delegate(ref char[] chars, ref int index) {index--;}},
            {'+',delegate(ref char[] chars, ref int index) {chars[index] = (char)((int)(chars[index]) + 1);}},
            {'-',delegate(ref char[] chars, ref int index) {chars[index] = (char)((int)(chars[index]) - 1);}},
            {'.',delegate(ref char[] chars, ref int index) {Console.Write(chars[index]);}},
            {',',delegate(ref char[] chars, ref int index) {chars[index] = Console.ReadKey().KeyChar;}},
            // [ ] handled specially

        };

        static void matchBrackets(string s, Dictionary<int,int> jumps )
        {
            Stack<int> ops = new Stack<int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '[')
                {
                    ops.Push(i);
                }
                else if (s[i] == ']')
                {
                    int temp = ops.Pop();
                    jumps.Add(temp, i);
                    jumps.Add(i, temp);
                }
            }
        }
        //http://esolangs.org/wiki/Clusterfuck remodel with bracket matching; also, just make characters map directly to delegates, no point making 2 table lookups
        static void Main(string[] args)
        {
            char[] array = new char[30000];
            Dictionary<int, int> jumps = new Dictionary<int, int>();
          
            int index = 0;
            LinkedList<char> ll = new LinkedList<char>();
            string s = "++++++++++[.-]";
            string s23 = @"
-,+[                         Read first character and start outer character reading loop
    -[                       Skip forward if character is 0
        >>++++[>++++++++<-]  Set up divisor (32) for division loop
                               (MEMORY LAYOUT: dividend copy remainder divisor quotient zero zero)
        <+<-[                Set up dividend (x minus 1) and enter division loop
            >+>+>-[>>>]      Increase copy and remainder / reduce divisor / Normal case: skip forward
            <[[>+<-]>>+>]    Special case: move remainder back to divisor and increase quotient
            <<<<<-           Decrement dividend
        ]                    End division loop
    ]>>>[-]+                 End skip loop; zero former divisor and reuse space for a flag
    >--[-[<->[-]]]<[         Zero that flag unless quotient was 2 or 3; zero quotient; check flag
        ++++++++++++<[       If flag then set up divisor (13) for second division loop
                               (MEMORY LAYOUT: zero copy dividend divisor remainder quotient zero zero)
            >-[>+>>]         Reduce divisor; Normal case: increase remainder
            >[+[<+>-]>+>>]   Special case: increase remainder / move it back to divisor / increase quotient
            <<<<<-           Decrease dividend
        ]                    End division loop
        >>[<+>-]             Add remainder back to divisor to get a useful 13
        >[                   Skip forward if quotient was 0
            -[               Decrement quotient and skip forward if quotient was 1
                -<<[-]>>     Zero quotient and divisor if quotient was 2
            ]<<[<<->>-]>>    Zero divisor and subtract 13 from copy if quotient was 1
        ]<<[<<+>>-]          Zero divisor and add 13 to copy if quotient was 0
    ]                        End outer skip loop (jump to here if ((character minus 1)/32) was not 2 or 3)
    <[-]                     Clear remainder from first division if second division was skipped
    <.[-]                    Output ROT13ed character from copy and clear it
    <-,+                     Read next character
]                            End character reading loop";
            string s1 = "++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.";//File.ReadAllText("");
            matchBrackets(s, jumps);
        /*    foreach (char i in s)
            {

                    ll.AddLast(i);
                                    
            }
            LinkedListNode<char> head = ll.First;*/
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '[')
                {
                    if (array[index] == 0)
                    {
                       // while (s[(i = i + 1)] != ']') ;
                        i = jumps[i];
                    }
                }
                else if (s[i] == ']')
                {
                    if (array[index] != 0)
                    {
                    //    while (s[(i = i - 1)] != '[') ;
                        i = jumps[i];
                    }
                }
                else if (ops.ContainsKey(s[i]))
                {
                    ops[s[i]](ref array, ref index);
                }
            }
           /*     do
                {
                    if (head.Value == '[')
                    {
                        if (array[index] == 0)
                        {
                            while ((head = head.Next).Value != ']') ;
                        }
                    }
                    else if (head.Value == ']')
                    {
                        if (array[index] != 0)
                        {
                            while ((head = head.Previous).Value != '[') ;
                        }
                    }
                    else if (ops.ContainsKey(head.Value))
                    {
                        ops[head.Value](ref array, ref index);
                    }
                } while ((head = head.Next) != null);*/
            Console.ReadKey();
        }
    }
}
