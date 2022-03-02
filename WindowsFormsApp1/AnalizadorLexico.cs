using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Por: Isaac Rey Fimbres Aragón - 357977

namespace WindowsFormsApp1
{
    public class Tokens
    {
        #region Atributos
        private int _Linea;
        private string _Lexema;
        private int _Token;
        #endregion
        #region Constructor
        public Tokens(int linea, string lexema, int token)
        {
            Linea = linea;
            Lexema = lexema;
            Token = token;
        }
        #endregion
        #region encapsulamiento
        public int Linea { get => _Linea; set => _Linea = value; }
        public string Lexema { get => _Lexema; set => _Lexema = value; }
        public int Token { get => _Token; set => _Token = value; }
        #endregion
    }
    public class UnidadesLexicas
    {
        public Dictionary<string, int> PalabrasReservadas = new Dictionary<string, int>()
        {
            {"auto", 1},
            {"break", 2 },
            {"case" , 3},
            {"char" , 4},
            {"&&", 5 },
            {"||", 6 },
            {"\\", 7 },
            {"\\r", 9 },
            {"\\n", 10 },
            {"\\b", 11 },
            {"\\t", 12 },
            {"\\0", 13 },
            {"float", 14 },
            {"int", 15 },
            {"<<", 16 },
            {">>", 17 },
            {"\"", 18 },
            {"==", 19 },
            {"foreach", 20 },
            {"in", 21 },
            {"{", 22 },
            {"}", 23 },
            {"/=", 24 },
            {"false", 25 },
            {"<", 26 },
            {">", 27 },
            {"<=", 28 },
            {">=", 29 },
            {"-=", 30 },
            {"+=", 31 },
            {"++", 32 },
            {"--", 33 },
            {"*=", 34 },
            {"(", 35 },
            {")", 36 },
            {".", 37 },
            {"," , 38},
            {"true" , 39},
            {":" , 40},
            {";" , 45},
            {"#", 355 },
            {"include" , 345},
            {"main", 346 },
            {"conio", 398 },
            {"+", 112 },
            {"-" , 113},
            {"double" , 114},
            {"*", 121 },
            {"|", 141 },
            {"/", 221 },
            {"[", 231 },
            {"]", 261 },
            {"^", 271 },
            {"'", 341 },
            {"_", 251 },
            {"%", 252 },
            {"&", 344 },
            {"for", 361 },
            {"while" , 371},
            {"switch" , 451},
            {"default", 351 },
            {"if" , 452},
            {"else", 352 },
            {"do" , 342},
            {"void", 995 },
            {"class", 996 },
            {"define", 997 },
            {"time" , 998},
            {"string", 999 },
            {"stdlib", 992 },
            {"scanf", 840 },
            {"printf" , 841},
            {"math", 842 },
            {"stdio", 843 },
            {"return", 844 },
            {"h", 8442 },
            {"g", 8440 },
            {"G", 8440 },
            {"f", 8441 },
            {"F", 8441 },
            {"e", 8443 },
            {"E", 8443 },
            {"x", 8444 },
            {"X", 8444 },
            {"d", 8445 },
            {"i", 8445 },
            {"c", 8446 },
            {"p", 8447 },
            {"o", 8448 },
            {"u", 8449 },
            {"s", 8450 },
            {"=", 845 }
        };
        public List<Dictionary<string, int>> LlenaLista()
        {
            List<Dictionary<string, int>> LstPalabraDiccionario = new List<Dictionary<string, int>>();
            foreach (var Palabra in PalabrasReservadas)
            {
                LstPalabraDiccionario.Add(PalabrasReservadas);
            }
            return LstPalabraDiccionario;
        }
        public int GetToken(string Lexema)
        {
            foreach (KeyValuePair<string, int> Lex in PalabrasReservadas)
            {
                if (Lexema == Lex.Key)
                {
                    return Lex.Value;
                }
            }
            return 1001;
        }

    }
    public class AnalizadorLexico
    {
        readonly UnidadesLexicas UL = new UnidadesLexicas();
        List<Tokens> LstTokens = new List<Tokens>();
        int Renglon = 1, Cont = 0;
        string Lexema = string.Empty;

        readonly string Digitos = "0123456789";
        readonly string Letras = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyzáéíóúÁÉÍÓÚ";
        readonly string Subguion = "_";
        readonly string UnSoloCaracter = "()[]{},.;:#";
        readonly string DobleCaracter = "*=<>|&!%^";
        readonly string Aritmetico = ".+-";
        readonly string DiagonalInvertida = "\\'";

