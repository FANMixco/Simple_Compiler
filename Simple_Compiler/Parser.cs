using Collections = System.Collections.Generic;
using Text = System.Text;

public sealed class Parser
{
	private int indice;
	private Collections.IList<object> tokens;
	private readonly Stmt resultado;

	public Parser(Collections.IList<object> tokens)
	{
		this.tokens = tokens;
		this.indice = 0;
		this.resultado = this.ParseStmt();
		
		if (this.indice != this.tokens.Count)
			throw new System.Exception("se esperaba el final del archivo");
	}

	public Stmt Resultado
	{
		get { return resultado; }
	}

    private Stmt ParseStmt()
    {
        Stmt resultado;

		if (this.indice == this.tokens.Count)
		{
			throw new System.Exception("se esperaban sentencias, se llego al final del archivo");
		}

        // <stmt> := print <expr> 

        // <expr> := <string>
	    // | <int>
	    // | <arith_expr>
	    // | <ident>
		if (this.tokens[this.indice].Equals("print"))
		{
			this.indice++;
			Print print = new Print();
			print.Expr = this.ParseExpr();
			resultado = print;
		}
		else if (this.tokens[this.indice].Equals("var"))
		{
			this.indice++;
			DeclareVar declareVar = new DeclareVar();

			if (this.indice < this.tokens.Count &&
				this.tokens[this.indice] is string)
			{
				declareVar.Ident = (string)this.tokens[this.indice];
			}
			else
			{
				throw new System.Exception("Se esperaba nombre de variable despues de 'var'");
			}

			this.indice++;

			if (this.indice == this.tokens.Count ||
				this.tokens[this.indice] != Scanner.Igual)
			{
				throw new System.Exception("se esperaba = despues de 'var ident'");
			}

			this.indice++;

			declareVar.Expr = this.ParseExpr();
			resultado = declareVar;
		}
        else if (this.tokens[this.indice].Equals("read_int"))
		{
			this.indice++;
			ReadInt readInt = new ReadInt();

			if (this.indice < this.tokens.Count &&
				this.tokens[this.indice] is string)
			{
				readInt.Ident = (string)this.tokens[this.indice++];
				resultado = readInt;
			}
			else
			{
				throw new System.Exception("Se esperaba el nombre de la variable 'read_int'");
			}
		}
        //*******************
        else if (this.tokens[this.indice].Equals("if"))
        {
            this.indice++;
            mcIf mcif = new mcIf();
            Expr temp = ParseExpr();
            if (this.tokens[this.indice] == Scanner.Eq || this.tokens[this.indice] == Scanner.Neq ||
                this.tokens[this.indice] == Scanner.Gt || this.tokens[this.indice] == Scanner.Gte ||
                this.tokens[this.indice] == Scanner.Lt || this.tokens[this.indice] == Scanner.Lte)
            {
                CompExpr compExpr = new CompExpr();
                compExpr.Left = temp;
                object op = this.tokens[this.indice++];
                if (op == Scanner.Eq)
                    compExpr.Op = CompOp.Eq;
                else if (op == Scanner.Neq)
                    compExpr.Op = CompOp.Neq;
                else if (op == Scanner.Gt)
                    compExpr.Op = CompOp.Gt;
                else if (op == Scanner.Gte)
                    compExpr.Op = CompOp.Gte;
                else if (op == Scanner.Lt)
                    compExpr.Op = CompOp.Lt;
                else if (op == Scanner.Lte)
                    compExpr.Op = CompOp.Lte;
                compExpr.Rigth = ParseExpr();
                temp = compExpr;
            }
            mcif.compExpr = temp;
            if (this.indice == this.tokens.Count || !this.tokens[this.indice].Equals("then"))
            {
                throw new System.Exception("Se esperaba el identificador 'then' despues de 'if'");
            }
            this.indice++;
            mcif.Then = ParseStmt();
            if (this.tokens[this.indice].Equals("else"))
            {
                this.indice++;
                mcif.Else = ParseStmt();
            }

            resultado = mcif;
            if (this.indice == this.tokens.Count ||
                !this.tokens[this.indice].Equals("end"))
            {
                throw new System.Exception("Sentencia if inconclusa");
            }

            this.indice++;
            
        }
        else if (this.tokens[this.indice].Equals("while"))
        {
            this.indice++;
            WhileLoop whileLoop = new WhileLoop();
            Expr temp = ParseExpr();
            if (this.tokens[this.indice] == Scanner.Eq || this.tokens[this.indice] == Scanner.Neq ||
                this.tokens[this.indice] == Scanner.Gt || this.tokens[this.indice] == Scanner.Gte ||
                this.tokens[this.indice] == Scanner.Lt || this.tokens[this.indice] == Scanner.Lte)
            {
                CompExpr compExpr = new CompExpr();
                compExpr.Left = temp;
                object op = this.tokens[this.indice++];
                if (op == Scanner.Eq)
                    compExpr.Op = CompOp.Eq;
                else if (op == Scanner.Neq)
                    compExpr.Op = CompOp.Neq;
                else if (op == Scanner.Gt)
                    compExpr.Op = CompOp.Gt;
                else if (op == Scanner.Gte)
                    compExpr.Op = CompOp.Gte;
                else if (op == Scanner.Lt)
                    compExpr.Op = CompOp.Lt;
                else if (op == Scanner.Lte)
                    compExpr.Op = CompOp.Lte;
                compExpr.Rigth = ParseExpr();
                temp = compExpr;
            }
            whileLoop.Cond = temp;
            if (this.indice == this.tokens.Count || !this.tokens[this.indice].Equals("do"))
            {
                throw new System.Exception("Se esperaba el identificador 'do' despues de 'while'");
            }
            this.indice++;
            whileLoop.Body = ParseStmt();
            resultado = whileLoop;
            if (this.indice == this.tokens.Count ||
                !this.tokens[this.indice].Equals("end"))
            {
                throw new System.Exception("sentencia while inconclusa");
            }
            this.indice++;
        }
        //*******************
        else if (this.tokens[this.indice].Equals("for"))
        {
            this.indice++;
            ForLoop forLoop = new ForLoop();

            if (this.indice < this.tokens.Count &&
                this.tokens[this.indice] is string)
            {
                forLoop.Ident = (string)this.tokens[this.indice];
            }
            else
            {
                throw new System.Exception("se esperaba un indentificador despues de 'for'");
            }

            this.indice++;

            if (this.indice == this.tokens.Count ||
                this.tokens[this.indice] != Scanner.Igual)
            {
                throw new System.Exception("no se encontro en la sentencia for '='");
            }

            this.indice++;

            forLoop.From = this.ParseExpr();

            if (this.indice == this.tokens.Count ||
                !this.tokens[this.indice].Equals("to"))
            {
                throw new System.Exception("se epsaraba 'to' despues de for");
            }

            this.indice++;

            forLoop.To = this.ParseExpr();

            if (this.indice == this.tokens.Count ||
                !this.tokens[this.indice].Equals("do"))
            {
                throw new System.Exception("se esperaba 'do' despues de la expresion en el ciclo for");
            }

            this.indice++;

            forLoop.Body = this.ParseStmt();
            resultado = forLoop;

            if (this.indice == this.tokens.Count ||
                !this.tokens[this.indice].Equals("end"))
            {
                throw new System.Exception("setencia for inconclusa");
            }

            this.indice++;
        }
        else if (this.tokens[this.indice] is string)
        {
            // assignment

            Assign assign = new Assign();
            assign.Ident = (string)this.tokens[this.indice++];

            if (this.indice == this.tokens.Count ||
                this.tokens[this.indice] != Scanner.Igual)
            {
                throw new System.Exception("se esperaba '='");
            }

            this.indice++;

            assign.Expr = this.ParseExpr();
            resultado = assign;
        }
        else
        {
            throw new System.Exception("Error en el token " + this.indice + ": " + this.tokens[this.indice]);
        }

		if (this.indice < this.tokens.Count && this.tokens[this.indice] == Scanner.PyC)
		{
			this.indice++;

			if (this.indice < this.tokens.Count &&
                !this.tokens[this.indice].Equals("end") && !this.tokens[this.indice].Equals("else"))
			{
				Sequence sequence = new Sequence();
				sequence.First = resultado;
				sequence.Second = this.ParseStmt();
				resultado = sequence;
			}
		}

        return resultado;
    }

