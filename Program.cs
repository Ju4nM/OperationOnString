using System.Text.RegularExpressions;

string input = "2x3/2+4-10";

// ------------------------------------------------------------------------------

bool IsSign(char Sign) => new List<char>(){'+', '-', '/', 'x'}.IndexOf(Sign) != -1;

string FindOperation (string Operation, int Middle, int Left, int Right) {

    int NewLeftIndex = Left - (Left == 0 ? 0 : 1);
    int NewRightIndex = Right + (Right == Operation.Length - 1 ? 0 : 1);
    char CharLeft = Operation[NewLeftIndex];
    char CharRight = Operation[NewRightIndex];


    if (IsSign(CharLeft) && IsSign(CharRight) ||
        IsSign(CharLeft) && NewRightIndex == Operation.Length - 1)
        return Operation.Substring(Left, Right - 1);

    if (NewLeftIndex == 0 && IsSign(CharRight) ||
        Left == 0 && Right == Operation.Length - 1)
         return Operation.Substring(Left, Right + 1);

    if (!IsSign(CharLeft) && NewLeftIndex != Left && Left <= Middle) Left--;
    if (!IsSign(CharRight) && NewRightIndex != Right && Right >= Middle) Right++;

    return FindOperation(Operation, Middle, Left, Right);
}

string DoOperation (string Operation, int Index) {
    
    char Sign = Operation[Index];
    string SubOperation = FindOperation(Operation, Index, Index, Index);
    string[] SubSplitted = SubOperation.Split(Sign);
    double Result = 0;
    if (Sign == '+') Result = Convert.ToDouble(SubSplitted[0]) + Convert.ToDouble(SubSplitted[1]);
    else if (Sign == 'x') Result = Convert.ToDouble(SubSplitted[0]) * Convert.ToDouble(SubSplitted[1]);
    else if (Sign == '/') Result = Convert.ToDouble(SubSplitted[0]) / Convert.ToDouble(SubSplitted[1]);
    else if (Sign == '-') Result = Convert.ToDouble(SubSplitted[0]) - Convert.ToDouble(SubSplitted[1]);
    Operation = String.Join(Convert.ToString(Result), Operation.Split(SubOperation));
    return Operation;
}

bool IsNegativeResult (string Value) {
    string[] ValueSplitted = Value.Split("-");
    return ValueSplitted.Length == 2 && ValueSplitted[0] == "";
}

string ResolveByPattern(string Operation, string[] Signs, int IndexOfSign) {
    
    if (IndexOfSign > Signs.Length - 1 || IsNegativeResult(Operation)) return Operation;
    int SignIndex = Operation.IndexOf(Signs[IndexOfSign]);
    if (SignIndex == -1) IndexOfSign++;
    else Operation = DoOperation(Operation, SignIndex);
    
    return ResolveByPattern(Operation, Signs, IndexOfSign);
}

string SolveOperation (string Operation) {

    Operation = ResolveByPattern(Operation, new String[]{"/", "x"}, 0);
    Operation = ResolveByPattern(Operation, new String[]{"+", "-"}, 0);
    return Operation; 
}

Console.WriteLine(SolveOperation(input));
