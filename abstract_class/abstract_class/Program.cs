using System;

namespace abstract_class
{
    class Program
    {
        public class Token
        {
            public string type;
            public string value;
            public int poition;
            public int lineNumber;
        }
        public abstract class Tokenizable
        {
            abstract public bool tokenizable(Tokenizer tokenizer);
            abstract public Token tokenize(Tokenizer tokenizer);

        }

        public class Tokenizer
        {
            public string input;
            public int currentPosition;
            public int lineNumber;


            public Tokenizer(string input)
            {
                this.input = input;
                this.currentPosition = -1;
                this.lineNumber = 1;

            }
            public char peek(int numOfposition = 1)
            {
                if (this.hasMore())
                {
                    return this.input[this.currentPosition + numOfposition];
                }
                else
                {
                    return '\0';
                }
            }

            public char next()
            {
                char currentChar = this.input[++this.currentPosition];
                if(currentPosition == '\n')
                {
                    this.lineNumber++;
                }
                return currentChar;
            }

              public bool hasMore() { return (this.currentPosition + 1) < this.input.Length; }
              public Token tokenize(Tokenizable[] handlers)
              {
                foreach(var t in handlers)
                {
                    if (t.tokenizable(this))
                    {
                        return t.tokenize(this);
                    }
                }
                return null;
                
              }
            
        }

         public class NumberTokenizer  : Tokenizable
         {
            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore() && Char.IsDigit(t.peek());
            }
             public override Token tokenize(Tokenizer t)
             {
                Token token = new Token();
                token.value = "";
                token.type = "number";
                token.poition = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (t.hasMore() && char.IsDigit(t.peek()))
                {
                    token.value += t.next();
                }
                return token;
             }
        }

        public class WhiteSpace : Tokenizable
        {
            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore() && Char.IsWhiteSpace(t.peek());
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.value = "";
                token.type = "space";
                token.poition = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (t.hasMore() && Char.IsWhiteSpace(t.peek()))
                {
                    token.value += t.next();
                }
                return token;
            }
        }

        public class idTokenizer : Tokenizable
        {
            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore() && Char.IsLetterOrDigit(t.peek()) || t.peek() == '_';
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.value = "";
                token.type = "id";
                token.poition = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (t.hasMore() && Char.IsLetterOrDigit(t.peek()))
                {
                    token.value += t.next();
                }
                return token;
            }
        }
        static void Main(string[] args)
        {
            Tokenizer t = new Tokenizer("1234 12if  jdcnkjd");
            Tokenizable[] handers = new Tokenizable[] { new NumberTokenizer(),new WhiteSpace(),new idTokenizer() };
            Token token = t.tokenize(handers);
            while(token != null)
            {

                Console.WriteLine(token.value);
                token = t.tokenize(handers);
            }
            
            
        }
    }
}
