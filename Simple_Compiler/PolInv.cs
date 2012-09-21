using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple_Compiler
{
    class PolInv
    {
        List<object> Tokens = new List<object>();
        Stack<Expr> algo = new Stack<Expr>();
        Stack<BinOp> Operadores = new Stack<BinOp>();
        public List<object> Salida = new List<object>();
        public bool error;

        public PolInv(BinExpr binExpr) {
            extraerTokens(binExpr);
        }

        public void calcularPolaca()
        {
            foreach (object temp in Tokens)
            {
                if (temp is Variable || temp is IntLiteral)
                    Salida.Add(temp);
                else
                {
                    switch ((BinOp)temp)
                    {
                        case BinOp.Sum:
                        case BinOp.Res:
                            if (Operadores.Count == 0)
                                Operadores.Push((BinOp)temp);
                            else {
                                while (Operadores.Peek() == BinOp.Mul || Operadores.Peek() == BinOp.Div)
                                {
                                    Salida.Add(Operadores.Pop());
                                    if (Operadores.Count == 0)
                                        break;
                                }
                                Operadores.Push((BinOp)temp);
                            }
                            break;
                        case BinOp.Mul:
                        case BinOp.Div:
                            if (Operadores.Count == 0)
                                Operadores.Push((BinOp)temp);
                            else
                            {
                                while (Operadores.Peek() == BinOp.Mul || Operadores.Peek() == BinOp.Div)
                                {
                                    Salida.Add(Operadores.Pop());
                                    if (Operadores.Count == 0)
                                        break;
                                }
                                Operadores.Push((BinOp)temp);
                            }
                            break;
                    }
                }
            }
            while (Operadores.Count > 0) Salida.Add(Operadores.Pop());
        }

        private void extraerTokens(BinExpr binExpr) {
            Tokens.Add(binExpr.Left);
            Tokens.Add(binExpr.Op);
            if (binExpr.Right is Variable || binExpr.Right is IntLiteral)
                Tokens.Add(binExpr.Right);
            else if(binExpr.Right is BinExpr)
                extraerTokens((BinExpr)binExpr.Right);
        }

    }
}

