using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;

namespace Raze.Script.Core.Statements;

internal interface IStatementVisitor<TState, TResult>
{
    public void VisitProgramExpression(ProgramExpression program, TState state, out TResult result);

    public void VisitNamespaceDeclarationStatement(NamespaceDeclarationStatement statement, TState state, out TResult result);

    public void VisitImportModuleStatement(ImportModuleStatement statement, TState state, out TResult result);

    public void VisitImportFileStatement(ImportFileStatement statement, TState state, out TResult result);

    public void VisitNamespaceAccessExpression(NamespaceAccessExpression expression, TState state, out TResult result);

    public void VisitVariableDeclarationStatement(VariableDeclarationStatement statement, TState state, out TResult result);

    public void VisitReturnStatement(ReturnStatement statement, TState state, out TResult result);

    public void VisitLoopStatement(LoopStatement statement, TState state, out TResult result);

    public void VisitIfElseStatement(IfElseStatement statement, TState state, out TResult result);

    public void VisitFunctionDeclarationStatement(FunctionDeclarationStatement statement, TState state, out TResult result);

    public void VisitContinueStatement(ContinueStatement statement, TState state, out TResult result);

    public void VisitCodeBlockStatement(CodeBlockStatement statement, TState state, out TResult result);

    public void VisitBreakStatement(BreakStatement statement, TState state, out TResult result);

    public void VisitAssignmentStatement(AssignmentStatement statement, TState state, out TResult result);

    public void VisitRuntimeValueExpression(RuntimeValueExpression expression, TState state, out TResult result);

    public void VisitIdentifierExpression(IdentifierExpression expression, TState state, out TResult result);

    public void VisitCallExpression(CallExpression expression, TState state, out TResult result);

    public void VisitBinaryExpression(BinaryExpression expression, TState state, out TResult result);

    public void VisitUnarySimpleExpression(UnarySimpleExpression expression, TState state, out TResult result);

    public void VisitUnaryMutationExpression(UnaryMutationExpression expression, TState state, out TResult result);

    public void VisitNullCheckerExpression(NullCheckerExpression expression, TState state, out TResult result);

    public void VisitBooleanLiteralExpression(BooleanLiteralExpression expression, TState state, out TResult result);

    public void VisitDecimalLiteralExpression(DecimalLiteralExpression expression, TState state, out TResult result);

    public void VisitIntegerLiteralExpression(IntegerLiteralExpression expression, TState state, out TResult result);

    public void VisitNullLiteralExpression(NullLiteralExpression expression, TState state, out TResult result);

    public void VisitStringLiteralExpression(StringLiteralExpression expression, TState state, out TResult result);
}