    private Expr ParseExpr()
    {
		if (this.indice == this.tokens.Count)
		{
			throw new System.Exception("Se esperaba una expresion, se llego al final del archivo");
		}

        if (this.tokens[this.indice] is Text.StringBuilder)
        {
            string value = ((Text.StringBuilder)this.tokens[this.indice++]).ToString();
            StringLiteral stringLiteral = new StringLiteral();
            stringLiteral.Value = value;
            return stringLiteral;
        }
        //*********
        else if (this.tokens[this.indice + 1] == Scanner.Sum || this.tokens[this.indice + 1] == Scanner.Res || this.tokens[this.indice + 1] == Scanner.Mul || this.tokens[this.indice + 1] == Scanner.Div)
        {
            BinExpr binExpr = new BinExpr();
            Expr left;

            if (this.tokens[this.indice] is int)
            {
                int intValue = (int)this.tokens[this.indice++];
                IntLiteral intLiteral = new IntLiteral();
                intLiteral.Value = intValue;
                left = intLiteral;
            }
            else if (this.tokens[this.indice] is string)
            {
                string ident = (string)this.tokens[this.indice++];
                Variable var = new Variable();
                var.Ident = ident;
                left = var;
            }
            else
            {
                throw new System.Exception("Se esperaba un entero o variable para la operacion.");
            }
            binExpr.Left = left;
            object op = this.tokens[this.indice++];
            if (op == Scanner.Sum)
                binExpr.Op = BinOp.Sum;
            else if (op == Scanner.Res)
                binExpr.Op = BinOp.Res;
            else if (op == Scanner.Mul)
                binExpr.Op = BinOp.Mul;
            else if (op == Scanner.Div)
                binExpr.Op = BinOp.Div;

            binExpr.Right = this.ParseExpr();
            return binExpr;
        }
        //*********
        else if (this.tokens[this.indice] is int)
        {
            int intValue = (int)this.tokens[this.indice++];
            IntLiteral intLiteral = new IntLiteral();
            intLiteral.Value = intValue;
            return intLiteral;
        }
        else if (this.tokens[this.indice] is string)
        {
            string ident = (string)this.tokens[this.indice++];
            Variable var = new Variable();
            var.Ident = ident;
            return var;
        }
        else
        {
            throw new System.Exception("se esperaba una cadena, entero o variable");
        }
    }

}
