// See https://aka.ms/new-console-template for more information
Console.WriteLine("There must be a space between each operator and the variables. Example: ( q AND p ) OR ( q AND r )");
Console.WriteLine("Valid list of operators include:\n  - (\n  - )\n  - !, ~\n  - and, &&\n  - or, ||");
Console.WriteLine("To quit at any time press 'CTRL + C'.");
while(true){
    Console.Write("Write statement to print truth table: ");
    string userInputStatement = Console.ReadLine() ?? "p AND q";

    var result = ParseUserInput(userInputStatement);
    PrintTruthTable(result.Variables, result.Statements);
    Console.WriteLine();
}


(IEnumerable<string> Variables, IEnumerable<Statement> Statements) ParseUserInput(string userInput){
    List<Statement> statements = new();
    List<string> variables = new();
    Stack<Operators> operations = new();
    Stack<string> previousVariable = new();
    string[] variablesAndOperators = userInput.Split(' ');
    foreach(string expresssion in variablesAndOperators) {
        switch(expresssion)  {
            case "(": 
                operations.Push(Operators.PARA);
                break;
            case ")": 
                operations.Pop();
                if(operations.Any() && operations.Peek() != Operators.PARA && statements.Count >= 2){
                    Operators op = operations.Pop();
                    string varA = statements[^2].ToString()!;
                    string varB = statements[^1].ToString()!;
                    statements.Add(new NormalStatement(varA, varB, op, VarPrintStyle.NoneVar));
                }
                break;
            case "!":
            case "~": 
                operations.Push(Operators.NOT);
                break;
            case "&&":
            case "and":
            case "AND": 
                operations.Push(Operators.AND);
                break;
            case "||":
            case "or":
            case "OR": 
                operations.Push(Operators.OR);
                break;
            default: {
                variables.Add(expresssion);

                if(operations.Any() && operations.Peek() == Operators.NOT)
                    statements.Add(new SingleVarStatement(expresssion, operations.Pop()));
                else if (!previousVariable.Any()){
                    if(operations.Any() && operations.Peek() != Operators.PARA){
                        Operators op = operations.Pop();
                        string varA = statements[^1].ToString()!;
                        statements.Add(new NormalStatement(varA, expresssion, op, VarPrintStyle.EndVar));
                    } else {
                        previousVariable.Push(expresssion);
                    }
                }
                else if (operations.Any() && operations.Peek() == Operators.PARA){
                    previousVariable.Push(expresssion);
                }
                else {
                    Operators op = operations.Pop();
                    string previous = previousVariable.Pop();
                    statements.Add(new NormalStatement(previous, expresssion, op, VarPrintStyle.BothVar));           
                }
                break;
            }
                
        }
    }
    if(operations.TryPop(out Operators missingOp) && previousVariable.TryPop(out string? missingVar)){
        string varB = statements[^1].ToString()!;
        statements.Add(new NormalStatement(missingVar, varB, missingOp, VarPrintStyle.StartVar));
    }

    return (variables.Distinct(), statements);
}

void PrintTruthTable(IEnumerable<string> variables, IEnumerable<Statement> statements){
    List<string> headers = new List<string>();
    headers.AddRange(variables);
    headers.AddRange(statements.Select(x => x.ToString()!));

    headers.ForEach(x => Console.Write($" {x}\t|"));
    Console.WriteLine();
    var truthValues = GetTruthValuesForVariables(variables.Count());
    foreach(List<bool> value in truthValues){
        int headerIndex = 0;
        for(int i = 0; i < variables.Count(); i++){
            Console.Write($" {value[i]}\t|");
            headerIndex++;
        }
            
        List<(string, bool)> truthValueMatch = variables.Zip(value).ToList();
        foreach(Statement s in statements){
            int width = headers[headerIndex].Length;
            string stringValue = " {0,-" + width + "}\t|";
            bool statementTruthValue = s.CalculateTruthValue(truthValueMatch);
            truthValueMatch.Add((s.ToString()!, statementTruthValue));

            Console.Write(stringValue, statementTruthValue);
            headerIndex++;
        }
        Console.WriteLine();
    }
}

List<List<bool>> GetTruthValuesForVariables(int variableCount)
{
    bool[] values = {true, false};
    bool[][] pools = new bool[variableCount][];
    for(int i = 0; i < variableCount; i++)
        pools[i] = values;
    
    List<List<bool>> result = new() { new() };
    foreach (bool[] pool in pools){
        List<List<bool>> newResult = new();
        foreach(List<bool> x in result){
            foreach(bool y in pool){
                List<bool> tmp = new(x);
                tmp.Add(y);
                newResult.Add(tmp);
            }
        }
        result = newResult;
    }
    return result;
}

interface Statement {
    bool CalculateTruthValue(IEnumerable<(string VarName, bool Value)> variables);
}
class SingleVarStatement: Statement {
    public string Variable { get; set; }
    public Operators Operator { get; set; }

    public SingleVarStatement(string variable, Operators op)
    {
        Variable = variable;
        Operator = op;
    }

    public bool CalculateTruthValue(IEnumerable<(string VarName, bool Value)> variables)
    {
        if(Operator == Operators.NOT)
            return !variables.First(x => x.VarName == Variable).Value;
        else
            throw new NotImplementedException();
    }

    public override string ToString()
        => $"~{Variable}";
    
}
class NormalStatement: Statement {
    public string VariableA { get; }
    public string VariableB { get; }
    public Operators Operator { get; }
    public VarPrintStyle Style { get; }

    public int VariableCount => 2;

    public NormalStatement(string variableA, string variableB, Operators op, VarPrintStyle style = VarPrintStyle.BothVar)
    {
        VariableA = variableA;
        VariableB = variableB;
        Operator = op;
        Style = style;
    }

    public bool CalculateTruthValue(IEnumerable<(string VarName, bool Value)> variables){
        bool varAValue = variables.First(x => x.VarName == VariableA).Value;
        bool varBValue = variables.First(x => x.VarName == VariableB).Value;

        return Operator switch {
            Operators.AND => varAValue && varBValue,
            Operators.OR => varAValue || varBValue,
            _ => false
        };
    }
        

    public override string ToString()
        => Style switch {
            VarPrintStyle.BothVar => $"{VariableA} {Operator} {VariableB}",
            VarPrintStyle.StartVar => $"{VariableA} {Operator} ( {VariableB} )",
            VarPrintStyle.EndVar => $"( {VariableA} ) {Operator} {VariableB}",
            VarPrintStyle.NoneVar => $"( {VariableA} ) {Operator} ( {VariableB} )",
            _ => throw new Exception("Style must be set")
        } ;
}

enum VarPrintStyle {
    StartVar,
    EndVar,
    BothVar,
    NoneVar
}

enum Operators {
    AND,
    OR,
    PARA,
    NOT
}