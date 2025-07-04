﻿namespace Raze.Script.Core.Statements;

internal abstract class Statement
{
    public int StartLine { get; private set; }
    public int StartColumn { get; private set; }
    public virtual bool RequireSemicolon => true;

    public Statement(int startLine, int startColumn)
    {
        StartLine = startLine;
        StartColumn = startColumn;
    }
}
