using Collections = System.Collections.Generic;
using IO = System.IO;
using Text = System.Text;

public sealed class Scanner
{
	private readonly Collections.IList<object> resultado;

    public Scanner(IO.TextReader input)
	{
		this.resultado = new Collections.List<object>();
		this.Scan(input);
	}

	public Collections.IList<object> Tokens
	{
		get { return this.resultado; }
	}

    #region ConstantesAritmeticas

    // Constants to represent arithmitic tokens. This could
    // be alternatively written as an enum.
    public static readonly object Sum = new object();
    public static readonly object Res = new object();
    public static readonly object Mul = new object();
    public static readonly object Div = new object();
    public static readonly object PyC = new object();
    public static readonly object Igual = new object();
    public static readonly object Gt = new object();
    public static readonly object Lt = new object();
    public static readonly object Gte = new object();
    public static readonly object Lte = new object();
    public static readonly object Eq = new object();
    public static readonly object Neq = new object();
    
    #endregion

	private void Scan(IO.TextReader input)
	{
		while (input.Peek() != -1)
		{
			char caracter = (char)input.Peek();

            // Scan individual tokens
			if (char.IsWhiteSpace(caracter))
			{
				// eat the current char and skip ahead!
				input.Read();
			}
			else if (char.IsLetter(caracter) || caracter == '_')
			{
				// keyword or identifier

				Text.StringBuilder accum = new Text.StringBuilder();

				while (char.IsLetter(caracter) || caracter == '_')
				{
					accum.Append(caracter);
					input.Read();

					if (input.Peek() == -1)
					{
						break;
					}
					else
					{
						caracter = (char)input.Peek();
					}
				}

				this.resultado.Add(accum.ToString());
			}
            else if (caracter == '"')
			{
				// string literal
				Text.StringBuilder accum = new Text.StringBuilder();

				input.Read(); // skip the '"'

                if (input.Peek() == -1)
				{
					throw new System.Exception("Cadena sin terminar");
				}

				while ((caracter = (char)input.Peek()) != '"')
				{
					accum.Append(caracter);
					input.Read();

					if (input.Peek() == -1)
					{
						throw new System.Exception("Cadena sin terminar");
					}
				}

				// skip the terminating "
				input.Read();
				this.resultado.Add(accum);
			}
			else if (char.IsDigit(caracter))
			{
				// numeric literal

				Text.StringBuilder accum = new Text.StringBuilder();

				while (char.IsDigit(caracter))
				{
					accum.Append(caracter);
					input.Read();

					if (input.Peek() == -1)
					{
						break;
					}
					else
					{
						caracter = (char)input.Peek();
					}
				}

				this.resultado.Add(int.Parse(accum.ToString()));
			}
			else switch (caracter)
			{
				case '+':
					input.Read();
					this.resultado.Add(Scanner.Sum);
					break;

				case '-':
					input.Read();
					this.resultado.Add(Scanner.Res);
					break;

				case '*':
					input.Read();
					this.resultado.Add(Scanner.Mul);
					break;

				case '/':
					input.Read();
					this.resultado.Add(Scanner.Div);
					break;

				case '=':
					input.Read();
                    if (input.Peek() == '=')
                    {
                        input.Read();
                        this.resultado.Add(Scanner.Eq);
                    }
                    else
                        this.resultado.Add(Scanner.Igual);
					break;

				case ';':
					input.Read();
					this.resultado.Add(Scanner.PyC);
					break;
                case '>':
                    input.Read();
                    if (input.Peek() == '=')
                    {
                        input.Read();
                        this.resultado.Add(Scanner.Gte);
                    }
                    else
                        this.resultado.Add(Scanner.Gt);
                    break;
                case '<':
                    input.Read();
                    if (input.Peek() == '=')
                    {
                        input.Read();
                        this.resultado.Add(Scanner.Lte);
                    }
                    else
                        this.resultado.Add(Scanner.Lt);
                    break;
                case '!':
                    input.Read();
                    if (input.Peek() == '=')
                    {
                        input.Read();
                        this.resultado.Add(Scanner.Neq);
                    }
                    else
                        throw new System.Exception("No se reconoce el siguiente caracter: '" + caracter + "'");
                    break;
				default:
					throw new System.Exception("No se reconoce el siguiente caracter: '" + caracter + "'");
			}

        }
    }
}