        protected int EsAlfabetoIdentificadorPalabraReservada(char c)
        {
            if (Letras.Contains(c))
                return 0;
            else if (Digitos.Contains(c))
                return 1;
            else if (Subguion.Contains(c))
                return 2;
            else
                return -1;
        }
        protected void GrIdentificadorPalabraReservada(string Archivo)
        {
            char c;
            int Estado = 0;
            int EstadoFinal = 1;
            int Simbolo;
            string Lexema = string.Empty;

            int[,] TT =
            {
                    {1, -1, 1 },
                    {1, 1, 1 }
                };

            do
            {
                c = Archivo[Cont];
                Simbolo = EsAlfabetoIdentificadorPalabraReservada(c);
                if (Simbolo.Equals(-1) || Estado.Equals(-1))
                {
                    Cont--;
                    break;
                }
                Lexema += c;
                Estado = TT[Estado, Simbolo];
                Cont++;

            } while (true && Archivo.Length > Cont);
            if (EstadoFinal.Equals(Estado))
            {
                LstTokens.Add(new Tokens(Renglon, Lexema, UL.GetToken(Lexema)));
                Lexema = string.Empty;
            }
        }
        protected void GtIdentificadorComentarios(string Archivos)
        {
            char c;
            int Estado = 0;
            int[] EstadoFin = { 4, 5, 6 };
            int Simbolo;
            string Lexema = string.Empty;
            int[,] TT2 =
            {
                    {1,-1,-1,-1 },
                    {5,6,2,-1 },
                    {2,2,3,2 },
                    {4,2,3,2 },
                    {-1,-1,-1,-1 },
                    {5,5,5,5 },
                    {-1,-1,-1,-1}
                };
            do
            {
                c = Archivos[Cont];
                Simbolo = EsIdentificadorComentario(c);
                if (Estado.Equals(1) && Simbolo.Equals(3))
                {
                    Cont--;
                    break;
                }
                else if (Estado.Equals(5) && c.Equals('\n'))
                {
                    Cont++;
                    Renglon++;
                    break;
                }
                else if (Estado.Equals(2) && c.Equals('\n'))
                {
                    Renglon++;
                }
                else if (Estado.Equals(6) || Estado.Equals(4))
                    break;
                if (Simbolo.Equals(-1))
                {
                    Cont--;
                    break;
                }
                Lexema += c;
                Estado = TT2[Estado, Simbolo];
                Cont++;
            } while (true && Archivos.Length > Cont);
            if (EstadoFin.Contains(Estado))
            {
                LstTokens.Add(new Tokens(Renglon, Lexema, 1002));
                Lexema = string.Empty;
            }
            else if (Estado.Equals(1) || Estado.Equals(2) || Estado.Equals(3))
            {
                if (Lexema.Equals("/"))
                {
                    LstTokens.Add(new Tokens(Renglon, Lexema, 221));
                    Lexema = string.Empty;
                }
                else
                {
                    LstTokens.Add(new Tokens(Renglon, Lexema, 1001));
                    Lexema = string.Empty;
                }
            }
        }
        protected int EsIdentificadorComentario(char c)
        {
            if ("/".Contains(c))
                return 0;
            if ("=".Contains(c))
                return 1;
            if ("*".Contains(c))
                return 2;
            if ((Digitos.Contains(c) || Letras.Contains(c) || Subguion.Contains(c) || UnSoloCaracter.Contains(c)))
                return 3;
            if ("\t\0\n ".Contains(c) || "' '".Contains(c))
                return 3;
            else
                return -1;
        }
        protected void GtIdentificadorNumeros(string Archivos)
        {
            char c;
            int Estado = 0;
            int[] EstadoFin = { 1, 4, 5 };
            int Simbolo;
            string Lexema = string.Empty;
            int[,] TT2 =
            {
                    {1,7,8,6,-1},
                    {1,-1,-1,6,2},
                    {4,3,3,-1,-1},
                    {4,-1,-1,-1,-1},
                    {4,-1,-1,-1,-1},
                    {5,-1,-1,-1,2},
                    {5,-1,-1,-1,-1},
                    {1,-1,-1,6,-1},
                    {1,-1,-1,6,-1}
                };
            do
            {
                c = Archivos[Cont];
                Simbolo = EsIdentificadorNumeros(c);
                if (Simbolo.Equals(-1))
                {
                    if (Estado.Equals(7) || Estado.Equals(8))
                    {
                        LstTokens.Add(new Tokens(Renglon, Lexema, UL.GetToken(Lexema)));
                        Lexema = string.Empty;
                        Cont--;
                        break;
                    }
                    else
                    {
                        Cont--;
                        break;

                    }
                }
                if (Simbolo.Equals(1))
                {
                    if (Estado.Equals(7))
                    {
                        Lexema += c;
                        LstTokens.Add(new Tokens(Renglon, Lexema, UL.GetToken(Lexema)));
                        Lexema = string.Empty;
                        break;
                    }
                }
                if (Simbolo.Equals(2))
                {
                    if (Estado.Equals(8))
                    {
                        Lexema += c;
                        LstTokens.Add(new Tokens(Renglon, Lexema, UL.GetToken(Lexema)));
                        Lexema = string.Empty;
                        break;
                    }
                }
                Lexema += c;
                Estado = TT2[Estado, Simbolo];
                Cont++;
            } while (true && Archivos.Length > Cont);
            if (EstadoFin.Contains(Estado))
            {
                LstTokens.Add(new Tokens(Renglon, Lexema, 1003));
                Lexema = string.Empty;
            }
        }
        protected int EsIdentificadorNumeros(char c)
        {
            if (Digitos.Contains(c))
                return 0;
            if ("+".Contains(c))
                return 1;
            if ("-".Contains(c))
                return 2;
            if (".".Contains(c))
                return 3;
            if ("e".Contains(c) || "E".Contains(c))
                return 4;
            else
                return -1;
        }
        protected void GtIdentificadorDosCaracteres(string Archivos)
        {
            char c;
            int Estado = 0;
            int[] EstadoFin = { 1, 2, 3, 4, 5, 7 };
            int Simbolo;
            string Lexema = string.Empty;
            int[,] TT2 =
            {
                    {1,2,3,4,5,5,5,5,6 },
                    {7,-1,-1,-1,-1,-1,-1,-1,-1 },
                    {-1,7,-1,-1,-1,-1,-1,-1,-1 },
                    {7,-1,7,-1,-1,-1,-1,-1,-1 },
                    {7,-1,-1,7,-1,-1,-1,-1,-1 },
                    {7,-1,-1,-1,-1,-1,-1,-1,-1 },
                    {-1,-1,-1,-1,-1,-1,-1,-1,7 },
                    {-1,-1,-1,-1,-1,-1,-1,-1,-1 }
                };
            do
            {
                c = Archivos[Cont];
                Simbolo = EsIdentificadorDosCaracteres(c);
                if (Simbolo.Equals(-1))
                {
                    Cont--;
                    break;
                }
                Lexema += c;
                Estado = TT2[Estado, Simbolo];
                Cont++;
            } while (true && Archivos.Length > Cont);
            if (EstadoFin.Contains(Estado))
            {
                LstTokens.Add(new Tokens(Renglon, Lexema, UL.GetToken(Lexema)));
                Lexema = string.Empty;
            }
        }
        protected int EsIdentificadorDosCaracteres(char c)
        {
            if ("=".Contains(c))
                return 0;
            if ("&".Contains(c))
                return 1;
            if ("<".Contains(c))
                return 2;
            if (">".Contains(c))
                return 3;
            if ("*".Contains(c))
                return 4;
            if ("!".Contains(c))
                return 5;
            if ("^".Contains(c))
                return 6;
            if ("%".Contains(c))
                return 7;
            if ("|".Contains(c))
                return 8;
            else
                return -1;
        }
        protected void GtIdentificadorDiagonalInvertida(string Archivos)
        {
            char c;
            int Estado = 0;
            int[] EstadoFin = { 2, 3, 4, 5, 6, 7, 8, 9 };
            int Simbolo;
            string Lexema = string.Empty;
            int[,] TT2 =
            {
                    {1,-1,-1,-1,-1,-1,-1,-1 },
                    {3,9,2,5,4,6,7,8 },
                    {-1,-1,-1,-1,-1,-1,-1,-1 },
                    {-1,-1,-1,-1,-1,-1,-1,-1},
                    {-1,-1,-1,-1,-1,-1,-1,-1 },
                    {-1,-1,-1,-1,-1,-1,-1,-1 },
                    {-1,-1,-1,-1,-1,-1,-1,-1 },
                    {-1,-1,-1,-1,-1,-1,-1,-1 }
                };
            do
            {
                c = Archivos[Cont];
                Simbolo = EsIdentificadorDiagonalInvertida(c);
                if (Estado.Equals(2))
                {
                    Cont--;
                    break;
                }
                if (Estado.Equals(3))
                {
                    Cont--;
                    break;
                }
                if (Estado.Equals(4))
                {
                    Cont--;
                    break;
                }
                if (Estado.Equals(5))
                {
                    Cont--;
                    break;
                }
                if (Estado.Equals(6))
                {
                    Cont--;
                    break;
                }
                if (Estado.Equals(7))
                {
                    Cont--;
                    break;
                }
                if (Estado.Equals(8))
                {
                    Cont--;
                    break;
                }
                if (Estado.Equals(9))
                {
                    Cont--;
                    break;
                }
                if (Simbolo.Equals(-1) || Estado.Equals(-1))
                {
                    Cont--;
                    break;
                }
                Lexema += c;
                Estado = TT2[Estado, Simbolo];
                Cont++;
            } while (true && Archivos.Length > Cont);
            if (EstadoFin.Contains(Estado))
            {
                LstTokens.Add(new Tokens(Renglon, Lexema, UL.GetToken(Lexema)));
                Lexema = string.Empty;
            }
            else if (Lexema.Contains("\'"))
            {
                LstTokens.Add(new Tokens(Renglon, Lexema, 8));
                Lexema = string.Empty;
            }
        }
        protected int EsIdentificadorDiagonalInvertida(char c)
        {
            if ("\\".Contains(c))
                return 0;
            if ("r".Contains(c) || "R".Contains(c))
                return 1;
            if ("n".Contains(c))
                return 2;
            if ("t".Contains(c))
                return 3;
            if ("0".Contains(c))
                return 4;
            if ("\'".Contains(c))
                return 5;
            if ("\"".Contains(c))
                return 6;
            if ("b".Contains(c) || "B".Contains(c))
                return 7;
            else
                return -1;
        }
        public List<Tokens> AnalisisLexico(string Archivo)
        {
            char c;
            while (Archivo.Length > Cont)
            {
                c = Archivo[Cont];
                if ("\t\0 ".Contains(c) || " ".Contains(c))
                {
                    Cont++;
                }
                else if (c.Equals('\n'))
                {
                    Renglon++;
                    Cont++;
                }
                else if (Letras.Contains(c) || Subguion.Contains(c))
                {
                    GrIdentificadorPalabraReservada(Archivo);
                    Cont++;
                }
                else if (UnSoloCaracter.Contains(c))
                {
                    LstTokens.Add(new Tokens(Renglon, c.ToString(), UL.GetToken(c.ToString())));
                    Cont++;
                }
                else if ("/".Contains(c))
                {
                    GtIdentificadorComentarios(Archivo);
                    Cont++;
                }
                else if (Aritmetico.Contains(c) || Digitos.Contains(c))
                {
                    GtIdentificadorNumeros(Archivo);
                    Cont++;
                }
                else if (DobleCaracter.Contains(c))
                {
                    GtIdentificadorDosCaracteres(Archivo);
                    Cont++;
                }
                else if (DiagonalInvertida.Contains(c))
                {
                    GtIdentificadorDiagonalInvertida(Archivo);
                    Cont++;
                }
                else
                {
                    Lexema += c;
                    LstTokens.Add(new Tokens(Renglon, c.ToString(), UL.GetToken(c.ToString())));
                    Lexema = string.Empty;
                    Cont++;
                }
            }
            return LstTokens;
        }
    }
    public class AnalizadorSintactico
    {
        readonly UnidadesLexicas UL = new UnidadesLexicas();
        List<Tokens> ListaTokens = new List<Tokens>();
        protected string Errores = string.Empty;


