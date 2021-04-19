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

         public class IntOrfloatTokenizer : Tokenizable
         {
            public override bool tokenizable(Tokenizer t)
            {
                return t.hasMore() && Char.IsDigit(t.peek());
            }
             public override Token tokenize(Tokenizer t)
             {
                Token token = new Token();
                int count = 0;
                int currentpos = 0;
                token.value = "";
                token.type = "int";
                token.poition = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (t.hasMore() && (char.IsDigit(t.peek())|| t.peek()=='.' || t.peek() == 'f' ) )
                {
                    if(t.peek() == '.' || t.peek() == 'f')
                    {
                        token.type = "float";
                        count++;
                        token.value += t.next();
                    }
                    if (t.peek() == 'f')
                    {
                        if(t.peek(2) != ' ')
                        {
                            return null;
                        }
                    }
                    if (count < 2)
                    {
                        token.value += t.next();
                        

                    }
                   
                }
                return token;
             }
        }


        //22.22
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
                return (t.hasMore() && (Char.IsLetter(t.peek()) || t.peek() == '_'));
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.value = "";
                token.type = "id";
                token.poition = t.currentPosition;
                token.lineNumber = t.lineNumber;

                while (t.hasMore() && (Char.IsLetterOrDigit(t.peek()) || t.peek() == '_'))
                {
                    
                    token.value += t.next();
                }
                
                return token;
            }
        }

        public class hachTokenizable : Tokenizable
        {
            public override bool tokenizable(Tokenizer t)
            {
                return (t.hasMore() && t.peek() == '#');
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.value = "";
                token.type = "Hach";
                token.poition = t.currentPosition;
                token.lineNumber = t.lineNumber;

                token.value += t.next();
                while (t.hasMore() && (Char.IsLetterOrDigit(t.peek())))
                {
                    token.value += t.next();
                }

                return token;
            }
        }

        public class userTokenizable : Tokenizable
        {
            public override bool tokenizable(Tokenizer t)
            {
                return (t.hasMore() && t.peek() == '@');
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.value = "";
                token.type = "user";
                token.poition = t.currentPosition;
                token.lineNumber = t.lineNumber;

                token.value += t.next();
                while (t.hasMore() && (Char.IsLetterOrDigit(t.peek())))
                {
                    token.value += t.next();
                }

                return token;
            }
        }

        public class xmlTokenizable : Tokenizable
        {
            public override bool tokenizable(Tokenizer t)
            {
                return (t.hasMore() && t.peek() == '<');
            }
            public override Token tokenize(Tokenizer t)
            {
                Token token = new Token();
                token.value = "";
                token.type = "open tag xml";
                token.poition = t.currentPosition;
                token.lineNumber = t.lineNumber;

                token.value += t.next();
                while (t.hasMore() && (Char.IsLetterOrDigit(t.peek()) || t.peek() == '>' || t.peek() == '/' ))
                {
                    if (t.peek() == '/')
                    {
                        if (token.value == "<")
                        {
                            token.type = "close tag xml";
                        }
                        else
                        {
                            
                            return null;
                        }
                     

                    }
                    token.value += t.next();
                    
                    if (t.peek() == '>')
                    {
                        token.value += t.next();
                        break;
                    }
                }


                return token;
            }
        }


        

        static void Main(string[] args)
        {
            Tokenizer t = new Tokenizer("#vjdnkd 20 20.12f 19 if  _jdc_nkjd  20f @Abdulrahman <to> hhh  </to> jjj ");
            Tokenizable[] handers = new Tokenizable[] { new xmlTokenizable(),new IntOrfloatTokenizer(),new WhiteSpace(),new idTokenizer(),new hachTokenizable(),new userTokenizable() };
            Token token = t.tokenize(handers);
            while(token != null)
            {

                Console.WriteLine(token.value+" | "+token.type);
                token = t.tokenize(handers);
            }
            
            
        }
    }
}
