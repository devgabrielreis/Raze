using Raze.Script.Core.Exceptions.ControlExceptions;
using Raze.Script.Core.Exceptions.InterpreterExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;
using Raze.Script.Core.Symbols;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Engine;

internal class Interpreter
{
    private enum ExecutionContexts
    {
        Loop
    }

    private Stack<ExecutionContexts> _contextStack;

    public Interpreter()
    {
        _contextStack = new Stack<ExecutionContexts>();
    }

    public void Reset()
    {
        _contextStack.Clear();
    }

    public RuntimeValue Evaluate(Statement statement, Scope scope)
    {
        switch (statement)
        {
            case ProgramExpression program:
                return EvaluateProgramExpression(program, scope);

            case IntegerLiteralExpression expr:
                return new IntegerValue(expr.IntValue);
            case DecimalLiteralExpression expr:
                return new DecimalValue(expr.DecValue);
            case BooleanLiteralExpression expr:
                return new BooleanValue(expr.BoolValue);
            case StringLiteralExpression expr:
                return new StringValue(expr.StrValue);
            case NullLiteralExpression:
                return new NullValue();

            case IdentifierExpression expr:
                return EvaluateIdentifierExpression(expr, scope);

            case CodeBlockStatement stmt:
                return EvaluateCodeBlock(stmt, scope);

            case VariableDeclarationStatement stmt:
                return EvaluateVariableDeclarationStatement(stmt, scope);

            case AssignmentStatement stmt:
                return EvaluateAssignmentStatement(stmt, scope);

            case BinaryExpression expr:
                return EvaluateBinaryExpression(expr, scope);

            case IfElseStatement stmt:
                return EvaluateIfElseStatement(stmt, scope);
            case LoopStatement stmt:
                return EvaluateLoopStatement(stmt, scope);

            case BreakStatement stmt:
                return EvaluateBreakStatement(stmt);
            case ContinueStatement stmt:
                return EvaluateContinueStatement(stmt);

            default:
                throw new UnsupportedStatementException(statement.GetType().Name, statement.StartLine, statement.StartColumn);
        }
    }

    private RuntimeValue EvaluateProgramExpression(ProgramExpression program, Scope scope)
    {
        RuntimeValue lastValue = new VoidValue();

        foreach (var statement in program.Body)
        {
            lastValue = Evaluate(statement, scope);
        }

        return lastValue;
    }

    private VoidValue EvaluateVariableDeclarationStatement(VariableDeclarationStatement statement, Scope scope)
    {
        VariableSymbol variable = new VariableSymbol(
            statement.Value is null ? null : Evaluate(statement.Value, scope),
            statement.Type,
            statement.IsConstant,
            statement.StartLine,
            statement.StartColumn
        );

        scope.DeclareVariable(statement.Identifier, variable, statement);
        return new VoidValue();
    }

    private VoidValue EvaluateAssignmentStatement(AssignmentStatement statement, Scope scope)
    {
        switch (statement.Target)
        {
            case IdentifierExpression expr:
                scope.AssignVariable(expr.Symbol, Evaluate(statement.Value, scope), statement);
                break;
            default:
                throw new InvalidAssignmentException(statement.StartLine, statement.StartColumn);
        }

        return new VoidValue();
    }

    private RuntimeValue EvaluateBinaryExpression(BinaryExpression expression, Scope scope)
    {
        RuntimeValue leftHand = Evaluate(expression.Left, scope);
        OperatorToken op = expression.Operator;
        RuntimeValue rightHand = Evaluate(expression.Right, scope);

        return leftHand.ExecuteBinaryOperation(op, rightHand, expression);
    }

    private static RuntimeValue EvaluateIdentifierExpression(IdentifierExpression expression, Scope scope)
    {
        var resolvedScope = scope.FindSymbolScope(expression.Symbol);

        if (resolvedScope is null)
        {
            throw new UndefinedIdentifierException(expression.Symbol, expression.StartLine, expression.StartColumn);
        }
 
        var result = resolvedScope.FindSymbol(expression.Symbol);

        if (result is VariableSymbol variable)
        {
            if (!variable.IsInitialized)
            {
                throw new UninitializedVariableException(expression.StartLine, expression.StartColumn);
            }

            return variable.Value;
        }

        throw new Exception("nao implementado ainda");
    }

    private VoidValue EvaluateCodeBlock(CodeBlockStatement codeBlock, Scope scope)
    {
        var codeBlockScope = new LocalScope(scope);

        foreach (var stmt in codeBlock.Body)
        {
            Evaluate(stmt, codeBlockScope);
        }

        return new VoidValue();
    }

    private VoidValue EvaluateIfElseStatement(IfElseStatement ifElse, Scope scope)
    {
        if (GetValidBooleanValue(ifElse.Condition, scope))
        {
            Evaluate(ifElse.Then, scope);
        }
        else if (ifElse.Else is not null)
        {
            Evaluate(ifElse.Else, scope);
        }

        return new VoidValue();
    }

    public VoidValue EvaluateLoopStatement(LoopStatement loopStmt, Scope scope)
    {
        var outerLoopScope = new LocalScope(scope);

        foreach (var stmt in loopStmt.Initialization)
        {
            Evaluate(stmt, outerLoopScope);
        }

        _contextStack.Push(ExecutionContexts.Loop);

        while (true)
        {
            if (loopStmt.Condition is not null)
            {
                if (!GetValidBooleanValue(loopStmt.Condition, outerLoopScope))
                {
                    break;
                }
            }

            try
            {
                var currentIterationScope = new LocalScope(outerLoopScope);
                EvaluateCodeBlock(loopStmt.Body, currentIterationScope);
            }
            catch (BreakException)
            {
                break;
            }
            catch (ContinueException)
            {
            }

            if (loopStmt.Update is not null)
            {
                Evaluate(loopStmt.Update, outerLoopScope);
            }
        }

        if (_contextStack.Count == 0 || _contextStack.Peek() != ExecutionContexts.Loop)
        {
            throw new Exception("Corrupted context stack");
        }

        _contextStack.Pop();

        return new VoidValue();
    }

    public VoidValue EvaluateBreakStatement(BreakStatement breakStmt)
    {
        if (_contextStack.Count == 0 || _contextStack.Peek() != ExecutionContexts.Loop)
        {
            throw new UnexpectedStatementException(
                "Cannot use break outside of a loop",
                breakStmt.StartLine,
                breakStmt.StartColumn
            );
        }

        throw new BreakException();
    }

    public VoidValue EvaluateContinueStatement(ContinueStatement continueStmt)
    {
        if (_contextStack.Count == 0 || _contextStack.Peek() != ExecutionContexts.Loop)
        {
            throw new UnexpectedStatementException(
                "Cannot use continue outside of a loop",
                continueStmt.StartLine,
                continueStmt.StartColumn
            );
        }

        throw new ContinueException();
    }

    private bool GetValidBooleanValue(Expression condition, Scope scope)
    {
        var conditionResult = Evaluate(condition, scope);

        if (conditionResult is not BooleanValue)
        {
            throw new UnexpectedTypeException(
                conditionResult.GetType().Name,
                nameof(BooleanValue),
                condition.StartLine,
                condition.StartColumn
            );
        }

        return (conditionResult as BooleanValue)!.BoolValue;
    }
}