        protected int Cont = 0;
        protected int ContC = 0;
        protected int ContF = 0;
        protected int Token;
        protected int MainC = 0;
        protected int ReturnVC = 0;
        protected int ReturnMC = 0;
        protected int Columnas = 0;
        protected int While = 0;
        protected int DefaultC = 0;
        protected int ContPf = 0;
        protected int Filas = 0;
        protected int ContParentesis = 0;


        protected bool Char = false;
        protected bool CharF = false;
        protected bool Void = false;
        protected bool LlamadaFuncion = false;
        protected bool Brake = false;
        protected bool Asignacion = false;
        public bool Compilacion = true;


        protected int[] Variables = { 8440, 8441, 8442, 8443, 8444, 8445, 8446, 8447, 8448, 8449, 8450, 1001 };
        protected int[] Incremento = { 24, 30, 31, 34 };
        protected int[] IncrementoC = { 32, 33 };
        protected int[] Aritmetico = { 112, 113, 121, 221, 252 };
        protected int[] Tipo_Datos = { 4, 114, 14, 15 };
        protected int[] Signos = { 112, 113 };


        void init()
        {
            ReturnMC = 0;
            ReturnVC = 0;
        }
        bool EsToken(int T)
        {
            if (ListaTokens[Cont].Token == T)
                return true;
            return false;
        }
        string GetLexema()
        {
            return ListaTokens[Cont].Lexema;
        }
        int GetToken()
        {
            return ListaTokens[Cont].Token;
        }
        int GetTokenSig()
        {
            return ListaTokens[Cont + 1].Token;
        }
        bool EsTokenSig(int T)
        {
            if (ListaTokens[Cont + 1].Token == T)
                return true;
            return false;
        }
        bool GrMatriz()
        {
            Cont++;
            if (Char)
            {
                if (EsToken(22))
                {
                    while (Cont < ListaTokens.Count())
                    {
                        if (EsToken(22))
                        {
                            while (Cont < ListaTokens.Count())
                            {
                                Cont++;
                                if (EsToken(8))
                                {
                                    Cont++;
                                    if (ListaTokens[Cont].Lexema.Length == 1)
                                    {
                                        Cont++;
                                        if (EsToken(8))
                                        {
                                            ContC++;
                                            Cont++;
                                            if (!EsToken(38))
                                            {
                                                if (Columnas != 0)
                                                {
                                                    if (ContC <= Columnas)
                                                    {
                                                        ContC = 0;
                                                        break;  // = {{'H','o','l','a'},{'H','o','l','a'}}
                                                    }
                                                    else
                                                    {
                                                        Errores = "Desbordamiento de matriz, línea: " + ListaTokens[Cont].Linea;
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            ContF++;
                            if (EsToken(23))
                            {
                                Cont++;
                                if (EsToken(23))
                                {
                                    Cont++;
                                    if (EsToken(45))
                                    {
                                        if (Filas != 0)
                                        {
                                            if (ContF <= Filas)
                                            {
                                                ContC = 0;
                                                ContF = 0;
                                                return true;
                                            }
                                            else
                                            {
                                                Errores = "Desbordamiento de matriz, línea: " + ListaTokens[Cont].Linea;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            ContC = 0;
                                            ContF = 0;
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        Errores = "Sintaxis incorrecta de asignación de matriz, línea: " + ListaTokens[Cont].Linea;
                                        return false;
                                    }

                                }
                                else if (!EsToken(38))
                                {
                                    Errores = "Sintaxis incorrecta de asignación de matriz, línea: " + ListaTokens[Cont].Linea;
                                    return false;
                                }
                            }
                            else
                            {
                                Errores = "Sintaxis incorrecta de asignación de matriz, línea: " + ListaTokens[Cont].Linea;
                                return false;
                            }
                            Cont++;
                        }
                    }
                }
            }
            else
            {
                if (EsToken(22))
                {
                    while (Cont < ListaTokens.Count())
                    {
                        if (EsToken(22))
                        {
                            while (Cont < ListaTokens.Count())
                            {
                                Cont++;
                                if (EsToken(1003))
                                {
                                    ContC++;
                                    Cont++;
                                    if (!EsToken(38))
                                    {
                                        if (Columnas != 0)
                                        {
                                            if (ContC <= Columnas)
                                            {
                                                ContC = 0;
                                                break;  // = {{1,2,3,4},{1,2,3,4},{1,2,3,4},{1,2,3,4}}
                                            }
                                            else
                                            {
                                                Errores = "Desbordamiento de matriz, línea: " + ListaTokens[Cont].Linea;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            ContF++;
                            if (EsToken(23))
                            {
                                Cont++;
                                if (EsToken(23))
                                {
                                    Cont++;
                                    if (EsToken(45))
                                    {
                                        if (Filas != 0)
                                        {
                                            if (ContF <= Filas)
                                            {
                                                ContC = 0;
                                                ContF = 0;
                                                return true;
                                            }
                                            else
                                            {
                                                Errores = "Desbordamiento de matriz, línea: " + ListaTokens[Cont].Linea;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            ContC = 0;
                                            ContF = 0;
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        Errores = "Sintaxis incorrecta de asignación de matriz, línea: " + ListaTokens[Cont].Linea;
                                        return false;
                                    }
                                }
                                else if (!EsToken(38))
                                {
                                    Errores = "Sintaxis incorrecta de asignación de matriz, línea: " + ListaTokens[Cont].Linea;
                                    return false;
                                }
                            }
                            else
                            {
                                Errores = "Sintaxis incorrecta de asignación de matriz, línea: " + ListaTokens[Cont].Linea;
                                return false;
                            }
                            Cont++;
                        }
                    }
                }
            }
            if (Variables.Contains(GetToken()))
            {
                Cont++;
                if (EsToken(231))
                {
                    return IdentMatrixVect();
                }
                else if (EsToken(35))
                {
                    return GrFuncion();
                }
                else if (EsToken(45))
                {
                    Asignacion = false;
                    return true;
                }
            }
            else if (EsToken(1003))
            {
                if (EsToken(45))
                {
                    Asignacion = false;
                    return true;
                }
            }
            Errores = "Sintaxis incorrecta de asignación de matriz, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrVector()
        {
            Cont++;
            if (Char)
            {
                if (EsToken(18))
                {
                    if (GrComillas())
                    {
                        Cont++;
                        if (EsToken(45))
                        {
                            if (Filas != 0)
                            {
                                if (ContC <= Filas)
                                {
                                    ContC = 0;
                                    return true; // = "Hola"
                                }
                                else
                                {
                                    Errores = "Desbordamiento de vector, línea: " + ListaTokens[Cont].Linea;
                                    return false;
                                }
                            }
                            else
                            {
                                ContC = 0;
                                return true;
                            }
                        }
                        else
                        {
                            Errores = "Sintaxis incorrecta de asignación de vector, línea: " + ListaTokens[Cont].Linea;
                            return false;
                        }
                    }
                }
                else if (EsToken(22))
                {
                    while (Cont < ListaTokens.Count())
                    {
                        Cont++;
                        if (EsToken(8))
                        {
                            Cont++;
                            if (ListaTokens[Cont].Lexema.Length == 1)
                            {
                                Cont++;
                                if (EsToken(8))
                                {
                                    ContC++;
                                    Cont++;
                                    if (!EsToken(38))
                                    {
                                        break;  // = {'H','o','l','a'}
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (EsToken(23))
                    {
                        Cont++;
                        if (EsToken(45))
                        {
                            if (Filas != 0)
                            {
                                if (ContC <= Filas)
                                {
                                    ContC = 0;
                                    return true;
                                }
                                else
                                {
                                    Errores = "Desbordamiento de vector, línea: " + ListaTokens[Cont].Linea;
                                    return false;
                                }
                            }
                            else
                            {
                                ContC = 0;
                                return true;
                            }
                        }
                        else
                        {
                            Errores = "Sintaxis incorrecta de asignación de vector, línea: " + ListaTokens[Cont].Linea;
                            return false;
                        }
                    }
                }
            }
            else if (EsToken(22))
            {
                while (Cont < ListaTokens.Count())
                {
                    Cont++;
                    if (EsToken(1003))
                    {

                        Cont++;
                        if (!EsToken(38))
                        {
                            break;  // = {1,2,3,4}
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (EsToken(23))
                {
                    Cont++;
                    if (EsToken(45))
                    {
                        if (Filas != 0)
                        {
                            if (ContC <= Filas)
                            {
                                ContC = 0;
                                return true;
                            }
                            else
                            {
                                Errores = "Desbordamiento de vector, línea: " + ListaTokens[Cont].Linea;
                                return false;
                            }
                        }
                        else
                        {
                            ContC = 0;
                            return true;
                        }
                    }
                    else
                    {
                        Errores = "Sintaxis incorrecta de asignación de vector, línea: " + ListaTokens[Cont].Linea;
                        return false;
                    }
                }
            }
            else if (Variables.Contains(GetToken()))
            {
                Cont++;
                if (EsToken(231))
                {
                    return IdentMatrixVect();
                }
                else if (EsToken(35))
                {
                    return GrFuncion();
                }
                else if (EsToken(45))
                {
                    Asignacion = false;
                    return true;
                }
            }
            else if (EsToken(1003))
            {
                if (EsToken(45))
                {
                    Asignacion = false;
                    return true;
                }
            }
            Errores = "Sintaxis incorrecta de asignación de vector, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool IdentMatrixVect()
        {
            Cont++;
            if (EsToken(261))
            {
                Cont++;
                if (EsToken(231))
                {
                    Cont++;
                    if (EsToken(261))
                    {
                        Cont++;
                        if (EsToken(45))
                        {
                            Filas = 0;
                            Columnas = 0;
                            return true;
                        }
                        else if (EsToken(38))
                        {
                            Filas = 0;
                            Columnas = 0;
                            return true;
                        }
                        else if (EsToken(845))
                        {
                            if (!Asignacion)
                            {
                                return GrMatriz();
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (EsToken(45))
                {
                    Filas = 0;
                    return true;
                }
                else if (EsToken(845))
                {
                    if (!Asignacion)
                    {
                        return GrVector();
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (EsToken(38))
                {
                    Filas = 0;
                    return true;
                }
            }
            else if (EsToken(1003))
            {
                try
                {
                    Filas = int.Parse(ListaTokens[Cont].Lexema);
                    Cont++;
                    if (EsToken(261))
                    {
                        Cont++;
                        if (EsToken(231))
                        {
                            Cont++;
                            if (EsToken(1003))
                            {
                                try
                                {
                                    Columnas = int.Parse(ListaTokens[Cont].Lexema);
                                    Cont++;
                                    if (EsToken(261))
                                    {
                                        Cont++;
                                        if (EsToken(45))
                                        {
                                            Columnas = 0;
                                            Filas = 0;
                                            return true;
                                        }
                                        else if (EsToken(38))
                                        {
                                            Columnas = 0;
                                            Filas = 0;
                                            return true;
                                        }
                                        else if (EsToken(845))
                                        {
                                            return GrMatriz();
                                        }
                                        else if (EsToken(36))
                                        {
                                            Columnas = 0;
                                            Filas = 0;
                                            return true;
                                        }
                                        else if (Aritmetico.Contains(GetToken()))
                                        {
                                            Filas = 0;
                                            Columnas = 0;
                                            return true;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Errores = "Tipo de dato ingresado no compatible, línea: " + ListaTokens[Cont].Linea + "\n    " + e;
                                    return false;
                                }
                            }
                        }
                        else if (EsToken(45))
                        {
                            Filas = 0;
                            return true;
                        }
                        else if (EsToken(845))
                        {
                            return GrVector();
                        }
                        else if (EsToken(38))
                        {
                            Filas = 0;
                            return true;
                        }
                        else if (EsToken(36))
                        {
                            Filas = 0;
                            return true;
                        }
                        else if (Aritmetico.Contains(GetToken()))
                        {
                            Filas = 0;
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Errores = "Tipo de dato ingresado no compatible, línea: " + ListaTokens[Cont].Linea + "\n    " + e;
                    return false;
                }
            }
            else if (Variables.Contains(GetToken()))
            {
                Cont++;
                if (EsToken(261))
                {
                    Cont++;
                    if (EsToken(231))
                    {
                        Cont++;
                        if (Variables.Contains(GetToken()))
                        {
                            Cont++;
                            if (EsToken(261))
                            {
                                Cont++;
                                if (EsToken(845))
                                {
                                    Filas = 0;
                                    Columnas = 0;
                                    Cont--;
                                    return GrAritmetico();
                                }
                                else
                                {
                                    Filas = 0;
                                    Columnas = 0;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (EsToken(845))
                        {
                            Filas = 0;
                            Cont--;
                            return GrAritmetico();
                        }
                        else
                        {
                            Filas = 0;
                            return true;
                        }
                    }
                }
            }
            Errores = "Sintaxis incorrecta de declaración, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrComilla()
        {
            //ContC = 0;
            if (EsToken(8))
            {
                Cont++;
                if (GetLexema().Length == 1)
                {
                    Cont++;
                    if (EsToken(8))
                        return true;
                }
                else
                {
                    Errores = "Desbordamiento de carácter char, línea: " + ListaTokens[Cont].Linea;
                    return false;
                }
            }
            Errores = "Sintaxis incorrecta de comilla simple, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrComillas()
        {
            if (EsToken(18))
            {
                Cont++;
                while (Cont < ListaTokens.Count)
                {
                    ContC += ListaTokens[Cont].Lexema.Length;
                    Cont++;
                    if (EsToken(18))
                        return true;
                }
            }
            Errores = "Sintaxis incorrecta de comillas, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrAsignacion()
        {
            Asignacion = false;
            if (EsToken(4))
            {
                Char = true;
                Cont++;
                if (Variables.Contains(GetToken()))
                {
                    Cont++;
                    if (EsToken(845))
                    {
                        Cont++;
                        if (EsToken(8))
                        {
                            Cont++;
                            if (GetLexema().Length == 1)
                            {
                                Cont++;
                                if (EsToken(8))
                                {
                                    Cont++;
                                    if (EsToken(45))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else if (EsToken(45))
                    {
                        return true;
                    }
                    else if (EsToken(231))
                    {
                        return IdentMatrixVect();
                    }
                }
            }
            else
            {
                Char = false;
                Cont++;
                if (Variables.Contains(GetToken()))
                {
                    Cont++;
                    if (EsToken(845))
                    {
                        Cont--;
                        return GrAritmetico();
                    }
                    else if (EsToken(45))
                    {
                        return true;
                    }
                    else if (EsToken(231))
                    {
                        return IdentMatrixVect();
                    }
                }
            }
            Errores = "Sintaxis incorrecta de asignación, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrCast()
        {
            Cont++;
            if (EsToken(36))
            {
                Cont++;
                if (Variables.Contains(GetToken()))
                {
                    if (EsTokenSig(35))
                    {
                        LlamadaFuncion = true;
                        if(GrFuncion()){
                            Cont--;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (EsTokenSig(231))
                    {
                        Cont++;
                        return IdentMatrixVect();
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool GrAuxAritmetico()
        {
            if (EsToken(35))
            {
                Cont++;
                while (Cont < ListaTokens.Count())
                {
                    if (EsToken(35))
                    {
                        Cont++;
                        if (Tipo_Datos.Contains(GetToken()))
                        {
                            if (GrCast())
                            {
                                Cont++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            return GrAuxAritmetico();
                        }
                    }
                    else if (Variables.Contains(GetToken()) || EsToken(1003))
                    {
                        Cont++;
                        if (Aritmetico.Contains(GetToken()))
                        {
                            Cont++;
                            if (EsToken(1003) || Variables.Contains(GetToken()))
                            {
                                Cont++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (EsToken(36))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool GrAritmetico()
        {
            Cont++;
            if (EsToken(845))
            {
                Cont++;
                if (Variables.Contains(GetToken()) || EsToken(1003) || EsToken(35))
                {
                    while (Cont < ListaTokens.Count())
                    {
                        if (EsToken(1003))
                        {
                            if (Variables.Contains(GetTokenSig()) || EsTokenSig(1003))
                            {
                                break;
                            }
                            else
                            {
                                Cont++;
                            }
                        }
                        else if (Variables.Contains(GetToken()))
                        {
                            Cont++;
                            if (Variables.Contains(GetToken()) || EsToken(1003))
                            {
                                break;
                            }
                            else if (EsToken(231))
                            {
                                if (IdentMatrixVect())
                                {
                                    if (EsToken(45))
                                    {
                                        return true;
                                    }
                                    else if(EsToken(261))
                                    {
                                        Cont++;
                                    }
                                }
                            }
                            else if (EsToken(35))
                            {
                                LlamadaFuncion = true;
                                Cont--;
                                if (GrFuncion())
                                {
                                    Cont++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (EsToken(35))
                        {
                            Cont++;
                            if (Tipo_Datos.Contains(GetToken()))
                            {
                                if (GrCast())
                                {
                                    Cont++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (EsToken(36))
                                {
                                    Cont++;
                                }
                                else
                                {
                                    ContParentesis+=1;
                                    while (Cont < ListaTokens.Count() && ContParentesis != 0)
                                    {
                                        if (EsToken(1003))
                                        {
                                            Cont++;
                                            if (Aritmetico.Contains(GetToken()))
                                            {
                                                if (!EsTokenSig(1003) && !Variables.Contains(GetTokenSig()) && !EsTokenSig(35))
                                                {
                                                    break;
                                                }
                                                Cont++;
                                            }
                                            else if (!EsToken(36))
                                            {
                                                break;
                                            }
                                        }
                                        else if (Aritmetico.Contains(GetToken()))
                                        {
                                            Cont++;
                                        }
                                        else if (Variables.Contains(GetToken()))
                                        {
                                            Cont++;
                                            if (Aritmetico.Contains(GetToken()))
                                            {
                                                if (!EsTokenSig(1003) && !Variables.Contains(GetTokenSig()) && !EsTokenSig(35))
                                                {
                                                    break;
                                                }
                                                Cont++;
                                            }
                                            else if (EsToken(231))
                                            {
                                                if (IdentMatrixVect())
                                                {
                                                    if (EsToken(45))
                                                    {
                                                        return true;
                                                    }
                                                    else if (EsToken(261))
                                                    {
                                                        Cont++;
                                                    }
                                                }
                                                else if (!EsToken(36))
                                                {
                                                    break;
                                                }
                                            }
                                            else if (EsToken(35))
                                            {
                                                LlamadaFuncion = true;
                                                Cont--;
                                                if (GrFuncion())
                                                {
                                                    //Cont++;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        else if (EsToken(35))
                                        {
                                            if (Tipo_Datos.Contains(GetTokenSig()))
                                            {
                                                Cont++;
                                                if (GrCast())
                                                {
                                                    Cont++;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                Cont++; ContParentesis++;
                                            }
                                        }
                                        else if (EsToken(36))
                                        {
                                            Cont++; ContParentesis--;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (EsToken(45))
                        {
                            return true;
                        }
                        else if (Aritmetico.Contains(GetToken()))
                        {
                            Cont++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else if (Incremento.Contains(GetToken()))
            {
                Cont++;
                if (Variables.Contains(GetToken()) || EsToken(1003))
                {
                    Cont++;
                    if (EsToken(45))
                    {
                        return true; // i += 5;
                    }
                }
            }
            else if (IncrementoC.Contains(GetToken()))
            {
                Cont++;
                if (EsToken(45))
                {
                    return true; // i++;
                }
            }
            else if (EsToken(231))
            {
                Asignacion = true;
                Char = false;
                return IdentMatrixVect(); // i[] = ?; o i[][] = ?;
            }
            else if (EsToken(35))
            {
                LlamadaFuncion = true;
                return GrFuncion();
            }
            Errores = "Sintaxis incorrecta en asignación, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrReturn()
        {
            if (EsToken(45))
            {
                return true;
            }
            else if (Variables.Contains(GetToken()))
            {
                Cont++;
                if (EsToken(45))
                {
                    ReturnVC++;
                    return true;
                }
                else if (EsToken(35))
                {
                    ReturnVC++;
                    Cont--; return GrFuncion();
                }
            }
            else if (EsToken(1003))
            {
                if (!CharF)
                {
                    if (GetLexema().Equals("0"))
                    {
                        Cont++;
                        if (EsToken(45))
                        {
                            ReturnMC++;
                            return true;
                        }
                    }
                    else
                    {
                        Cont++;
                        if (EsToken(45))
                        {
                            ReturnVC++;
                            return true;
                        }
                    }
                }
            }
            else if (EsToken(18))
            {
                if (CharF)
                {
                    if (GrComillas())
                    {
                        Cont++;
                        if (EsToken(45))
                        {
                            ReturnVC++;
                            return true;
                        }
                    }
                }
            }
            else if (EsToken(8))
            {
                if (CharF)
                {
                    if (GrComilla())
                    {
                        Cont++;
                        if (EsToken(45))
                        {
                            ReturnVC++;
                            return true;
                        }
                    }
                }
            }
            Errores = "Sintaxis incorrecta en el comando ( return ), línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrLibreria()
        {
            if (EsToken(398) || EsToken(842) || EsToken(843) || EsToken(992) || EsToken(999) || EsToken(998))
            {
                Cont++;
                if (EsToken(37))
                {
                    Cont++;
                    if (EsToken(8442))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool GrGato()
        {
            Cont++;
            if (EsToken(997)) //define
            {
                Cont++;
                if (EsToken(1001))
                {
                    Cont++;
                    if (EsToken(1003))
                    {
                        return true;
                    }
                }
            }
            else if (EsToken(345)) //include
            {
                Cont++;
                if (EsToken(26))
                {
                    Cont++;
                    if (GrLibreria())
                    {
                        Cont++;
                        if (EsToken(27))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        Errores = "Sintaxis incorrecta: librería desconocida o mal escrita, línea: " + ListaTokens[Cont].Linea;
                        return false;
                    }
                }
            }
            Errores = "Sintaxis incorrecta del operador ( # ), línea: " + ListaTokens[Cont].Linea;
            return false;
        }

        //Gramaticas de C++
        /*bool GrNameSpace()
        {
            Cont++;
            if (EsToken(999))
            {
                Cont++;
                if (EsToken(992))
                {
                    Cont++;
                    if (EsToken(45))
                    {
                        return true;
                    }
                }
            }
            Errores = "Sintaxis incorrecta del comando Using, línea: " + ListaTokens[Cont].Linea;
            return false;
        }*/
        /*bool GrCout()
        {
            Cont++;
            if (EsToken(16))
            {
                Cont++;
                if (EsToken(1001))
                {
                    Cont++;
                    if (EsToken(45))
                        return true;
                    if (EsToken(16))
                        Cont--; return GrCout();
                }
                else if (EsToken(18))
                {
                    Cont++;
                    while (!EsToken(18) && Cont < ListaTokens.Count)
                    {
                        Cont++;
                    }
                    Cont++;
                    if (EsToken(45))
                        return true;
                    if (EsToken(16))
                        Cont--; return GrCout();
                }
                else if (EsToken(843))
                {
                    Cont++;
                    if (EsToken(45))
                        return true;
                    if (EsToken(16))
                        Cont--; return GrCout();
                }
            }
            Errores = "Sintaxis incorrecta de línea Cout, línea: " + ListaTokens[Cont - 1].Linea;
            return false;
        }*/
        /*bool GrCin()
        {
            Cont++;
            if (EsToken(17))
            {
                Cont++;
                if (EsToken(1001))
                {
                    Cont++;
                    while (!EsToken(45) && Cont < ListaTokens.Count)
                    {
                        if (EsToken(17))
                        {
                            Cont++;
                            if (!EsToken(1001))
                            {
                                Errores = "Sintaxis incorrecta de línea Cin, línea: " + ListaTokens[Cont - 1].Linea;
                                return false;
                            }
                        }
                        else
                        {
                            Errores = "Sintaxis incorrecta de línea Cin, línea: " + ListaTokens[Cont - 1].Linea;
                            return false;
                        }
                        Cont++;
                    }
                    return true;
                }
            }
            Errores = "Sintaxis incorrecta de línea Cin, línea: " + ListaTokens[Cont - 1].Linea;
            return false;
        }*/

        bool GrScanf()
        {
            Cont++;
            if (EsToken(35))
            {
                Cont++;
                if (GrComillasf(0))
                {
                    Cont++;
                    if (ContPf != 0)
                    {
                        if (GrParametrosf(0))
                        {
                            Cont++;
                            if (EsToken(45))
                            {
                                ContPf = 0;
                                return true;// scanf("%d %i", &15, &d1);
                            }
                        }
                    }
                }
            }
            Errores = "Sintaxis incorrecta en el comando Scanf, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrPrintf()
        {
            Cont++;
            if (EsToken(35))
            {
                Cont++;
                if (GrComillasf(1))
                {
                    Cont++;
                    if (ContPf != 0)
                    {
                        if (GrParametrosf(1))
                        {
                            Cont++;
                            if (EsToken(45))
                            {
                                ContPf = 0;
                                return true;// printf("%d %i", 15, d1);
                            }
                        }
                    }
                    else
                    {
                        if (EsToken(36))
                        {
                            Cont++;
                            if (EsToken(45))
                            {
                                return true; // printf("hola");
                            }
                        }
                    }
                }
            }
            Errores = "Sintaxis incorrecta en el comando Printf, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrComillasf(int op)
        {
            if (EsToken(18))
            {
                Cont++;
                while (Cont < ListaTokens.Count)
                {
                    if (EsToken(252))
                    {
                        Cont++;
                        switch (ListaTokens[Cont].Token)
                        {
                            case 8440:
                            case 8441:
                            case 8443:
                            case 8444:
                            case 8445:
                            case 8446:
                            case 8447:
                            case 8448:
                            case 8449:
                            case 8450: Cont++; ContPf++; break;
                            default: Errores = "Sintaxis incorrecta de operador en comillas, línea: " + ListaTokens[Cont].Linea; return false;
                        }
                    }
                    else if (EsToken(18))
                        return true;
                    else
                    {
                        if (op == 0)
                        {
                            Errores = "Sintaxis incorrecta de operador en comillas, línea: " + ListaTokens[Cont].Linea;
                            return false;
                        }
                        else
                        {
                            Cont++;
                        }
                    }
                }
            }
            Errores = "Sintaxis incorrecta de comillas, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrParametrosf(int op)
        {
            if (op != 0)
            {
                if (EsToken(38))
                {
                    Cont++;
                    while (Cont < ListaTokens.Count)
                    {
                        if (Variables.Contains(GetToken()) || EsToken(1003))
                        {
                            ContPf--;
                            Cont++;
                        }
                        else if (EsToken(36))
                            break;
                        else if (EsToken(38))
                            Cont++;
                        else
                            break;
                    }
                    if (EsToken(36) && ContPf == 0)
                        return true;
                }
            }
            else
            {
                if (EsToken(38))
                {
                    Cont++;
                    while (Cont < ListaTokens.Count)
                    {
                        if (EsToken(344))
                        {
                            Cont++;
                            if (Variables.Contains(GetToken()) || EsToken(1003))
                            {
                                ContPf--;
                                Cont++;
                            }
                        }
                        else if (EsToken(36))
                            break;
                        else if (EsToken(38))
                            Cont++;
                        else
                            break;
                    }
                    if (EsToken(36) && ContPf == 0)
                        return true;
                }
            }
            Errores = "Sintaxis incorrecta de parametros, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrBloqueLineas()
        {
            Cont++;
            do
            {
                Token = ListaTokens[Cont].Token;
                switch (Token)
                {
                    case 23: return true;
                    case 4:
                    case 114:
                    case 14:
                    case 15:
                        if (GrAsignacion())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 451:
                        if (GrSwitch())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 840:
                        if (GrScanf())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 841:
                        if (GrPrintf())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 361:
                        if (GrFor())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 342:
                        if (GrDo())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 371:
                        if (GrWhile())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 452:
                        if (GrIf())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 844:
                        Cont++;
                        if (GrReturn())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 1002:
                        if (GrComentarios())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            return false;
                        }
                    case 1003:
                    case 1001:
                    default:
                        if (GrAritmetico())
                        {
                            Cont++; break;
                        }
                        else
                        {
                            Errores = "Lexema no válido ( " + GetLexema() + " ), línea: " + ListaTokens[Cont].Linea; Compilacion = false; break;
                        }
                }
            } while (Cont < ListaTokens.Count && Compilacion);
            if (Token != 23)
            {
                Errores = "Falta cierre de llave, línea: " + ListaTokens[Cont - 1].Linea;
                return false;
            }
            else
            {
                return true;
            }
        }
        bool GrLinea()
        {
            bool ok = true;
            Token = ListaTokens[Cont].Token;
            switch (Token)
            {
                case 23: ok = false; break;
                case 4:
                case 114:
                case 14:
                case 15: return GrAsignacion();
                case 840: return GrScanf();
                case 841: return GrPrintf();
                case 844: Cont++; return GrReturn();
                case 1002: return GrComentarios();
                case 1003:
                case 1001:
                default:
                    if (GrAritmetico())
                    {
                        return true;
                    }
                    else
                    {
                        Errores = "Lexema no válido ( " + GetLexema() + " ), línea: " + ListaTokens[Cont].Linea; return false;
                    }
            }
            if (!ok)
            {
                Errores = "Sintaxis incorrecta de línea, línea: " + ListaTokens[Cont - 1].Linea;
                return false;
            }
            return ok;
        }
        bool GrMain()
        {
            if (MainC == 1)
            {
                Cont++;
                if (EsToken(35) && EsTokenSig(36))
                {
                    Cont += 2;
                    if (EsToken(22))
                    {
                        if (GrBloqueLineas())
                        {
                            if (ReturnMC == 1)
                            {
                                init();
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    Errores = "Falta apertura de parámetros en la función main, línea: " + ListaTokens[Cont].Linea;
                    return false;
                }
            }
            else
            {
                Errores = "No puede haber más de una función main declarada, línea: " + ListaTokens[Cont].Linea;
                return false;
            }
            Errores = "Sintaxis incorrecta en la función main, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrParametros()
        {
            Cont++;
            if (EsToken(36))
            {
                return true; //no tiene parametros
            }
            else
            {
                if (!LlamadaFuncion)
                {
                    do
                    {
                        if (Tipo_Datos.Contains(GetToken()))
                        {
                            Cont++;
                            if (EsToken(1001))
                            {
                                Cont++;
                                if (EsToken(231))
                                {
                                    if (IdentMatrixVect())
                                    {
                                        if (!EsToken(38))
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else if (!EsToken(38))
                                {
                                    break;
                                }
                                Cont++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else if (EsToken(1003) || EsToken(1001))
                        {
                            Cont++;
                            if (!EsToken(38))
                            {
                                break;
                            }
                            Cont++;
                        }
                        else
                        {
                            break;
                        }
                    } while (Cont < ListaTokens.Count());
                }
                else
                {
                    do
                    {
                        if (Variables.Contains(GetToken()))
                        {
                            Cont++;
                            if (EsToken(231))
                            {
                                if (IdentMatrixVect())
                                {
                                    if (!EsToken(38))
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (!EsToken(38))
                            {
                                break;
                            }
                            Cont++;
                        }
                        else if (EsToken(1003))
                        {
                            Cont++;
                            if (!EsToken(38))
                            {
                                break;
                            }
                            Cont++;
                        }
                        else
                        {
                            break;
                        }
                    } while (Cont < ListaTokens.Count());
                }
                if (EsToken(36))
                {
                    LlamadaFuncion = false;
                    return true;
                }
            }
            Errores = "Sintaxis incorrecta de los parametros, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrFuncion()
        {
            Cont++;
            if (EsToken(35))
            {
                if (GrParametros())
                {
                    Cont++;
                    if (EsToken(45))
                    {
                        return true; //Es una declaracion de prototipo
                    }
                    else if (EsToken(22))
                    {
                        if (GrBloqueLineas())
                        {
                            if (Void)
                            {
                                if (ReturnVC == 0)
                                {
                                    init();
                                    return true; //funcion completa
                                }
                            }
                            else
                            {
                                if (ReturnVC >= 1 || ReturnMC >= 1)
                                {
                                    init();
                                    return true; //funcion completa
                                }
                            }
                        }
                    }
                    else if (EsToken(36))
                    {
                        return true;
                    }
                }
            }
            Errores = "Sintaxis incorrecta de la funcion, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrCondicionales()
        {
            Cont++;
            if (Variables.Contains(GetToken()) || EsToken(1003))
            {
                Cont++;
                if (EsToken(19) || EsToken(26) || EsToken(27) || EsToken(28) || EsToken(29))
                {
                    Cont++;
                    if (Variables.Contains(GetToken()) || EsToken(1003))
                    {
                        Cont++;
                        if (EsToken(36) || EsToken(45))
                        {
                            return true; // condicional simple
                        }
                        else
                        {
                            if (EsToken(5) || EsToken(6))
                                return GrCondicionales(); // condicional multiple
                        }
                    }
                }
            }
            Errores = "Sintaxis incorrecta en una condicion, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrIf()
        {
            Cont++;
            if (EsToken(35))
            {
                if (GrCondicionales())
                {
                    Cont++;
                    if (EsToken(22))
                    {
                        if (GrBloqueLineas())
                        {
                            if (EsTokenSig(352))
                            {
                                Cont += 2;
                                if (EsToken(22))
                                {
                                    return GrBloqueLineas();//if con else
                                }
                                else
                                {
                                    return GrIf(); //else if
                                }
                            }
                            else
                            {
                                return true; //if de bloque de sentencias
                            }
                        }
                    }
                    else
                    {
                        return GrLinea(); //if de una sentencia
                    }

                }
            }
            Errores = "Sintaxis incorrecta del comando if, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrIncremento()
        {
            Cont++;
            if (Variables.Contains(GetToken()))
            {
                Cont++;
                if (Incremento.Contains(GetToken()))
                {
                    Cont++;
                    if (Variables.Contains(GetToken()) || EsToken(1003))
                    {
                        Cont++;
                        if (EsToken(45) || EsToken(36))
                        {
                            return true; // i += 5;
                        }
                    }
                }
                else if (IncrementoC.Contains(GetToken()))
                {
                    Cont++;
                    if (EsToken(45) || EsToken(36))
                    {
                        return true; // i++;
                    }
                }
            }
            Errores = "Sintaxis incorrecta de la sentencia incremento, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrFor()
        {
            Cont++;
            if (EsToken(35))
            {
                Cont++;
                if (Variables.Contains(GetToken()))
                {
                    if (GrAritmetico())
                    {
                        if (GrCondicionales())
                        {
                            if (GrIncremento())
                            {
                                Cont++;
                                if (EsToken(22))
                                {
                                    return GrBloqueLineas();
                                }
                            }
                        }
                    }
                }
                else if (EsToken(15))
                {
                    if (GrAsignacion())
                    {
                        if (GrCondicionales())
                        {
                            if (GrIncremento())
                            {
                                Cont++;
                                if (EsToken(22))
                                {
                                    return GrBloqueLineas();
                                }
                            }
                        }
                    }
                }
            }
            Errores = "Sintaxis incorrecta en el ciclo for, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrDo()
        {
            Cont++;
            if (EsToken(22))
            {
                if (GrBloqueLineas())
                {
                    Cont++;
                    return GrWhile();
                }
                else
                {
                    Errores = "Sintaxis incorrecta en el ciclo do-while, línea: " + ListaTokens[Cont].Linea;
                    return false;
                }
            }
            Errores = "Sintaxis incorrecta en el ciclo do-while, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrWhile()
        {
            Cont++;
            if (EsToken(35))
            {
                if (GrCondicionales())
                {
                    Cont++;
                    if (EsToken(22))
                    {
                        return GrBloqueLineas();
                    }
                    else if (EsToken(45))
                    {
                        return true;
                    }
                }
            }
            Errores = "Sintaxis incorrecta en ciclo while, línea: " + ListaTokens[Cont - 1].Linea;
            return false;
        }
        bool GrCase()
        {
            if (EsToken(40))
            {
                Cont++;
                do
                {
                    Token = ListaTokens[Cont].Token;
                    switch (Token)
                    {
                        case 23: return false;
                        case 45: Brake = false; return true;
                        case 3: Brake = false; return true;
                        case 351: Brake = false; return true;
                        case 2:
                            Cont++;
                            if (EsToken(45))
                            {
                                Brake = true;
                                Cont++;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case 4:
                        case 114:
                        case 14:
                        case 15:
                            if (GrAsignacion())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 840:
                            if (GrScanf())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 841:
                            if (GrPrintf())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 361:
                            if (GrFor())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 342:
                            if (GrDo())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 371:
                            if (GrWhile())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 452:
                            if (GrIf())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 844:
                            Cont++;
                            if (GrReturn())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 1002:
                            if (GrComentarios())
                            {
                                Cont++; break;
                            }
                            else
                            {
                                return false;
                            }
                        case 1003:
                        case 1001:
                        default: /*Errores = "Lexema no válido ( " + ListaTokens[Cont].Lexema + " ), línea: " + ListaTokens[Cont].Linea; Compilacion = false;*/ Cont++; break;
                    }
                } while (Cont < ListaTokens.Count && Compilacion);
            }
            return false;
        }
        bool GrSwitch()
        {
            Cont++;
            if (EsToken(35))
            {
                Cont++;
                if (Variables.Contains(GetToken()))
                {
                    Cont++;
                    if (EsToken(36))
                    {
                        Cont++;
                        if (EsToken(22))
                        {
                            Cont++;
                            while (Cont < ListaTokens.Count())
                            {
                                if (EsToken(3))
                                {
                                    if (DefaultC == 0)
                                    {
                                        Cont++;
                                        if (EsToken(1003))
                                        {
                                            Cont++;
                                            if (!GrCase())
                                            {
                                                Errores = "Sintaxis incorrecta en Switch, línea: " + ListaTokens[Cont].Linea; return false;
                                            }
                                        }
                                        else if (EsToken(8))
                                        {
                                            if (GrComilla())
                                            {
                                                Cont++;
                                                if (!GrCase())
                                                {
                                                    Errores = "Sintaxis incorrecta en Switch, línea: " + ListaTokens[Cont].Linea; return false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (EsToken(351))
                                {
                                    Cont++;
                                    DefaultC++;
                                    if (DefaultC == 1)
                                    {
                                        if (!GrCase())
                                        {
                                            Errores = "Sintaxis incorrecta en Switch, línea: " + ListaTokens[Cont].Linea; return false;
                                        }
                                    }
                                }
                                else if (EsToken(23))
                                {
                                    if (DefaultC != 0)
                                    {
                                        if (Brake == true)
                                        {
                                            DefaultC = 0;
                                            Brake = false;
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    Errores = "Sintaxis incorrecta en Switch, línea: " + ListaTokens[Cont].Linea; return false;
                                }
                            }
                        }
                    }
                }
            }
            Errores = "Sintaxis incorrecta en Switch, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        bool GrComentarios()
        {
            string Lex = GetLexema();
            if (Lex.Contains("//"))
            {
                return true;
            }
            else if (Lex.Contains("/*") && Lex.Contains("*/"))
            {
                return true;
            }
            Errores = "Sintaxis incorrecta en comentario, línea: " + ListaTokens[Cont].Linea;
            return false;
        }
        public string AnalisisSintactico(List<Tokens> pLista)
        {
            ListaTokens = pLista;
            while (Cont < ListaTokens.Count() && Compilacion)
            {
                Token = ListaTokens[Cont].Token;
                switch (Token)
                {
                    case 15:
                        CharF = false;
                        Cont++; if (EsToken(346)) //main
                        {
                            Void = false; MainC++; Compilacion = GrMain();
                        }
                        else if (EsTokenSig(35)) //funcion
                        {
                            Void = false; Compilacion = GrFuncion();
                        }
                        else //asignacion
                        {
                            Cont--; Compilacion = GrAsignacion(); //variable global
                        }
                        break;
                    case 14:
                        CharF = false;
                        Cont++; if (EsTokenSig(35)) //funcion
                        {
                            Void = false; Compilacion = GrFuncion();
                        }
                        else //asignacion
                        {
                            Cont--; Compilacion = GrAsignacion(); //variable global
                        }
                        break;
                    case 114:
                        CharF = false;
                        Cont++; if (EsTokenSig(35)) //funcion
                        {
                            Void = false; Compilacion = GrFuncion();
                        }
                        else //asignacion
                        {
                            Cont--; Compilacion = GrAsignacion(); //variable global
                        }
                        break;
                    case 4:
                        CharF = true;
                        Cont++; if (EsTokenSig(35)) //funcion
                        {
                            Void = false; Compilacion = GrFuncion();
                        }
                        else //asignacion
                        {
                            Cont--; Compilacion = GrAsignacion(); //variable global
                        }
                        break;
                    case 995: Cont++; Void = true; Compilacion = GrFuncion(); break; //funcion
                    case 355: Compilacion = GrGato(); break;
                    case 1002: Compilacion = GrComentarios(); break;
                    case 1003:
                    case 1001:
                    default: Errores = "Lexema no válido ( " + GetLexema() + " ), línea: " + ListaTokens[Cont].Linea; Compilacion = false; break;
                }
                Cont++;
            }
            if (MainC != 1)
            {
                if(Errores == string.Empty)
                {
                    Errores = "La funcion main no fue encontrada en el archivo";
                }
            }
            return Errores;
        }
    }
}
