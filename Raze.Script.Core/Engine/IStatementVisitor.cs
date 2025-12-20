using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;

namespace Raze.Script.Core.Engine;

internal interface IStatementVisitor<TState, TResult>
{
    public TResult VisitProgramExpression(ProgramExpression program, TState state);

    public TResult VisitVariableDeclarationStatement(VariableDeclarationStatement statement, TState state);

    public TResult VisitReturnStatement(ReturnStatement statement, TState state);

    public TResult VisitLoopStatement(LoopStatement statement, TState state);

    public TResult VisitIfElseStatement(IfElseStatement statement, TState state);

    public TResult VisitFunctionDeclarationStatement(FunctionDeclarationStatement statement, TState state);

    public TResult VisitContinueStatement(ContinueStatement statement, TState state);

    public TResult VisitCodeBlockStatement(CodeBlockStatement statement, TState state);

    public TResult VisitBreakStatement(BreakStatement statement, TState state);

    public TResult VisitAssignmentStatement(AssignmentStatement statement, TState state);

    public TResult VisitRuntimeValueExpression(RuntimeValueExpression expression, TState state);

    public TResult VisitIdentifierExpression(IdentifierExpression expression, TState state);

    public TResult VisitCallExpression(CallExpression expression, TState state);

    public TResult VisitBinaryExpression(BinaryExpression expression, TState state);

    public TResult VisitUnarySimpleExpression(UnarySimpleExpression expression, TState state);

    public TResult VisitUnaryMutationExpression(UnaryMutationExpression expression, TState state);

    public TResult VisitNullCheckerExpression(NullCheckerExpression expression, TState state);

    public TResult VisitBooleanLiteralExpression(BooleanLiteralExpression expression, TState state);

    public TResult VisitDecimalLiteralExpression(DecimalLiteralExpression expression, TState state);

    public TResult VisitIntegerLiteralExpression(IntegerLiteralExpression expression, TState state);

    public TResult VisitNullLiteralExpression(NullLiteralExpression expression, TState state);

    public TResult VisitStringLiteralExpression(StringLiteralExpression expression, TState state);
}
