using System.Collections.Generic;
using System.Text;



using System;

namespace Scl.Grammar {



public class Parser {
	public const int _EOF = 0;
	public const int _l = 1;
	public const int _s = 2;
	public const int _sVal = 3;
	public const int _cVal = 4;
	public const int maxT = 7;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public class SclElement
{
    public string Name;
    public SclElement Parent;
    public readonly List<SclVal> Vals = new List<SclVal>();
    public readonly List<SclElement> Children = new List<SclElement>();
    
    public SclElement(SclElement parent)
    {
        Parent = parent;
    }
    
    internal void AddVal(SclVal val)
    {
        Vals.Add(val);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var val in Vals)
        {
            sb.Append(val);
            sb.Append(",");
        }

        if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);

        return string.Format("Name: {0}, Vals: [{1}]", Name, sb);
    }
}

public class SclVal
{
    public SclVal(Token token, SclElement owner)
    {
        Token = token;
        Owner = owner;
        Val = Read(token);
    }

    private string Read(Token token)
    {
        var str = token.val;

        if (token.kind == _sVal)
            return str;

        var sb = new StringBuilder(str.Length - 2);

        for (int i = 1; i < str.Length - 1; i++)
        {
            var c = str[i];

            if (c == '\\')
            {
                i++;
                c = str[i];

                if (c == 'a') c = '\a';
                else if (c == 'b') c = '\b';
                else if (c == 'f') c = '\f';
                else if (c == 'n') c = '\n';
                else if (c == 'r') c = '\r';
                else if (c == 't') c = '\t';
                else if (c == 'v') c = '\v';
                else if (c == '"') c = '"';
                else if (c == '\\') c = '\\';
            }
            
            sb.Append(c);
        }

        return sb.ToString();
    }

    public Token Token;
    public SclElement Owner;
    public string Val;

    public override string ToString()
    {
        return Val;
    }
}

public readonly SclElement root = new SclElement(null) {Name = "_root_"};



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Scl() {
		while (la.kind == 1 || la.kind == 2 || la.kind == 3) {
			if (la.kind == 3) {
				Element(root);
			} else {
				if (la.kind == 1) {
					Get();
				} else {
					Get();
				}
			}
		}
	}

	void Element(SclElement p) {
		var el = new SclElement(p); p.Children.Add(el); 
		Key();
		el.Name = t.val; 
		while (la.kind == 3 || la.kind == 4) {
			Val();
			el.AddVal(new SclVal(t, el)); 
		}
		if (la.kind == 5) {
			Get();
			while (la.kind == 1 || la.kind == 2) {
				if (la.kind == 1) {
					Get();
				} else {
					Get();
				}
			}
			while (la.kind == 3) {
				Element(el);
				while (la.kind == 1 || la.kind == 2) {
					if (la.kind == 1) {
						Get();
					} else {
						Get();
					}
				}
			}
			Expect(6);
		}
		if (la.kind == 1) {
			Get();
		} else if (la.kind == 2) {
			Get();
		} else SynErr(8);
	}

	void Key() {
		Expect(3);
	}

	void Val() {
		if (la.kind == 3) {
			Key();
		} else if (la.kind == 4) {
			Get();
		} else SynErr(9);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Scl();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "l expected"; break;
			case 2: s = "s expected"; break;
			case 3: s = "sVal expected"; break;
			case 4: s = "cVal expected"; break;
			case 5: s = "\"{\" expected"; break;
			case 6: s = "\"}\" expected"; break;
			case 7: s = "??? expected"; break;
			case 8: s = "invalid Element"; break;
			case 9: s = "invalid Val"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}